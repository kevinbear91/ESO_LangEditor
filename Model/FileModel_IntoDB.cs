using static System.Convert;

namespace ESO_Lang_Editor.Model
{
    class FileModel_IntoDB
    {
        public int stringID { get; set; }

        public int stringUnknown { get; set; }

        public int stringIndex { get; set; }

        public int Istranslated { get; set; }

        public string EN_text { get; set; }

        public string ZH_text { get; set; }

        public string GetUniqueID(bool isGetFieldID)
        {
            string fieldID = ToUInt32(stringID).ToString(); 
            string fieldUnknown = ToUInt32(stringUnknown).ToString(); 
            string fieldIndex = ToUInt32(stringIndex).ToString();
            if (isGetFieldID)
            {
                return fieldID;
            }
            else
            {
                return fieldID + '-' + fieldUnknown + '-' + fieldIndex;
            }
        }

    }
}
