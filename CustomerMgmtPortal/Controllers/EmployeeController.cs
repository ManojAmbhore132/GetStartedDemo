using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomerMgmtPortal.Models;
using CustomerMgmtPortal.ViewModel;
using CustomerMgmtPortal.DAL;
using System.Net;
using CustomerMgmtPortal.Filters;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using PagedList;
using System.Collections;


namespace CustomerMgmtPortal.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeeBusinessLayer empBL = new EmployeeBusinessLayer();
        // GET: Employee
        [Authorize]
        //[HeaderFooterFilter]
        public ActionResult Index(int? page, string searchString, string sortOrder)
      {
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<EmployeeVM> empPList = null;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            bool IsAdmin = false;
            if (User.Identity.Name == "admin")
            {
                IsAdmin = true;
                Session["IsAdmin"] = IsAdmin;
            }
            EmployeeListVM emplListVM = new EmployeeListVM();
            List<EmployeeVM> employeeList = new List<EmployeeVM>();

            List<Employee> employees = empBL.GetEmployees();

            IEnumerable<Employee> emps = employees;

            if (!String.IsNullOrEmpty(searchString))
            {                                
                emps = employees.Where(s => s.FirstName.Contains(searchString) || s.LastName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    emps = emps.OrderByDescending(s => s.FirstName);
                    break;
                case "Date":
                    emps = emps.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    emps = emps.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    emps = emps.OrderBy(s => s.FirstName);
                    break;
            }

            foreach (Employee employee in emps)
            {
                EmployeeVM empVM = new EmployeeVM();

                empVM.EmpName = employee.FirstName + " " + employee.LastName;
                empVM.EmployeeId = employee.EmployeeId;
                empVM.Age = employee.Age.ToString();
                empVM.EnrollmentDate = employee.EnrollmentDate;                               
                employeeList.Add(empVM);
            }            
            emplListVM.Employees = employeeList as IPagedList<EmployeeVM>;
            empPList =  employeeList.ToPagedList(pageIndex, pageSize);
            
            return View("Index", empPList);
        }

        [AdminFilter]
        [HeaderFooterFilter]
        public ActionResult AddNew()
        {
            EmployeeVM empVM = new EmployeeVM();          
            return View("CreateEmployee", empVM);
        }

        [AdminFilter]
        [HeaderFooterFilter]
        public ActionResult SaveEmployee(Employee emp, string btnSave)
        {
            //2nd parameter 'btnSave' should be same as name of Save button and Cancel button, for auto Model Binder
            switch (btnSave)
            {
                case "Save":
                    if (ModelState.IsValid)
                    {
                        empBL.SaveEmployee(emp);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        EmployeeVM empVM = new EmployeeVM();
                        return View("CreateEmployee", empVM);
                    }
                case "Cancel":
                    return RedirectToAction("Index");
            }
            return new EmptyResult(); // TO handle all code paths return value
        }

        //This HTTP Get `Edit` method doesn't edit the specified emp, it returns a view of the emp where you can submit (HttpPost) the edit.        
        public ActionResult EditEmployee(int id = 0)
        {
            //When user click 'Edit', extract the id of the record, fetch its corresponding object from DB and display the field values on Edit page.
            Employee emp = empBL.FindEmployee(id);          
            if (emp == null)
            {
                return HttpNotFound();
            }                      
            return View(emp);
        }

        [HttpPost]       
        public ActionResult EditEmployee(Employee emp)
        {
            //This action is invoked when user hits update button
            if (ModelState.IsValid)
            {
                empBL.EditEmployee(emp);
                return RedirectToAction("Index");
            }
            return View(emp);
        }

        //No separate view is created for Delete, but we can do (Eg: Create Delete View to show msg 'Are you sure you want to delete?')
        public ActionResult DeleteEmployee(int id)
        {
            empBL.DeleteEmployee(id);
            return RedirectToAction("Index");
        }

        public ActionResult GetAddNewLink()
        {
            if (Convert.ToBoolean(Session["IsAdmin"]))
                return PartialView("AddNewLink");
            else
                return new EmptyResult();
        }

        public ActionResult MyFooter()
        {
            FooterVM footerData = new FooterVM();
            footerData.CompanyName = "Manoj Inc";
            footerData.Year = DateTime.Now.Year.ToString();
            return View("Footer", footerData);
        }

        public ActionResult MyHeader()
        {
            HeaderVM headerData = new HeaderVM();
            headerData.UserName = User.Identity.Name;
            headerData.Desc = "Customer Mgmt Portal";
            return View("Header", headerData);
        }

        public FileResult CreatePdf()
        {
            //FileResult which used to send binary file content to the response.
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            //file name to be created   
            string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 5 columns  
            PdfPTable tableLayout = new PdfPTable(5);
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table  

            //file will be created in this path  
            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);

            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF   
            doc.Add(Add_Content_To_PDF(tableLayout));

            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return File(workStream, "application/pdf", strPDFFileName);
        }

        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {
            float[] headers = { 50, 24, 45, 35, 50}; //Header Widths. Since pdf has 5 cols, so set 5 headers here.
             
            tableLayout.SetWidths(headers); 
            tableLayout.WidthPercentage = 100;
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  

            List<Employee> employees = empBL.GetEmployees(); 



            tableLayout.AddCell(new PdfPCell(new Phrase("Customer Details", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) {
                Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER
            });


            ////Add header  
            AddCellToHeader(tableLayout, "EmployeeId");
            AddCellToHeader(tableLayout, "First Name");
            AddCellToHeader(tableLayout, "Last Name");
            AddCellToHeader(tableLayout, "Age");
            AddCellToHeader(tableLayout, "Enrollment Date");

            ////Add body  
            foreach (var emp in employees)
            {

                AddCellToBody(tableLayout, emp.EmployeeId.ToString());
                AddCellToBody(tableLayout, emp.FirstName);
                AddCellToBody(tableLayout, emp.LastName);
                AddCellToBody(tableLayout, emp.Age.ToString());
                AddCellToBody(tableLayout, emp.EnrollmentDate.ToString());
            }

            return tableLayout;
        }
        // Method to add single cell to the Header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0)
            });
        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
             {
                HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
             });
        }



    }
    }