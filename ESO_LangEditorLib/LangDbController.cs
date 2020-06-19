using ESO_LangEditorLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;

namespace ESO_LangEditorLib
{
    public class LangDbController
    {

        public void InsertDataFromCsv(List<LangText> data)
        {
            using (var DbContext = new LangDbContext())
            {

                DbContext.LangData.AddRange(data);
                DbContext.SaveChanges();
            }
        }

        public void InsertDataFromold(List<LuaUIData> data)
        {
            using (var DbContext = new LangDbContext())
            {
                DbContext.LuaLang.AddRange(data);
                DbContext.SaveChanges();
            }
        }

        public async Task<int> UpdateOrInsertLua(List<LuaUIData> luaList)
        {
            using var Db = new LangDbContext();

            foreach (var l in luaList)
            {
                if (Db.LuaLang.Where(d => d.UniqueID.Contains(l.UniqueID)).Count() == 1)
                {
                    if (l.DataEnum == 2)
                    {
                        Db.Attach(l);
                        Db.Entry(l).Property("DataEnum").CurrentValue = 3;
                        //Db.Entry(l).Property("RowStats").IsModified = true;
                    }
                    else
                    {
                        Db.Add(l);
                    }

                }
                else
                {
                    Db.Attach(l);
                    Db.Entry(l).Property("DataEnum").CurrentValue = 1;
                }
            }

            return await Db.SaveChangesAsync();
        }

        /// <summary>
        /// 搜索 lang_data 文本数据表，返回搜索到的内容
        /// <para>@field 搜索字段索引，0为ID(int)，1为英文，2为中文，3为更新版本，4为行状态(int)，5为翻译状态(int)</para>
        /// <para>@searchPos 搜索文本出现的位置，0为关键字在任意位置，1为关键字仅在开头，2为关键字仅在末尾。</para>
        /// <para>@searchWord 关键字。</para>
        /// </summary>
        /// <param name="field">搜索字段</param>
        /// <param name="searchPos">搜索位置</param>
        /// <param name="searchWord">搜索文本</param>
        /// <returns></returns>
        public async Task<List<LangText>> GetLangsListAsync(int field, int searchPos, string searchWord)
        {
            List<LangText> data = new List<LangText>();

            string searchPosAndWord = searchPos switch  //设定关键字出现的位置
            {
                0 => "%" + searchWord + "%",     //任意位置
                1 => searchWord + "%",           //仅在开头
                2 => "%" + searchWord,           //仅在末尾
                _ => "%" + searchWord + "%",     //默认 - 任意位置
            };

            using (var Db = new LangDbContext())
            {
                data = field switch
                {
                    0 => await Db.LangData.Where(d => d.ID == ToInt32(searchWord)).ToListAsync(),
                    1 => await Db.LangData.Where(d => EF.Functions.Like(d.Text_EN, searchPosAndWord)).ToListAsync(),
                    2 => await Db.LangData.Where(d => EF.Functions.Like(d.Text_ZH, searchPosAndWord)).ToListAsync(),
                    3 => await Db.LangData.Where(d => EF.Functions.Like(d.UpdateStats, searchPosAndWord)).ToListAsync(),
                    4 => await Db.LangData.Where(d => d.RowStats == ToInt32(searchWord)).ToListAsync(),
                    5 => await Db.LangData.Where(d => d.IsTranslated == ToInt32(searchWord)).ToListAsync(),
                    _ => await Db.LangData.Where(d => EF.Functions.Like(d.Text_EN, searchPosAndWord)).ToListAsync(),
                };
                //await Db.langData.Where(d => EF.Functions.Like(d.UpdateStats, searchPosAndWord)).ToDictionaryAsync(d => d.UniqueID),
                //data = await q.ToDictionaryAsync(q => q.UniqueID);
            }

            return data;
        }

        public async Task<List<LuaUIData>> GetLuaLangsListAsync(int field, int searchPos, string searchWord)
        {
            List<LuaUIData> data = new List<LuaUIData>();

            string searchPosAndWord = searchPos switch  //设定关键字出现的位置
            {
                0 => "%" + searchWord + "%",     //任意位置
                1 => searchWord + "%",           //仅在开头
                2 => "%" + searchWord,           //仅在末尾
                _ => "%" + searchWord + "%",     //默认 - 任意位置
            };

            using (var Db = new LangDbContext())
            {
                data = field switch
                {
                    0 => await Db.LuaLang.Where(d => d.UniqueID == searchWord).ToListAsync(),
                    1 => await Db.LuaLang.Where(d => EF.Functions.Like(d.Text_EN, searchPosAndWord)).ToListAsync(),
                    2 => await Db.LuaLang.Where(d => EF.Functions.Like(d.Text_ZH, searchPosAndWord)).ToListAsync(),
                    3 => await Db.LuaLang.Where(d => EF.Functions.Like(d.UpdateStats, searchPosAndWord)).ToListAsync(),
                    4 => await Db.LuaLang.Where(d => d.RowStats == ToInt32(searchWord)).ToListAsync(),
                    5 => await Db.LuaLang.Where(d => d.IsTranslated == ToInt32(searchWord)).ToListAsync(),
                    _ => await Db.LuaLang.Where(d => EF.Functions.Like(d.Text_EN, searchPosAndWord)).ToListAsync(),
                };
                //await Db.langData.Where(d => EF.Functions.Like(d.UpdateStats, searchPosAndWord)).ToDictionaryAsync(d => d.UniqueID),
                //data = await q.ToDictionaryAsync(q => q.UniqueID);
            }

            return data;
        }

        public async Task<List<LangText>> GetAllLangsListAsync()
        {
            List<LangText> data = new List<LangText>();

            using (var Db = new LangDbContext())
            {
                data = await Db.LangData.ToListAsync();

                //data = q.ToDictionary(q => q.UniqueID);
            }
            return data;
        }


        public async Task<Dictionary<string, LangText>> GetAllLangsDictionaryAsync()
        {
            Dictionary<string, LangText> data = new Dictionary<string, LangText>();

            using (var Db = new LangDbContext())
            {
                data = await Db.LangData.ToDictionaryAsync(d => d.UniqueID);

                //data = q.ToDictionary(q => q.UniqueID);
            }
            return data;
        }

        public async Task<List<LuaUIData>> GetAllLuaLangsListAsync()
        {
            List<LuaUIData> data = new List<LuaUIData>();

            using (var Db = new LangDbContext())
            {
                data = await Db.LuaLang.ToListAsync();

                //data = q.ToDictionary(q => q.UniqueID);
            }
            return data;
        }

        public async Task<Dictionary<string, LuaUIData>> GetAllLuaLangsDictionaryAsync()
        {
            Dictionary<string, LuaUIData> data = new Dictionary<string, LuaUIData>();

            using (var Db = new LangDbContext())
            {
                data = await Db.LuaLang.ToDictionaryAsync(d => d.UniqueID);

                //data = q.ToDictionary(q => q.UniqueID);
            }
            return data;
        }



        public async Task AddNewLangs(List<LangText> langList)
        {
            using var Db = new LangDbContext();
            Db.AddRange(langList);
            await Db.SaveChangesAsync();
        }

        public async Task AddNewLangs(List<LuaUIData> langList)
        {
            using var Db = new LangDbContext();
            Db.AddRange(langList);
            await Db.SaveChangesAsync();
        }

        public async Task UpdateLangsEN(List<LangText> langList)
        {
            using var Db = new LangDbContext();

            //int i = 0;
            foreach (var l in langList)    //EF core 不支持List批量标记，但循环速度不差，2700条执行时间基本在1秒左右。
            {
                //i += 1;
                Db.Attach(l);

                Db.Entry(l).Property("Text_EN").IsModified = true;
                Db.Entry(l).Property("UpdateStats").IsModified = true;
                Db.Entry(l).Property("IsTranslated").IsModified = true;
                Db.Entry(l).Property("RowStats").IsModified = true;

                //Debug.WriteLine("UniqueID: " + l.UniqueID + ", "
                //    + "Text_EN: " + l.Text_EN + ", "
                //    + "UpdateStats: " + l.UpdateStats + ", "
                //    + "Counter: " + i);
            }

            //Db.AttachRange(langList);
            //Db.Entry(langList).Property("Text_EN").IsModified = true;
            ////Db.Entry(langList).Property("Text_ZH").IsModified = true;
            //Db.Entry(langList).Property("UpdateStats").IsModified = true;
            //Db.Entry(langList).Property("IsTranslated").IsModified = true;
            //Db.Entry(langList).Property("RowStats").IsModified = true;

            //Db.UpdateRange(langList);
            await Db.SaveChangesAsync();
        }

        public async Task UpdateLangsEN(List<LuaUIData> langList)
        {
            using var Db = new LangDbContext();

            foreach (var l in langList)    //EF core 不支持List批量标记，但循环速度不差，2700条执行时间基本在1秒左右。
            {
                Db.Attach(l);

                Db.Entry(l).Property("Text_EN").IsModified = true;
                Db.Entry(l).Property("UpdateStats").IsModified = true;
                Db.Entry(l).Property("IsTranslated").IsModified = true;
                Db.Entry(l).Property("RowStats").IsModified = true;

            }

            await Db.SaveChangesAsync();
        }

        public async Task DeleteLangs(List<LangText> langList)
        {
            using var Db = new LangDbContext();
            Db.RemoveRange(langList);
            await Db.SaveChangesAsync();
        }

        public async Task DeleteLangs(List<LuaUIData> langList)
        {
            using var Db = new LangDbContext();
            Db.RemoveRange(langList);
            await Db.SaveChangesAsync();
        }


        public async Task UpdateLangsZH(LangText langList)
        {
            using var Db = new LangDbContext();
            Db.Attach(langList);
            Db.Entry(langList).Property("Text_ZH").IsModified = true;
            Db.Entry(langList).Property("IsTranslated").IsModified = true;
            Db.Entry(langList).Property("RowStats").IsModified = true;

            //Db.Update(langList);
            await Db.SaveChangesAsync();
            
        }

        public async Task<int> UpdateLangsZH(List<LangText> langList)
        {
            using var Db = new LangDbContext();

            foreach(var l in langList)  
            {
                if (Db.LangData.Where(d => d.UniqueID == l.UniqueID).Count() == 1)
                {
                    if (l.RowStats == 0)
                    {
                        Db.Attach(l);
                        Db.Entry(l).Property("Text_ZH").IsModified = true;
                        Db.Entry(l).Property("IsTranslated").IsModified = true;
                        //Db.Entry(l).Property("RowStats").IsModified = true;
                    }
                    else
                    {
                        Db.Attach(l);
                        Db.Entry(l).Property("Text_ZH").IsModified = true;
                        Db.Entry(l).Property("IsTranslated").IsModified = true;
                        Db.Entry(l).Property("RowStats").IsModified = true;
                    }
                    
                }
            }

            //Db.AttachRange(langList);
            //Db.Entry(langList).Property("Text_ZH").IsModified = true;
            //Db.Entry(langList).Property("IsTranslated").IsModified = true;
            //Db.Entry(langList).Property("RowStats").IsModified = true;

            //Db.UpdateRange(langList);
            return await Db.SaveChangesAsync();
            //Debug.WriteLine(list);
        }

        public async Task UpdateLangsZH(LuaUIData langUIList)
        {
            using var Db = new LangDbContext();
            Db.Attach(langUIList);
            Db.Entry(langUIList).Property("Text_ZH").IsModified = true;
            Db.Entry(langUIList).Property("IsTranslated").IsModified = true;
            Db.Entry(langUIList).Property("RowStats").IsModified = true;

            //Db.Update(langList);
            await Db.SaveChangesAsync();

        }

        public async Task<int> UpdateLangsZH(List<LuaUIData> langUIList)
        {
            using var Db = new LangDbContext();

            foreach (var l in langUIList)
            {
                if (Db.LuaLang.Where(d => d.UniqueID == l.UniqueID).Count() == 1)
                {
                    if (l.RowStats == 0)
                    {
                        Db.Attach(l);
                        Db.Entry(l).Property("Text_ZH").IsModified = true;
                        Db.Entry(l).Property("IsTranslated").IsModified = true;
                        //Db.Entry(l).Property("RowStats").IsModified = true;
                    }
                    else
                    {
                        Db.Attach(l);
                        Db.Entry(l).Property("Text_ZH").IsModified = true;
                        Db.Entry(l).Property("IsTranslated").IsModified = true;
                        Db.Entry(l).Property("RowStats").IsModified = true;
                    }

                }
                Debug.WriteLine("{0},{1}",l.UniqueID,l.Text_ZH);
            }

            return await Db.SaveChangesAsync();
            //Debug.WriteLine(list);
        }



        public async Task<int> UpdateOldTranslateLangsZH(List<LangText> langList)
        {
            using var Db = new LangDbContext();

            foreach (var l in langList)
            {
                if (Db.LangData.Where(d => d.UniqueID == l.UniqueID).Count() == 1)
                {
                    Db.Attach(l);
                    Db.Entry(l).Property("Text_ZH").IsModified = true;
                    Db.Entry(l).Property("IsTranslated").IsModified = true;
                    //Db.Entry(l).Property("RowStats").IsModified = true;
                }
            }

            //Db.AttachRange(langList);
            //Db.Entry(langList).Property("Text_ZH").IsModified = true;
            //Db.Entry(langList).Property("IsTranslated").IsModified = true;
            //Db.Entry(langList).Property("RowStats").IsModified = true;

            //Db.UpdateRange(langList);
            return await Db.SaveChangesAsync();
            //Debug.WriteLine(list);
        }



        public List<LangDataOld> GetSearchOldDataAsync()
        {
            List<LangDataOld> oldDB = new List<LangDataOld>();
            using (var DbContext = new langOldDbContext())
            {
                var dbList = DbContext.langOldTable.FromSqlRaw(@"SELECT name FROM sqlite_master WHERE TYPE='table'").ToList();

                foreach (var table in dbList)
                {
                    Debug.WriteLine(table.name);
                    oldDB.AddRange(DbContext.langOldData.FromSqlRaw(@"SELECT * FROM " + table.name + " WHERE RowStats != 30 AND RowStats != 40"));
                    //oldDB.Add(DbContext.langOldData.ToList());
                    
                }

                Debug.WriteLine(oldDB.Count());
                return oldDB;
                
                //return dbList;
                //return await DbContext.langOldData.ToListAsync();
            }
        }

        public List<LuaUIDataOld> GetSearchLuaOldDataAsync()
        {
            List<LuaUIDataOld> oldDB = new List<LuaUIDataOld>();
            Dictionary<string, LuaUIData> oldDict = new Dictionary<string, LuaUIData>();

            using (var DbContext = new langOldLuaDbContext())
            {
                var dbList = DbContext.langOldLuaTable.FromSqlRaw(@"SELECT name FROM sqlite_master WHERE TYPE='table'").ToList();

                foreach (var table in dbList)
                {
                    Debug.WriteLine(table.name);
                    oldDB.AddRange(DbContext.langOldLuaData.FromSqlRaw(@"SELECT * FROM " + table.name + " WHERE RowStats != 30 AND RowStats != 40"));
                    //oldDB.Add(DbContext.langOldData.ToList());

                }


                Debug.WriteLine(oldDB.Count());
                return oldDB;

                //return dbList;
                //return await DbContext.langOldData.ToListAsync();
            }
        }

        //public List<LuaUIDataOld> GetSearchLuaOldDataAsync(string dbName)
        //{
        //    List<LuaUIDataOld> oldDB = new List<LuaUIDataOld>();
        //    using (var DbContext = new langOldLuaDbContext())
        //    {
        //        //var dbList = DbContext.langOldLuaTable.FromSqlRaw(@"SELECT name FROM sqlite_master WHERE TYPE='table'").ToList();
        //        oldDB.AddRange(DbContext.langOldLuaData.FromSqlRaw(@"SELECT * FROM " + dbName + " WHERE RowStats != 30 AND RowStats != 40"));
        //        //foreach (var table in dbList)
        //        //{
        //        //    Debug.WriteLine(table.name);

        //        //    //oldDB.Add(DbContext.langOldData.ToList());

        //        //}

        //        Debug.WriteLine(oldDB.Count());
        //        return oldDB;

        //        //return dbList;
        //        //return await DbContext.langOldData.ToListAsync();
        //    }
        //}

    }
    
}
