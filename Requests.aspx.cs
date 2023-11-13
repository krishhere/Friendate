using System;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web.UI;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Configuration;

namespace WebApplication
{
    public partial class Requests : System.Web.UI.Page
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);
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
            DataTable dt = new DataTable();
            try
            {
                int profileId = 0;
                if (HttpContext.Current.Request.Cookies["salngId"] != null)
                {
                    profileId = Convert.ToInt32(HttpContext.Current.Request.Cookies["salngId"].Value);
                }
                //string query = $"SELECT * FROM (select * from (with cte as(SELECT u.id id,Name,IF(u.gender=1,'Male','Female') AS gender,(YEAR(curdate())-right(dob,4)) as age,city,about,image1,IF(i.reading=1,'Reading',NULL) AS reading,IF(i.trekking=1,'Trekking',NULL) AS trekking,IF(i.hiking=1,'Hiking',NULL) AS hiking,IF(i.singing=1,'Singing',NULL) AS singing,IF(i.dancing=1,'Dancing',NULL) AS dancing,IF(i.listenMusic=1,'Music lover',NULL) AS listenMusic,IF(i.gardening=1,'Gardening',NULL) AS gardening,IF(i.cooking=1,'Cooking',NULL) AS cooking,IF(i.gym=1,'Gym',NULL) AS gym,IF(i.foodie=1,'Foodie',NULL) AS foodie,IF(i.travelling=1,'travelling',NULL) AS travelling,IF(i.art=1,'Artist',NULL) AS art,IF(i.photography=1,'Photography',NULL) AS photography,IF(i.teaching=1,'Teaching',NULL) AS teaching,IF(i.technology=1,'Technology',NULL) AS technology,IF(i.coding=1,'Coding',NULL) AS coding,IF(i.petCaring=1,'Pet caring',NULL) AS petCaring,IF(i.outdoorGaming=1,'Outdoor gaming',NULL) AS outdoorGaming,IF(i.indoorGaming=1,'Indoor gaming',NULL) AS indoorGaming,IF(i.fashion=1,'Fashion',NULL) AS fashion,IF(i.nightLife=1,'Night life',NULL) AS nightLife,IF(i.daylife=1,'Day life',NULL) AS daylife,IF(i.investing=1,'Investing',NULL) AS investing,IF(i.business=1,'Business',NULL) AS business FROM mySite.users u INNER JOIN mySite.interest i ON u.id = i.id) SELECT id as senderId,d.userId as ReceiverId,Name,gender,age,city,about,image1,CONCAT_WS(',', reading,trekking,hiking,singing,dancing,listenMusic,gardening,cooking,gym,foodie,travelling,art,photography,teaching,technology,coding,petCaring,outdoorGaming,indoorGaming,fashion,nightLife,daylife,investing,business) AS interests ,case WHEN d.userid is not null THEN 'Date' ELSE 'Friend' end as RequestType FROM cte c inner join DateRequest d on c.id=d.profileId) as t1 union select * from (with cte2 as(SELECT u.id id,Name,IF(u.gender=1,'Male','Female') AS gender,(YEAR(curdate())-right(dob,4)) as age,city,about,image1,IF(i.reading=1,'Reading',NULL) AS reading,IF(i.trekking=1,'Trekking',NULL) AS trekking,IF(i.hiking=1,'Hiking',NULL) AS hiking,IF(i.singing=1,'Singing',NULL) AS singing,IF(i.dancing=1,'Dancing',NULL) AS dancing,IF(i.listenMusic=1,'Music lover',NULL) AS listenMusic,IF(i.gardening=1,'Gardening',NULL) AS gardening,IF(i.cooking=1,'Cooking',NULL) AS cooking,IF(i.gym=1,'Gym',NULL) AS gym,IF(i.foodie=1,'Foodie',NULL) AS foodie,IF(i.travelling=1,'travelling',NULL) AS travelling,IF(i.art=1,'Artist',NULL) AS art,IF(i.photography=1,'Photography',NULL) AS photography,IF(i.teaching=1,'Teaching',NULL) AS teaching,IF(i.technology=1,'Technology',NULL) AS technology,IF(i.coding=1,'Coding',NULL) AS coding,IF(i.petCaring=1,'Pet caring',NULL) AS petCaring,IF(i.outdoorGaming=1,'Outdoor gaming',NULL) AS outdoorGaming,IF(i.indoorGaming=1,'Indoor gaming',NULL) AS indoorGaming,IF(i.fashion=1,'Fashion',NULL) AS fashion,IF(i.nightLife=1,'Night life',NULL) AS nightLife,IF(i.daylife=1,'Day life',NULL) AS daylife,IF(i.investing=1,'Investing',NULL) AS investing,IF(i.business=1,'Business',NULL) AS business FROM mySite.users u INNER JOIN mySite.interest i ON u.id = i.id) SELECT id as senderId,f.userid as ReceiverId,Name,gender,age,city,about,image1,CONCAT_WS(',', reading,trekking,hiking,singing,dancing,listenMusic,gardening,cooking,gym,foodie,travelling,art,photography,teaching,technology,coding,petCaring,outdoorGaming,indoorGaming,fashion,nightLife,daylife,investing,business) AS interests ,case WHEN f.userid is not null THEN 'Friend' ELSE 'Date' end as RequestType FROM cte2 c2 inner join FriendRequest f on c2.id=f.profileId) as t2) AS result WHERE ReceiverId={profileId}";
                //using (MySqlCommand cmd = new MySqlCommand(query, con))
                //{
                //    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                //    {
                //        da.Fill(dt);
                //    }
                //}
                MySqlParameter[] msp = new MySqlParameter[1];
                msp[0] = new MySqlParameter("p_profileId", MySqlDbType.Int32);
                msp[0].Value = profileId;
                
                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.Connection = con;
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandText = "Pro_FriendDateRequests";
                cmd2.Parameters.AddRange(msp);
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd2);
                da.Fill(dt);
            }
            catch
            {

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
                Image imgPost = e.Item.FindControl("imgSenderPost") as Image;
                DataRowView drimg = (DataRowView)e.Item.DataItem;
                if (!Convert.IsDBNull(drimg["image1"]))
                {
                    imgPost.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])drimg["image1"]);
                }

                Label requestAccept = e.Item.FindControl("lblRequestAccept") as Label;
                string strrequestAccept = requestAccept.Text;
                if (strrequestAccept == "Friend Accepted" || strrequestAccept == "Date Accepted")
                {
                    Panel pnlAcceptReject = e.Item.FindControl("pnlAcceptReject") as Panel;
                    Panel pnlMsg = e.Item.FindControl("pnlMsg") as Panel;
                    pnlAcceptReject.Visible = false;
                    pnlMsg.Visible = true;
                }

                Repeater rptInterest = e.Item.FindControl("rptUserInterests") as Repeater;
                DataRowView drInterest = (DataRowView)e.Item.DataItem;
                string strInterests = drInterest["interests"].ToString();
                arryInterests = strInterests.Split(',');
                rptInterest.DataSource = arryInterests;
                rptInterest.DataBind();
            }
        }
        protected void rptUserInterests_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label LookFor = e.Item.FindControl("lblInterest") as Label;
            LookFor.Text = e.Item.DataItem.ToString();
        }
        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int profileId = Convert.ToInt32(HttpContext.Current.Request.Cookies["salngId"].Value);
            int userId = Convert.ToInt32((e.Item.FindControl("lblId") as Label).Text);
            string type = (e.Item.FindControl("lblRequestedFor") as Label).Text;

            Panel pnlAcceptReject = e.Item.FindControl("pnlAcceptReject") as Panel;
            Panel pnlMsg = e.Item.FindControl("pnlMsg") as Panel;

            if (e.CommandName == "Accept")
            {
                int acceptReject = 1;
                AcceptReject(userId, profileId,  acceptReject, type, pnlAcceptReject, pnlMsg);
            }
            else if (e.CommandName == "Reject")
            {
                int acceptReject = 0;
                AcceptReject(userId, profileId, acceptReject, type, pnlAcceptReject, pnlMsg);
            }
        }
        private void AcceptReject(int profileId, int userId, int acceptReject, string type, Panel pnlAcceptReject, Panel pnlMsg)
        {
            try
            {
                MySqlParameter[] msp = new MySqlParameter[4];
                msp[0] = new MySqlParameter("p_profileId", MySqlDbType.Int32);
                msp[1] = new MySqlParameter("p_userId", MySqlDbType.Int32);
                msp[2] = new MySqlParameter("P_acceptReject", MySqlDbType.Int32);
                msp[3] = new MySqlParameter("P_type", MySqlDbType.VarChar);

                msp[0].Value = profileId;
                msp[1].Value = userId;
                msp[2].Value = acceptReject;
                msp[3].Value = type;

                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.Connection = con;
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandText = "Pro_AcceptReject";
                cmd2.Parameters.AddRange(msp);
                con.Open();
                cmd2.ExecuteNonQuery();
                BindData();
                if (acceptReject != 0)
                {
                    pnlAcceptReject.Visible = false;
                    pnlMsg.Visible = true;
                }
                else
                {
                    Response.Redirect("Request.aspx");
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
    }
}