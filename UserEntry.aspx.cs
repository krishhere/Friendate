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
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);

        string clientid = "784366017075-stgfbdmbddfkci4gm7esiqqd5uov31k8.apps.googleusercontent.com";
        string clientsecret = "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";
        string redirection_url = "https://localhost:44329/UserEntry.aspx";
        string url = "https://accounts.google.com/o/oauth2/token";
        string cookieName = "salngName";
        string cookieEmail = "salngEmail";
        static string city;
        static string name;
        static string email;
        List<string> lstInterst = new List<string>();
        bool flag = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lstInterst.Clear();
                if (Request.QueryString["code"] != null)
                {
                    GetToken(Request.QueryString["code"].ToString());
                }
                if (HttpContext.Current.Request.Cookies[cookieName] == null && HttpContext.Current.Request.Cookies[cookieEmail] == null)
                {
                    RemoveValueInCookies("salngEmail");
                    RemoveValueInCookies("salngName");
                    RemoveValueInCookies("salngId");
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    //string id = HttpContext.Current.Request.Cookies["salngId"].Value;
                    //BindUserId(HttpContext.Current.Request.Cookies[cookieEmail].Value);
                    //BindUserInterests(id);
                }
                //city = GetLocation();
                city = "Hyd";
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
                bool flag = false;
                if(imgType.Contains("jpeg") || imgType.Contains("application/octet-stream"))
                {
                    flag = true;
                }
                if (txtDob.Text.Trim() != "" && txtAbout.Text.Trim() != "" && flag==true)
                {
                    int id = Convert.ToInt32(HttpContext.Current.Request.Cookies["salngId"].Value);
                    UserInsert(id);
                    Thread.Sleep(500);
                    UploadInterests(id);
                    StoreValueInCookies("salngName", txtName.Text.Trim());
                    Response.Redirect("Default.aspx", false);
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
        }
        private void UserInsert(int id)
        {
            try
            {
                byte[] bytes;
                //BinaryReader br = new BinaryReader(FileUpload1.PostedFile.InputStream);
                //bytes = br.ReadBytes(FileUpload1.PostedFile.ContentLength);

                string base64ImageData = hdnCroppedImageData.Value.Split(',')[1];
                byte[] imageBytes = Convert.FromBase64String(base64ImageData);
                int x = 300;
                int y = 300;
                using (MagickImage image = new MagickImage(imageBytes))
                {
                    image.Format = image.Format;
                    //image.Resize(x, y);
                    image.Quality = 100;
                    var memStream = new MemoryStream();
                    image.Write(memStream);   //image.Write(@"C:\Users\krish\OneDrive\Desktop\lowSize\YourFinalImage.jpg");
                    bytes = image.ToByteArray();
                }

                decimal size = Convert.ToInt32(bytes.Length);
                size = Convert.ToDecimal(Math.Round(size / 1024) / 1024);
                if (size > 3)
                {
                    string Error = "Image size is bigger";
                    lblMsg.Visible = true;
                    lblMsg.Text = Error;
                    return;
                }

                MySqlParameter[] msp = new MySqlParameter[8];
                msp[0] = new MySqlParameter("p_name", MySqlDbType.VarChar);
                msp[1] = new MySqlParameter("p_city", MySqlDbType.VarChar);
                msp[2] = new MySqlParameter("p_dob", MySqlDbType.VarChar);
                msp[3] = new MySqlParameter("p_gender", MySqlDbType.Int16);
                msp[4] = new MySqlParameter("p_lookFor", MySqlDbType.Int16);
                msp[5] = new MySqlParameter("p_about", MySqlDbType.VarChar);
                msp[6] = new MySqlParameter("p_image1", MySqlDbType.MediumBlob);
                msp[7] = new MySqlParameter("p_id", MySqlDbType.Int64);

                msp[0].Value = txtName.Text.Trim();
                msp[1].Value = txtCity.Text.Trim();
                msp[2].Value = txtDob.Text.Trim();
                msp[3].Value = ddlGen.SelectedValue == "female" ? 0 : 1;
                if (ddlLookingFor.SelectedValue == "Friend")
                {
                    msp[4].Value = 0;
                }
                else if (ddlLookingFor.SelectedValue == "Date")
                {
                    msp[4].Value = 1;
                }
                else
                {
                    msp[4].Value = 2;
                }
                msp[5].Value = txtAbout.Text.Trim();
                msp[6].Value = bytes;
                msp[7].Value = id;

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Pro_User_Insert";
                cmd.Parameters.AddRange(msp);
                con.Open();
                cmd.ExecuteNonQuery();

                string query = "insert into interest(id) values(@p_id)";
                MySqlCommand cmd2 = new MySqlCommand(query, con);
                cmd2.Parameters.AddWithValue("@p_id", id);
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
        protected Tuple<int,bool> BindUserId(string emailId)
        {
            Tuple<int, bool> tuple =null;
            try
            {
                con.Open();
                MySqlDataReader dr=null;
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT id,Name,city,dob,gender,lookFor,about,image1 FROM mySite.users where email='"+ emailId + "'";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    bool flag = Session["edit"]?.ToString() == "EditProfile" ? true : false;
                    int id= Convert.ToInt32(dr["id"]);
                    txtName.Text = dr["Name"]?.ToString();
                    txtCity.Text = dr["city"]?.ToString();
                    txtDob.Text = dr["dob"]?.ToString();
                    string gender = dr["gender"]?.ToString();
                    if (gender == "1")
                    {
                        ddlGen.Items.FindByValue("male").Selected = true;
                    }
                    else
                    {
                        ddlGen.Items.FindByValue("female").Selected = true;
                    }
                    if(flag == true)
                    {
                        ddlGen.Attributes.Add("disabled", "disabled");
                        ddlGen.CssClass = "form-select";
                        txtName.Enabled = false;
                        txtName.CssClass = "form-control";
                        txtDob.Enabled = false;
                        txtDob.CssClass = "form-control";
                    }
                    string lookforValue = dr["lookfor"]?.ToString();
                    if (lookforValue == "0")
                    {
                        ddlLookingFor.Items.FindByValue("Friend").Selected = true;
                    }
                    else if(lookforValue == "1")
                    {
                        ddlLookingFor.Items.FindByValue("Date").Selected = true;
                    }
                    else
                    {
                        ddlLookingFor.Items.FindByValue("Both").Selected = true;
                    }
                    txtAbout.Text = dr["about"]?.ToString();
                    bool isImage= Convert.IsDBNull(dr["image1"]);
                    if (isImage==false)
                    {
                        imgCrop.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])dr["image1"]);
                    }
                    tuple = new Tuple<int, bool>(id, isImage);
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
            return tuple;
        }
        protected void BindUserInterests(string value)
        {
            try
            {
                con.Open();
                MySqlDataReader dr = null;
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "with cte as(SELECT id,IF(i.reading=1,'Reading',NULL) AS reading,IF(i.trekking=1,'Trekking',NULL) AS trekking,IF(i.hiking=1,'Hiking',NULL) AS hiking,IF(i.singing=1,'Singing',NULL) AS singing,IF(i.dancing=1,'Dancing',NULL) AS dancing,IF(i.listenMusic=1,'Music lover',NULL) AS listenMusic,IF(i.gardening=1,'Gardening',NULL) AS gardening,IF(i.cooking=1,'Cooking',NULL) AS cooking,IF(i.fitness=1,'Gym',NULL) AS fitness,IF(i.foodie=1,'Foodie',NULL) AS foodie,IF(i.travelling=1,'travelling',NULL) AS travelling,IF(i.art=1,'Artist',NULL) AS art,IF(i.photography=1,'Photography',NULL) AS photography,IF(i.teaching=1,'Teaching',NULL) AS teaching,IF(i.technology=1,'Technology',NULL) AS technology,IF(i.coding=1,'Coding',NULL) AS coding,IF(i.petCaring=1,'Pet caring',NULL) AS petCaring,IF(i.outdoorGaming=1,'Outdoor gaming',NULL) AS outdoorGaming,IF(i.indoorGaming=1,'Indoor gaming',NULL) AS indoorGaming,IF(i.fashion=1,'Fashion',NULL) AS fashion,IF(i.nightLife=1,'Night life',NULL) AS nightLife,IF(i.daylife=1,'Day life',NULL) AS daylife,IF(i.investing=1,'Investment',NULL) AS investing,IF(i.business=1,'Business',NULL) AS business FROM mySite.interest i) SELECT id,CONCAT_WS(',', reading,trekking,hiking,singing,dancing,listenMusic,gardening,cooking,fitness,foodie,travelling,art,photography,teaching,technology,coding,petCaring,outdoorGaming,indoorGaming,fashion,nightLife,daylife,investing,business) AS interests FROM cte where id=" + value+ "";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblInterests.Visible = true;
                    lblInterests.Text = dr["interests"]?.ToString()==null?"": "your interests-"+ dr["interests"]?.ToString();
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
        private void updateUserDetails()
        {

        }
        private void UploadInterests(int id)
        {
            try
            {
                MySqlParameter[] msp = new MySqlParameter[30];
                msp[0] = new MySqlParameter("@p_reading", MySqlDbType.Int16);
                msp[1] = new MySqlParameter("@p_trekking", MySqlDbType.Int16);
                msp[2] = new MySqlParameter("@p_hiking", MySqlDbType.Int16);
                msp[3] = new MySqlParameter("@p_singing", MySqlDbType.Int16);
                msp[4] = new MySqlParameter("@p_dancing", MySqlDbType.Int16);
                msp[5] = new MySqlParameter("@p_listenMusic", MySqlDbType.Int16);
                msp[6] = new MySqlParameter("@p_gardening", MySqlDbType.Int16);
                msp[7] = new MySqlParameter("@p_cooking", MySqlDbType.Int16);
                msp[8] = new MySqlParameter("@p_fitness", MySqlDbType.Int16);
                msp[9] = new MySqlParameter("@p_foodie", MySqlDbType.Int16);
                msp[10] = new MySqlParameter("@p_travelling", MySqlDbType.Int16);
                msp[11] = new MySqlParameter("@p_art", MySqlDbType.Int16);
                msp[12] = new MySqlParameter("@p_photography", MySqlDbType.Int16);
                msp[13] = new MySqlParameter("@p_teaching", MySqlDbType.Int16);
                msp[14] = new MySqlParameter("@p_technology", MySqlDbType.Int16);
                msp[15] = new MySqlParameter("@p_coding", MySqlDbType.Int16);
                msp[16] = new MySqlParameter("@p_petCaring", MySqlDbType.Int16);
                msp[17] = new MySqlParameter("@p_outdoorGaming", MySqlDbType.Int16);
                msp[18] = new MySqlParameter("@p_indoorGaming", MySqlDbType.Int16);
                msp[19] = new MySqlParameter("@p_fashion", MySqlDbType.Int16);
                msp[20] = new MySqlParameter("@p_nightLife", MySqlDbType.Int16);
                msp[21] = new MySqlParameter("@p_daylife", MySqlDbType.Int16);
                msp[22] = new MySqlParameter("@p_investment", MySqlDbType.Int16);
                msp[23] = new MySqlParameter("@p_business", MySqlDbType.Int16);
                msp[24] = new MySqlParameter("@p_movies", MySqlDbType.Int16);
                msp[25] = new MySqlParameter("@p_shopping", MySqlDbType.Int16);
                msp[26] = new MySqlParameter("@p_roadtrips", MySqlDbType.Int16);
                msp[27] = new MySqlParameter("@p_politics", MySqlDbType.Int16);
                msp[28] = new MySqlParameter("@p_chillatbar", MySqlDbType.Int16);
                msp[29] = new MySqlParameter("@p_id", MySqlDbType.Int64);

                msp[0].Value = lstInterst.Contains("reading") ? 1 : 0;
                msp[1].Value = lstInterst.Contains("trekking") ? 1 : 0;
                msp[2].Value = lstInterst.Contains("hiking") ? 1 : 0;
                msp[3].Value = lstInterst.Contains("singing") ? 1 : 0;
                msp[4].Value = lstInterst.Contains("dancing") ? 1 : 0;
                msp[5].Value = lstInterst.Contains("listenMusic") ? 1 : 0;
                msp[6].Value = lstInterst.Contains("gardening") ? 1 : 0;
                msp[7].Value = lstInterst.Contains("cooking") ? 1 : 0;
                msp[8].Value = lstInterst.Contains("fitness") ? 1 : 0;
                msp[9].Value = lstInterst.Contains("foodie") ? 1 : 0;
                msp[10].Value = lstInterst.Contains("travelling") ? 1 : 0;
                msp[11].Value = lstInterst.Contains("art") ? 1 : 0;
                msp[12].Value = lstInterst.Contains("photography") ? 1 : 0;
                msp[13].Value = lstInterst.Contains("teaching") ? 1 : 0;
                msp[14].Value = lstInterst.Contains("technology") ? 1 : 0;
                msp[15].Value = lstInterst.Contains("coding") ? 1 : 0;
                msp[16].Value = lstInterst.Contains("petCaring") ? 1 : 0;
                msp[17].Value = lstInterst.Contains("outdoorGaming") ? 1 : 0;
                msp[18].Value = lstInterst.Contains("indoorGaming") ? 1 : 0;
                msp[19].Value = lstInterst.Contains("fashion") ? 1 : 0;
                msp[20].Value = lstInterst.Contains("nightLife") ? 1 : 0;
                msp[21].Value = lstInterst.Contains("daylife") ? 1 : 0;
                msp[22].Value = lstInterst.Contains("investment") ? 1 : 0;
                msp[23].Value = lstInterst.Contains("business") ? 1 : 0;
                msp[24].Value = lstInterst.Contains("movies") ? 1 : 0;
                msp[25].Value = lstInterst.Contains("shopping") ? 1 : 0;
                msp[26].Value = lstInterst.Contains("roadtrips") ? 1 : 0;
                msp[27].Value = lstInterst.Contains("politics") ? 1 : 0;
                msp[28].Value = lstInterst.Contains("chillatbar") ? 1 : 0;
                msp[29].Value = id;

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Pro_UserInterests_Insert";
                cmd.Parameters.AddRange(msp);
                con.Open();
                cmd.ExecuteNonQuery();
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
        protected void txtName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string query = "select COUNT(Name) from users where Name=@pName";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@pName", txtName.Text.Trim());
                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    lblNameMsg.Text = "Profile name is existed, try again";
                    lblNameMsg.Style["color"] = "#e72525";
                }
                else
                {
                    lblNameMsg.Text= "Profile name is available";
                    lblNameMsg.Style["color"] = "#03ba00";
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
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