using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAMBULL_GAMC.UTILITY.Logger
{
    public class LogAttribute : ActionFilterAttribute
    {
        LogHandler logger;

        public LogAttribute()
        {
            logger = new LogHandler();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            StringBuilder parameterList = new StringBuilder();
            parameterList.Append("Parameters List - ");
            parameterList.Append(JsonConvert.SerializeObject(filterContext.ActionArguments).ToString());
            logger.WriteTrace(filterContext.RouteData.Values, parameterList, "OnActionExecuting");
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var type = filterContext.Result.GetType().Name;
            StringBuilder result = new StringBuilder();
            result.Append("Result - ");
            if (type != "RedirectToActionResult")
            {
                result.Append(JsonConvert.SerializeObject(filterContext.Result).ToString());
                logger.WriteTrace(filterContext.RouteData.Values, result, "OnResultExecuted");
            }
        }

    }
}
