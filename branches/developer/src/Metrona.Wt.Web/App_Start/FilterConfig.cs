using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Metrona.Wt.Web.App_Start
{
    using System.Web.Mvc;

    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}