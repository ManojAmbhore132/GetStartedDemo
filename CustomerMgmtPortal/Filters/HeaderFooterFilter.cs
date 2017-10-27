using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomerMgmtPortal.ViewModel;

namespace CustomerMgmtPortal.Filters
{
    public class HeaderFooterFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewResult vResult = filterContext.Result as ViewResult;
            if(vResult != null)
            {
                BaseVM bvm = vResult.Model as BaseVM;

                if(bvm !=null) // bvm will be null when we want a View without Header n Footer
                {
                    bvm.UserName = HttpContext.Current.User.Identity.Name;
                    bvm.Desc = "Customer Mgmt Portal";
                    bvm.FooterData = new FooterVM();//Footer Data
                    bvm.FooterData.CompanyName = "Manoj Inc";
                    bvm.FooterData.Year = DateTime.Now.Year.ToString();
                }
            }
        }
    }
}