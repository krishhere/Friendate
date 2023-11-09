using System;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web.UI;
using System.Web;
using System.Web.UI.HtmlControls;

namespace WebApplication
{
    public partial class Requests : System.Web.UI.Page
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString; 
        string[] arryInterests;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataTable dt = BindData();
                //Session["requestDT"] = dt;
                rptUsers.DataSource = dt;
                rptUsers.DataBind();
            }
        }
        private DataTable BindData()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                int profileId = 0;
                if (HttpContext.Current.Request.Cookies["salngId"] != null)
                {
                    profileId = Convert.ToInt32(HttpContext.Current.Request.Cookies["salngId"].Value);
                }
                string query = $"SELECT * FROM (select * from (with cte as(SELECT u.id id,Name,IF(u.gender=1,'Male','Female') AS gender,(YEAR(curdate())-right(dob,4)) as age,city,about,image1,IF(i.reading=1,'Reading',NULL) AS reading,IF(i.trekking=1,'Trekking',NULL) AS trekking,IF(i.hiking=1,'Hiking',NULL) AS hiking,IF(i.singing=1,'Singing',NULL) AS singing,IF(i.dancing=1,'Dancing',NULL) AS dancing,IF(i.listenMusic=1,'Music lover',NULL) AS listenMusic,IF(i.gardening=1,'Gardening',NULL) AS gardening,IF(i.cooking=1,'Cooking',NULL) AS cooking,IF(i.gym=1,'Gym',NULL) AS gym,IF(i.foodie=1,'Foodie',NULL) AS foodie,IF(i.travelling=1,'travelling',NULL) AS travelling,IF(i.art=1,'Artist',NULL) AS art,IF(i.photography=1,'Photography',NULL) AS photography,IF(i.teaching=1,'Teaching',NULL) AS teaching,IF(i.technology=1,'Technology',NULL) AS technology,IF(i.coding=1,'Coding',NULL) AS coding,IF(i.petCaring=1,'Pet caring',NULL) AS petCaring,IF(i.outdoorGaming=1,'Outdoor gaming',NULL) AS outdoorGaming,IF(i.indoorGaming=1,'Indoor gaming',NULL) AS indoorGaming,IF(i.fashion=1,'Fashion',NULL) AS fashion,IF(i.nightLife=1,'Night life',NULL) AS nightLife,IF(i.daylife=1,'Day life',NULL) AS daylife,IF(i.investing=1,'Investing',NULL) AS investing,IF(i.business=1,'Business',NULL) AS business FROM mySite.users u INNER JOIN mySite.interest i ON u.id = i.id) SELECT id as senderId,d.userId as ReceiverId,Name,gender,age,city,about,image1,CONCAT_WS(',', reading,trekking,hiking,singing,dancing,listenMusic,gardening,cooking,gym,foodie,travelling,art,photography,teaching,technology,coding,petCaring,outdoorGaming,indoorGaming,fashion,nightLife,daylife,investing,business) AS interests ,case WHEN d.userid is not null THEN 'Date' ELSE 'Friend' end as RequestType FROM cte c inner join DateRequest d on c.id=d.profileId) as t1 union select * from (with cte2 as(SELECT u.id id,Name,IF(u.gender=1,'Male','Female') AS gender,(YEAR(curdate())-right(dob,4)) as age,city,about,image1,IF(i.reading=1,'Reading',NULL) AS reading,IF(i.trekking=1,'Trekking',NULL) AS trekking,IF(i.hiking=1,'Hiking',NULL) AS hiking,IF(i.singing=1,'Singing',NULL) AS singing,IF(i.dancing=1,'Dancing',NULL) AS dancing,IF(i.listenMusic=1,'Music lover',NULL) AS listenMusic,IF(i.gardening=1,'Gardening',NULL) AS gardening,IF(i.cooking=1,'Cooking',NULL) AS cooking,IF(i.gym=1,'Gym',NULL) AS gym,IF(i.foodie=1,'Foodie',NULL) AS foodie,IF(i.travelling=1,'travelling',NULL) AS travelling,IF(i.art=1,'Artist',NULL) AS art,IF(i.photography=1,'Photography',NULL) AS photography,IF(i.teaching=1,'Teaching',NULL) AS teaching,IF(i.technology=1,'Technology',NULL) AS technology,IF(i.coding=1,'Coding',NULL) AS coding,IF(i.petCaring=1,'Pet caring',NULL) AS petCaring,IF(i.outdoorGaming=1,'Outdoor gaming',NULL) AS outdoorGaming,IF(i.indoorGaming=1,'Indoor gaming',NULL) AS indoorGaming,IF(i.fashion=1,'Fashion',NULL) AS fashion,IF(i.nightLife=1,'Night life',NULL) AS nightLife,IF(i.daylife=1,'Day life',NULL) AS daylife,IF(i.investing=1,'Investing',NULL) AS investing,IF(i.business=1,'Business',NULL) AS business FROM mySite.users u INNER JOIN mySite.interest i ON u.id = i.id) SELECT id as senderId,f.userid as ReceiverId,Name,gender,age,city,about,image1,CONCAT_WS(',', reading,trekking,hiking,singing,dancing,listenMusic,gardening,cooking,gym,foodie,travelling,art,photography,teaching,technology,coding,petCaring,outdoorGaming,indoorGaming,fashion,nightLife,daylife,investing,business) AS interests ,case WHEN f.userid is not null THEN 'Friend' ELSE 'Date' end as RequestType FROM cte2 c2 inner join FriendRequest f on c2.id=f.profileId) as t2 ) AS result WHERE ReceiverId={profileId}";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        protected void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Image imgPost = e.Item.FindControl("imgSenderPost") as Image;
                DataRowView drimg = (DataRowView)e.Item.DataItem;
                if (!Convert.IsDBNull(drimg["image1"]))
                {
                    imgPost.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])drimg["image1"]);
                }
                //Label LookFor = e.Item.FindControl("lblRequestedFor") as Label;
                //DataRowView dr = (DataRowView)e.Item.DataItem;
                //string value = "Requested for ";
                    
                //int i = Convert.ToInt32(dr["RequestType"].ToString());
                //if (i == 0)
                //{
                //    value = value + "friend";
                //}
                //else if (i == 1)
                //{
                //    value = value + "date";
                //}
                //else
                //{
                //    value = value + "friend or date";
                //}
                //LookFor.Text = value;
                
                Repeater rptInterest = e.Item.FindControl("rptUserInterests") as Repeater;
                DataRowView drInterest = (DataRowView)e.Item.DataItem;
                string strInterests = drInterest["interests"].ToString();
                arryInterests = strInterests.Split(',');
                rptInterest.DataSource = arryInterests;
                rptInterest.DataBind();

                //int profileId = Convert.ToInt32(HttpContext.Current.Request.Cookies["salngId"].Value);
                //int senderId = Convert.ToInt32((e.Item.FindControl("lblId") as Label).Text);
                //string queryFriend = $"select * from FriendRequest where profileid={profileId} and userId={userId}";
                //bool flag = IsRequest(queryFriend);
                //if (flag == true)
                //{
                //    LinkButton linkButtonFriend = e.Item.FindControl("lnkFriend") as LinkButton;
                //    linkButtonFriend.Style["color"] = "#0047ff";
                //    LinkButton linkButtonDate = e.Item.FindControl("lnkDate") as LinkButton;
                //    linkButtonDate.Style["color"] = "#a1a1a8";
                //    linkButtonDate.Style["text-decoration"] = "line-through";
                //    linkButtonDate.Style["cursor"] = "not-allowed";
                //    linkButtonDate.Style["pointer-events"] = "none";
                //}
                //string queryDate = $"select * from DateRequest where profileid={profileId} and userId={userId}";
                //bool flag1 = IsRequest(queryDate);
                //if (flag1 == true)
                //{
                //    LinkButton linkButtonDate = e.Item.FindControl("lnkDate") as LinkButton;
                //    linkButtonDate.Style["color"] = "red";
                //    LinkButton linkButtonFriend = e.Item.FindControl("lnkFriend") as LinkButton;
                //    linkButtonFriend.Style["color"] = "#a1a1a8";
                //    linkButtonFriend.Style["text-decoration"] = "line-through";
                //    linkButtonFriend.Style["cursor"] = "not-allowed";
                //    linkButtonFriend.Style["pointer-events"] = "none";
                //}
            }
        }
        protected void rptUserInterests_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label LookFor = e.Item.FindControl("lblInterest") as Label;
            LookFor.Text = e.Item.DataItem.ToString();
        }
    }
}