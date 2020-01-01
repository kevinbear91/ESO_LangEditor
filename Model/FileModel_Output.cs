using FileHelpers;

namespace ESO_Lang_Editor.Model
{
    [DelimitedRecord(",")]
    class FileModel_Output
    {
        [FieldQuoted]
        public string csvItemID { get; set; }
        [FieldQuoted]
        public string csvItemEN { get; set; }
        [FieldQuoted]
        public string csvItemCN { get; set; }
    }
}
