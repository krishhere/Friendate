using System;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI;

namespace WebApplication
{
    public partial class EditProfile : System.Web.UI.Page
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);
        string cookieName = "salngName";
        string cookieEmail = "salngEmail";
        string cookieId = "salngId";
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
                MySqlDataReader dr = null;
                string query = "with cte as(SELECT u.id id,Name,Email,dob,IF(u.gender=1,'Male','Female') AS gender,city,IF(lookfor=2,'friend or date', IF(lookfor=1,'date','friend')) AS lookFor,about,image1,IF(i.reading=1,'Reading',NULL) AS reading,IF(i.trekking=1,'Trekking',NULL) AS trekking,IF(i.hiking=1,'Hiking',NULL) AS hiking,IF(i.singing=1,'Singing',NULL) AS singing,IF(i.dancing=1,'Dancing',NULL) AS dancing,IF(i.listenMusic=1,'Music lover',NULL) AS listenMusic,IF(i.gardening=1,'Gardening',NULL) AS gardening,IF(i.cooking=1,'Cooking',NULL) AS cooking,IF(i.fitness=1,'Fitness',NULL) AS fitness,IF(i.foodie=1,'Foodie',NULL) AS foodie,IF(i.travelling=1,'travelling',NULL) AS travelling,IF(i.art=1,'Artist',NULL) AS art,IF(i.photography=1,'Photography',NULL) AS photography,IF(i.teaching=1,'Teaching',NULL) AS teaching,IF(i.technology=1,'Technology',NULL) AS technology,IF(i.coding=1,'Coding',NULL) AS coding,IF(i.petCaring=1,'Pet caring',NULL) AS petCaring,IF(i.outdoorGaming=1,'Outdoor gaming',NULL) AS outdoorGaming,IF(i.indoorGaming=1,'Indoor gaming',NULL) AS indoorGaming,IF(i.fashion=1,'Fashion',NULL) AS fashion,IF(i.nightLife=1,'Night life',NULL) AS nightLife,IF(i.daylife=1,'Day life',NULL) AS daylife,IF(i.investment=1,'investment',NULL) AS investment,IF(i.business=1,'Business',NULL) AS business,IF(i.movies=1,'Movies',NULL) AS movies,IF(i.shopping=1,'Shopping',NULL) AS shopping,IF(i.roadtrips=1,'Road trips',NULL) AS roadtrips,IF(i.politics=1,'Politics',NULL) AS politics,IF(i.chillatbar=1,'Chill at bar',NULL) AS chillatbar FROM mySite.users u inner JOIN mySite.interest i ON u.id = i.id) SELECT id,Name,Email,dob,gender,city,lookFor,about,image1,CONCAT_WS(',', reading,trekking,hiking,singing,dancing,listenMusic,gardening,cooking,fitness,foodie,travelling,art,photography,teaching,technology,coding,petCaring,outdoorGaming,indoorGaming,fashion,nightLife,daylife,investment,business,movies,shopping,roadtrips,politics,chillatbar) AS interests FROM cte where Email=@pEmail";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@pEmail", emailId);
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblProfileName.Text = dr["Name"]?.ToString();
                    lblGender.Text = dr["gender"]?.ToString();
                    lblDob.Text = dr["dob"]?.ToString();
                    lblCity.Text = dr["city"]?.ToString();
                    lblAbout.Text = dr["about"]?.ToString();
                    string interests = dr["interests"]?.ToString();
                    lblInterests.Text = interests.Replace(",",", ");
                    string lookforValue = dr["lookfor"]?.ToString();
                    if (lookforValue == "Friend")
                    {
                        ddlLookingFor.Items.FindByValue("Friend").Selected = true;
                    }
                    else if (lookforValue == "Date")
                    {
                        ddlLookingFor.Items.FindByValue("Date").Selected = true;
                    }
                    else
                    {
                        ddlLookingFor.Items.FindByValue("Both").Selected = true;
                    }
                    ImgProfile.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])dr["image1"]);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
            return id;
        }
        private void RemoveValueInCookies(string key)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}