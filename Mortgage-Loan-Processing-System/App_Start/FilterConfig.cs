using System.Web;
using System.Web.Mvc;

namespace Mortgage_Loan_Processing_System
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
