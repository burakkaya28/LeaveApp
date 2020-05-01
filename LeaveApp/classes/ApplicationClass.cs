using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using log4net;

namespace DemoWebApp.classes
{
    public class ApplicationClass : Page
    {
        public string SenderMail = ConfigurationManager.AppSettings["SenderMail"];
        public string SenderMailPass = ConfigurationManager.AppSettings["SenderMailPass"];
        public string SenderHost = ConfigurationManager.AppSettings["SenderHost"];
        public bool SenderSsl = Convert.ToBoolean(int.Parse(ConfigurationManager.AppSettings["SenderSsl"]));
        public int SenderPort = int.Parse(ConfigurationManager.AppSettings["SenderPort"]);

        public readonly string Constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string IsAdmin(string userId)
        {
            string value = null;
            try
            {
                using (var con = new SqlConnection(Constr))
                {
                    
                    using (var cmd = new SqlCommand("select 'Y' from Admins where UserId = '" + userId + "'", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        value = Convert.ToString(cmd.ExecuteScalar());
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return value;
        }

        public string IsManager(string userId)
        {
            string value = null;
            try
            {
                using (var con = new SqlConnection(Constr))
                {
                    using (var cmd = new SqlCommand("select 'Y' from Managers where UserId = '" + userId + "' ", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        value = Convert.ToString(cmd.ExecuteScalar());
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }

            return value;
        }

        public string IsSecondManager(string userId)
        {
            string value = null;
            try
            {
                using (var con = new SqlConnection(Constr))
                {
                    using (var cmd = new SqlCommand("select 'Y' from SecondManagers where UserId = '" + userId + "' ", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        value = Convert.ToString(cmd.ExecuteScalar());
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return value;
        }

        public string IsFinancialUser(string userId)
        {
            string value = null;
            try
            {
                using (var con = new SqlConnection(Constr))
                {
                    using (var cmd = new SqlCommand("select 'Y' from FinancialTeamUsers where UserId = '" + userId + "' ", con))
                    {
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        value = Convert.ToString(cmd.ExecuteScalar());
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return value;
        }

        public string GetUserRole(int userId)
        {
            string role = null;
            try
            {
                using (var con = new SqlConnection(Constr))
                {
                    using (var cmd = new SqlCommand("select Role from users where UserId ='" + userId + "'"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        role = Convert.ToString(cmd.ExecuteScalar());
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return role;
        }

        public string GetUserFullName(int userId)
        {
            string name = null;
            try
            {
                using (var con = new SqlConnection(Constr))
                {
                    using (var cmd = new SqlCommand("select FullName from users where UserId ='" + userId + "'"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        name = Convert.ToString(cmd.ExecuteScalar());
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return name;
        }

        public string SetDateFormat(string dateToBeConvert)
        {
            if (dateToBeConvert.Contains("-") || dateToBeConvert.Contains("/"))
            {
                var dateDay = dateToBeConvert.Substring(0, 2);
                var dateMonth = dateToBeConvert.Substring(3, 2);
                var dateYear = dateToBeConvert.Substring(6, 4);

                var formattedDate = dateYear + "/" + dateMonth + "/" + dateDay;

                return formattedDate;
            }

            return dateToBeConvert;
        }

        public bool SendMail(string konu, string icerik, string requestId, int state)
        {
            try
            {
                var usermail = "";
                var managermail = "";
                var secondmanagermail = "";
                var uName = "";
                var mName = "";
                var sName = "";
                var stDate = "";
                var eDate = "";
                var leaveLanguage = "";
                var leaveType = "";
                var leaveDuration = "";
                var description = "";
                var locationId = "";
                
                using (var con = new SqlConnection(Constr))
                {
                    using (var cmd = new SqlCommand("select u.Email UMAIL, u2.Email MMAIL,u3.Email SMAIL, u.FullName FullName , u2.FullName ManagerName ,u3.FullName SecondManagerName,r.Description, r.StartDate StartDate, r.EndDate EndDate, r.LeaveLanguage LeaveLanguage, lt.LeaveType, lt.LeaveTypeTr, r.day LeaveDuration, u.LocationId from LeaveRequests r join Users u on u.UserId = r.UserId join Users u2 on r.ManagerId = u2.UserId left join Users u3 on u3.UserId=u.SecondManager join LeaveTypes lt on lt.LeaveTypeId=r.LeaveTypeId and r.Id ='" + requestId+"'"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        var sreader = cmd.ExecuteReader();
                        while (sreader.Read())
                        {
                            usermail = sreader["UMAIL"].ToString();
                            managermail = sreader["MMAIL"].ToString();
                            secondmanagermail = sreader["SMAIL"].ToString();
                            uName = sreader["FullName"].ToString();
                            mName = sreader["ManagerName"].ToString();
                            sName = sreader["SecondManagerName"].ToString();
                            var startDate = Convert.ToDateTime(sreader["StartDate"]);
                            var endDate = Convert.ToDateTime(sreader["EndDate"]);
                            leaveLanguage = sreader["LeaveLanguage"].ToString();
                            description = sreader["Description"].ToString();
                            leaveType = Login.Language=="tr" ? sreader["LeaveTypeTr"].ToString() : sreader["LeaveType"].ToString();
                            leaveDuration = sreader["LeaveDuration"].ToString();
                            locationId = sreader["LocationId"].ToString();

                            stDate = startDate.ToString("dd MMMM yyyy", new CultureInfo(leaveLanguage));
                            eDate = endDate.ToString("dd MMMM yyyy", new CultureInfo(leaveLanguage));

                        }
                        con.Close();
                    }

                }
                string contentHtml;
                if(leaveLanguage == "tr")
                    contentHtml = @"<html> <head></head> <body leftmargin='0' topmargin='0' marginwidth='0' marginheight='0'> <center> <table border='0' cellpadding='0' cellspacing='0' style='word-wrap:break-word;font-family: Segoe UI;'> <tr> <td> <table border='0' cellpadding='10' cellspacing='7' class='bodytable2' align='center'> <!-- HEAD --> <tr> <td style='font-size: 18px; font-weight: bold; color: black; background-color:#F4F4F4; border: solid 1px #D3D3D3;'> <table cellpadding='0' cellspacing='5' width='100%' style = 'font-size: 18px;' > <tr> <td colspan='2' nowrap><a href='http://izin.optiim.com/LeaveDetails?id=" + requestId + "'>#" + requestId + " </a> </td> </tr> </table> </td> </tr> <!-- DETAILS --> <tr> <td style='background-color:#F4F4F4; border: solid 1px #D3D3D3;'> <table border='0' cellspacing='0' cellpadding='5' style='word-wrap:break-word;font-family: Segoe UI;'> <tr> <td>" + requestId + " nolu izin talebi " + icerik + ".</td> </tr> </table> </td> </tr> <tr style='width:5px;word-wrap:break-word;font-family: Segoe UI;'> <td style='background-color:#F4F4F4; border: solid 1px #D3D3D3;'> <table border='0' cellspacing='0' cellpadding='5' style='width:425px;table-layout: fixed;word-wrap:break-word;font-family: Segoe UI;'> <tr> <td valign='top' nowrap style='font-size:14px;font-weight:bold;'>Açıklama:</td> <tr> <td style='font-size:14px'>" + description + "</td> </tr> </table> </td> </tr> <tr> <td style='background-color:#F4F4F4; border: solid 1px #D3D3D3;'> <table border='0' cellspacing='0' cellpadding='5' style = 'font-size: 13px;'> <tr> <td valign='top' nowrap style='font-weight:bold;'>İzin No</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + requestId + "</td> </tr> <tr> <td valign='top' nowrap style='font-weight:bold;'>İzin Türü</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + leaveType + "</td> </tr> <tr> <td valign='top' nowrap style='font-weight:bold;'>Başlangıç Tarihi</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + stDate + "</td> </tr> <tr> <td valign='top' nowrap style='font-weight:bold;'>Bitiş Tarihi</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + eDate + "</td> </tr> <tr> <td valign='top' nowrap style='font-weight:bold;'>İzin Süresi</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + leaveDuration + "</td> </tr> <tr> <td valign='top' nowrap style='font-weight:bold;'>Oluşturan</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + uName + "</td> </tr> <tr> <td valign='top' nowrap style='font-weight:bold;'>Yönetici</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + mName + "</td> </tr> <tr> <td valign='top' nowrap style='font-weight:bold;'>İkinci Yönetici</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + sName + "</td> </tr> </table> </td> </tr> <tr> <td valign='top' nowrap style='font-size: 12px; background-color:#F4F4F4;color:black;' style='background-color:#F4F4F4; border: solid 1px #D3D3D3;'><b>İzin detayına ulaşmak için lütfen yukarıdaki izin numarasina tıklayınız.</a></td> </tr> </table> </td> </tr> </table> </center> </body> </html>";
                else
                    contentHtml = @"<html> <head></head> <body leftmargin='0' topmargin='0' marginwidth='0' marginheight='0'> <center> <table border='0' cellpadding='0' cellspacing='0' style='word-wrap:break-word;font-family: Segoe UI;'> <tr> <td> <table border='0' cellpadding='10' cellspacing='7' class='bodytable2' align='center'> <!-- HEAD --> <tr> <td style='font-size: 18px; font-weight: bold; color: black; background-color:#F4F4F4; border: solid 1px #D3D3D3;'> <table cellpadding='0' cellspacing='5' width='100%' style = 'font-size: 18px;' > <tr> <td colspan='2' nowrap><a href='http://izin.optiim.com/LeaveDetails?id=" + requestId + "'>#" + requestId + " </a> </td> </tr> </table> </td> </tr> <!-- DETAILS --> <tr> <td style='background-color:#F4F4F4; border: solid 1px #D3D3D3;'> <table border='0' cellspacing='0' cellpadding='5' style='word-wrap:break-word;font-family: Segoe UI;'> <tr> <td>" + "Leave " + requestId + " request" + icerik + ".</td> </tr> </table> </td> </tr> <tr style='width:5px;word-wrap:break-word;font-family: Segoe UI;'> <td style='background-color:#F4F4F4; border: solid 1px #D3D3D3;'> <table border='0' cellspacing='0' cellpadding='5' style='width:425px;table-layout: fixed;word-wrap:break-word;font-family: Segoe UI;'> <tr> <td valign='top' nowrap style='font-size:14px;font-weight:bold;'>Description:</td> <tr> <td style='font-size:14px'>" + description + "</td> </tr> </table> </td> </tr> <tr> <td style='background-color:#F4F4F4; border: solid 1px #D3D3D3;'> <table border='0' cellspacing='0' cellpadding='5' style = 'font-size: 13px;'> <tr> <td valign='top' nowrap style='font-weight:bold;'>Leave No</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + requestId + "</td> </tr> <tr> <td valign='top' nowrap style='font-weight:bold;'>Leave Type</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + leaveType + "</td> </tr> <tr> <td valign='top' nowrap style='font-weight:bold;'>Start Date</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + stDate + "</td> </tr> <tr> <td valign='top' nowrap style='font-weight:bold;'>End Date</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + eDate + "</td> </tr> <tr> <td valign='top' nowrap style='font-weight:bold;'>Leave Duration</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + leaveDuration + "</td> </tr> <tr> <td valign='top' nowrap style='font-weight:bold;'>Created By</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + uName + "</td> </tr> <tr> <td valign='top' nowrap style='font-weight:bold;'>Manager</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + mName + "</td> </tr> <tr> <td valign='top' nowrap style='font-weight:bold;'>Second Manager</td> <td valign='top' width = '1'>:</td> <td valign='top'>" + sName + "</td> </tr> </table> </td> </tr> <tr> <td valign='top' nowrap style='font-size: 12px; background-color:#F4F4F4;color:black;' style='background-color:#F4F4F4; border: solid 1px #D3D3D3;'><b>Please click the leave number above to access the leave detail.</a></td> </tr> </table> </td> </tr> </table> </center> </body> </html>";


                var client = new SmtpClient(SenderHost, SenderPort);
                var mesaj = new MailMessage();

                if (managermail != "")
                {
                    mesaj.To.Add(managermail);
                }

                if (state == 5)
                {
                    if (secondmanagermail != "")
                    {
                        mesaj.To.Clear();
                        mesaj.To.Add(secondmanagermail);
                    }
                }

                if (usermail != "")
                {
                    mesaj.CC.Add(usermail);
                }

                if (state == 1) //If request is approved
                {
                    //Get the financial team members and add them into CC
                    using (var con = new SqlConnection(Constr))
                    {
                        using (var cmd = new SqlCommand("select u.Email from FinancialTeamUsers ft join Users u ON u.UserId = ft.UserId"))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = con;
                            con.Open();
                            var sreader = cmd.ExecuteReader();
                            while (sreader.Read())
                            {
                                var financialUserEmail = sreader["Email"].ToString();
                                if(financialUserEmail != "")
                                {
                                    mesaj.CC.Add(financialUserEmail);
                                }
                            }
                            con.Close();
                        }
                    }
                }

                //Check the location responsibles and add them into CC
                using (var con = new SqlConnection(Constr))
                {
                    using (var cmd = new SqlCommand("select u.Email from LocationResponsibles lp join Users u on u.UserId = lp.UserId where lp.LocationId = '"+locationId+"'"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        var sreader = cmd.ExecuteReader();
                        while (sreader.Read())
                        {
                            var locationResponsibleMail = sreader["Email"].ToString();

                            if(locationResponsibleMail != "")
                            {
                                mesaj.CC.Add(locationResponsibleMail);
                            }
                        }
                        con.Close();
                    }
                }

                mesaj.From = new MailAddress(SenderMail);
                mesaj.Subject = konu;
                mesaj.IsBodyHtml = true;
                mesaj.Body = contentHtml;
                var mailSecurity = new NetworkCredential(SenderMail, SenderMailPass);
                client.Credentials = mailSecurity;
                client.EnableSsl = SenderSsl;
                client.Send(mesaj);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return false;
            }

            return true;
        }

        public string GetBase64String(byte[] buffer)
        {
            return Convert.ToBase64String(buffer);
        }

        protected override void InitializeCulture()
        {
            if (!string.IsNullOrEmpty(Request["selection_lang"]))
            {
                Session["selection_lang"] = Request["selection_lang"];
            }
            var lang = Convert.ToString(Session["selection_lang"]);
            var culture = string.Empty;

            if (string.Compare(lang.ToLower(), "en", StringComparison.Ordinal) == 0 || string.IsNullOrEmpty(culture))
            {
                culture = "en-US";

                Login.Language = "en";
            }
            if (string.Compare(lang.ToLower(), "tr", StringComparison.Ordinal) == 0)
            {
                culture = "tr-TR";
                Login.Language = "tr";
            }
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

            base.InitializeCulture();
        }

        public void ExportToExcel(HtmlGenericControl tableBody, HttpResponse response)
        {
            response.Clear();
            response.Buffer = true;
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("content-disposition", "attachment;filename=Report_" + DateTime.Now.ToString("yyyy-MM-ddhh:mm:ss") + ".xls");
            response.Write("<style type=\"text/css\">");
            response.Write("</style>");
            EnableViewState = false;
            var sw = new StringWriter();
            response.Charset = "";
            response.ContentEncoding = Encoding.GetEncoding("windows-1254");
            var htw = new HtmlTextWriter(sw);
            tableBody.RenderControl(htw);
            response.Write(" <meta http-equiv='Content-Type' content='text/html; charset=windows-1254' />" + sw);
            response.End();
        }

        public void AdminAuthorityCheck(HttpResponse response)
        {
            try
            {
                if (Convert.ToBoolean(new UserClass().AdminControl(new Login().User.Identity.Name)) == false)
                {
                    response.Write(
                        Login.Language == "tr" ?
                            "<script lang='Javascript'>alert('Bu sayfaya erişim yetkiniz bulunmamaktadır.'); window.location = 'Dashboard'</script>"
                            : "<script lang='Javascript'>alert('You do not have authority to access this page.'); window.location = 'Dashboard'</script>");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        public void AdminAndManagerAuthorityCheck(HttpResponse response)
        {
            try
            {
                if (Convert.ToBoolean(new UserClass().AdminControl(new Login().User.Identity.Name)) == false && Convert.ToBoolean(new UserClass().ManagerControl(new Login().User.Identity.Name)) == false)
                {
                    Response.Write(
                        Login.Language == "tr" ?
                            "<script lang='Javascript'>alert('Bu sayfaya erişim yetkiniz bulunmamaktadır.'); window.location = 'Dashboard'</script>"
                            : "<script lang='Javascript'>alert('You do not have authority to access this page.'); window.location = 'Dashboard'</script>");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        public bool SetUserAdminStatus(string userId, string adminCheckStatus)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm;
                if (adminCheckStatus == "on") //If admin option is checked
                {
                    if (IsAdmin(userId) != "Y") //If user is not admin
                    {
                        comm = new SqlCommand("insert into Admins values ('" + userId + "')", conn);
                        comm.ExecuteNonQuery();
                    }
                }
                else //If admin option is not checked
                {
                    if (IsAdmin(userId) == "Y") //If user is admin
                    {
                        comm = new SqlCommand("delete from Admins where UserId = '" + userId + "'", conn);
                        comm.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool SetUserManagerStatus(string userId, string managerCheckStatus)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm;
                if (managerCheckStatus == "on") //If manager option is checked
                {
                    if (IsManager(userId) != "Y") //If user is not manager
                    {
                        comm = new SqlCommand("insert into Managers values ('" + userId + "')", conn);
                        comm.ExecuteNonQuery();
                    }
                }
                else //If manager option is not checked
                {
                    if (IsManager(userId) == "Y") //If user is manager
                    {
                        comm = new SqlCommand("delete from Managers where UserId = '" + userId + "'", conn);
                        comm.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool SetUserSecondManagerStatus(string userId, string secondManagerCheckStatus)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm;
                if (secondManagerCheckStatus == "on") //If second manager option is checked
                {
                    if (IsSecondManager(userId) != "Y") //If user is not second manager
                    {
                        comm = new SqlCommand("insert into SecondManagers values ('" + userId + "')", conn);
                        comm.ExecuteNonQuery();
                    }
                }
                else //If second manager option is not checked
                {
                    if (IsSecondManager(userId) == "Y") //If user is second manager
                    {
                        comm = new SqlCommand("delete from SecondManagers where UserId = '" + userId + "'", conn);
                        comm.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool SetUserFinancialUserStatus(string userId, string financialUserCheckStatus)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm;
                if (financialUserCheckStatus == "on") //If financial user option is checked
                {
                    if (IsFinancialUser(userId) != "Y") //If user is not financial user
                    {
                        comm = new SqlCommand("insert into FinancialTeamUsers values ('" + userId + "')", conn);
                        comm.ExecuteNonQuery();
                    }
                }
                else //If financial user option is not checked
                {
                    if (IsFinancialUser(userId) == "Y") //If user is financial user
                    {
                        comm = new SqlCommand("delete from FinancialTeamUsers where UserId = '" + userId + "'", conn);
                        comm.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public int GetNumberOfWorkingDays(DateTime start, DateTime stop)
        {
            var interval = stop - start;

            var totalWeek = interval.Days / 7;
            var totalWorkingDays = 5 * totalWeek;

            var remainingDays = interval.Days % 7;


            for (var i = 0; i <= remainingDays; i++)
            {
                var test = (DayOfWeek)(((int)start.DayOfWeek + i) % 7);
                if (test >= DayOfWeek.Monday && test <= DayOfWeek.Friday)
                    totalWorkingDays++;
            }

            return totalWorkingDays;
        }

        public string GetBaseUrl(HttpRequest req)
        {
            if (req.ApplicationPath != null)
                return req.Url.Scheme + "://" + req.Url.Authority +
                       req.ApplicationPath.TrimEnd('/') + "/";
            return "";
        }
    }
}