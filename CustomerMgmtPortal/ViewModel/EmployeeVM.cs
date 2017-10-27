using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerMgmtPortal.ViewModel
{
    public class EmployeeVM : BaseVM
    {
        public int EmployeeId { get; set; }
        public string EmpName { get; set; }
        public string Age { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}