using System;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

namespace WebApplication
{
    public partial class Default : System.Web.UI.Page
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);
        static DataTable dt;
        int maxCount;
        string[] arryInterests;
        List<string> listInterests = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                dt = BindTopics();

                rptUsers.DataSource = dt;
                rptUsers.DataBind();
                //maxCount = dt.Rows.Count;
                //if (maxCount > 0)
                //{
                //    GetOneDataRadomly();
                //}
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "swiperInit", "initSwiper();", true);
            }
        }
        private int random()
        {
            int num;
            Random random = new Random();
            num = random.Next(1, maxCount);
            return num;
        }
        protected DataTable BindTopics()
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "with cte as(SELECT u.id id,Name,gender,city,lookFor,about,image1,IF(i.reading=1,'Reading',NULL) AS reading,IF(i.trekking=1,'Trekking',NULL) AS trekking,IF(i.hiking=1,'Hiking',NULL) AS hiking,IF(i.singing=1,'Singing',NULL) AS singing,IF(i.dancing=1,'Dancing',NULL) AS dancing,IF(i.listenMusic=1,'Music lover',NULL) AS listenMusic,IF(i.gardening=1,'Gardening',NULL) AS gardening,IF(i.cooking=1,'Cooking',NULL) AS cooking,IF(i.gym=1,'Gym',NULL) AS gym,IF(i.foodie=1,'Foodie',NULL) AS foodie,IF(i.travelling=1,'travelling',NULL) AS travelling,IF(i.art=1,'Artist',NULL) AS art,IF(i.photography=1,'Photography',NULL) AS photography,IF(i.teaching=1,'Teaching',NULL) AS teaching,IF(i.technology=1,'Technology',NULL) AS technology,IF(i.coding=1,'Coding',NULL) AS coding,IF(i.petCaring=1,'Pet caring',NULL) AS petCaring,IF(i.outdoorGaming=1,'Outdoor gaming',NULL) AS outdoorGaming,IF(i.indoorGaming=1,'Indoor gaming',NULL) AS indoorGaming,IF(i.fashion=1,'Fashion',NULL) AS fashion,IF(i.nightLife=1,'Night life',NULL) AS nightLife,IF(i.daylife = 1, 'Day life', NULL) AS daylife FROM mySite.users u INNER JOIN mySite.interest i ON u.id = i.id) SELECT id,Name,gender,city,lookFor,about,image1,CONCAT_WS(',', reading,trekking,hiking,singing,dancing,listenMusic,gardening,cooking,gym,foodie,travelling,art,photography,teaching,technology,coding,petCaring,outdoorGaming,indoorGaming,fashion,nightLife,daylife) AS interests FROM cte";
                cmd.ExecuteNonQuery();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "alert('Technical issues. please try later');", true);
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        protected void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
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
                        //liLnkFriendItem.Style.Add("display", "contents");
                        //liLnkDateItem.Style.Add("display", "none");
                        lbFriend.Visible = true;
                        lbDate.Visible = false;
                    }
                    else if (i==1)
                    {
                        value = value + "date";
                        lbDate.Visible = true;
                        lbFriend.Visible = false;
                        //liLnkDateItem.Style.Add("display", "contents");
                        //liLnkFriendItem.Style.Add("display", "none");
                    }
                    else
                    {
                        value = value + "friend or date";
                        //liLnkFriendItem.Style.Add("display", "contents");
                        //liLnkDateItem.Style.Add("display", "contents");
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
                //foreach (string interest in arryInterests)
                //{
                //    listInterests.Add(interest);
                //}
                rptInterest.DataSource = arryInterests;
                rptInterest.DataBind();
            }
        }
        protected void rptUserInterests_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label LookFor = e.Item.FindControl("lblInterest") as Label;
            LookFor.Text = e.Item.DataItem.ToString();
        }
        private void GetOneDataRadomly()
        {
            int j = random();
            DataTable dataTable = dt.Clone();
            DataRow dr = dt.Rows[j];
            dataTable.Rows.Add(dr.ItemArray);
            rptUsers.DataSource = dataTable;
            rptUsers.DataBind();
        }
    }
}