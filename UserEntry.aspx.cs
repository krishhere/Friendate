using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI;
using ImageMagick;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace WebApplication
{
    public partial class UserEntry : Page
    {
        string clientid = "784366017075-stgfbdmbddfkci4gm7esiqqd5uov31k8.apps.googleusercontent.com";
        string clientsecret = "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";
        string redirection_url = "https://localhost:44329/UserEntry.aspx";
        string url = "https://accounts.google.com/o/oauth2/token";
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);

        string cookieName = "salngName";
        string cookieEmail = "salngEmail";
        static string city;
        static string name;
        static string email;
        List<string> lstInterst = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lstInterst.Clear();
                if (Request.QueryString["code"] != null)
                {
                    GetToken(Request.QueryString["code"].ToString());
                }
                if (HttpContext.Current.Request.Cookies[cookieName] != null && HttpContext.Current.Request.Cookies[cookieEmail] != null)
                {
                    int id = BindUserId(HttpContext.Current.Request.Cookies[cookieEmail].Value);
                    if (id!=0)
                    {
                        Response.Redirect("Default.aspx");
                    }
                    name = HttpContext.Current.Request.Cookies[cookieName].Value;
                    txtName.Text = name;
                }
                if (city == null)
                {
                    //city = GetLocation();
                    city = "Hyd";
                }
                txtCity.Text = city;
            }
        }
        public void GetToken(string code)
        {
            string poststring = "grant_type=authorization_code&code=" + code + "&client_id=" + clientid + "&client_secret=" + clientsecret + "&redirect_uri=" + redirection_url + "";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            UTF8Encoding utfenc = new UTF8Encoding();
            byte[] bytes = utfenc.GetBytes(poststring);
            Stream outputstream = null;
            try
            {
                request.ContentLength = bytes.Length;
                outputstream = request.GetRequestStream();
                outputstream.Write(bytes, 0, bytes.Length);
            }
            catch { }
            var response = (HttpWebResponse)request.GetResponse();
            var streamReader = new StreamReader(response.GetResponseStream());
            string responseFromServer = streamReader.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            Tokenclass obj = js.Deserialize<Tokenclass>(responseFromServer);
            GetuserProfile(obj.access_token);
        }
        public void GetuserProfile(string accesstoken)
        {
            string url = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + accesstoken + "";
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            response.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            Userclass userinfo = js.Deserialize<Userclass>(responseFromServer);
            string id = userinfo.id;
            email = userinfo.email;
            string locale = userinfo.locale;
            string name = userinfo.name;
        }
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListItem item in multiSelectListBox.Items)
                {
                    if (item.Selected)
                    {
                        string val = item.Value;
                        lstInterst.Add(val);
                    }
                }
                
                string imgType = FileUpload1.PostedFile.ContentType.ToString();
                if (txtDob.Text.Trim() != "" && txtAbout.Text.Trim() != "" && imgType.Contains("jpeg"))
                {
                    email = HttpContext.Current.Request.Cookies[cookieEmail].Value;
                    UserInsert(email);
                    int id = BindUserId(email);
                    Thread.Sleep(1000);
                    UploadInterests(id);
                    StoreValueInCookies("salngEmail", email);
                    StoreValueInCookies("salngName", name);
                    Response.Redirect("Default.aspx",false);
                }
                else
                {
                    string Error = "Supports jpeg images";
                    lblMsg.Visible = true;
                    lblMsg.Text = Error;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "alert('Technical issue to save your details.');", true);
            }
            finally
            {
                con.Close();
            }
        }
        private void UserInsert(string email)
        {
            try
            {
                byte[] bytes;
                BinaryReader br = new BinaryReader(FileUpload1.PostedFile.InputStream);
                bytes = br.ReadBytes(FileUpload1.PostedFile.ContentLength);

                using (MagickImage image = new MagickImage(bytes))
                {
                    image.Format = image.Format;
                    image.Resize(540, 540);
                    image.Quality = 50;
                    var memStream = new MemoryStream();
                    image.Write(memStream);   //image.Write(@"C:\Users\krish\OneDrive\Desktop\lowSize\YourFinalImage.jpg");
                    bytes = image.ToByteArray();
                }
                decimal size = Convert.ToInt32(bytes.Length);
                size = Convert.ToDecimal(Math.Round(size / 1024) / 1024);
                if (size > 1)
                {
                    string Error = "Image size is bigger";
                    lblMsg.Visible = true;
                    lblMsg.Text = Error;
                    return;
                }

                MySqlParameter[] msp = new MySqlParameter[9];
                msp[0] = new MySqlParameter("p_Name", MySqlDbType.VarChar);
                msp[1] = new MySqlParameter("p_Email", MySqlDbType.VarChar);
                msp[2] = new MySqlParameter("p_Password", MySqlDbType.VarChar);
                msp[3] = new MySqlParameter("p_city", MySqlDbType.VarChar);
                msp[4] = new MySqlParameter("p_dob", MySqlDbType.VarChar);
                msp[5] = new MySqlParameter("p_gender", MySqlDbType.Int16);
                msp[6] = new MySqlParameter("p_lookFor", MySqlDbType.Int16);
                msp[7] = new MySqlParameter("p_about", MySqlDbType.VarChar);
                msp[8] = new MySqlParameter("p_image1", MySqlDbType.MediumBlob);

                msp[0].Value = txtName.Text.Trim();
                msp[1].Value = email;
                msp[2].Value = Session["password"].ToString();
                msp[3].Value = txtCity.Text.Trim();
                msp[4].Value = txtDob.Text.Trim();
                if (ddlGen.SelectedValue == "Female")
                {
                    msp[5].Value = 0;
                }
                else
                {
                    msp[5].Value = 1;
                }
                if (ddlLookingFor.SelectedValue == "Friend")
                {
                    msp[6].Value = 0;
                }
                else if (ddlLookingFor.SelectedValue == "Date")
                {
                    msp[6].Value = 1;
                }
                else
                {
                    msp[6].Value = 2;
                }
                msp[7].Value = txtAbout.Text.Trim();
                msp[8].Value = bytes;

                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.Connection = con;
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandText = "Pro_User_Insert";
                cmd2.Parameters.AddRange(msp);
                con.Open();
                cmd2.ExecuteNonQuery();
            }
            catch
            {
                string Error = "Issue with user uploadind data, Please try later.";
                lblMsg.Visible = true;
                lblMsg.Text = Error;
            }
            finally
            {
                con.Close();
            }
        }
        protected int BindUserId(string emailId)
        {
            int id=0;
            try
            {
                con.Open();
                MySqlDataReader dr=null;
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT id FROM mySite.users where email='"+ emailId + "'";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    id = Convert.ToInt32(dr["id"]);
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
            return id;
        }
        private void UploadInterests(int id)
        {
            try
            {
                MySqlParameter[] msp = new MySqlParameter[23];
                msp[0] = new MySqlParameter("p_id", MySqlDbType.Int16);
                msp[1] = new MySqlParameter("p_reading", MySqlDbType.Int16);
                msp[2] = new MySqlParameter("p_trekking", MySqlDbType.Int16);
                msp[3] = new MySqlParameter("p_hiking", MySqlDbType.Int16);
                msp[4] = new MySqlParameter("p_singing", MySqlDbType.Int16);
                msp[5] = new MySqlParameter("p_dancing", MySqlDbType.Int16);
                msp[6] = new MySqlParameter("p_listenMusic", MySqlDbType.Int16);
                msp[7] = new MySqlParameter("p_gardening", MySqlDbType.Int16);
                msp[8] = new MySqlParameter("p_cooking", MySqlDbType.Int16);
                msp[9] = new MySqlParameter("p_gym", MySqlDbType.Int16);
                msp[10] = new MySqlParameter("p_foodie", MySqlDbType.Int16);
                msp[11] = new MySqlParameter("p_travelling", MySqlDbType.Int16);
                msp[12] = new MySqlParameter("p_art", MySqlDbType.Int16);
                msp[13] = new MySqlParameter("p_photography", MySqlDbType.Int16);
                msp[14] = new MySqlParameter("p_teaching", MySqlDbType.Int16);
                msp[15] = new MySqlParameter("p_technology", MySqlDbType.Int16);
                msp[16] = new MySqlParameter("p_coding", MySqlDbType.Int16);
                msp[17] = new MySqlParameter("p_petCaring", MySqlDbType.Int16);
                msp[18] = new MySqlParameter("p_outdoorGaming", MySqlDbType.Int16);
                msp[19] = new MySqlParameter("p_indoorGaming", MySqlDbType.Int16);
                msp[20] = new MySqlParameter("p_fashion", MySqlDbType.Int16);
                msp[21] = new MySqlParameter("p_nightLife", MySqlDbType.Int16);
                msp[22] = new MySqlParameter("p_daylife", MySqlDbType.Int16);

                msp[0].Value = id;
                if (lstInterst.Contains("reading")){ msp[1].Value = 1; }
                            else { msp[1].Value = 0; }
                if (lstInterst.Contains("trekking")) { msp[2].Value = 1; }
                            else { msp[2].Value = 0; }
                if (lstInterst.Contains("hiking"))
                {
                    msp[3].Value = 1;
                }
                else
                {
                    msp[3].Value = 0;
                }
                if (lstInterst.Contains("singing"))
                {
                    msp[4].Value = 1;
                }
                else
                {
                    msp[4].Value = 0;
                }
                if (lstInterst.Contains("dancing"))
                {
                    msp[5].Value = 1;
                }
                else
                {
                    msp[5].Value = 0;
                }
                if (lstInterst.Contains("listenMusic"))
                {
                    msp[6].Value = 1;
                }
                else
                {
                    msp[6].Value = 0;
                }
                if (lstInterst.Contains("gardening"))
                {
                    msp[7].Value = 1;
                }
                else
                {
                    msp[7].Value = 0;
                }
                if (lstInterst.Contains("cooking"))
                {
                    msp[8].Value = 1;
                }
                else
                {
                    msp[8].Value = 0;
                }
                if (lstInterst.Contains("gym"))
                {
                    msp[9].Value = 1;
                }
                else
                {
                    msp[9].Value = 0;
                }
                if (lstInterst.Contains("foodie"))
                {
                    msp[10].Value = 1;
                }
                else
                {
                    msp[10].Value = 0;
                }
                if (lstInterst.Contains("travelling"))
                {
                    msp[11].Value = 1;
                }
                else
                {
                    msp[11].Value = 0;
                }
                if (lstInterst.Contains("art"))
                {
                    msp[12].Value = 1;
                }
                else
                {
                    msp[12].Value = 0;
                }
                if (lstInterst.Contains("photography"))
                {
                    msp[13].Value = 1;
                }
                else
                {
                    msp[13].Value = 0;
                }
                if (lstInterst.Contains("teaching"))
                {
                    msp[14].Value = 1;
                }
                else
                {
                    msp[14].Value = 0;
                }
                if (lstInterst.Contains("technology"))
                {
                    msp[15].Value = 1;
                }
                else
                {
                    msp[15].Value = 0;
                }
                if (lstInterst.Contains("coding"))
                {
                    msp[16].Value = 1;
                }
                else
                {
                    msp[16].Value = 0;
                }
                if (lstInterst.Contains("petCaring"))
                {
                    msp[17].Value = 1;
                }
                else
                {
                    msp[17].Value = 0;
                }
                if (lstInterst.Contains("outdoorGaming"))
                {
                    msp[18].Value = 1;
                }
                else
                {
                    msp[18].Value = 0;
                }
                if (lstInterst.Contains("indoorGaming"))
                {
                    msp[19].Value = 1;
                }
                else
                {
                    msp[19].Value = 0;
                }
                if (lstInterst.Contains("fashion"))
                {
                    msp[20].Value = 1;
                }
                else
                {
                    msp[20].Value = 0;
                }
                if (lstInterst.Contains("nightLife"))
                {
                    msp[21].Value = 1;
                }
                else
                {
                    msp[21].Value = 0;
                }
                if (lstInterst.Contains("daylife"))
                {
                    msp[22].Value = 1;
                }
                else
                {
                    msp[22].Value = 0;
                }

                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.Connection = con;
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandText = "Pro_UserInterests_Insert";
                cmd2.Parameters.AddRange(msp);
                con.Open();
                cmd2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "alert('Technical issue to save your details.');", true);
            }
            finally
            {
                con.Close();
            }
        }
        private void StoreValueInCookies(string key, string value)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        private string GetLocation()
        {
            string loc = "";
            string apiUrl = "http://ipinfo.io/json?token=1e12d734abdbd0";
            HttpClient client = new HttpClient();
            try
            {
                WebRequest request = WebRequest.Create(apiUrl);
                request.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                response.Close();
                JavaScriptSerializer js = new JavaScriptSerializer();
                IPInfo userinfo = js.Deserialize<IPInfo>(responseFromServer);
                string ip = userinfo.Ip;
                string Hostname = userinfo.Hostname;
                string city = userinfo.City;
                string Region = userinfo.Region;
                string Country = userinfo.Country;
                loc = Country + " - " + city;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return loc;
        }
    }
    public class Tokenclass
    {
        public string access_token
        {
            get;
            set;
        }
        public string token_type
        {
            get;
            set;
        }
        public int expires_in
        {
            get;
            set;
        }
        public string refresh_token
        {
            get;
            set;
        }
    }
    public class Userclass
    {
        public string id
        {
            get;
            set;
        }
        public string name
        {
            get;
            set;
        }
        public string given_name
        {
            get;
            set;
        }
        public string family_name
        {
            get;
            set;
        }
        public string link
        {
            get;
            set;
        }
        public string picture
        {
            get;
            set;
        }
        public string gender
        {
            get;
            set;
        }
        public string locale
        {
            get;
            set;
        }
        public string email
        {
            get;
            set;
        }
    }
    public class IPInfo
    {
        public string Ip { get; set; }
        public string Hostname { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
    }
}