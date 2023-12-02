using System;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace WebApplication
{
    public partial class Signup : System.Web.UI.Page
    {
        MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString);
        static string date;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnMailConfirm_Click(object sender, EventArgs e)
        {
            int count = emailVerify();
            if (count > 0)
            {
                lblEmailMsg.Text = "Email is registered, please <a href='Login.aspx'>login</a>";
                lblEmailMsg.Style["color"] = "#e72525";
            }
            else
            {
                date = DateTime.Now.ToString("MMyydd");
                //Task task = new Task(sendMail);
                //task.Start();
                txtCode.Visible = true;
                btnCodeConfirm.Visible = true;
                txtEmail.Visible = false;
                btnMailConfirm.Visible = false;
            }
        }
        protected void btnCodeConfirm_Click(object sender, EventArgs e)
        {
            if (txtCode.Text.Trim() == date)
            {
                lblError.Text="";
                pnlMail.Visible = false;
                pnlPswd.Visible = true;
            }
        }
        private void sendMail()
        {
            try
            {
                using (MailMessage mm = new MailMessage("krishhere0@gmail.com", txtEmail.Text.Trim()))
                {
                    mm.Subject = "Heartbel confirmation code";
                    string data = "<html><head> <meta http-equiv='content-type' content='text/html; charset=utf-8'> <meta name='viewport' content='width=device-width, initial-scale=1.0;'> <meta name='format-detection' content='telephone=no'/> <style> body { margin: 0; padding: 0; min-width: 100%; width: 100% !important; height: 100% !important;} body, table, td, div, p, a { -webkit-font-smoothing: antialiased; text-size-adjust: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; line-height: 100%; } table, td { mso-table-lspace: 0pt; mso-table-rspace: 0pt; border-collapse: collapse !important; border-spacing: 0; } img { border: 0; line-height: 100%; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic; } #outlook a { padding: 0; } .ReadMsgBody { width: 100%; } .ExternalClass { width: 100%; } .ExternalClass, .ExternalClass p, .ExternalClass span, .ExternalClass font, .ExternalClass td, .ExternalClass div { line-height: 100%; } @media all and (min-width: 560px) { .container { border-radius: 8px; -webkit-border-radius: 8px; -moz-border-radius: 8px; -khtml-border-radius: 8px;} } a, a:hover { color: #127DB3; } .footer a, .footer a:hover { color: #999999; } </style> <title>Get this responsive email template</title> </head> <body topmargin='0' rightmargin='0' bottommargin='0' leftmargin='0' marginwidth='0' marginheight='0' width='100%' style='border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; width: 100%; height: 100%; -webkit-font-smoothing: antialiased; text-size-adjust: 100%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; line-height: 100%;background-color: #F0F0F0;color: #000000;'bgcolor='#F0F0F0'text='#000000'> <table width='100%' align='center' border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; width: 100%;' class='background'><tr><td align='center' valign='top' style='border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0;' bgcolor='#F0F0F0'> <table border='0' cellpadding='0' cellspacing='0' align='center' width='560' style='border-collapse: collapse; border-spacing: 0; padding: 0; width: inherit; max-width: 560px;' class='wrapper'> <tr> <td align='center' valign='top' style='border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-left: 6.25%; padding-right: 6.25%; width: 87.5%; padding-top: 20px; padding-bottom: 20px;'> <div style='display: none; visibility: hidden; overflow: hidden; opacity: 0; font-size: 1px; line-height: 1px; height: 0; max-height: 0; max-width: 0; color: #F0F0F0;' class='preheader'> Available on&nbsp;GitHub and&nbsp;CodePen. Highly compatible. Designer friendly. More than 50%&nbsp;of&nbsp;total email opens occurred on&nbsp;a&nbsp;mobile device&nbsp;— a&nbsp;mobile-friendly design is&nbsp;a&nbsp;must for&nbsp;email campaigns.</div> </td> </tr> </table> <table border='0' cellpadding='0' cellspacing='0' align='center' bgcolor='#FFFFFF' width='560' style='border-collapse: collapse; border-spacing: 0; padding: 0; width: inherit;max-width: 560px;' class='container'> <tr> <td align='center' valign='top' style='border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-left: 6.25%; padding-right: 6.25%; width: 87.5%; font-size: 24px; font-weight: bold; line-height: 130%;padding-top: 25px;color: #000000;font-family: sans-serif;' class='header'> Confirmation email from HeartBel </td> </tr> <tr> <td align='center' valign='top' style='border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-left: 6.25%; padding-right: 6.25%; width: 87.5%; padding-top: 25px;' class='line'><hr color='#E0E0E0' align='center' width='100%' size='1' noshade style='margin: 0; padding: 0;' /> </td> </tr> <tr> <td align='center' valign='top' style='border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-left: 6.25%; padding-right: 6.25%;' class='list-item'><table align='center' border='0' cellspacing='0' cellpadding='0' style='width: inherit; margin: 0; padding: 0; border-collapse: collapse; border-spacing: 0;'> <tr> <td align='left' valign='top' style='font-size: 17px; font-weight: 400; line-height: 160%; border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-top: 25px; color: #000000; font-family: sans-serif;' class='paragraph'> <b style='color: #333333;'><br/><br/> We wanted to take a moment to express our heartfelt appreciation for choosing to join [HeartBel].<br/>Your decision to sign up with us is a significant step towards finding meaningful connections and potential love interests, and we are truly delighted to have you as part of our community. </td> </tr> </table></td> </tr> <tr> <td align='center' valign='top' style='border-collapse: collapse; border-spacing: 0; margin: 0; padding: 0; padding-left: 6.25%; padding-right: 6.25%; width: 87.5%; padding-top: 25px; padding-bottom: 5px;' class='button'> <table border='0' cellpadding='0' cellspacing='0' align='center' style='max-width: 240px; min-width: 120px; border-collapse: collapse; border-spacing: 0; padding: 0;'><tr><td align='center' valign='middle' style='padding: 12px 24px; margin: 0;border-collapse: collapse; border-spacing: 0; border-radius: 4px; -webkit-border-radius: 4px; -moz-border-radius: 4px; -khtml-border-radius: 4px;' bgcolor='#E9703E'>Email verification code: <span style='text-decoration: none;color: #FFFFFF; font-family: sans-serif; font-size: 17px; font-weight: 400; line-height: 120%;'> "+date+" </span> </td></tr></table> </td> </tr> </table> </td></tr></table> </body> </html>";
                    mm.Body = data;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("krishhere0@gmail.com", "ulrw qfcl gqbd icfs");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
            }
            catch (Exception e)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "validation", $"<script language='javascript'>alert('{e.Message.ToString()}')</script>");
            }
        }
        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            if (txtPswd.Text.Trim() == txtConPswd.Text.Trim())
            {
                UserInsert();
                int id = BindUserId(txtEmail.Text.Trim());
                StoreValueInCookies("salngId", id.ToString());
                StoreValueInCookies("salngEmail", txtEmail.Text.Trim());
                Response.Redirect("UserEntry.aspx");
            }
        }
        private void UserInsert()
        {
            try
            {
                MySqlParameter[] msp = new MySqlParameter[2];
                msp[0] = new MySqlParameter("p_Email", MySqlDbType.VarChar);
                msp[1] = new MySqlParameter("p_Password", MySqlDbType.VarChar);

                msp[0].Value = txtEmail.Text.Trim();
                msp[1].Value = txtPswd.Text.Trim();

                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.Connection = con;
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.CommandText = "Pro_UserEmail_Insert";
                cmd2.Parameters.AddRange(msp);
                con.Open();
                cmd2.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        protected int BindUserId(string emailId)
        {
            int id = 0;
            try
            {
                con.Open();
                MySqlDataReader dr = null;
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT id FROM mySite.users where email='" + emailId + "'";
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
        private void StoreValueInCookies(string key, string value)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int count = emailVerify();
                if (count > 0)
                {
                    lblEmailMsg.Text = "Email is registered, please <a href='Login.aspx'>login</a>";
                    lblEmailMsg.Style["color"] = "#e72525";
                }
                else
                {
                    lblEmailMsg.Visible = false;
                }
            }
            catch
            {

            }
            finally
            {
                con.Close();
            }
        }
        private int emailVerify()
        {
            int count = 0;
            try
            {
                string query = "select COUNT(email) from users where email=@pEmail";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@pEmail", txtEmail.Text.Trim());
                con.Open();
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch
            {

            }
            finally
            {
                con.Close();
            }
            return count;
        }
    }
}