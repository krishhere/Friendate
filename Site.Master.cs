using System;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI;

namespace WebApplication
{
    public partial class Site : System.Web.UI.MasterPage
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);
        string cookieName = "salngName";
        string cookieEmail = "salngEmail";
        string cookieId= "salngId";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HttpContext.Current.Request.Cookies[cookieName] != null && HttpContext.Current.Request.Cookies[cookieEmail] != null && HttpContext.Current.Request.Cookies[cookieId] != null)
                {
                    string mail = HttpContext.Current.Request.Cookies["salngEmail"].Value;
                    BindUser(mail);
                }
                else
                {
                    RemoveValueInCookies("salngEmail");
                    RemoveValueInCookies("salngName");
                    RemoveValueInCookies("salngId");
                    Response.Redirect("Login.aspx");
                }
            }
        }
        protected int BindUser(string emailId)
        {
            int id = 0;
            try
            {
                con.Open();
                MySqlDataReader dr = null;
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT id,name,email,image1 FROM mySite.users where email='" + emailId + "'";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblProfileName.Text = dr["Name"].ToString();
                    ImgProfile.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])dr["image1"]);
                }
                else {
                    RemoveValueInCookies("salngEmail");
                    RemoveValueInCookies("salngName");
                    RemoveValueInCookies("salngId"); 
                    Response.Redirect("Login.aspx"); 
                }
            }
            catch (Exception ex)
            {
                RemoveValueInCookies("salngEmail");
                RemoveValueInCookies("salngName");
                RemoveValueInCookies("salngId");
                Response.Redirect("Login.aspx");
            }
            finally
            {
                con.Close();
            }
            return id;
        }
        protected void lbrnSignOut_Click(object sender, EventArgs e)
        {
            RemoveValueInCookies("salngEmail");
            RemoveValueInCookies("salngName");
            RemoveValueInCookies("salngId");
            Response.Redirect("Login.aspx");
        }
        private void RemoveValueInCookies(string key)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        protected void lbProfile_Click(object sender, EventArgs e)
        {
            Session["edit"] = "EditProfile";
            Response.Redirect("UserEntry");
        }
    }
}