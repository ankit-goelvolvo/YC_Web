using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using YCWeb.Data;
using YCWeb.Models;

namespace YCWeb.Filter
{
    public class CustomActionFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx.Session["User"] == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
                return;
            }
            var sessionEntity = ctx.Session["User"] as SessionEntity;
            using (YCEntities ycDb = new YCEntities())
            {
                ActionLog log = new ActionLog()
                {
                    ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                    Action = string.Concat(filterContext.ActionDescriptor.ActionName, " (Logged By: Custom Action Filter)"),
                    IPAddress = GetLocalIPAddress(),
                    CreatedDate = filterContext.HttpContext.Timestamp,
                    UserId= sessionEntity.UserID
                };
                ycDb.ActionLogs.Add(log);
                ycDb.SaveChanges();
                base.OnActionExecuting(filterContext);
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "No IP found";
        }
    }
}