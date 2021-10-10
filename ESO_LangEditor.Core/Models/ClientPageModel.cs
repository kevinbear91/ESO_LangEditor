using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ESO_LangEditor.Core.Models
{
    public class ClientPageModel
    {
        public int PageNumber { get; set; }
        public bool IsCurrentPage { get; set; }
        public ICommand GetPageCommand { get; set; }
    }
}
