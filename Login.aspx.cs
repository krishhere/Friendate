using System;
using System.Web;

namespace WebApplication
{
    public partial class Login : System.Web.UI.Page
    {
        string clientid = "784366017075-stgfbdmbddfkci4gm7esiqqd5uov31k8.apps.googleusercontent.com";
        string redirection_url = "https://localhost:44329/UserEntry.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {
            string cookieName = "salngName";
            string cookieEmail = "salngEmail";
            if (HttpContext.Current.Request.Cookies[cookieEmail] != null)
            {
                Response.Redirect("Default.aspx");
            }
        }
        protected void btnSignInGoogle_Click(object sender, EventArgs e)
        {
            string url = "https://accounts.google.com/o/oauth2/v2/auth?scope=profile&include_granted_scopes=true&redirect_uri=" + redirection_url + "&response_type=code&client_id=" + clientid + "";
            Response.Redirect(url);
        }
    }
}