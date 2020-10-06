using ESO_LangEditorLib.Models.Client.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditorGUI.Interface
{
    public interface IStartupDBCheck
    {
        bool CheckDbUpdateExist();
        bool IsDBExist();

        Task<ProcessDbUpdateResult> ProcessUpdateMerge();
    }
}
