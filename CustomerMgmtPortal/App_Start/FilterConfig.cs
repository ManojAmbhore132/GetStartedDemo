using System.Web;
using System.Web.Mvc;

namespace CustomerMgmtPortal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());// Added to make all Controllers Action methods secured globally.
        }
    }
}
