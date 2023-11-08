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
            if (!Page.IsPostBack)
            {
                maxCount = MaxRecord();
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
                    else if (i==1)
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
        private int random()
        {
            int num;
            Random random = new Random();
            num = random.Next(1, maxCount);
            return num;
        }
    }
}