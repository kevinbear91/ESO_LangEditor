using ESO_LangEditorLib.DatabaseModels;
using ESO_LangEditorLib.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditorLib
{
    public class SqliteController
    {

        SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = @"Data Source=Data\LangContent.db",//必填, 数据库连接字符串
            DbType = DbType.Sqlite,         //必填, 数据库类型
            IsAutoCloseConnection = true,       //默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
            InitKeyType = InitKeyType.SystemTable    //默认SystemTable, 字段信息读取, 如：该属性是不是主键，是不是标识列等等信息
        });

        public void CreateTable()
        {
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");

            if (!File.Exists(@"Data\LangContent.db"))
            {
                db.CurrentConnectionConfig.InitKeyType = InitKeyType.Attribute;

                db.CodeFirst.InitTables(typeof(LangData));
            }
        }

        public async Task InsertDataFromCsv(LangData data)
        {
            await db.Insertable(data).ExecuteCommandAsync();
        }

    }
}
