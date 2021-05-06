using ESO_LangEditorDatabaseModifier.Controller.Server;
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

        public List<LangText> GeAlltLangTexts_v4()
        {
            using (var db = new LangDbContextV4())
            {
                return db.Langtexts.ToList();

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
        public void AddUsersToV4(List<UserInClient> v4UserList)
        {

            using (var db = new LangDbContextV4())
            {
                db.Database.EnsureCreated();
                db.AddRange(v4UserList);
                db.SaveChanges();
            }

            Debug.WriteLine("Save Done!");
        }
        public UserInClient GetUsersToV4(Guid v4UserId)
        {
            using (var db = new LangDbContextV4())
            {
                return db.Users.Find(v4UserId);
            }
        }

        public Dictionary<Guid, UserInClient> GetUserAllToV4()
        {
            using (var db = new LangDbContextV4())
            {
                return db.Users.ToDictionary(u => u.Id);
            }

        }

        public Dictionary<Guid, ESO_LangEditor.Core.Entities.User> GetUserAllFromServer()
        {
            using (var db = new LangServerDbContext())
            {
                return db.Users.ToDictionary(u => u.Id);
            }

        }

        public void AddUsersToServer(List<ESO_LangEditor.Core.Entities.User> ServerUserList)
        {

            using (var db = new LangServerDbContext())
            {
                db.AddRange(ServerUserList);
                db.SaveChanges();
            }

            Debug.WriteLine("Save User Done!");
        }

        public void AddLangTextsToServer(List<ESO_LangEditor.Core.Entities.LangText> ServerLangTextList)
        {

            using (var db = new LangServerDbContext())
            {
                db.AddRange(ServerLangTextList);
                db.SaveChanges();
            }

            Debug.WriteLine("Save User Done!");
        }
    }
}
