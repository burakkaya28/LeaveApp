using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Security;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class Details : ApplicationClass
    {
        private readonly LeaveClass _leaveClass = new LeaveClass();
        private readonly Login _loginClass = new Login();
        private readonly UserClass _userClass = new UserClass();
        private readonly ApplicationClass _appClass = new ApplicationClass();
        private readonly string _manager = "";
        string _leaveLanguage = "";

        public string LeaveType { get; private set; }

        protected override void InitializeCulture()
        {
            LeaveType = Login.Language;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Login.Language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Login.Language);
            base.InitializeCulture();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            var id = "";
            if (Request.Params.Get("id") != null)
            {
                id = Request.Params.Get("id");

                Guid.NewGuid();
            }
            else
                Response.Redirect("~/Dashboard");

            id = Regex.Match(id, @"\d+").Value;

            var haveRequest = "";
            var scon = new SqlConnection(Constr);
            scon.Open();
            var scmd = new SqlCommand("select 'Y' HaveRequest from LeaveRequests where Id = '" + id + "' ", scon);
            var sreader = scmd.ExecuteReader();
            while (sreader.Read())
            {
                haveRequest = sreader["HaveRequest"].ToString();
            }
            if (haveRequest != "Y")
            {
                Response.Redirect("~/Dashboard");
            }

            sreader.Close();
            
            var username = _loginClass.User.Identity.Name;
            var result = Convert.ToBoolean(_userClass.ManagerControl(username));

            if (result==false)
            {
                var sonuc = Convert.ToBoolean(_leaveClass.LeaveRequestControl(username, id));
                if (sonuc == false)
                {
                    var warning = Login.Language == "tr" ? "Bu izini görmek için yetkiniz bulunmamaktadır." : "You are not authorized to see this leave.";
                    
                    Response.Write("<script lang='Javascript'>alert('"+warning+ "'); window.location = 'Dashboard'</script>");
                }
            }

            var controlId = Request.Params.Get("id");
            scmd = new SqlCommand("select LeaveLanguage from LeaveRequests where Id= '" + controlId + "'", scon);
            _leaveLanguage = Convert.ToString(scmd.ExecuteScalar());
            
            var secondManagerControl = "0";
            scmd = new SqlCommand("select UserId from SecondManagers", scon);
            sreader = scmd.ExecuteReader();

            while (sreader.Read())
            {
                if (sreader["userId"].ToString() == username)
                {
                    secondManagerControl = "1";
                }

            }

            sreader.Close();

            if (secondManagerControl != "1") return;
            _leaveClass.GetLeaveManagerId(id);
            _leaveClass.GetLeaveManagerFullName(_manager);
        }

        protected void Approve_Click(object sender, EventArgs e)
        {
            var userId = _loginClass.User.Identity.Name;
            var leaveId = Request.Params.Get("id");
            var leaveUserId = new UserClass().GetUserIdByLeaveRequest(leaveId);
            var secondManagerControl = "0";
            
            var conn = new SqlConnection(Constr);
            var comm = new SqlCommand("select UserId from SecondManagers", conn);
            conn.Open();
            var dr = comm.ExecuteReader();

            while (dr.Read())
            {
                if (dr["userId"].ToString() == userId)
                {
                    secondManagerControl = "1";
                }
            }
            dr.Close();

            string mailDesc;
            string mailTitle;
            if (secondManagerControl == "1" || new UserClass().CheckUserSecondManagerExist(leaveUserId) == false)
            {
                _leaveClass.LeaveUpdate(leaveId, "1");

                if (_leaveLanguage == "tr")
                {
                    mailDesc = " ikinci yönetici tarafından onaylanmıştır";
                    mailTitle = leaveId + " Numaralı izin kaydı ikinci yönetici tarafından onaylanmıştır.";
                }
                else
                {
                    mailDesc = " has been approved by second manager";
                    mailTitle = "Leave " + leaveId + " has been approved by second manager.";
                }
                
                
                _appClass.SendMail(mailTitle, mailDesc, leaveId, 1);
            }
            else
            {
                _leaveClass.LeaveUpdate(leaveId, "5");

                if (_leaveLanguage == "tr")
                {
                    mailDesc = " ikinci yönetici onayı beklenmektedir";
                    mailTitle = leaveId + " Numaralı izin kaydı onaylanmıştır ve ikinci yönetici onayı beklenmektedir.";
                }
                else
                {
                    mailDesc = " has been approved and waiting for second manager approval";
                    mailTitle = "Leave " + leaveId + " has been approved and waiting for second manager approval.";
                }

                _appClass.SendMail(mailTitle, mailDesc, leaveId, 5);
            }

        }

        protected void Reject_Click(object sender, EventArgs e)
        {
            var userId = _loginClass.User.Identity.Name;
            var leaveId = Request.Params.Get("id");
            var leaveUserId = new UserClass().GetUserIdByLeaveRequest(leaveId);
            var secondManagerControl = "0";
        
            var conn = new SqlConnection(Constr);
            var comm = new SqlCommand("select UserId from SecondManagers", conn);
            conn.Open();
            var dr = comm.ExecuteReader();

            while (dr.Read())
            {
                if (dr["userId"].ToString() == userId)
                {
                    secondManagerControl = "1";
                }
            }
            dr.Close();

            var id = Request.Params.Get("id");
            _leaveClass.LeaveUpdate(id, "2");

            string mailDesc;
            string mailTitle;

            if (secondManagerControl == "1" || new UserClass().CheckUserSecondManagerExist(leaveUserId) == false)
            {
                if (_leaveLanguage == "tr")
                {
                    mailDesc = id + " Numaralı izin kaydı ikinci yönetici tarafından reddedilmiştir.";
                    mailTitle = " ikinci yönetici tarafından reddedilmiştir.";
                }
                else
                {
                    mailDesc = "Leave " + id + " has been rejected by second manager.";
                    mailTitle = " has been rejected by second manager.";
                }
            }
            else
            {
                if (_leaveLanguage == "tr")
                {
                    mailDesc = id + " Numaralı izin reddedilmiştir";
                    mailTitle = " reddedilmiştir";
                }
                else
                {
                    mailDesc = "Leave " + id + " was rejected.";
                    mailTitle = " was rejected";
                }
            }
            
            _appClass.SendMail(mailDesc, mailTitle, id, 2);
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            var id = Request.Params.Get("id");
            _leaveClass.LeaveUpdate(id, "3");

            string mailDesc;
            string mailTitle;

            if (_leaveLanguage == "tr")
            {
                mailDesc = id + " Numaralı izin iptal edilmiştir";
                mailTitle = " iptal edilmiştir";
            }
            else
            {
                mailDesc = "Leave " + id + " was cancelled.";
                mailTitle = " was cancelled";
            }

            _appClass.SendMail(mailDesc, mailTitle, id, 3);
        }
    }
}