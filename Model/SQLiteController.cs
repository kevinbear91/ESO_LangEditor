using System;
using System.IO;
using System.Collections.Generic;
using static System.Convert;
using System.Data.SQLite;
using ESO_Lang_Editor.View;

namespace ESO_Lang_Editor.Model
{
    class SQLiteController
    {
        SQLiteConnection Conn;
        string FilePath = @"..\..\Data\CsvTest.db";

        public void ConnectSQLite()
        {
            //SQLiteConnection Conn;

            if (!File.Exists(FilePath))
            {
                SQLiteConnection.CreateFile(FilePath);
            }
            try
            {
                Conn = new SQLiteConnection("Data Source=" + FilePath + ";Version=3;");
                Conn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("打开数据库：" + FilePath + "的连接失败：" + ex.Message);
            }
        }

        public void CreateTable(int CsvID)
        {
            try
            {
                string sql = "CREATE TABLE ID_" + CsvID + "(ID_Type int, ID_Unknown int, ID_Index int, ID_Offset int, Text_EN text, Text_SC text, Text_JF text)";
                SQLiteCommand command = new SQLiteCommand(sql, Conn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("创建数据表" + CsvID + "失败：" + ex.Message);
            }

        }

        public void CreateTableArray(List<int> CsvID)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + FilePath + ";Version=3;"))
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
                        cmd.CommandText = "CREATE TABLE ID_" + id + "(ID_Type int, ID_Unknown int, ID_Index int, ID_Offset int, Text_EN text, Text_SC text)";
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("创建了{0}表", id);
                    }
                    tx.Commit();
                    //string sql = "CREATE TABLE ID_" + CsvID + "(ID_Type int, ID_Unknown int, ID_Index int, ID_Offset int, Text_EN text, Text_SC text, Text_JF text)";
                    //SQLiteCommand command = new SQLiteCommand(sql, Conn);
                    //command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("创建数据表" + CsvID + "失败：" + ex.Message);
                }


            }
        }


        public void AddDataArray(Dictionary<string, FileModel_IntoDB> CsvContent)
        {
            
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + FilePath + ";Version=3;"))
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
                        //cmd.CommandText = "INSERT INTO ID_" + ToInt32(line.stringID) +
                        //    "(ID_Type, ID_Index, ID_Unknown, ID_Offset, Text_EN, Text_SC, Text_JF) " +
                        //    "VALUES ('" + ToInt32(line.stringID) + "', " + ToInt32(line.stringIndex) + "', " + ToInt32(line.stringUnknow) + "', " + ToInt32(line.stringOffset) + "', " + line.textContent + "', " + Text_SC + "', " + Text_JF + ")";

                        cmd.CommandText = "INSERT INTO ID_" + line.Value.stringID + " VALUES(@ID_Type, @ID_Unknown, @ID_Index, @ID_Offset, @Text_EN, @Text_SC)";
                        cmd.Parameters.AddRange(new[]
                        {
                            new SQLiteParameter("@ID_Type", line.Value.stringID),
                            new SQLiteParameter("@ID_Unknown", line.Value.stringUnknow),
                            new SQLiteParameter("@ID_Index", line.Value.stringIndex),
                            new SQLiteParameter("@ID_Offset", line.Value.stringOffset),
                            new SQLiteParameter("@Text_SC", line.Value.ZH_text),
                            new SQLiteParameter("@Text_EN", line.Value.EN_text),
                        });

                        cmd.ExecuteNonQuery();
                        //lineContent = line.stringID.ToString() + line.stringUnknow.ToString() + line.stringIndex.ToString();
                        Console.WriteLine("插入了{0}, {1}, {2}", line.Value.stringID, line.Value.stringUnknow, line.Value.stringIndex);
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception("插入数据：" + CsvContent.Count + "失败：" + ex.Message);
                }
            }
        }


        public List<LangSearchModel> SearchData(string CsvContent)
        {
            var _LangViewData = new List<LangSearchModel>();
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + FilePath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                //SQLiteTransaction tx = conn.BeginTransaction();
                //cmd.Transaction = tx;

                //string lineContent = "Null";
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
                        cmd.CommandText = "SELECT * FROM " + t + " WHERE Text_EN LIKE @SEARCH";
                        cmd.Parameters.AddWithValue("@SEARCH", CsvContent);     //遍历全库查询要搜索在任意位置的文本
                        sr = cmd.ExecuteReader();

                        while (sr.Read())
                        {
                            //Console.WriteLine("查询了{0},{1},{2}", sr.GetInt32(0), sr.GetInt32(2), sr.GetString(5));
                            _LangViewData.Add(new LangSearchModel {
                                ID_Table = t.ToString(),                   //数据表名
                                //IndexDB = sr.FieldCount,                   //数据表索引列
                                ID_Type = sr.GetInt32(0).ToString(),       //游戏内文本ID
                                ID_Unknown = sr.GetInt32(1),               //游戏内文本Unknown列
                                ID_Index = sr.GetInt32(2),                 //游戏内文本Index
                                Text_EN = sr.GetString(4),                 //英语原文
                                Text_SC = sr.GetString(5)                  //汉化文本
                            });
                        }
                        sr.Close();
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception("查询数据：" + CsvContent + "失败：" + ex.Message);
                }
                return _LangViewData;
            }
            
        }



        public void UpdateDataArrayEN(List<FileModel_Csv> CsvContent)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + FilePath + ";Version=3;"))
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
                        cmd.CommandText = "UPDATE ID_" + ToInt32(line.stringID) + " SET Text_EN='@Text_EN' WHERE (ID_Index='" + ToInt32(line.stringIndex) + "')";
                        cmd.Parameters.AddRange(new[]
                        {
                            new SQLiteParameter("@Text_EN", line.textContent),
                        });

                        cmd.ExecuteNonQuery();
                        //lineContent = line.stringID.ToString() + line.stringUnknow.ToString() + line.stringIndex.ToString();
                        Console.WriteLine("更新了{0}, {1}, {2}", line.stringID, line.stringUnknow, line.stringIndex);
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    throw new Exception("插入数据：" + CsvContent.Count + "失败：" + ex.Message);
                }

            }

        }

        public string UpdateDataFromEditor(LangSearchModel CsvContent)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + FilePath + ";Version=3;"))
            {
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;

                try
                {
                    cmd.CommandText = "UPDATE " + CsvContent.ID_Table + " SET Text_SC=@Text_SC" 
                        + " WHERE (ID_Unknown='" + ToInt32(CsvContent.ID_Unknown)              //Unknown + Index 才是唯一，只有Index会数据污染。
                        + "'AND ID_Index='" + ToInt32(CsvContent.ID_Index) + "')";

                    cmd.Parameters.Add(new SQLiteParameter("@Text_SC", CsvContent.Text_SC));
                    cmd.ExecuteNonQuery();
                    return CsvContent.Text_SC + " 更新成功！";

                        //lineContent = line.stringID.ToString() + line.stringUnknow.ToString() + line.stringIndex.ToString();
                        //Console.WriteLine("更新了{0}, {1}, {2}", line.stringID, line.stringUnknow, line.stringIndex);

                }
                catch (Exception ex)
                {
                    return "插入数据：" + CsvContent.Text_SC + " 失败：" + ex.Message;
                    throw new Exception("插入数据：" + CsvContent.Text_SC + "失败：" + ex.Message);
                }

            }

        }


        public void AddData(int CsvID, int ID, int Unknown, int Offset, string Text_EN, string Text_SC, string Text_JF)
        {

            /*
            try
            {
                string sql = "insert into " + CsvID + " (ID_Type, ID_Unknown, ID_Index, ID_Offset, Text_EN, Text_SC, Text_JF) values ('" + ID + "', " + Unknown + "', " + Offset + "', " + Text_EN + "', " + Text_SC + "', " + Text_JF + ")";
                SQLiteCommand command = new SQLiteCommand(sql, Conn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("插入数据：" + CsvID + ":" + ID + "," + Unknown + "," + Offset + "," + Text_EN + "," + Text_SC + "," + Text_JF + "失败：" + ex.Message);
            }
            */

        }
    }
}
