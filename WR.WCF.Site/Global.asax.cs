using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WR.WCF.Site
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //开启日志记录
            WR.Utils.LogService.InitializeService(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "web.config"));

            //WR.DAL.EF.BFdbContext db = new WR.DAL.EF.BFdbContext();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            ////验证
            //if (HttpContext.Current.Request.Url.AbsolutePath.EndsWith(".svc")
            //    && HttpContext.Current.Request.UrlReferrer.Host != "localhost")
            //{
            //    if (!DataCache.IsAnth)
            //    {
            //        //HttpContext.Current.Response.Write("test");
            //        HttpContext.Current.Response.ClearHeaders();
            //        HttpContext.Current.Response.AppendHeader("Linese_No", "1");
            //        HttpContext.Current.Response.StatusCode = 302;
            //        HttpContext.Current.Response.End();
            //    }
            //}
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}