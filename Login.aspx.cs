using System;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.IO;
using System.Web;

namespace WebApplication
{
    public partial class Login : System.Web.UI.Page
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);

        string clientid = "784366017075-stgfbdmbddfkci4gm7esiqqd5uov31k8.apps.googleusercontent.com";
        string redirection_url = "https://localhost:44329/UserEntry.aspx";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string cookieName = "salngName";
                string cookieEmail = "salngEmail";
                if (HttpContext.Current.Request.Cookies[cookieEmail] != null)
                {
                    string email = HttpContext.Current.Request.Cookies[cookieEmail].Value;
                    Response.Redirect("Default.aspx");
                }
            }
        }
        protected void btnSignInGoogle_Click(object sender, EventArgs e)
        {
            string url = "https://accounts.google.com/o/oauth2/v2/auth?scope=profile&include_granted_scopes=true&redirect_uri=" + redirection_url + "&response_type=code&client_id=" + clientid + "";
            Response.Redirect(url);
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            List<string> lstUserDetails = VerifyUser();
            if (lstUserDetails.Count==2)
            {
                StoreValueInCookies("salngEmail", txtEmail.Text.Trim());
                StoreValueInCookies("salngId", lstUserDetails[0].ToString());
                StoreValueInCookies("salngName", lstUserDetails[1].ToString());
                Response.Redirect("Default.aspx");
            }
        }
        protected List<string> VerifyUser()
        {
            List<string> names = new List<string>();
            try
            {
                con.Open();
                MySqlDataReader dr = null;
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT id,Name FROM mySite.users where Email='{txtEmail.Text.Trim()}' and password='{txtPswd.Text.Trim()}'";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    names.Add(dr["id"].ToString());
                    names.Add(dr["Name"].ToString());
                    break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "alert('Technical issues. please try later');", true);
            }
            finally
            {
                con.Close();
            }
            return names;
        }
        private void StoreValueInCookies(string key, string value)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        private void RemoveValueInCookies(string key)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}