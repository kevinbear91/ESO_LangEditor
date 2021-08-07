using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading.Tasks;
using static System.Convert;

namespace ESO_LangEditor.GUI.Services
{
    public class LangFile : ILangFile
    {
        //Dictionary<string, LangTextDto> _data;
        private ILangTextRepoClient _langTextRepo;

        public LangFile(/*ILangTextRepoClient langTextRepo*/)
        {
            //_langTextRepo = langTextRepo;
        }

        public async Task<Dictionary<string, LangTextDto>> ParseCsvFile(string filePath)
        {
            string result;
            Dictionary<string, LangTextDto> csvData = new Dictionary<string, LangTextDto>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                Debug.WriteLine("Opened file.");


                bool passedFirstLine = false;

                if (csvData.Count >= 1)
                {
                    csvData.Clear();
                }

                while ((result = await reader.ReadLineAsync()) != null)
                {
                    string[] words = result.Trim().Split(new char[] { ',' }, 5);

                    if (passedFirstLine)
                    {
                        var lang = ParserCsvAddToDict(words);
                        csvData.Add(lang.TextId, lang);
                    }
                    else
                    {
                        //id = words[0].Trim('"');

                        if (words[0].Trim('"') == "ID") //If csv first column string is ID.
                        {
                            passedFirstLine = true;
                            Debug.WriteLine("Have header line, set to skip.");
                        }
                        else
                        {
                            var lang = ParserCsvAddToDict(words);
                            csvData.Add(lang.TextId, lang);
                        }
                    }
                }
                reader.Close();
                Debug.WriteLine("Total lines: " + csvData.Count);
                //MessageBox.Show("读取完毕，共 " + csvData.Count + " 行数据。");
            }
            return csvData;

            static LangTextDto ParserCsvAddToDict(string[] words)
            #region Parse CSV column，and return LangData
            {
                string id;
                string unknown;
                string index;
                string text;

                id = words[0].Trim('"');
                unknown = words[1].Trim('"');
                index = words[2].Trim('"');
                text = words[4].Substring(1, words[4].Length - 2).Replace("\"\"", "\"");
                string key = id + "-" + unknown + "-" + index;

                LangTextDto lang = new LangTextDto
                {
                    IdType = ToInt32(id),
                    TextId = key,
                    TextEn = text,
                    LangTextType = LangType.LangText,
                };
                //Debug.WriteLine("ID: " + id + ", "
                //    + "Unknown: " + unknown + ", "
                //    + "Index: " + index + ", "
                //    + "Text: " + text);
                return lang;
            }
            #endregion
        }

        public async Task<Dictionary<string, LangTextDto>> ParseLangFile(string filePath)
        {
            uint _filesize;
            const uint _textIdRecoredSize = 16;
            uint _recoredCount;
            uint _fileId;
            byte[] buffer = new byte[8];
            byte[] langIdBuffer = new byte[16];
            uint textBeginOffset;

            byte[] data = File.ReadAllBytes(filePath);

            Dictionary<string, LangTextDto> _data = new Dictionary<string, LangTextDto>();

            _filesize = (uint)data.Length;

            Array.Copy(data, buffer, 8);
            Array.Reverse(buffer, 0, buffer.Length);  //Reverse bytes order, new readed on head.

            _fileId = (uint)BitConverter.ToInt32(buffer, 4);
            _recoredCount = (uint)BitConverter.ToInt32(buffer, 0);

            Debug.WriteLine("field Id: {0}", _fileId);
            Debug.WriteLine("count int: {0}", _recoredCount);

            textBeginOffset = _recoredCount * _textIdRecoredSize + 8;
            Debug.WriteLine("textBeginOffset: {0}", textBeginOffset);

            if (data == null || data.Length <= 0)
            {
                throw new Exception("Error: Invaild data!");
            }

            if (_filesize < 8)
            {
                throw new Exception("Error: Invaild Lang file size!");
            }

            if (_filesize > int.MaxValue)
            {
                throw new Exception("Error: Lang file too big");
            }

            byte[] textUtf8Buffer = new byte[1];

            //if (_data.Count >= 1)
            //{
            //    _data.Clear();
            //}

            for (uint i = 0; i < _recoredCount; ++i)
            {
                uint offset = 8 + i * _textIdRecoredSize;

                //Debug.WriteLine("Offset: {0}", offset);

                Array.Copy(data, offset, langIdBuffer, 0, langIdBuffer.Length);
                Array.Reverse(langIdBuffer, 0, langIdBuffer.Length);

                uint langId = (uint)BitConverter.ToInt32(langIdBuffer, 12);
                uint unknown = (uint)BitConverter.ToInt32(langIdBuffer, 8);
                uint index = (uint)BitConverter.ToInt32(langIdBuffer, 4);
                uint offeset = (uint)BitConverter.ToInt32(langIdBuffer, 0);
                string text;

                LangTextDto lang = new LangTextDto
                {
                    TextId = langId + "-" + unknown + "-" + index,
                    LangTextType = LangType.LangText
                    //Text = text,
                };

                uint textOffset = offeset + textBeginOffset;

                if (textOffset < _filesize)
                {
                    string textbuffer = "";

                    for (int c = 0; c + textOffset < _filesize; ++c)
                    {
                        int ost = c + (int)textOffset;
                        Array.Copy(data, ost, textUtf8Buffer, 0, textUtf8Buffer.Length);

                        var hex = BitConverter.ToString(textUtf8Buffer);

                        if (hex == "00")
                        {
                            break;
                        }
                        else
                        {
                            textbuffer += hex;
                        }
                    }

                    byte[] stringByte = FromHex(textbuffer);
                    text = Encoding.UTF8.GetString(stringByte);

                    text = text.Replace("\x0a", @"\n");
                    text = text.Replace("\x0d", @"\r");

                    lang.TextEn = text;

                    //Debug.WriteLine("text: {0}", text);
                }

                _data.Add(lang.TextId, lang);

                //Debug.WriteLine("id: {0}, unknwon: {1}, index: {2}, offset: {3}, text: {4}",
                //    langId, unknown, index, offeset, lang.TextEn);
            }

            return _data;
        }

        public async Task<Dictionary<string, LangTextDto>> ParseLuaFile(List<string> filePath)
        {
            bool isClient;
            string input;
            string pattern = @"^[\s]*SafeAddString\(?[\s]*(\w+)[,\s]+\""(.+)\""[^\""]+$";

            var luaResult = new Dictionary<string, LangTextDto>();

            foreach (var file in filePath)
            {
                if (file.EndsWith("en_client.lua"))
                    isClient = true;
                else
                    isClient = false;

                using StreamReader sr = new StreamReader(file);
                while ((input = await sr.ReadLineAsync()) != null)
                {
                    foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
                    {
                        string id = match.Groups[1].Value;
                        string text_en = match.Groups[2].Value;
                        int idType = 100;

                        if (luaResult.Count >= 1 && luaResult.TryGetValue(id, out LangTextDto luaResultValue))
                        {
                            if (luaResultValue.TextId.Contains(id))
                            {

                                luaResult[id].LangTextType = LangType.LuaBoth;
                            }
                            else
                            {
                                if (isClient)
                                {
                                    luaResult.Add(id, new LangTextDto
                                    {
                                        TextId = id,
                                        IdType = idType,
                                        TextEn = text_en,
                                        LangTextType = LangType.LuaClient,
                                    });
                                }
                                else
                                {
                                    luaResult.Add(id, new LangTextDto
                                    {
                                        TextId = id,
                                        IdType = idType,
                                        TextEn = text_en,
                                        LangTextType = LangType.LuaPreGame,
                                    });
                                }
                            }

                        }
                        else
                        {
                            if (isClient)
                            {
                                luaResult.Add(id, new LangTextDto
                                {
                                    TextId = id,
                                    IdType = idType,
                                    TextEn = text_en,
                                    LangTextType = LangType.LuaClient,
                                });
                            }
                            else
                            {
                                luaResult.Add(id, new LangTextDto
                                {
                                    TextId = id,
                                    IdType = idType,
                                    TextEn = text_en,
                                    LangTextType = LangType.LuaPreGame,
                                });
                            }
                        }
                        //Debug.WriteLine("ID: {0}, Content: {1}",
                        //                  match.Groups[1].Value, match.Groups[2].Value);
                    }

                }
            }
            return luaResult;

            //foreach(var lua in luaResult)
            //{
            //    Debug.WriteLine("ID: {0}, EN: {1}, dataenum: {2}", lua.Value.UniqueID, lua.Value.Text_EN, lua.Value.DataEnum);
            //}

            //Debug.WriteLine(luaResult.Count);

        }

        public async Task<string> ExportLangTextsAsJson(List<LangTextDto> langtextsList, LangChangeType changeType)
        {
            string fileName;

            langtextsList.ForEach(lang => lang.IsTranslated = 2);

            var json = new JsonFileDto
            {
                LangTexts = langtextsList,
                Version = "1",
                ExportTime = DateTime.Now,
                ChangeType = changeType,
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),

            };

            var options1 = new JsonWriterOptions
            {
                Indented = true
            };

            string jsonString = JsonSerializer.Serialize(json, options);

            if (changeType == LangChangeType.ChangedZH)
            {
                fileName = @"Export\Translate_" + GetTimeToFileName() + ".json";
            }
            else
            {
                fileName = @"Export\DatabaseRev_" + GetTimeToFileName() + "_" + changeType.ToString() + ".json";
            }

            try
            {
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    await sw.WriteLineAsync(jsonString);
                }

                return fileName;
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    await sw.WriteLineAsync(jsonString);
                }
                return fileName;
            }
        }

        public async Task ExportToText(List<LangTextDto> langList)
        {
            var outputIDFile = new List<string>();
            var outputTextFile = new List<string>();

            if (!Directory.Exists("_tmp"))
                Directory.CreateDirectory("_tmp");

            foreach (var l in langList)
            {
                outputIDFile.Add(l.TextId);
                outputTextFile.Add(l.TextZh);
            }

            StreamWriter[] writer = new StreamWriter[2];
            writer[0] = new StreamWriter("_tmp/ID.txt");
            writer[1] = new StreamWriter("_tmp/Text.txt");

            foreach (var l in langList)
            {
                await writer[0].WriteLineAsync(l.TextId);
                await writer[1].WriteLineAsync(l.TextZh);
            }
            writer[0].Flush();
            writer[0].Close();

            //using (StreamWriter sw = new StreamWriter("_tmp/ID.txt"))
            //{
            //    foreach (var l in langList)
            //    {
            //        sw.WriteLine(l.TextId);
            //    }
            //    sw.Flush();
            //    sw.Close();
            //}

            //using (StreamWriter sw = new StreamWriter("_tmp/Text.txt"))
            //{
            //    foreach (string s in outputTextFile)
            //    {
            //        sw.WriteLine(s);
            //    }
            //    sw.Flush();
            //    sw.Close();
            //}
        }

        public async Task ExportLuaToStr(List<LangTextDto> langList)
        {
            List<string> clientData = new List<string>();
            List<string> pregameData = new List<string>();
            string line;

            StreamReader file = new StreamReader(@"Data\FontLib.txt");

            while ((line = await file.ReadLineAsync()) != null)
            {
                clientData.Add(line);
                pregameData.Add(line);
            }
            file.Close();
            file.Dispose();

            if (!Directory.Exists("Export"))
                Directory.CreateDirectory("Export");

            foreach (var d in langList)
            {
                switch (d.LangTextType)
                {
                    case LangType.LuaPreGame:
                        pregameData.Add("[" + d.TextId + "]"
                        + " = "
                        + "\"" + d.TextZh + "\"");
                        break;
                    case LangType.LuaClient:
                        clientData.Add("[" + d.TextId + "]"
                        + " = "
                        + "\"" + d.TextZh + "\"");
                        break;
                    case LangType.LuaBoth:
                        pregameData.Add("[" + d.TextId + "]"
                        + " = "
                        + "\"" + d.TextZh + "\"");
                        clientData.Add("[" + d.TextId + "]"
                        + " = "
                        + "\"" + d.TextZh + "\"");
                        break;
                }

            }

            using (StreamWriter sw = new StreamWriter(@"Export\zh_client.str"))
            {
                foreach (string s in clientData)
                {
                    await sw.WriteLineAsync(s);
                }
                sw.Flush();
                sw.Close();
            }
            using (StreamWriter sw = new StreamWriter(@"Export\zh_pregame.str"))
            {

                foreach (string s in pregameData)
                {
                    await sw.WriteLineAsync(s);
                }

                sw.Flush();
                sw.Close();
            }
        }

        public async Task ExportToLang(Dictionary<string, LangTextDto> langList)
        {
            //var langDict = await _langTextRepo.GetAlltLangTextsDictionaryAsync();
            var orderedLang = langList.OrderBy(lang => lang.Value.IdType);

            List<LangFileDto> langFiles = new List<LangFileDto>();
            Dictionary<string, int> langSameTextOffsetDict = new Dictionary<string, int>();

            int recoredCount = orderedLang.Count();
            const int _textIdRecoredSize = 16;
            int textBeginOffset = recoredCount * _textIdRecoredSize + 8;

            byte[] headVersion = BitConverter.GetBytes(0x02);
            byte[] recoredCountByte = BitConverter.GetBytes(recoredCount);

            byte[] langHeadBuffer = new byte[8];
            byte[] langTextRecoredbuffer = new byte[16];

            int id;
            int unknown;
            int index;
            int offset = 0;
            int lastVaildOffset = 0;
            string text;
            List<byte> fileByteList = new List<byte>();
            List<byte> textBytesList = new List<byte>();

            using (var b = new BinaryWriter(File.Open("zh.lang", FileMode.Create)))
            {
                Array.Reverse(headVersion);
                Array.Reverse(recoredCountByte);

                headVersion.CopyTo(langHeadBuffer, 0);
                recoredCountByte.CopyTo(langHeadBuffer, 4);

                fileByteList.AddRange(langHeadBuffer);

                //b.Write(headVersion);
                //b.Write(recoredCountByte);

                //int recoredOffset = 8;

                foreach (var lang in orderedLang)
                {
                    string[] keySplit = lang.Key.Trim().Split(new char[] { '-' }, 3);

                    id = ToInt32(keySplit[0]);
                    unknown = ToInt32(keySplit[1]);
                    index = ToInt32(keySplit[2]);

                    text = lang.Value.TextZh == null ? lang.Value.TextEn : lang.Value.TextZh;

                    text = text.Replace(@"\n", "\x0a");
                    text = text.Replace(@"\r", "\x0d");
                    text = text.Replace("\"\"", "\"");

                    //Debug.WriteLine($"text: {text}, Length: {currentTextLength}");

                    //foreach(var textbyte in textBytes)
                    //{
                    //    Debug.WriteLine($"text bytes: {textbyte}");
                    //}



                    byte[] idBytes = BitConverter.GetBytes(id);
                    byte[] unknownBytes = BitConverter.GetBytes(unknown);
                    byte[] indexBytes = BitConverter.GetBytes(index);
                    byte[] currentOffsetBytes = BitConverter.GetBytes(offset);

                    Array.Reverse(idBytes);
                    Array.Reverse(unknownBytes);
                    Array.Reverse(indexBytes);
                    Array.Reverse(currentOffsetBytes);

                    idBytes.CopyTo(langTextRecoredbuffer, 0);
                    unknownBytes.CopyTo(langTextRecoredbuffer, 4);
                    indexBytes.CopyTo(langTextRecoredbuffer, 8);
                    currentOffsetBytes.CopyTo(langTextRecoredbuffer, 12);

                    fileByteList.AddRange(langTextRecoredbuffer);

                    var textBytes = Encoding.UTF8.GetBytes(text + "\0");
                    int currentTextLength = textBytes.Length;

                    lastVaildOffset += currentTextLength;    //文本偏移 = 当前偏移 + 当前文本长度(包含Null字节)
                    offset = lastVaildOffset;
                    //langSameTextOffsetDict.Add(text, lastVaildOffset);

                    textBytesList.AddRange(textBytes);

                    //if (langSameTextOffsetDict.ContainsKey(text))
                    //{
                    //    offset = langSameTextOffsetDict[text];
                    //}
                    //else
                    //{
                    //    var textBytes = Encoding.UTF8.GetBytes(text + "\0");
                    //    int currentTextLength = textBytes.Length;

                    //    lastVaildOffset += currentTextLength;    //文本偏移 = 当前偏移 + 当前文本长度(包含Null字节)
                    //    offset = lastVaildOffset;
                    //    langSameTextOffsetDict.Add(text, lastVaildOffset);

                    //    textBytesList.AddRange(textBytes);
                    //}

                    //Debug.WriteLine($"ID: {id}, unknown: {unknown}, Index: {index}, offset: {currentOffset}, text: {text}");

                }

                fileByteList.AddRange(textBytesList);

                var fileByteArray = fileByteList.ToArray();

                //foreach (var t in langSameTextOffsetDict)
                //{
                //    //var tKey = t.Key;
                //    //tKey = tKey.Replace(@"\n", "\x0a");
                //    //tKey = tKey.Replace(@"\r", "\x0d");
                //    var textBytes = Encoding.UTF8.GetBytes(t.Key + "\0");

                //    textBytes.CopyTo(fileByteArray, t.Value);
                //}

                //fileByteList.ToArray();
                b.Write(fileByteArray);
            }


            //using (var s = new FileStream("zh.lang", FileMode.Append, FileAccess.Write))
            //{
            //    s.WriteAsync
            //}


            //await File.WriteAllBytesAsync("zh.lang",)
            //FileStream fs = new FileStream("aaa.lang", FileMode.Create);
            //fs.WriteAsync



            //using (StreamWriter sw = new StreamWriter())
            //{
            //    sw.
            //}


        }

        public JsonFileDto JsonDtoDeserialize(string path)
        {
            string jsonString;

            using (StreamReader reader = new StreamReader(path))
            {
                jsonString = reader.ReadToEnd();
            }

            return JsonSerializer.Deserialize<JsonFileDto>(jsonString);
        }

        private static string GetTimeToFileName()
        {
            return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }

        /// <summary>
        /// Turn hex byte string to byte array.
        /// 
        /// From https://stackoverflow.com/a/724905
        /// 
        /// <para>For example: 49-44-4C-45 is string IDLE.</para>
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        private static bool IsEn(string langText)
        {
            if (string.IsNullOrWhiteSpace(langText))
            {
                return true;
            }

            var bytes = Encoding.UTF8.GetBytes(langText);
            bool result = bytes.Length == langText.Length;
            return result;
        }
    }
}
