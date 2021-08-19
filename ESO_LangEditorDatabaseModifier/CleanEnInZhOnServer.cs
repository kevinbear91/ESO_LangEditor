using ESO_LangEditor.EFCore;
using ESO_LangEditorDatabaseModifier.Controller.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ESO_LangEditorDatabaseModifier
{
    public class CleanEnInZhOnServer
    {
        public void ProcessData()
        {

            int noZhCount = 0;

            using (var db = new LangServerDbContext())
            {
                var entityList = db.Langtexts.ToList();

                foreach(var entity in entityList)
                {
                    if (!IsZh(entity.TextZh))
                    {
                        entity.TextZh = null;
                        noZhCount++;
                    }
                }
                db.UpdateRange(entityList);
                db.SaveChanges();
            }
            Console.WriteLine($"Clean EN: {noZhCount}");
            
        }

        private static bool IsZh(string langText)
        {
            if (string.IsNullOrWhiteSpace(langText))
            {
                return true;
            }

            Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
            Match m = RegCHZN.Match(langText);
            return m.Success;
        }


    }
}
