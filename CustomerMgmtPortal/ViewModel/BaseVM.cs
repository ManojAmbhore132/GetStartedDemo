using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerMgmtPortal.ViewModel
{
    public class BaseVM
    {
        public string UserName { get; set; }
        public string Desc { get; set; }
        public FooterVM FooterData { get; set; }
    }
}