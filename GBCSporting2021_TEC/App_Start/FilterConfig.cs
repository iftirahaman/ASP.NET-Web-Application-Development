using System.Web;
using System.Web.Mvc;

namespace GBCSporting2021_TEC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
