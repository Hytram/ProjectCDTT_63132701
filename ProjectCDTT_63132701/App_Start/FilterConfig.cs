using System.Web;
using System.Web.Mvc;

namespace ProjectCDTT_63132701
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
