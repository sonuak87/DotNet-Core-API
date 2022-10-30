using log4net;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VeggiFoodAPI.Models.ViewModels;

namespace GAMBULL_GAMC.UTILITY.Logger
{
    public class LogHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void WriteError(ExceptionModel objExceptionModel)
        {
            StackFrame objStackFrame = new StackFrame(1);
            StringBuilder errorTrace = new StringBuilder();
            errorTrace.AppendLine("=============================================Start====================================================");
            errorTrace.AppendLine("Action - " + objExceptionModel.Method + ", Controller - " + objExceptionModel.Controller);
            errorTrace.AppendLine("Date and Time - " + DateTime.Now.ToString());
            errorTrace.AppendLine("Message - " + objExceptionModel.Message);
            errorTrace.AppendLine("InnerException - " + objExceptionModel.InnerException);
            errorTrace.AppendLine("StackTrace - " + objExceptionModel.StackTrace);
            errorTrace.AppendLine("Data - " + objExceptionModel.Data);
            errorTrace.AppendLine("=============================================End====================================================");
            Log.Error(errorTrace);
        }
        public void WriteTrace(RouteValueDictionary routeData, StringBuilder parameterList, string v)
        {
            var controllerName = routeData["controller"];
            var actionName = routeData["action"];

            StringBuilder logTrace = new StringBuilder();
            logTrace.AppendLine("===============================================Start==================================================");
            logTrace.AppendLine("Action - " + actionName + ", Controller - " + controllerName + ", Event - " + v);
            logTrace.AppendLine("Date and Time - " + DateTime.Now.ToString());
            logTrace.AppendLine(parameterList.ToString());
            logTrace.AppendLine("================================================End==================================================");
            Log.Info(logTrace);
        }
    }
}
