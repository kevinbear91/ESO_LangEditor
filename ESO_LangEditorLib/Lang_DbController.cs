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
    public class Lang_DbController
    {

        public void InsertDataFromCsv(List<LangData> data)
        {
            using (var DbContext = new Lang_DbContext())
            {
                using (var transaction = DbContext.Database.BeginTransaction())
                {
                    try
                    {
                        DbContext.langData.AddRange(data);
                        DbContext.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }
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
        public async Task<List<LangData>> GetLangsListAsync(int field, int searchPos, string searchWord)
        {
            List<LangData> data = new List<LangData>();

            string searchPosAndWord = searchPos switch  //设定关键字出现的位置
            {
                0 => "%" + searchWord + "%",     //任意位置
                1 => searchWord + "%",           //仅在开头
                2 => "%" + searchWord,           //仅在末尾
                _ => "%" + searchWord + "%",     //默认 - 任意位置
            };

            using (var Db = new Lang_DbContext())
            {
                data = field switch
                {
                    0 => await Db.langData.Where(d => d.ID == ToInt32(searchWord)).ToListAsync(),
                    1 => await Db.langData.Where(d => EF.Functions.Like(d.Text_EN, searchPosAndWord)).ToListAsync(),
                    2 => await Db.langData.Where(d => EF.Functions.Like(d.Text_ZH, searchPosAndWord)).ToListAsync(),
                    3 => await Db.langData.Where(d => EF.Functions.Like(d.UpdateStats, searchPosAndWord)).ToListAsync(),
                    4 => await Db.langData.Where(d => d.RowStats == ToInt32(searchWord)).ToListAsync(),
                    5 => await Db.langData.Where(d => d.IsTranslated == ToInt32(searchWord)).ToListAsync(),
                    _ => await Db.langData.Where(d => EF.Functions.Like(d.Text_EN, searchPosAndWord)).ToListAsync(),
                };
                //await Db.langData.Where(d => EF.Functions.Like(d.UpdateStats, searchPosAndWord)).ToDictionaryAsync(d => d.UniqueID),
                //data = await q.ToDictionaryAsync(q => q.UniqueID);
            }

            return data;
        }


        public async Task<Dictionary<string, LangData>> GetAllLangsDictionaryAsync()
        {
            Dictionary<string, LangData> data = new Dictionary<string, LangData>();

            using (var Db = new Lang_DbContext())
            {
                data = await Db.langData.ToDictionaryAsync(d => d.UniqueID);

                //data = q.ToDictionary(q => q.UniqueID);
            }
            return data;
        }



        public async Task AddNewLangs(List<LangData> langList)
        {
            using var Db = new Lang_DbContext();
            Db.AddRange(langList);
            await Db.SaveChangesAsync();
        }

        public async Task UpdateLangsEN(List<LangData> langList)
        {
            using var Db = new Lang_DbContext();
            Db.UpdateRange(langList);
            await Db.SaveChangesAsync();
        }
        public async Task DeleteLangs(List<LangData> langList)
        {
            using var Db = new Lang_DbContext();
            Db.RemoveRange(langList);
            await Db.SaveChangesAsync();
        }


        public async Task UpdateLangsZH(LangData langList)
        {
            using var Db = new Lang_DbContext();
            Db.Update(langList);
            await Db.SaveChangesAsync();
            
        }
        public async Task UpdateLangsZH(List<LangData> langList)
        {
            using var Db = new Lang_DbContext();
            Db.UpdateRange(langList);
            await Db.SaveChangesAsync();
        }




        public List<LangData_Old> GetSearchOldDataAsync()
        {
            List<LangData_Old> oldDB = new List<LangData_Old>();
            using (var DbContext = new lang_OldDbContext())
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

    }
    
}
