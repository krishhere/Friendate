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
    public partial class Default : System.Web.UI.Page
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);

        int maxCount;
        string[] arryInterests;
        protected void Page_Load(object sender, EventArgs e)
        {
            maxCount = MaxRecord();
            if (maxCount == 0)
            {
                RemoveValueInCookies("salngEmail");
                RemoveValueInCookies("salngName");
                RemoveValueInCookies("salngId");
                Response.Redirect("Login.aspx");
            }
            if (!Page.IsPostBack)
            {
                Data data = new Data();
                DataTable dt = data.GetRandomDataRecords(5);
                Session["DataTable"] = dt;
                rptUsers.DataSource = dt;
                rptUsers.DataBind();
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "swiperInit", "initSwiper();", true);
        }
        protected void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                try
                {
                    Image imgPost = e.Item.FindControl("imgUserPost") as Image;
                    DataRowView drimg = (DataRowView)e.Item.DataItem;
                    if (!Convert.IsDBNull(drimg["image1"]))
                    {
                        imgPost.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])drimg["image1"]);
                    }

                    Label LookFor = e.Item.FindControl("lblLookFor") as Label;
                    DataRowView dr = (DataRowView)e.Item.DataItem;
                    string value = "Looking for ";
                    if (!Convert.IsDBNull(dr["lookFor"]))
                    {
                        HtmlGenericControl liLnkFriendItem = (HtmlGenericControl)e.Item.FindControl("liLnkFriend");
                        HtmlGenericControl liLnkDateItem = (HtmlGenericControl)e.Item.FindControl("liLnkDate");
                        int i = Convert.ToInt32(dr["lookFor"].ToString());
                        LinkButton lbFriend = e.Item.FindControl("lnkFriend") as LinkButton;
                        LinkButton lbDate = e.Item.FindControl("lnkDate") as LinkButton;
                        if (i == 0)
                        {
                            value = value + "friend";
                            lbFriend.Visible = true;
                            lbDate.Visible = false;
                        }
                        else if (i == 1)
                        {
                            value = value + "date";
                            lbDate.Visible = true;
                            lbFriend.Visible = false;
                        }
                        else
                        {
                            value = value + "friend or date";
                            lbFriend.Visible = true;
                            lbDate.Visible = true;
                        }
                        LookFor.Text = value;
                    }
                    else
                    {
                        LookFor.Text = value + "fun";
                    }

                    Repeater rptInterest = e.Item.FindControl("rptUserInterests") as Repeater;
                    DataRowView drInterest = (DataRowView)e.Item.DataItem;
                    string strInterests = drInterest["interests"].ToString();
                    arryInterests = strInterests.Split(',');
                    rptInterest.DataSource = arryInterests;
                    rptInterest.DataBind();

                    int profileId = Convert.ToInt32(HttpContext.Current.Request.Cookies["salngId"].Value);
                    int userId = Convert.ToInt32((e.Item.FindControl("lblId") as Label).Text);
                    string queryFriend = $"select * from FriendRequest where profileid={profileId} and userId={userId}";
                    bool flag = IsRequest(queryFriend);
                    if (flag == true)
                    {
                        LinkButton linkButtonFriend = e.Item.FindControl("lnkFriend") as LinkButton;
                        linkButtonFriend.Style["color"] = "#0047ff";
                        LinkButton linkButtonDate = e.Item.FindControl("lnkDate") as LinkButton;
                        linkButtonDate.Style["color"] = "#a1a1a8";
                        linkButtonDate.Style["cursor"] = "not-allowed";
                        linkButtonDate.Style["pointer-events"] = "none";
                        linkButtonDate.Style["text-decoration"] = "line-through";
                        linkButtonDate.Style["cursor"] = "not-allowed";
                        linkButtonDate.Style["pointer-events"] = "none";
                    }
                    string queryDate = $"select * from DateRequest where profileid={profileId} and userId={userId}";
                    bool flag1 = IsRequest(queryDate);
                    if (flag1 == true)
                    {
                        LinkButton linkButtonDate = e.Item.FindControl("lnkDate") as LinkButton;
                        linkButtonDate.Style["color"] = "red";
                        LinkButton linkButtonFriend = e.Item.FindControl("lnkFriend") as LinkButton;
                        linkButtonFriend.Style["color"] = "#a1a1a8";
                        linkButtonFriend.Style["cursor"] = "not-allowed";
                        linkButtonFriend.Style["pointer-events"] = "none";
                        linkButtonFriend.Style["text-decoration"] = "line-through";
                        linkButtonFriend.Style["cursor"] = "not-allowed";
                        linkButtonFriend.Style["pointer-events"] = "none";
                    }
                }
                catch(Exception ex)
                {
                    string hghj = ex.Message;
                    if(ex.Message== "Object reference not set to an instance of an object.")
                    {
                        RemoveValueInCookies("salngEmail");
                        RemoveValueInCookies("salngName");
                        RemoveValueInCookies("salngId");
                        Response.Redirect("Login.aspx");
                    }
                }
            }
        }
        protected void rptUserInterests_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label LookFor = e.Item.FindControl("lblInterest") as Label;
            LookFor.Text = e.Item.DataItem.ToString();
        }
        protected int MaxRecord()
        {
            int cnt = 0;
            try
            {
                con.Open();
                MySqlDataReader dr = null;
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select max(id) as cnt from mySite.users";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    cnt = Convert.ToInt32(dr["cnt"]);
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
            return cnt;
        }
        protected void btnLoadMore_Click(object sender, EventArgs e)
        {
            DataTable existingData = (DataTable)Session["DataTable"];
            Session.Remove("DataTable");
            Data dataAccess = new Data();
            DataTable NewDt = dataAccess.GetRandomDataRecords(5);
            existingData.Merge(NewDt);
            Session["DataTable"] = existingData;
            rptUsers.DataSource = existingData;
            rptUsers.DataBind();
        }
        protected void lnkFriend_Click(object sender, EventArgs e)
        {
            RepeaterItem item = (sender as LinkButton).NamingContainer as RepeaterItem;

            int profileId = Convert.ToInt32(HttpContext.Current.Request.Cookies["salngId"].Value);
            int userId = Convert.ToInt32((item.FindControl("lblId") as Label).Text);
            FriendRequest(profileId,userId);
            LinkButton linkButtonFriend = item.FindControl("lnkFriend") as LinkButton;
            linkButtonFriend.Style["color"] = "#0047ff";
            linkButtonFriend.Style["cursor"] = "not-allowed";
            linkButtonFriend.Style["pointer-events"] = "none";
            LinkButton linkButtonDate = item.FindControl("lnkDate") as LinkButton;
            linkButtonDate.Style["color"] = "#a1a1a8";
            linkButtonDate.Style["text-decoration"] = "line-through";
            linkButtonDate.Style["cursor"] = "not-allowed";
            linkButtonDate.Style["pointer-events"] = "none";
        }
        protected void lnkDate_Click(object sender, EventArgs e)
        {
            int profileId = Convert.ToInt32(HttpContext.Current.Request.Cookies["salngId"].Value);
            RepeaterItem item = (sender as LinkButton).NamingContainer as RepeaterItem;
            int userId = Convert.ToInt32((item.FindControl("lblId") as Label).Text);
            DateRequest(profileId, userId);
            LinkButton linkButtonDate = item.FindControl("lnkDate") as LinkButton;
            linkButtonDate.Style["color"] = "red";
            LinkButton linkButtonFriend = item.FindControl("lnkFriend") as LinkButton;
            linkButtonFriend.Style["color"] = "#a1a1a8";
            linkButtonFriend.Style["cursor"] = "not-allowed";
            linkButtonFriend.Style["pointer-events"] = "none";
            linkButtonFriend.Style["text-decoration"] = "line-through";
            linkButtonFriend.Style["cursor"] = "not-allowed";
            linkButtonFriend.Style["pointer-events"] = "none";
        }
        private void FriendRequest(int profileId,int userId)
        {
            try
            {
                string queryFriend = $"select * from FriendRequest where profileid={profileId} and userId={userId}";
                bool flag = IsRequest(queryFriend);
                if (flag == true)
                {
                    return;
                }

                MySqlParameter[] msp = new MySqlParameter[2];
                msp[0] = new MySqlParameter("p_profileId", MySqlDbType.Int32);
                msp[1] = new MySqlParameter("p_userId", MySqlDbType.Int32);

                msp[0].Value = profileId;
                msp[1].Value = userId;

                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.Connection = con;
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandText = "Pro_FriendRequests";
                cmd2.Parameters.AddRange(msp);
                con.Open();
                cmd2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        private void DateRequest(int profileId, int userId)
        {
            try
            {
                string queryDate = $"select * from DateRequest where profileid={profileId} and userId={userId}";
                bool flag = IsRequest(queryDate);
                if (flag == true)
                {
                    return;
                }

                MySqlParameter[] msp = new MySqlParameter[2];
                msp[0] = new MySqlParameter("p_profileId", MySqlDbType.Int32);
                msp[1] = new MySqlParameter("p_userId", MySqlDbType.Int32);

                msp[0].Value = profileId;
                msp[1].Value = userId;

                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.Connection = con;
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandText = "Pro_DateRequests";
                cmd2.Parameters.AddRange(msp);
                con.Open();
                cmd2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        private bool IsRequest(string query)
        {
            bool flag = false;
            try
            {
                con.Open();
                MySqlDataReader dr = null;
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    flag = true;
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
            return flag;
        }
        private int random()
        {
            int num;
            Random random = new Random();
            num = random.Next(1, maxCount);
            return num;
        }
        private void RemoveValueInCookies(string key)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}