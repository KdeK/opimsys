using OPIMsys.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace OPIMsys
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var config = GlobalConfiguration.Configuration;
            config.Formatters.Insert(0, new OPIMsys.Formatters.JsonpMediaTypeFormatter());

           

            System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<OPIMsys.Models.OPIMsysContext>());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

           // GlobalConfiguration.Configuration.MessageHandlers.Add(new ApiKeyHandler());
            GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsHandler());

            if (!WebSecurity.Initialized)
               WebSecurity.InitializeDatabaseConnection("OPIMsysContext", "UserProfile", "UserId", "UserName", true);
            
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
       //     if (this.Context.Request.Path.Contains("api"))
        //    {
         //       this.Context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type,X-ApiKey");
         //       this.Context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
         //       this.Context.Response.AddHeader("Access-Control-Max-Age", "3600");
         //   }
        }

    }
}