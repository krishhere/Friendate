using System;
using System.Web;
using System.Web.Routing;

namespace WebApplication
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(System.Web.Routing.RouteTable.Routes);
        }
        //void RegisterRoutes(RouteCollection routes)
        //{
        //    routes.MapPageRoute("RequestsRoute", "Requests", "~/Requests.aspx");
        //    routes.MapPageRoute("LoginRoute", "Login", "~/Login.aspx");
        //    routes.MapPageRoute("DefaultRoute", "Default", "~/Default.aspx");
        //}
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            
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