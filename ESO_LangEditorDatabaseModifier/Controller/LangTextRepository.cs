using ESO_LangEditorDatabaseModifier.Controller.v2;
using ESO_LangEditorDatabaseModifier.Controller.v3;
using ESO_LangEditorDatabaseModifier.Controller.v4;
using ESO_LangEditorDatabaseModifier.Model.v2;
using ESO_LangEditorDatabaseModifier.Model.v3;
using ESO_LangEditorDatabaseModifier.Model.v4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ESO_LangEditorDatabaseModifier.Controller
{
    public class LangTextRepository
    {

        public List<LangTextDto_v2> GeAlltLangTexts_v2()
        {
            using (var db = new LangDbContextV2())
            {
                return db.LangData.ToList();

                //return data;
            }
        }

        public List<LuaUIData_v2> GeAlltLangLuas_v2()
        {
            using (var db = new LangDbContextV2())
            {
                return db.LuaLang.ToList();

                //return data;
            }
        }

        public List<LangTextDto_v3> GeAlltLangTexts_v3()
        {
            using (var db = new LangDbContextV3())
            {
                return db.LangData.ToList();

                //return data;
            }
        }

        public void AddNewLangs(List<LangTextDto_v3> v3List)
        {

            using (var db = new LangDbContextV3())
            {
                db.Database.EnsureCreated();
                db.AddRange(v3List);
                db.SaveChanges();
            }

            Debug.WriteLine("Save Done!");
        }

        public void AddNewLangsToV4(List<LangText> v4List)
        {

            using (var db = new LangDbContextV4())
            {
                db.Database.EnsureCreated();
                db.AddRange(v4List);
                db.SaveChanges();
            }

            Debug.WriteLine("Save Done!");
        }

    }
}
