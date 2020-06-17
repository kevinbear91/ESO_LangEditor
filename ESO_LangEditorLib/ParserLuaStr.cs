using ESO_LangEditorLib.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ESO_LangEditorLib
{
    public class ParserLuaStr
    {

        public Dictionary<string, LuaUIData> LuaStrParser(string luaPath)
        {
            string input;
            string pattern = @"^[\s]*SafeAddString\(?[\s]*(\w+)[,\s]+\""(.+)\""[^\""]+$";

            Dictionary<string, LuaUIData> luaResult = new Dictionary<string, LuaUIData>();

            using StreamReader sr = new StreamReader(luaPath);
            while ((input = sr.ReadLine()) != null)
            {
                foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
                {
                    string id = match.Groups[1].Value;
                    string text_en = match.Groups[2].Value;

                    luaResult.Add(id, new LuaUIData { 
                    UniqueID = id,
                    Text_EN = text_en,
                    });

                    //Debug.WriteLine("ID: {0}, Content: {1}",
                    //                  match.Groups[1].Value, match.Groups[2].Value);
                }
                
            }
            Debug.WriteLine(luaResult.Count);
            return luaResult;
        }
        public Dictionary<string, LuaUIData> LuaStrParser(List<string> luaPaths)
        {
            bool isClient;
            string input;
            string pattern = @"^[\s]*SafeAddString\(?[\s]*(\w+)[,\s]+\""(.+)\""[^\""]+$";

            Dictionary<string, LuaUIData> luaResult = new Dictionary<string, LuaUIData>();

            foreach(var file in luaPaths)
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

                        if(luaResult.Count >= 1 && luaResult.TryGetValue(id, out LuaUIData luaResultValue))
                        {
                            if (luaResultValue.UniqueID.Contains(id))
                            {

                                luaResult[id].DataEnum = 3;
                            }
                            else
                            {
                                if(isClient)
                                {
                                    luaResult.Add(id, new LuaUIData
                                    {
                                        UniqueID = id,
                                        Text_EN = text_en,
                                        DataEnum = 2,
                                    });
                                }
                                else
                                {
                                    luaResult.Add(id, new LuaUIData
                                    {
                                        UniqueID = id,
                                        Text_EN = text_en,
                                        DataEnum = 1,
                                    });
                                }
                            }
                            
                        }
                        else
                        {
                            if (isClient)
                            {
                                luaResult.Add(id, new LuaUIData
                                {
                                    UniqueID = id,
                                    Text_EN = text_en,
                                    DataEnum = 2,
                                });
                            }
                            else
                            {
                                luaResult.Add(id, new LuaUIData
                                {
                                    UniqueID = id,
                                    Text_EN = text_en,
                                    DataEnum = 1,
                                });
                            }
                        }


                        

                        //Debug.WriteLine("ID: {0}, Content: {1}",
                        //                  match.Groups[1].Value, match.Groups[2].Value);
                    }

                }
            }

            //foreach(var lua in luaResult)
            //{
            //    Debug.WriteLine("ID: {0}, EN: {1}, dataenum: {2}", lua.Value.UniqueID, lua.Value.Text_EN, lua.Value.DataEnum);
            //}

            //Debug.WriteLine(luaResult.Count);
            return luaResult;
        }
    }
}
