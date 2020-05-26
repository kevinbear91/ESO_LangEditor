using ESO_Lang_Editor.View;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using static System.Convert;

namespace ESO_Lang_Editor.Model
{
    class SQLiteController
    {
        SQLiteConnection Conn;
        string csvDataPath = @"Data\CsvData.db";
        List<string> csvTableID;

        public void ConnectSQLite()
        {
            //SQLiteConnection Conn;

            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");

            if (!File.Exists(csvDataPath))
            {
                SQLiteConnection.CreateFile(csvDataPath);
            }

        }

        /*
        public void GetTableID()
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;

                //List<string> tableName = new List<string>();   //表名列表

                cmd.CommandText = "SELECT name FROM sqlite_master WHERE TYPE='table'";   //获得当前所有表名
                SQLiteDataReader sr = cmd.ExecuteReader();

                while (sr.Read())
                {
                    csvTableID.Add(sr.GetString(0));
                }
                sr.Close();
            }
        }
        */

        public void CreateTable(int CsvID)
        {
            try
            {
                string sql = "CREATE TABLE ID_" + CsvID + "(ID_Type int, ID_Unknown int, ID_Index int, Text_EN text, Text_SC text, isTranslated int, UpdateStats text)";
                SQLiteCommand command = new SQLiteCommand(sql, Conn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("创建数据表" + CsvID + "失败：" + ex.Message);
            }

        }

        public string CreateDBFileFromCSV(List<FileModel_IntoDB> Content)
        {
            List<int> tableID = new List<int>();
            //csvDataPath = @"Data\CsvTest.db";

            foreach (var table in Content)
            {
                if (!tableID.Contains(table.stringID))
                    tableID.Add(table.stringID);
            }

            ConnectSQLite();
            CreateTableArray(tableID);
            AddDataArray(Content);

            return "创建完成！";

        }

        /// <summary>
        /// 批量创建表和字段。
        /// ID_Type 为文本ID
        /// ID_Unknow 为文本位置列
        /// ID_Index 为索引列
        /// Text_EN 为英语原文
        /// Text_SC 为译文
        /// isTranslated 为是否汉化的标记，供导出用。
        /// </summary>
        /// <param name="CsvID"></param>
        public void CreateTableArray(List<int> CsvID)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    foreach (var id in CsvID)
                    {
                        cmd.CommandText = "CREATE TABLE ID_" + id + "(ID_Type int, ID_Unknown int, ID_Index int, Text_EN text, Text_SC text, isTranslated int, UpdateStats text)";
                        cmd.ExecuteNonQuery();
                        //Console.WriteLine("创建了{0}表", id);
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception("创建数据表" + CsvID + "失败：" + ex.Message);
                }


            }
        }


        public void AddDataArray(List<FileModel_IntoDB> CsvContent)
        {

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;

                //string lineContent = "Null";
                try
                {
                    foreach (var line in CsvContent)
                    {
                        if (csvTableID.Contains("ID_" + line.stringID))
                        {
                            cmd.CommandText = "INSERT INTO ID_" + line.stringID 
                            + " VALUES(@ID_Type, @ID_Unknown, @ID_Index, @Text_EN, @Text_SC, @isTranslated, @UpdateStats)";
                            cmd.Parameters.AddRange(new[]
                            {
                                new SQLiteParameter("@ID_Type", line.stringID),
                                new SQLiteParameter("@ID_Unknown", line.stringUnknown),
                                new SQLiteParameter("@ID_Index", line.stringIndex),
                                new SQLiteParameter("@Text_SC", line.ZH_text),
                                new SQLiteParameter("@Text_EN", line.EN_text),
                                new SQLiteParameter("@isTranslated", line.Istranslated),
                                new SQLiteParameter("@UpdateStats", line.UpdateStats),
                            });
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("插入了{0}, {1}, {2}", line.stringID, line.stringUnknown, line.stringIndex);
                        }
                        else
                        {
                            CreateTable(line.stringID);

                            cmd.CommandText = "INSERT INTO ID_" + line.stringID
                            + " VALUES(@ID_Type, @ID_Unknown, @ID_Index, @Text_EN, @Text_SC, @isTranslated, @UpdateStats)";
                            cmd.Parameters.AddRange(new[]
                            {
                                new SQLiteParameter("@ID_Type", line.stringID),
                                new SQLiteParameter("@ID_Unknown", line.stringUnknown),
                                new SQLiteParameter("@ID_Index", line.stringIndex),
                                new SQLiteParameter("@Text_SC", line.ZH_text),
                                new SQLiteParameter("@Text_EN", line.EN_text),
                                new SQLiteParameter("@isTranslated", line.Istranslated),
                                new SQLiteParameter("@UpdateStats", line.UpdateStats),
                            });
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("插入了{0}, {1}, {2}", line.stringID, line.stringUnknown, line.stringIndex);
                        }
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception("插入数据：" + CsvContent.Count + "失败：" + ex.Message);
                }
            }
        }


        public List<LangSearchModel> SearchData(string CsvContent, int SearchField, bool SearchAbandonContent)
        {
            var _LangViewData = new List<LangSearchModel>();
            string fieldName;
            string rowStatsInt = " AND RowStats < 30";

            if (SearchAbandonContent)
                rowStatsInt = "";

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;

                switch (SearchField)
                {
                    case 0:
                        fieldName = "ID_Type";
                        break;
                    case 1:
                        fieldName = "Text_EN";
                        break;
                    case 2:
                        fieldName = "Text_SC";
                        break;
                    case 3:
                        fieldName = "isTranslated";
                        break;
                    default:
                        fieldName = "Text_EN";
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
                            _LangViewData.Add(new LangSearchModel
                            {
                                ID_Table = t.ToString(),                   //数据表名
                                ID_Type = sr.GetInt32(0),                  //游戏内文本ID
                                ID_Unknown = sr.GetInt32(1),               //游戏内文本Unknown列
                                ID_Index = sr.GetInt32(2),                 //游戏内文本Index
                                Text_EN = sr.GetString(3),                 //英语原文
                                Text_SC = sr.GetString(4),                 //汉化文本
                                isTranslated = sr.GetInt32(5),              //是否翻译
                                RowStats = sr.GetInt32(6),
                                UpdateStats = sr.GetString(7),
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

        public List<LangSearchModel> FullSearchData(bool SearchAbandonContent)
        {
            var _LangViewData = new List<LangSearchModel>();

            string rowStatsInt = " WHERE RowStats in (0,10,20)";

            if (SearchAbandonContent)
                rowStatsInt = "";


            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
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
                            //Console.WriteLine("查询了{0},{1},{2}", sr.GetInt32(0), sr.GetInt32(2), sr.GetString(5));
                            _LangViewData.Add(new LangSearchModel
                            {
                                //ID_Table = t.ToString(),                   //数据表名
                                //IndexDB = sr.FieldCount,                   //数据表索引列
                                ID_Type = sr.GetInt32(0),                   //游戏内文本ID
                                ID_Unknown = sr.GetInt32(1),               //游戏内文本Unknown列
                                ID_Index = sr.GetInt32(2),                 //游戏内文本Index
                                Text_EN = sr.GetString(3),                 //英语原文
                                Text_SC = sr.GetString(4),                 //汉化文本
                                isTranslated = sr.GetInt32(5),              //是否已翻译
                                UpdateStats = sr.GetString(7),
                            });
                            Console.WriteLine("查询了{0},{1},{2},{3}", sr.GetInt32(0), sr.GetInt32(1), sr.GetInt32(2), sr.GetString(3));
                        }
                        sr.Close();
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("查询数据：失败：" + ex.Message);
                }
                return _LangViewData;
            }

        }

        public List<FileModel_IntoDB> SearchZHbyIndexWithUnknown(List<LangSearchModel> CsvContent)
        {
            var _LangViewData = new List<FileModel_IntoDB>();
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;

                try
                {
                    /*
                    List<string> tableName = new List<string>();   //表名列表

                    cmd.CommandText = "SELECT name FROM sqlite_master WHERE TYPE='table'";   //获得当前所有表名
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {
                        tableName.Add(sr.GetString(0));
                    }
                    sr.Close();
                    */

                    foreach (var data in CsvContent)
                    {
                        cmd.CommandText = "SELECT * FROM ID_" + data.ID_Type 
                            + " WHERE ID_Unknown=" + data.ID_Unknown
                            + " AND ID_Index=" + data.ID_Index;

                        SQLiteDataReader sr = cmd.ExecuteReader();

                        while (sr.Read())
                        {
                            //Console.WriteLine("查询了{0},{1},{2}", sr.GetInt32(0), sr.GetInt32(2), sr.GetString(5));
                            _LangViewData.Add(new FileModel_IntoDB
                            {
                                //ID_Table = t.ToString(),                   //数据表名
                                //IndexDB = sr.FieldCount,                   //数据表索引列
                                stringID = sr.GetInt32(0),                   //游戏内文本ID
                                stringUnknown = sr.GetInt32(1),               //游戏内文本Unknown列
                                stringIndex = sr.GetInt32(2),                 //游戏内文本Index
                                EN_text = sr.GetString(3),                 //英语原文
                                ZH_text = sr.GetString(4),                 //汉化文本
                                Istranslated = sr.GetInt32(5)              //是否已翻译
                            });
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



        public void UpdateDataArrayEN(List<FileModel_Csv> CsvContent)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
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
                        cmd.CommandText = "UPDATE ID_" + ToInt32(line.stringID) 
                            + " SET Text_EN='@Text_EN' WHERE (ID_Index='" + ToInt32(line.stringIndex) + "')";
                        cmd.Parameters.AddRange(new[]
                        {
                            new SQLiteParameter("@Text_EN", line.textContent),
                        });

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("更新了{0}, {1}, {2}", line.stringID, line.stringUnknown, line.stringIndex);
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception("插入数据：" + CsvContent.Count + "失败：" + ex.Message);
                }

            }

        }

        public void UpdateDataArrayFromCompare(List<FileModel_IntoDB> CsvContent)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
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
                        cmd.CommandText = "UPDATE ID_" + ToInt32(line.stringID) 
                            + " SET Text_EN=@Text_EN,Text_SC=@Text_SC,isTranslated=@isTranslated "
                            + "WHERE (ID_Unknown='" + ToInt32(line.stringUnknown)              //Unknown + Index 才是唯一，只有Index会数据污染。
                            + "'AND ID_Index='" + ToInt32(line.stringIndex) + "')";
                        cmd.Parameters.AddRange(new[]
                        {
                            new SQLiteParameter("@Text_EN", line.EN_text),
                            new SQLiteParameter("@Text_SC", line.ZH_text),
                            new SQLiteParameter("@isTranslated", line.Istranslated),
                        });

                        cmd.ExecuteNonQuery();
                        //lineContent = line.stringID.ToString() + line.stringUnknow.ToString() + line.stringIndex.ToString();
                        Console.WriteLine("更新了{0}, {1}, {2}", line.stringID, line.stringUnknown, line.stringIndex);
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception("插入数据：" + CsvContent.Count + "失败：" + ex.Message);
                }

            }

        }

        /// <summary>
        /// 删除库内数据，条件：
        /// stringID 为表名
        /// stringUnknown 与 stringIndex 为条件
        /// </summary>
        /// <param name="CsvContent"></param>
        public void DeleteDataList(List<FileModel_IntoDB> CsvContent)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
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
                        cmd.CommandText = "UPDATE FROM ID_" + ToInt32(line.stringID)
                            + " WHERE (ID_Unknown='" + ToInt32(line.stringUnknown)              //Unknown + Index 才是唯一，只有Index会数据污染。
                            + "'AND ID_Index='" + ToInt32(line.stringIndex) + "')";

                        cmd.ExecuteNonQuery();
                        //lineContent = line.stringID.ToString() + line.stringUnknow.ToString() + line.stringIndex.ToString();
                        Console.WriteLine("删除了{0}, {1}, {2}", line.stringID, line.stringUnknown, line.stringIndex);
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception("删除数据：" + CsvContent.Count + "失败：" + ex.Message);
                }

            }
        }

        /// <summary>
        /// 新增数据，条件：
        /// stringID 为表名
        /// stringUnknown 与 stringIndex 为条件
        /// 
        /// RowStats 当前行状态
        /// UpdateStats 哪个版本做出的修改
        /// </summary>
        /// <param name="CsvContent"></param>
        public void AddDataList(List<FileModel_IntoDB> CsvContent)
        {
            List<string> csvTableID = new List<string>();

            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;

                cmd.CommandText = "SELECT name FROM sqlite_master WHERE TYPE='table'";   //获得当前所有表名
                SQLiteDataReader sr = cmd.ExecuteReader();

                while (sr.Read())
                {
                    csvTableID.Add(sr.GetString(0));
                }
                sr.Close();

                try
                {
                    foreach (var line in CsvContent)
                    {
                        if (csvTableID.Contains("ID_" + line.stringID))
                        {
                            cmd.CommandText = "INSERT INTO ID_" + line.stringID
                            + " VALUES(@ID_Type, @ID_Unknown, @ID_Index, @Text_EN, @Text_SC, @isTranslated, @RowStats, @UpdateStats)";
                            cmd.Parameters.AddRange(new[]
                            {
                                new SQLiteParameter("@ID_Type", line.stringID),
                                new SQLiteParameter("@ID_Unknown", line.stringUnknown),
                                new SQLiteParameter("@ID_Index", line.stringIndex),
                                new SQLiteParameter("@Text_SC", line.ZH_text),
                                new SQLiteParameter("@Text_EN", line.EN_text),
                                new SQLiteParameter("@isTranslated", line.Istranslated),
                                new SQLiteParameter("@RowStats", 10),            //新插入的内容状态一律为10
                                new SQLiteParameter("@UpdateStats", line.UpdateStats),
                            });

                            cmd.ExecuteNonQuery();
                            Console.WriteLine("插入了{0}, {1}, {2}", line.stringID, line.stringUnknown, line.stringIndex);
                        }
                        else
                        {
                            cmd.CommandText = "CREATE TABLE ID_" + line.stringID + "(ID_Type int, ID_Unknown int, ID_Index int, Text_EN text, Text_SC text, isTranslated int, RowStats int, UpdateStats text)";
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("创建了 ID_{0} 表", line.stringID);
                            csvTableID.Add("ID_" + line.stringID);

                            cmd.CommandText = "INSERT INTO ID_" + line.stringID
                            + " VALUES(@ID_Type, @ID_Unknown, @ID_Index, @Text_EN, @Text_SC, @isTranslated, @RowStats, @UpdateStats)";
                            cmd.Parameters.AddRange(new[]
                            {
                                new SQLiteParameter("@ID_Type", line.stringID),
                                new SQLiteParameter("@ID_Unknown", line.stringUnknown),
                                new SQLiteParameter("@ID_Index", line.stringIndex),
                                new SQLiteParameter("@Text_SC", line.ZH_text),
                                new SQLiteParameter("@Text_EN", line.EN_text),
                                new SQLiteParameter("@isTranslated", line.Istranslated),
                                new SQLiteParameter("@RowStats", 10),            //新插入的内容状态一律为10
                                new SQLiteParameter("@UpdateStats", line.UpdateStats),
                            });
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("插入了{0}, {1}, {2}", line.stringID, line.stringUnknown, line.stringIndex);
                        }

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
        /// 标记修改数据，条件：
        /// stringID 为表名
        /// stringUnknown 与 stringIndex 为条件
        /// 
        /// RowStats 当前行状态
        /// UpdateStats 哪个版本做出的修改
        /// </summary>
        /// <param name="CsvContent"></param>
        public void MarkChangedDataList(List<FileModel_IntoDB> CsvContent)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
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
                        cmd.CommandText = "UPDATE ID_" + ToInt32(line.stringID)
                            + " SET RowStats=@RowStats"
                            + " WHERE (ID_Unknown='" + ToInt32(line.stringUnknown)              //Unknown + Index 才是唯一，只有Index会数据污染。
                            + "'AND ID_Index='" + ToInt32(line.stringIndex) + "')";
                        cmd.Parameters.AddRange(new[]
                        {
                            new SQLiteParameter("@RowStats", 40),          //修改的内容一律为40
                            //new SQLiteParameter("@UpdateStats", line.UpdateStats),
                        });

                        cmd.ExecuteNonQuery();
                        //lineContent = line.stringID.ToString() + line.stringUnknow.ToString() + line.stringIndex.ToString();
                        Console.WriteLine("标记修改{0}, {1}, {2}", line.stringID, line.stringUnknown, line.stringIndex);



                        //插入修改的内容
                        cmd.CommandText = "INSERT INTO ID_" + line.stringID
                            + " VALUES(@ID_Type, @ID_Unknown, @ID_Index, @Text_EN, @Text_SC, @isTranslated, @RowStats, @UpdateStats)";
                        cmd.Parameters.AddRange(new[]
                        {
                            new SQLiteParameter("@ID_Type", line.stringID),
                            new SQLiteParameter("@ID_Unknown", line.stringUnknown),
                            new SQLiteParameter("@ID_Index", line.stringIndex),
                            new SQLiteParameter("@Text_SC", line.ZH_text),
                            new SQLiteParameter("@Text_EN", line.EN_text),
                            new SQLiteParameter("@isTranslated", line.Istranslated),
                            new SQLiteParameter("@RowStats", 20),            //新插入的修改内容状态一律为20
                            new SQLiteParameter("@UpdateStats", line.UpdateStats),
                        });

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("插入了{0}, {1}, {2}", line.stringID, line.stringUnknown, line.stringIndex);

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
        public void MarkDeleteDataList(List<FileModel_IntoDB> CsvContent)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
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
                        cmd.CommandText = "UPDATE ID_" + ToInt32(line.stringID)
                            + " SET RowStats=@RowStats,UpdateStats=@UpdateStats"
                            + " WHERE (ID_Unknown='" + ToInt32(line.stringUnknown)              //Unknown + Index 才是唯一，只有Index会数据污染。
                            + "'AND ID_Index='" + ToInt32(line.stringIndex) + "')";
                        cmd.Parameters.AddRange(new[]
                        {
                            new SQLiteParameter("@RowStats", 30),                   //标记删除的内容一律为30
                            new SQLiteParameter("@UpdateStats", line.UpdateStats),
                        });

                        cmd.ExecuteNonQuery();
                        //lineContent = line.stringID.ToString() + line.stringUnknow.ToString() + line.stringIndex.ToString();
                        Console.WriteLine("标记删除{0}, {1}, {2}", line.stringID, line.stringUnknown, line.stringIndex);
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception("标记删除：" + CsvContent.Count + "失败：" + ex.Message);
                }

            }
        }





        #region 增加字段，初始内容类型为 int 的重载
        public void FieldAdd(string FieldName, string FieldType, int InitContent)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;

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
                    
                    foreach (var table in tableName)
                    {

                        //添加字段
                        cmd.CommandText = "ALTER TABLE " + table
                                + " ADD COLUMN " + FieldName + " " + FieldType;

                        cmd.ExecuteNonQuery();

                        Console.WriteLine("表：{0}, 字段名：{1}, 字段类型：{2}", table, FieldName, FieldType);
                        
                        //初始化字段内容
                        cmd.CommandText = "UPDATE " + table
                            + " SET " + FieldName + "=@FieldName";
                        cmd.Parameters.AddRange(new[]
                        {
                            new SQLiteParameter("@FieldName", InitContent),
                        });

                        cmd.ExecuteNonQuery();

                        Console.WriteLine("表：{0}, 字段名：{1}, 初始内容：{2}", table, FieldName, InitContent);
                    }
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
        public void FieldAdd(string FieldName, string FieldType, string InitContent)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;

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

                    foreach (var table in tableName)
                    {

                        //添加字段
                        cmd.CommandText = "ALTER TABLE " + table
                                + " ADD COLUMN " + FieldName + " " + FieldType;

                        cmd.ExecuteNonQuery();

                        Console.WriteLine("表：{0}, 字段名：{1}, 字段类型：{2}", table, FieldName, FieldType);

                        //初始化字段内容
                        cmd.CommandText = "UPDATE " + table
                            + " SET " + FieldName + "=@FieldName";
                        cmd.Parameters.AddRange(new[]
                        {
                            new SQLiteParameter("@FieldName", InitContent),
                        });

                        cmd.ExecuteNonQuery();

                        Console.WriteLine("表：{0}, 字段名：{1}, 初始内容：{2}", table, FieldName, InitContent);
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception("增加字段失败：" + ex.Message);
                }

            }
        }
        #endregion

        public string UpdateDataFromEditor(LangSearchModel CsvContent)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;

                try
                {
                    cmd.CommandText = "UPDATE " + CsvContent.ID_Table 
                        + " SET Text_SC=@Text_SC,isTranslated=@isTranslated"
                        + " WHERE (ID_Unknown='" + ToInt32(CsvContent.ID_Unknown)       //Unknown + Index 才是唯一，只有Index会数据污染。
                        + "'AND ID_Index='" + ToInt32(CsvContent.ID_Index) + "')";

                    cmd.Parameters.AddRange(new[]
                    {
                        new SQLiteParameter("@Text_SC", CsvContent.Text_SC),
                        new SQLiteParameter("@isTranslated", CsvContent.isTranslated)

                    });
                    cmd.ExecuteNonQuery();
                    return CsvContent.Text_SC + " 更新成功！";

                }
                catch (Exception ex)
                {
                    return "插入数据：" + CsvContent.Text_SC + " 失败：" + ex.Message;
                    throw new Exception("插入数据：" + CsvContent.Text_SC + "失败：" + ex.Message);
                }

            }

        }

        public bool UpdateTextScFromImportDB(List<LangSearchModel> CsvContent)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + csvDataPath + ";Version=3;"))
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
                        cmd.CommandText = "UPDATE ID_" + ToInt32(line.ID_Type) 
                            + " SET Text_SC=@Text_SC,isTranslated=@isTranslated"
                            + " WHERE (ID_Unknown='" + ToInt32(line.ID_Unknown)              //Unknown + Index 才是唯一，只有Index会数据污染。
                            + "'AND ID_Index='" + ToInt32(line.ID_Index) + "')";
                        cmd.Parameters.AddRange(new[]
                        {
                            //new SQLiteParameter("@Text_EN", line.EN_text),
                            new SQLiteParameter("@Text_SC", line.Text_SC),
                            new SQLiteParameter("@isTranslated", line.isTranslated),
                        });

                        cmd.ExecuteNonQuery();
                        //lineContent = line.stringID.ToString() + line.stringUnknow.ToString() + line.stringIndex.ToString();
                        Console.WriteLine("更新了{0}, {1}, {2}", line.ID_Table, line.ID_Unknown, line.ID_Index);
                    }
                    tx.Commit();
                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                    throw new Exception("更新数据失败：" + ex.Message);
                }

            }

        }

        public bool CheckTableIfExist(string tableName, string TranselatedDBPath)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + TranselatedDBPath + ";Version=3;"))
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

        public void CreateTableToTranselateDB(string TableName, string TranselatedDBPath)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + TranselatedDBPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                try
                {
                    cmd.CommandText = "CREATE TABLE " + TableName + "(ID_Type int, ID_Unknown int, ID_Index int, Text_EN text, Text_SC text, isTranslated int)";
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw new Exception("创建数据表" + TableName + "失败：" + ex.Message);
                }
            }
        }

        public void CreateTranslateDBwithData(List<LangSearchModel> CsvContent, string TranselatedDBPath)
        {
            if (!File.Exists(TranselatedDBPath))
            {
                SQLiteConnection.CreateFile(TranselatedDBPath);
            }
            /*
            try
            {
                Conn = new SQLiteConnection("Data Source=" + TranselatedDBPath + ";Version=3;");
                Conn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("打开数据库：" + TranselatedDBPath + "的连接失败：" + ex.Message);
            }
            */

            foreach (var table in CsvContent)
            {
                if (!CheckTableIfExist(table.ID_Table, TranselatedDBPath))
                {
                    CreateTableToTranselateDB(table.ID_Table, TranselatedDBPath);
                }

            }


            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + TranselatedDBPath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                SQLiteTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;

                try
                {
                    foreach (var content in CsvContent)
                    {
                        cmd.CommandText = "SELECT * FROM " + content.ID_Table
                        + " WHERE (ID_Unknown='" + ToInt32(content.ID_Unknown)              //Unknown + Index 才是唯一，只有Index会数据污染。
                        + "'AND ID_Index='" + ToInt32(content.ID_Index) + "')";
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 0)
                        {
                            cmd.CommandText = "INSERT INTO " + content.ID_Table + " VALUES(@ID_Type, @ID_Unknown, @ID_Index, @Text_EN, @Text_SC, @isTranslated)";
                            cmd.Parameters.AddRange(new[]
                            {
                            new SQLiteParameter("@ID_Type", content.ID_Type),
                            new SQLiteParameter("@ID_Unknown", content.ID_Unknown),
                            new SQLiteParameter("@ID_Index", content.ID_Index),
                            new SQLiteParameter("@Text_EN", content.Text_EN),
                            new SQLiteParameter("@Text_SC", content.Text_SC),
                            new SQLiteParameter("@isTranslated", content.isTranslated),
                            });
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd.CommandText = "UPDATE " + content.ID_Table + " SET Text_SC=@Text_SC"
                            + " WHERE (ID_Unknown='" + ToInt32(content.ID_Unknown)              //Unknown + Index 才是唯一，只有Index会数据污染。
                            + "'AND ID_Index='" + ToInt32(content.ID_Index) + "')";

                            cmd.Parameters.Add(new SQLiteParameter("@Text_SC", content.Text_SC));
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



        public List<LangSearchModel> FullSearchTranslateDB(string dbFile)
        {
            var _LangViewData = new List<LangSearchModel>();
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + dbFile + ";Version=3;"))
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
                        cmd.CommandText = "SELECT * FROM " + t;
                        sr = cmd.ExecuteReader();

                        while (sr.Read())
                        {
                            _LangViewData.Add(new LangSearchModel
                            {
                                //ID_Table = t.ToString(),                   //数据表名
                                //IndexDB = sr.FieldCount,                   //数据表索引列
                                ID_Type = sr.GetInt32(0),                   //游戏内文本ID
                                ID_Unknown = sr.GetInt32(1),               //游戏内文本Unknown列
                                ID_Index = sr.GetInt32(2),                 //游戏内文本Index
                                Text_EN = sr.GetString(3),                 //英语原文
                                Text_SC = sr.GetString(4)                  //汉化文本

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

    }
}
