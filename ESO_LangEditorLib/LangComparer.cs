using ESO_LangEditorLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorLib
{
    public class LangComparer
    {


        public class LangTextEnComparer : EqualityComparer<LangData>
        {
            public override bool Equals(LangData data1, LangData data2)
            {
                return String.Equals(data1?.Text_EN, data2?.Text_EN);
            }

            public override int GetHashCode(LangData data)
            {
                return data.UniqueID.GetHashCode();
            }
        }
        
    }
}
