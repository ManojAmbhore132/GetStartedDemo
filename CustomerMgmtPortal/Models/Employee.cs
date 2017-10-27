using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CustomerMgmtPortal.Validations;

namespace CustomerMgmtPortal.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }        
        [FirstNameValidation]
        public string FirstName { get; set; }
        [StringLength(12, ErrorMessage ="Max length 12")]
        public string LastName { get; set; }        
        public int Age { get; set; }
        public DateTime EnrollmentDate { get; set; }

    }
}