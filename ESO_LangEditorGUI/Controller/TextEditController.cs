using ESO_LangEditorGUI.View;
using ESO_LangEditorLib;
using ESO_LangEditorLib.Models.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorGUI.Controller
{
    public class TextEditController
    {
        private LangTextDto editData;
        private readonly TextEditor _textEditorWindow;
        private LangDbController db = new LangDbController();

        public TextEditController(TextEditor textEditorWindow)
        {
            _textEditorWindow = textEditorWindow;
        }

        public void SaveEditedZh(LangTextDto langText)
        {
            LangTextDto currentEditedLangText;

            currentEditedLangText = langText;

            currentEditedLangText.IsTranslated = 1;
            currentEditedLangText.ZhLastModifyTimestamp = DateTime.Now;

           // int result = db.UpdateLangsZH(currentEditedLangText);

        }

        private int SetRowStats(int rowStats)
        {
            int row = rowStats;

            if (row == 2)
                row = 4;
            else if (row != 4)
                row = 3;
            else
                row = 4;

            return row;
        }

    }
}
