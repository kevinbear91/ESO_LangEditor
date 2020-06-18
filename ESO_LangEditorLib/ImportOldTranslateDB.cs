using ESO_LangEditorLib.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorLib
{
    public class ImportOldTranslateDB
    {

        public List<LangText> FullSearchData(string dbPath)
        {
            var _LangViewData = new List<LangText>();

            //string rowStatsInt = " WHERE RowStats in (0,10,20)";

            //if (SearchAbandonContent)
            //    rowStatsInt = "";


            using (SqliteConnection conn = new SqliteConnection("Data Source=" + dbPath))
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;

                try
                {
                    List<string> tableName = new List<string>();   //表名列表

                    cmd.CommandText = "SELECT name FROM sqlite_master WHERE TYPE='table'";   //获得当前所有表名
                    SqliteDataReader sr = cmd.ExecuteReader();
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
                            //string uniqueID;
                            int id = sr.GetInt32(0);
                            int unknown = sr.GetInt32(1);
                            int index = sr.GetInt32(2);
                            string text_en = sr.GetString(3);
                            string text_zh = sr.GetString(4);
                            int istranslated = sr.GetInt32(5);
                            //string updatestats = sr.GetString(7);
                            //int rowStats;//  = sr.GetInt32(6);


                            //Console.WriteLine("查询了{0},{1},{2}", sr.GetInt32(0), sr.GetInt32(2), sr.GetString(5));
                            _LangViewData.Add(new LangText
                            {
                                UniqueID = id + "-" + unknown + "-" + index,
                                ID = id,                   //游戏内文本ID
                                Unknown = unknown,               //游戏内文本Unknown列
                                Lang_Index = index,                 //游戏内文本Index
                                Text_EN = text_en,                 //英语原文
                                Text_ZH = text_zh,                 //汉化文本
                                IsTranslated = istranslated,              //是否已翻译
                                //UpdateStats = updatestats,
                                //RowStats = rowStats,
                            });
                            Console.WriteLine("查询了{0},{1},{2}", id + "-" + unknown + "-" + index, text_en, text_zh);
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

        public List<LuaUIData> FullSearchStrDB(string LuaStrpath)
        {
            var _LangViewData = new List<LuaUIData>();

            //string rowStatsInt = " WHERE RowStats < 30";

            //if (SearchAbandonContent)
            //    rowStatsInt = "";

            using (SqliteConnection conn = new SqliteConnection("Data Source=" + LuaStrpath))
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;

                try
                {
                    List<string> tableName = new List<string>();   //表名列表

                    cmd.CommandText = "SELECT name FROM sqlite_master WHERE TYPE='table'";   //获得当前所有表名
                    SqliteDataReader sr = cmd.ExecuteReader();
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
                            _LangViewData.Add(new LuaUIData
                            {
                                
                                //IndexDB = sr.FieldCount,                   
                                UniqueID = sr.GetString(0),
                                Text_EN = sr.GetString(1),
                                Text_ZH = sr.GetString(2),
                                RowStats = sr.GetInt32(4),
                                IsTranslated = sr.GetInt32(5),
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
        public List<LuaUIData> FullSearchStrDB(string LuaStrpath, string tableName)
        {
            var _LangViewData = new List<LuaUIData>();

            //string rowStatsInt = " WHERE RowStats < 30";

            //if (SearchAbandonContent)
            //    rowStatsInt = "";

            using (SqliteConnection conn = new SqliteConnection("Data Source=" + LuaStrpath))
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand();
                cmd.Connection = conn;

                try
                {
                    cmd.CommandText = "SELECT * FROM " + tableName;

                    SqliteDataReader sr = cmd.ExecuteReader();

                    while (sr.Read())
                    {
                        _LangViewData.Add(new LuaUIData
                        {

                            //IndexDB = sr.FieldCount,                   
                            UniqueID = sr.GetString(0),
                            Text_EN = sr.GetString(1),
                            Text_ZH = sr.GetString(2),
                            RowStats = sr.GetInt32(4),
                            IsTranslated = sr.GetInt32(5),
                            UpdateStats = sr.GetString(6),

                        });
                        //Console.WriteLine("查询了{0}, {1}, {2}", sr.GetInt32(0).ToString(), sr.GetInt32(1), sr.GetString(4));
                    }
                    sr.Close();
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
