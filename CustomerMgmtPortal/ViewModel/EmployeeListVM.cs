using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerMgmtPortal.ViewModel
{
    public class EmployeeListVM
    {
        //public List<EmployeeVM> Employees { get; set; }  
        public PagedList.IPagedList<EmployeeVM> Employees { get; set; }
    }
}