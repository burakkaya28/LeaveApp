using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using log4net;
using static System.String;

namespace DemoWebApp.classes
{
    public class ReportMail : ApplicationClass
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string _reportMessage = Empty;
        private static SqlConnection _con;
        private static SqlDataReader _sqlDataReader;

        public void SendAnEmail(string body, string sender, string senderPass, string host, bool enableSsl, int port, string to, string reportName)
        {
            try
            {
                Log.Info(@"Sending E-Mail process is starting...");
                Log.Info(@"To => " + to);
                Log.Info(@"Report Name => " + reportName);
                var message = new MailMessage()
                {
                    From = new MailAddress(sender)
                };

                message.To.Add(new MailAddress(to));

                //Mail Content
                message.Subject = reportName + " " + DateTime.Now.ToString("dd.MM.yyy");
                message.Body = body;
                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient()
                {
                    Credentials = new NetworkCredential(sender, senderPass),
                    Host = host,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = enableSsl,
                    Port = port
                };

                smtpClient.Send(message);

                Log.Info(@"Sending E-Mail process has completed succesfullly.");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public void AllLeavesReportviaMail(string to, string reportName, string sqlQuery, string columnNumber)
        {
            _reportMessage =
              "<table border='1' style='width: 100%; border: 1px solid black;'>" +
              "   <thead>" +
              "       <tr>" +
              "           <td colspan='"+columnNumber+"' style='text-align: center;'>" +
              "               <b> " + reportName + " </b>" +
              "           </td>" +
              "       </tr>" +
              "       <tr>" +
              "           <th>Leave ID</th>" +
              "           <th>Resource</th>" +
              "           <th>Description</th>" +
              "           <th>Start Date</th>" +
              "           <th>Finish Date</th>" +
              "           <th>Leave Duration</th>" +
              "           <th>Leave Type</th>" +
              "           <th>Status</th>" +
              "       </tr>" +
              "   </thead>" +
              "<tbody style='text-align: center;'>";

            _con = new SqlConnection(new ApplicationClass().Constr);
            _con.Open();
            _sqlDataReader = new SqlCommand(sqlQuery, _con).ExecuteReader();
            while (_sqlDataReader.Read())
            {
                var leaveId = _sqlDataReader["Id"].ToString();
                var resourceName = _sqlDataReader["FullName"].ToString();
                var description = _sqlDataReader["Description"].ToString();
                var startDate = DateTime.Parse(_sqlDataReader["StartDate"].ToString()).Date;
                var endDate = DateTime.Parse(_sqlDataReader["EndDate"].ToString()).Date;
                var leaveDuration = _sqlDataReader["Duration"].ToString();
                var leaveType = _sqlDataReader["LeaveType"].ToString();
                var status = _sqlDataReader["Status"].ToString();

                _reportMessage +=
                "<tr>" +
                    "<td><a href='http://izin.optiim.com/LeaveDetails?id=" + leaveId + "'>#" + leaveId + "</a></td>" +
                    "<td>" + resourceName + "</td>" +
                    "<td>" + description + "</td>" +
                    "<td>" + startDate.ToString(CultureInfo.DefaultThreadCurrentCulture).Replace(" 12:00:00 AM", "").Replace("00:00:00", "") + "</td>" +
                    "<td>" + endDate.ToString(CultureInfo.DefaultThreadCurrentCulture).Replace(" 12:00:00 AM", "").Replace("00:00:00", "") + "</td>" +
                    "<td>" + leaveDuration + "</td>" +
                    "<td>" + leaveType + "</td>" +
                    "<td>" + status + "</td>" +
                "</tr>";
            }
            _sqlDataReader.Close();
            _con.Close();

            _reportMessage += "</tbody></table>";

            SendAnEmail(_reportMessage, SenderMail, SenderMailPass, SenderHost, SenderSsl, SenderPort, to, reportName);
        }

        public void ApprovedLeavesReportviaMail(string to, string reportName, string sqlQuery, string columnNumber)
        {
            _reportMessage =
              "<table border='1' style='width: 100%; border: 1px solid black;'>" +
              "   <thead>" +
              "       <tr>" +
              "           <td colspan='" + columnNumber + "' style='text-align: center;'>" +
              "               <b> " + reportName + " </b>" +
              "           </td>" +
              "       </tr>" +
              "       <tr>" +
              "           <th>Leave ID</th>" +
              "           <th>Resource</th>" +
              "           <th>Description</th>" +
              "           <th>Start Date</th>" +
              "           <th>Finish Date</th>" +
              "           <th>Leave Duration</th>" +
              "           <th>Leave Type</th>" +
              "           <th>Status</th>" +
              "       </tr>" +
              "   </thead>" +
              "<tbody style='text-align: center;'>";

            _con = new SqlConnection(new ApplicationClass().Constr);
            _con.Open();
            _sqlDataReader = new SqlCommand(sqlQuery, _con).ExecuteReader();
            while (_sqlDataReader.Read())
            {
                var leaveId = _sqlDataReader["Id"].ToString();
                var resourceName = _sqlDataReader["FullName"].ToString();
                var description = _sqlDataReader["Description"].ToString();
                var startDate = DateTime.Parse(_sqlDataReader["StartDate"].ToString()).Date;
                var endDate = DateTime.Parse(_sqlDataReader["EndDate"].ToString()).Date;
                var leaveDuration = _sqlDataReader["Duration"].ToString();
                var leaveType = _sqlDataReader["LeaveType"].ToString();
                var status = _sqlDataReader["Status"].ToString();

                _reportMessage +=
                "<tr>" +
                    "<td><a href='http://izin.optiim.com/LeaveDetails?id=" + leaveId + "'>#" + leaveId + "</a></td>" +
                    "<td>" + resourceName + "</td>" +
                    "<td>" + description + "</td>" +
                    "<td>" + startDate.ToString(CultureInfo.DefaultThreadCurrentCulture).Replace(" 12:00:00 AM", "").Replace("00:00:00", "") + "</td>" +
                    "<td>" + endDate.ToString(CultureInfo.DefaultThreadCurrentCulture).Replace(" 12:00:00 AM", "").Replace("00:00:00", "") + "</td>" +
                    "<td>" + leaveDuration + "</td>" +
                    "<td>" + leaveType + "</td>" +
                    "<td>" + status + "</td>" +
                "</tr>";
            }
            _sqlDataReader.Close();
            _con.Close();

            _reportMessage += "</tbody></table>";

            SendAnEmail(_reportMessage, SenderMail, SenderMailPass, SenderHost, SenderSsl, SenderPort, to, reportName);
        }

        public void RejectedLeavesReportviaMail(string to, string reportName, string sqlQuery, string columnNumber)
        {
            _reportMessage =
              "<table border='1' style='width: 100%; border: 1px solid black;'>" +
              "   <thead>" +
              "       <tr>" +
              "           <td colspan='" + columnNumber + "' style='text-align: center;'>" +
              "               <b> " + reportName + " </b>" +
              "           </td>" +
              "       </tr>" +
              "       <tr>" +
              "           <th>Leave ID</th>" +
              "           <th>Resource</th>" +
              "           <th>Description</th>" +
              "           <th>Start Date</th>" +
              "           <th>Finish Date</th>" +
              "           <th>Leave Duration</th>" +
              "           <th>Leave Type</th>" +
              "           <th>Status</th>" +
              "       </tr>" +
              "   </thead>" +
              "<tbody style='text-align: center;'>";

            _con = new SqlConnection(new ApplicationClass().Constr);
            _con.Open();
            _sqlDataReader = new SqlCommand(sqlQuery, _con).ExecuteReader();
            while (_sqlDataReader.Read())
            {
                var leaveId = _sqlDataReader["Id"].ToString();
                var resourceName = _sqlDataReader["FullName"].ToString();
                var description = _sqlDataReader["Description"].ToString();
                var startDate = DateTime.Parse(_sqlDataReader["StartDate"].ToString()).Date;
                var endDate = DateTime.Parse(_sqlDataReader["EndDate"].ToString()).Date;
                var leaveDuration = _sqlDataReader["Duration"].ToString();
                var leaveType = _sqlDataReader["LeaveType"].ToString();
                var status = _sqlDataReader["Status"].ToString();

                _reportMessage +=
                "<tr>" +
                    "<td><a href='http://izin.optiim.com/LeaveDetails?id=" + leaveId + "'>#" + leaveId + "</a></td>" +
                    "<td>" + resourceName + "</td>" +
                    "<td>" + description + "</td>" +
                    "<td>" + startDate.ToString(CultureInfo.DefaultThreadCurrentCulture).Replace(" 12:00:00 AM", "").Replace("00:00:00", "") + "</td>" +
                    "<td>" + endDate.ToString(CultureInfo.DefaultThreadCurrentCulture).Replace(" 12:00:00 AM", "").Replace("00:00:00", "") + "</td>" +
                    "<td>" + leaveDuration + "</td>" +
                    "<td>" + leaveType + "</td>" +
                    "<td>" + status + "</td>" +
                "</tr>";
            }
            _sqlDataReader.Close();
            _con.Close();

            _reportMessage += "</tbody></table>";

            SendAnEmail(_reportMessage, SenderMail, SenderMailPass, SenderHost, SenderSsl, SenderPort, to, reportName);
        }

        public void PendingApprovalReportviaMail(string to, string reportName, string sqlQuery, string columnNumber)
        {
            _reportMessage =
              "<table border='1' style='width: 100%; border: 1px solid black;'>" +
              "   <thead>" +
              "       <tr>" +
              "           <td colspan='" + columnNumber + "' style='text-align: center;'>" +
              "               <b> " + reportName + " </b>" +
              "           </td>" +
              "       </tr>" +
              "       <tr>" +
              "           <th>Leave ID</th>" +
              "           <th>Resource</th>" +
              "           <th>Waiting Approver</th>" +
              "           <th>Approvel Level</th>" +
              "           <th>Start Date</th>" +
              "           <th>Finish Date</th>" +
              "           <th>Leave Duration</th>" +
              "           <th>Leave Type</th>" +
              "           <th>Description</th>" +
              "           <th>Status</th>" +
              "       </tr>" +
              "   </thead>" +
              "<tbody style='text-align: center;'>";

            _con = new SqlConnection(new ApplicationClass().Constr);
            _con.Open();
            _sqlDataReader = new SqlCommand(sqlQuery, _con).ExecuteReader();
            while (_sqlDataReader.Read())
            {
                var leaveId = _sqlDataReader["Id"].ToString();
                var resourceName = _sqlDataReader["FullName"].ToString();
                var waitingApprover = _sqlDataReader["WaitingApprover"].ToString();
                var approvalLevel = _sqlDataReader["AppLevel"].ToString();
                var description = _sqlDataReader["Description"].ToString();
                var startDate = DateTime.Parse(_sqlDataReader["StartDate"].ToString()).Date;
                var endDate = DateTime.Parse(_sqlDataReader["EndDate"].ToString()).Date;
                var leaveDuration = _sqlDataReader["LeaveDuration"].ToString();
                var leaveType = _sqlDataReader["LeaveType"].ToString();
                var status = _sqlDataReader["Status"].ToString();

                _reportMessage +=
                "<tr>" +
                    "<td><a href='http://izin.optiim.com/LeaveDetails?id=" + leaveId + "'>#" + leaveId + "</a></td>" +
                    "<td>" + resourceName + "</td>" +
                    "<td>" + waitingApprover + "</td>" +
                    "<td>" + approvalLevel + "</td>" +
                    "<td>" + startDate.ToString(CultureInfo.DefaultThreadCurrentCulture).Replace(" 12:00:00 AM", "").Replace("00:00:00", "") + "</td>" +
                    "<td>" + endDate.ToString(CultureInfo.DefaultThreadCurrentCulture).Replace(" 12:00:00 AM", "").Replace("00:00:00", "") + "</td>" +
                    "<td>" + leaveDuration + "</td>" +
                    "<td>" + leaveType + "</td>" +
                    "<td>" + description + "</td>" +
                    "<td>" + status + "</td>" +
                "</tr>";
            }
            _sqlDataReader.Close();
            _con.Close();

            _reportMessage += "</tbody></table>";

            SendAnEmail(_reportMessage, SenderMail, SenderMailPass, SenderHost, SenderSsl, SenderPort, to, reportName);
        }

        public void LeaveSummaryReportviaMail(string to, string reportName, string sqlQuery, string columnNumber)
        {
            _reportMessage =
              "<table border='1' style='width: 100%; border: 1px solid black;'>" +
              "   <thead>" +
              "       <tr>" +
              "           <td colspan='" + columnNumber + "' style='text-align: center;'>" +
              "               <b> " + reportName + " </b>" +
              "           </td>" +
              "       </tr>" +
              "       <tr>" +
              "           <th>Resource</th>" +
              "           <th>Manager</th>" +
              "           <th>Location</th>" +
              "           <th>Work Year</th>" +
              "           <th>Total Leave Right</th>" +
              "           <th>Total Used Leave</th>" +
              "           <th>Total Leave Left</th>" +
              "       </tr>" +
              "   </thead>" +
              "<tbody style='text-align: center;'>";

            _con = new SqlConnection(new ApplicationClass().Constr);
            _con.Open();
            _sqlDataReader = new SqlCommand(sqlQuery, _con).ExecuteReader();
            while (_sqlDataReader.Read())
            {
                var userFullName = _sqlDataReader["FullName"].ToString();
                var managerName = _sqlDataReader["ManagerName"].ToString();
                var locationName = Login.Language == "tr" ? _sqlDataReader["LocationNameTR"].ToString() : 
                    _sqlDataReader["LocationName"].ToString();
                var workYear = _sqlDataReader["work_year"].ToString();
                var totalLeaveRight = _sqlDataReader["total_leave_right"].ToString();
                var totalUsedLeave = _sqlDataReader["total_used_leave"].ToString();
                var totalLeaveLeft = _sqlDataReader["total_leave_left"].ToString();

                _reportMessage +=
                "<tr>" +
                    "<td>" + userFullName + "</td>" +
                    "<td>" + managerName + "</td>" +
                    "<td>" + locationName + "</td>" +
                    "<td>" + workYear + "</td>" +
                    "<td>" + totalLeaveRight + "</td>" +
                    "<td>" + totalUsedLeave + "</td>" +
                    "<td>" + totalLeaveLeft + "</td>" +
                "</tr>";
            }
            _sqlDataReader.Close();
            _con.Close();

            _reportMessage += "</tbody></table>";

            SendAnEmail(_reportMessage, SenderMail, SenderMailPass, SenderHost, SenderSsl, SenderPort, to, reportName);
        }
    }
}