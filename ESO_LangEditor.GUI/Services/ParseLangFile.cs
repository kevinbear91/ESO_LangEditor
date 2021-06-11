using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Convert;

namespace ESO_LangEditor.GUI.Services
{
    public class ParseLangFiles : IParseLangFile
    {
        Dictionary<string, LangTextDto> _data = new Dictionary<string, LangTextDto>();

        public Dictionary<string, LangTextDto> ParseCsvFile(string filePath)
        {
            string result;
            //Dictionary<string, LangTextDto> csvData = new Dictionary<string, LangTextDto>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                Debug.WriteLine("Opened file.");

                
                bool passedFirstLine = false;

                if (_data.Count >= 1)
                {
                    _data.Clear();
                }

                while ((result = reader.ReadLine()) != null)
                {
                    string[] words = result.Trim().Split(new char[] { ',' }, 5);

                    if (passedFirstLine)
                    {
                        var lang = ParserCsvAddToDict(words);
                        _data.Add(lang.TextId, lang);
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
                            _data.Add(lang.TextId, lang);
                        }
                    }
                }
                reader.Close();
                Debug.WriteLine("Total lines: " + _data.Count);
                //MessageBox.Show("读取完毕，共 " + csvData.Count + " 行数据。");
            }
            return _data;

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

        public Dictionary<string, LangTextDto> ParseLangFile(string filePath)
        {
            uint _filesize;
            const uint _textIdRecoredSize = 16;
            uint _recoredCount;
            uint _fileId;
            byte[] buffer = new byte[8];
            byte[] langIdBuffer = new byte[16];
            uint textBeginOffset;

            byte[] data = File.ReadAllBytes(filePath);

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

            if (_data.Count >= 1)
            {
                _data.Clear();
            }

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

                    lang.TextEn = text;

                    //Debug.WriteLine("text: {0}", text);
                }

                _data.Add(lang.TextId, lang);

                Debug.WriteLine("id: {0}, unknwon: {1}, index: {2}, offset: {3}, text: {4}",
                    langId, unknown, index, offeset, lang.TextEn);
            }

            return _data;
        }

        public Dictionary<string, LangTextDto> ParseLuaFile(List<string> filePath)
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
                while ((input = sr.ReadLine()) != null)
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

        public JsonFileDto JsonDtoDeserialize(string path)
        {
            string jsonString;

            using (StreamReader reader = new StreamReader(path))
            {
                jsonString = reader.ReadToEnd();
            }

            return JsonSerializer.Deserialize<JsonFileDto>(jsonString);
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

        
    }
}
