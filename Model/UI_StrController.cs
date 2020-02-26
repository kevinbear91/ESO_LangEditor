using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using System.IO;
using static System.Convert;
using System.Threading.Tasks;

namespace ESO_Lang_Editor.Model
{
    class UI_StrController
    {
        private string UIstrDBPath = @"Data/UI_Str.db";

        public Dictionary<string, UIstrFile> ParserLua(string Path)
        {
            //char[] delimiterChars = { '(',')' };
            //String pattern = @"(SI_\w+)^\,\w+$\,";
            String pattern = @"SafeAddString\((SI_\w+\,)";
            //String pattern = @"^SafeAddString\((\bSI_\w+)";

            //String path = @"D:\eso_zh\ESO_LangEditor\str\en_client.lua";

            var lines = File.ReadAllLines(Path, Encoding.UTF8);

            //string[] words;

            Dictionary<string, UIstrFile> strlist = new Dictionary<string, UIstrFile>();


            foreach (var line in lines)
            {
                if (line.StartsWith("SafeAddString"))
                {

                    if (Regex.Split(line, pattern).Count() != 0)
                    {

                        string stringID = Regex.Split(line, pattern)[1].Trim(',');
                        string stringText = Regex.Split(line, pattern)[2].Substring(2, Regex.Split(line, pattern)[2].LastIndexOf(',') - 3);
                        string stringVersion = Regex.Split(line, pattern)[2].Substring(Regex.Split(line, pattern)[2].LastIndexOf(',') + 2).Trim(')');

                        strlist.Add(stringID, new UIstrFile
                        {
                            //addString = line.Split('(')[1].Split(',')[0].Trim(),
                            UI_ID = stringID,
                            UI_EN = stringText,
                            UI_Version = ToInt32(stringVersion),
                        });
                        Console.WriteLine("UIstrFile.ParserLua(), ID: {0}, EN: {1}, version: {2}.", stringID, stringText, stringVersion);
                    }
                }

            }


            return strlist;
        }

        public Dictionary<string, UIstrFile> ParserStr(string Path)
        {
            //String path = @"D:\eso_zh\简体打包\AddOns\EsoUI\lang\zh_client.str";

            var lines = File.ReadAllLines(Path, Encoding.UTF8);

            Dictionary<string, UIstrFile> strlist = new Dictionary<string, UIstrFile>();


            foreach (var line in lines)
            {
                if (line.StartsWith("[SI_"))
                {
                    string[] words = line.Split('=');

                    string textID = words[0].Trim().Substring(1, words[0].LastIndexOf(']') - 1);
                    string zhText = words[1].Trim().Substring(1, words[1].LastIndexOf('"') - 1);

                    strlist.Add(textID, new UIstrFile
                    {
                        UI_ID = textID,
                        UI_ZH = zhText.TrimEnd('"'),
                    });
                    Console.WriteLine("UIstrFile.ParserStr(), ID:{0}, zh:{1}", textID, zhText);
                }
            }
            return strlist;
        }

        public void createDB(Dictionary<string, UIstrFile> Lua_EN, Dictionary<string, UIstrFile> Str_ZH, string TableName, string updateStats)
        {
            var luaData = Lua_EN;
            var strData = Str_ZH;

            Dictionary<string, UIstrFile> uiData = new Dictionary<string, UIstrFile>();

            foreach (var lua in luaData)
            {
                uiData.Add(lua.Key, new UIstrFile
                {
                    UI_Table = TableName,
                    UI_ID = lua.Value.UI_ID,
                    UI_EN = lua.Value.UI_EN,
                    UI_ZH = lua.Value.UI_EN,
                    UI_Version = lua.Value.UI_Version,
                    RowStats = 0,
                    UpdateStats = updateStats,
                });
            }

            foreach (var str in strData)
            {
                if (uiData.ContainsKey(str.Key))
                {
                    uiData[str.Key].UI_ZH = str.Value.UI_ZH;
                    uiData[str.Key].isTranslated = 1;
                }

            }

            CreateStrDBwithData(uiData);
        }


        #region Lua增改删对比

        public Dictionary<string, string> LuaCompareEdited(Dictionary<string, string> NewDict)
        {
            var ContentEdited = new Dictionary<string, string>();
            var DBContent = LoadDB();

            foreach (var entry in DiffDictionary(DBContent, NewDict))
            {
                if (entry.Value == "different")
                    ContentEdited.Add(entry.Key, NewDict[entry.Key]);

            }
            return ContentEdited;
        }

        public Dictionary<string, string> LuaCompareAdded(Dictionary<string, string> NewDict)
        {
            var ContentAdded = new Dictionary<string, string>();
            var DBContent = LoadDB();

            foreach (var entry in DiffDictionary(DBContent, NewDict))
            {
                if (entry.Value == "added")
                    ContentAdded.Add(entry.Key, NewDict[entry.Key]);

            }
            return ContentAdded;
        }

        public Dictionary<string, string> LuaCompareRemove(Dictionary<string, string> NewDict)
        {
            var ContentRemoved = new Dictionary<string, string>();
            var DBContent = LoadDB();

            foreach (var entry in DiffDictionary(DBContent, NewDict))
            {
                if (entry.Value == "removed")
                    ContentRemoved.Add(entry.Key, DBContent[entry.Key]);
            }
            return ContentRemoved;
        }


        private static Dictionary<string, string> DiffDictionary(Dictionary<string, string> first, Dictionary<string, string> second)
        {
            var diff = first.ToDictionary(e => e.Key, e => "removed");
            foreach (var other in second)
            {
                string firstValue;
                if (first.TryGetValue(other.Key, out firstValue))
                {
                    diff[other.Key] = firstValue.Equals(other.Value) ? "same" : "different";
                }
                else
                {
                    diff[other.Key] = "added";
                }
            }
            return diff;
        }

        #endregion

        #region 遍历数据库并返回字典

        public Dictionary<string, string> LoadDB()
        {
            Dictionary<string, string> DataDict = new Dictionary<string, string>();

            var searchData = FullSearchStrDB(false);

            foreach (var data in searchData)
            {
                DataDict.Add(data.UI_ID, data.UI_EN);
            }

            return DataDict;
        }

        #endregion

        #region 数据库增删改标记

        /// <summary>
        /// 新增数据，条件：
        /// stringID 为表名
        /// stringUnknown 与 stringIndex 为条件
        /// 
        /// RowStats 当前行状态
        /// UpdateStats 哪个版本做出的修改
        /// </summary>
        /// <param name="CsvContent"></param>
        public void AddDataList(List<UIstrFile> Content, string DBTable)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + UIstrDBPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;

                try
                {
                    foreach (var line in Content)
                    {
                        cmd.CommandText = "INSERT INTO " + DBTable
                            + " VALUES(@UI_ID, @UI_EN, @UI_ZH, @UI_Version, @RowStats, @isTranslated, @UpdateStats)";
                        cmd.Parameters.AddRange(new[]
                        {
                            new SQLiteParameter("@UI_ID", line.UI_ID),
                            new SQLiteParameter("@UI_EN", line.UI_EN),
                            new SQLiteParameter("@UI_ZH", line.UI_ZH),
                            new SQLiteParameter("@UI_Version", line.UI_Version),
                            new SQLiteParameter("@RowStats", line.RowStats),
                            new SQLiteParameter("@isTranslated", line.isTranslated),
                            new SQLiteParameter("@UpdateStats", line.UpdateStats),
                        });

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("插入了{0}, {1}", line.UI_ID, line.UI_EN);

                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception("标记修改：" + Content.Count + "失败：" + ex.Message);
                }

            }
        }


        /// <summary>
        /// 标记修改数据，条件：
        /// stringID 为表名
        /// stringUnknown 与 stringIndex 为条件
        /// 
        /// RowStats 当前行状态
        /// UpdateStats 哪个版本做出的修改
        /// </summary>
        /// <param name="CsvContent"></param>
        public void MarkChangedDataList(List<UIstrFile> CsvContent, string DBTable)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + UIstrDBPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;

                try
                {
                    foreach (var line in CsvContent)
                    {

                        //标记当前内容为已修改
                        cmd.CommandText = "UPDATE " + DBTable
                            + " SET RowStats=@RowStats"
                            + " WHERE UI_ID='" + line.UI_ID + "'";
                        cmd.Parameters.AddRange(new[]
                        {
                            new SQLiteParameter("@RowStats", 40),          //修改的内容一律为40
                            //new SQLiteParameter("@UpdateStats", line.UpdateStats),
                        });

                        cmd.ExecuteNonQuery();
                        //lineContent = line.stringID.ToString() + line.stringUnknow.ToString() + line.stringIndex.ToString();
                        Console.WriteLine("标记修改{0}, {1}", line.UI_ID, line.UI_EN);



                        //插入修改的内容
                        cmd.CommandText = "INSERT INTO " + DBTable
                            + " VALUES(@UI_ID, @UI_EN, @UI_ZH, @UI_Version, @RowStats, @isTranslated, @UpdateStats)";
                        cmd.Parameters.AddRange(new[]
                        {
                            new SQLiteParameter("@UI_ID", line.UI_ID),
                            new SQLiteParameter("@UI_EN", line.UI_EN),
                            new SQLiteParameter("@UI_ZH", line.UI_ZH),
                            new SQLiteParameter("@UI_Version", line.UI_Version),
                            new SQLiteParameter("@RowStats", 20),
                            new SQLiteParameter("@isTranslated", line.isTranslated),
                            new SQLiteParameter("@UpdateStats", line.UpdateStats),
                        });

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("插入了{0}, {1}", line.UI_ID, line.UI_EN);

                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception("标记修改：" + CsvContent.Count + "失败：" + ex.Message);
                }

            }
        }






        /// <summary>
        /// 标记删除数据，条件：
        /// stringID 为表名
        /// stringUnknown 与 stringIndex 为条件
        /// 
        /// RowStats 当前行状态
        /// UpdateStats 哪个版本做出的修改
        /// </summary>
        /// <param name="CsvContent"></param>
        public void MarkDeleteDataList(List<UIstrFile> Content, string DBTable)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + UIstrDBPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;

                try
                {
                    foreach (var line in Content)
                    {
                        cmd.CommandText = "UPDATE " + DBTable
                            + " SET RowStats=@RowStats,UpdateStats=@UpdateStats"
                            + " WHERE UI_ID='" + line.UI_ID + "'";
                        cmd.Parameters.AddRange(new[]
                        {
                            new SQLiteParameter("@RowStats", 30),                   //标记删除的内容一律为30
                            new SQLiteParameter("@UpdateStats", line.UpdateStats),
                        });

                        cmd.ExecuteNonQuery();
                        //lineContent = line.stringID.ToString() + line.stringUnknow.ToString() + line.stringIndex.ToString();
                        Console.WriteLine("标记删除{0}, {1}", line.UI_ID, line.UI_EN);
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception("标记删除：" + Content.Count + "失败：" + ex.Message);
                }

            }
        }

        #endregion



        #region Str数据库相关 -- 创建数据库与添加内容
        public void CreateStrDBwithData(Dictionary<string, UIstrFile> UIContent)
        {
            if (!File.Exists(UIstrDBPath))
            {
                SQLiteConnection.CreateFile(UIstrDBPath);
            }


            foreach (var table in UIContent)
            {
                if (!CheckTableIfExist(table.Value.UI_Table, UIstrDBPath))
                {
                    CreateTableToStrDB(table.Value.UI_Table, UIstrDBPath);
                }

            }


            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + UIstrDBPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;

                try
                {
                    foreach (var content in UIContent)
                    {
                        cmd.CommandText = "SELECT * FROM " + content.Value.UI_Table
                        + " WHERE UI_ID='" + content.Value.UI_ID + "'";
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 0)
                        {
                            cmd.CommandText = "INSERT INTO " + content.Value.UI_Table + " VALUES(@UI_ID, @UI_EN, @UI_ZH, @UI_Version, @RowStats, @isTranslated, @UpdateStats)";
                            cmd.Parameters.AddRange(new[]
                            {
                            new SQLiteParameter("@UI_ID", content.Value.UI_ID),
                            new SQLiteParameter("@UI_EN", content.Value.UI_EN),
                            new SQLiteParameter("@UI_ZH", content.Value.UI_ZH),
                            new SQLiteParameter("@UI_Version", content.Value.UI_Version),
                            new SQLiteParameter("@RowStats", content.Value.RowStats),
                            new SQLiteParameter("@isTranslated", content.Value.isTranslated),
                            new SQLiteParameter("@UpdateStats", content.Value.UpdateStats),
                            });
                            Console.WriteLine("插入了{0}, {1}, {2}", content.Value.UI_ID, content.Value.UI_EN, content.Value.UI_ZH);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd.CommandText = "UPDATE " + content.Value.UI_Table + " SET UI_ZH=@UI_ZH"
                            + " WHERE UI_ID='" + content.Value.UI_ID + "'";

                            cmd.Parameters.Add(new SQLiteParameter("@UI_ZH", content.Value.UI_ZH));
                            Console.WriteLine("插入了{0}, {1}, {2}", content.Value.UI_ID, content.Value.UI_EN, content.Value.UI_ZH);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();

                }
                catch (Exception ex)
                {
                    //return "插入数据失败：" + ex.Message;
                    throw new Exception("插入数据失败：" + ex.Message);
                }

            }

        }

        #endregion

        #region STR数据库相关 -- 全局搜索

        public List<UIstrFile> FullSearchStrDB(bool SearchAbandonContent)
        {
            var _LangViewData = new List<UIstrFile>();

            string rowStatsInt = " WHERE RowStats < 30";

            if (SearchAbandonContent)
                rowStatsInt = "";

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + UIstrDBPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;

                try
                {
                    List<string> tableName = new List<string>();   //表名列表

                    cmd.CommandText = "SELECT name FROM sqlite_master WHERE TYPE='table'";   //获得当前所有表名
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {
                        tableName.Add(sr.GetString(0));
                    }
                    sr.Close();


                    foreach (var t in tableName)
                    {
                        cmd.CommandText = "SELECT * FROM " + t
                            + rowStatsInt;

                        sr = cmd.ExecuteReader();

                        while (sr.Read())
                        {
                            _LangViewData.Add(new UIstrFile
                            {
                                //ID_Table = t.ToString(),                   
                                //IndexDB = sr.FieldCount,                   
                                UI_ID = sr.GetString(0),
                                UI_EN = sr.GetString(1),
                                UI_ZH = sr.GetString(2),
                                UI_Version = sr.GetInt32(3),
                                RowStats = sr.GetInt32(4),
                                isTranslated = sr.GetInt32(5),
                                UpdateStats = sr.GetString(6),

                            });
                            //Console.WriteLine("查询了{0}, {1}, {2}", sr.GetInt32(0).ToString(), sr.GetInt32(1), sr.GetString(4));
                        }
                        sr.Close();
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("查询数据：失败：" + ex.Message);
                }
                return _LangViewData;
            }

        }

        #endregion

        #region Str数据库 -- 更新编辑后的文本

        public string UpdateStrFromEditor(UIstrFile Content)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + UIstrDBPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;

                try
                {
                    cmd.CommandText = "UPDATE " + Content.UI_Table
                        + " SET UI_ZH=@UI_ZH, isTranslated=@isTranslated"
                        + " WHERE UI_ID = '" + Content.UI_ID + "'";

                    cmd.Parameters.AddRange(new[]
                    {
                        new SQLiteParameter("@UI_ZH", Content.UI_ZH),
                        new SQLiteParameter("@isTranslated", Content.isTranslated)

                    });
                    cmd.ExecuteNonQuery();
                    return Content.UI_ZH + " 更新成功！";

                }
                catch (Exception ex)
                {
                    return "插入数据：" + Content.UI_ZH + " 失败：" + ex.Message;
                    throw new Exception("插入数据：" + Content.UI_ZH + "失败：" + ex.Message);
                }

            }

        }

        #endregion

        #region  检查表是否存在

        public bool CheckTableIfExist(string tableName, string DBPath)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + DBPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;

                try
                {
                    cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='" + tableName + "';";
                    var table = cmd.ExecuteScalar();

                    if (table != null && table.ToString() == tableName)
                        return true;
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    throw new Exception("查询数据表" + tableName + "失败：" + ex.Message);
                }


            }
        }

        #endregion

        #region  创建表

        public void CreateTableToStrDB(string TableName, string DBPath)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + DBPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                try
                {
                    cmd.CommandText = "CREATE TABLE " + TableName + "(UI_ID text, UI_EN text, UI_ZH text, UI_Version int, RowStats int, isTranslated int, UpdateStats text)";
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw new Exception("创建数据表" + TableName + "失败：" + ex.Message);
                }
            }
        }

        #endregion

        #region 增加字段，初始内容类型为 int 的重载
        public void FieldAdd(string FieldName, string FieldType, int InitContent, string tableName)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + UIstrDBPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;

                try
                {
                    //添加字段
                    cmd.CommandText = "ALTER TABLE " + tableName
                                + " ADD COLUMN " + FieldName + " " + FieldType;

                    cmd.ExecuteNonQuery();

                    Console.WriteLine("表：{0}, 字段名：{1}, 字段类型：{2}", tableName, FieldName, FieldType);

                    //初始化字段内容
                    cmd.CommandText = "UPDATE " + tableName
                            + " SET " + FieldName + "=@FieldName";
                    cmd.Parameters.AddRange(new[]
                    {
                        new SQLiteParameter("@FieldName", InitContent),
                    });

                    cmd.ExecuteNonQuery();

                    Console.WriteLine("表：{0}, 字段名：{1}, 初始内容：{2}", tableName, FieldName, InitContent);

                    tx.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception("增加字段失败：" + ex.Message);
                }

            }
        }
        #endregion

        #region 增加字段，初始内容类型为 string 的重载
        public void FieldAdd(string FieldName, string FieldType, string InitContent, string tableName)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + UIstrDBPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;

                try
                {
                    //添加字段
                    cmd.CommandText = "ALTER TABLE " + tableName
                                + " ADD COLUMN " + FieldName + " " + FieldType;

                    cmd.ExecuteNonQuery();

                    Console.WriteLine("表：{0}, 字段名：{1}, 字段类型：{2}", tableName, FieldName, FieldType);

                    //初始化字段内容
                    cmd.CommandText = "UPDATE " + tableName
                            + " SET " + FieldName + "=@FieldName";
                    cmd.Parameters.AddRange(new[]
                    {
                        new SQLiteParameter("@FieldName", InitContent),
                    });

                    cmd.ExecuteNonQuery();

                    Console.WriteLine("表：{0}, 字段名：{1}, 初始内容：{2}", tableName, FieldName, InitContent);

                    tx.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception("增加字段失败：" + ex.Message);
                }

            }
        }
        #endregion

        #region 搜索数据库

        public List<UIstrFile> SearchData(string CsvContent, int SearchField, bool SearchAbandonContent)
        {
            var _LangViewData = new List<UIstrFile>();
            string fieldName;
            string rowStatsInt = " AND RowStats < 30";

            if (SearchAbandonContent)
                rowStatsInt = "";

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + UIstrDBPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;

                switch (SearchField)
                {
                    case 0:
                        fieldName = "UI_ID";
                        break;
                    case 1:
                        fieldName = "UI_EN";
                        break;
                    case 2:
                        fieldName = "UI_ZH";
                        break;
                    case 3:
                        fieldName = "isTranslated";
                        break;
                    default:
                        fieldName = "UI_EN";
                        break;
                }

                try
                {
                    List<string> tableName = new List<string>();   //表名列表

                    cmd.CommandText = "SELECT name FROM sqlite_master WHERE TYPE='table'";   //获得当前所有表名
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {
                        tableName.Add(sr.GetString(0));
                    }
                    sr.Close();


                    foreach (var t in tableName)
                    {
                        cmd.CommandText = "SELECT * FROM " + t
                            + " WHERE " + fieldName
                            + " LIKE @SEARCH"
                            + rowStatsInt;
                        cmd.Parameters.AddWithValue("@SEARCH", CsvContent);     //遍历全库查询要搜索在任意位置的文本
                        sr = cmd.ExecuteReader();

                        while (sr.Read())
                        {
                            _LangViewData.Add(new UIstrFile
                            {
                                UI_Table = t.ToString(),                   //数据表名
                                UI_ID = sr.GetString(0),                  //游戏内文本ID
                                UI_EN = sr.GetString(1),               //
                                UI_ZH = sr.GetString(2),                 //
                                RowStats = sr.GetInt32(4),
                                isTranslated = sr.GetInt32(5),              //是否翻译
                                UpdateStats = sr.GetString(6),
                            });
                        }
                        sr.Close();
                    }
                    //conn.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("查询数据：" + CsvContent + "失败：" + ex.Message);
                }
                return _LangViewData;
            }

        }

        #endregion
    }
}
