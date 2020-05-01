using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class RequestDayOff : ApplicationClass
    {
        string _add;
        int _uid;
        int _sayac1 = 0;
        private readonly LeaveClass _leaveClass = new LeaveClass();
        private readonly Login _loginClass = new Login();
        private readonly ApplicationClass _appClass = new ApplicationClass();
        private readonly UserClass _userClass = new UserClass();

        //private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected string RequestDay => Login.Language;
        protected bool ContractFlag => _leaveClass.ContractFlag(_uid);

        protected override void InitializeCulture()
        {
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

            var userId = _loginClass.User.Identity.Name;

            var command = "select lt.LeaveTypeId, lt.LeaveType, lt.LeaveTypeTr from Users u join Locations l on l.LocationId = u.LocationId join LocationLeaveTypes llt on llt.LocationId = l.LocationId join LeaveTypes lt on lt.LeaveTypeId = llt.LeaveTypeId where u.UserId = " + userId;

            var subjects = new DataTable();

            using (var con = new SqlConnection(Constr))
            {
                try
                {
                    var adapter = new SqlDataAdapter(command, con);
                    adapter.Fill(subjects);

                    LeaveType.DataSource = subjects;
                    LeaveType.DataValueField = "LeaveTypeId";
                    LeaveType.DataTextField = Login.Language=="tr" ? "LeaveTypeTr" : "LeaveType";

                    LeaveType.DataBind();
                }
                catch (Exception)
                {
                    // Handle the error
                }

            }

            _add = userId;
            _uid = Convert.ToInt32(userId);
            if (_add != "")
            {
                var sonuc = _leaveClass.ContractControl(_add);
                accordion.Visible = sonuc != 1;
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Div3.Visible = true;
        }
        
        protected void Button1_Click(object sender, EventArgs e)
        {
            var std = DateTime.Now;
            var end = DateTime.Now;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
            var userId = _loginClass.User.Identity.Name;

            var stdate = Request.Form["stdate"];
            var enddate = Request.Form["enddate"];
            var description = Request.Form["description"];
            var leaveTypeValue = int.Parse(LeaveType.SelectedValue);
            var leaveSuitability = false;
            var userLocation = _userClass.GetUserLocation(userId);
            var allowLeaveIfZero = new LeavePolicyClass().GetPolicyValueByKey("ALLOW_GET_LEAVE_IF_RESOURCE_NO_LEAVE");
            var allowGetLeaveIfNoLeftLeave = true;

            if (stdate != "") { std = Convert.ToDateTime(stdate); }
            if (enddate != "") { end = Convert.ToDateTime(enddate); }

            double dayDiff = new ApplicationClass().GetNumberOfWorkingDays(std, end);
                
            if (stdate == enddate)
                dayDiff = 1;

            if (Request.Form["halfday_add"] != null && Request.Form["halfday_add"] == "on")
                dayDiff += 0.5;

            if (Request.Form["halfday"] != null && Request.Form["halfday"] == "on")
                dayDiff = 0.5;


            var fieldControl = false;
            if (stdate == "" || enddate == "" || (description == "" || description.Trim().Length == 0))
            {
                Label1.Text = Login.Language == "tr" ? "Tüm alanları doldurunuz!" : "Fill in all fields!";
                LeaveType.Items.Clear();

                var command = Login.Language == "tr" ? "select LeaveTypeId,LeaveTypeTr from LeaveTypes" : "select LeaveTypeId,LeaveType from LeaveTypes";

                var subjects = new DataTable();

                using (var con = new SqlConnection(Constr))
                {
                    try
                    {
                        var adapter = new SqlDataAdapter(command, con);
                        adapter.Fill(subjects);
                        
                        LeaveType.DataSource = subjects;
                        LeaveType.DataValueField = "LeaveTypeId";
                        LeaveType.DataTextField = Login.Language == "tr" ? "LeaveTypeTr" : "LeaveType";

                        LeaveType.DataBind();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            else
                fieldControl = true;

            var result = _leaveClass.ContractFlag(_uid);
            if (result == false && chcSozlesme.Checked == false)
            {
                Label1.Text = Login.Language == "tr" ? "Yıllık izin taahütnamesini kabul etmeniz gerekiyor!" : "You must accept the annual leave notice!";

                LeaveType.Items.Clear();

                var command = Login.Language == "tr" ? "select LeaveTypeId,LeaveTypeTr from LeaveTypes" : "select LeaveTypeId,LeaveType from LeaveTypes";

                var subjects = new DataTable();

                using (var con = new SqlConnection(Constr))
                {
                    try
                    {
                        var adapter = new SqlDataAdapter(command, con);
                        adapter.Fill(subjects);
                        LeaveType.DataSource = subjects;
                        LeaveType.DataValueField = "LeaveTypeId";
                        LeaveType.DataTextField = Login.Language == "tr" ? "LeaveTypeTr" : "LeaveType";

                        LeaveType.DataBind();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            
            //If User's location is not Turkey
            if (userLocation != "Turkey")
            {
                //If leave is smaller than 2 weeks...
                if (dayDiff < 15)
                {
                    //Mark leave suitablilty status as True
                    leaveSuitability = true;
                }
            }
            else //If location is Turkey
            {
                leaveSuitability = true;
            }

            //If getting leave when resource does not have leave left is not allowed
            if (allowLeaveIfZero == "0")
            {
                //If resource leave left day is equal zero or smaller than zero
                if (new UserClass().GetUserLeaveLeft(userId) <= 0)
                {
                    allowGetLeaveIfNoLeftLeave = false;
                }
            }

            //If leave is suitable to take
            if (leaveSuitability)
            {
                if (fieldControl && (result || (result == false && chcSozlesme.Checked)))
                {
                    if (_sayac1 < 100 && _sayac1 >= 0)
                    {
                        if (allowGetLeaveIfNoLeftLeave)
                        {
                            Label1.Text = "";
                            using (var con = new SqlConnection(Constr))
                            {
                                int reqId;
                                using (var cmd = new SqlCommand("Insert_DayOffRequests"))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@StartDate", std);
                                    cmd.Parameters.AddWithValue("@EndDate", end);
                                    cmd.Parameters.AddWithValue("@userId", userId);
                                    cmd.Parameters.AddWithValue("@ReportImage", "");
                                    cmd.Parameters.AddWithValue("@Status", 0);
                                    cmd.Parameters.AddWithValue("@Description", description);
                                    cmd.Parameters.AddWithValue("@Day", dayDiff);
                                    cmd.Parameters.AddWithValue("@LeaveLanguage", Login.Language);
                                    cmd.Parameters.AddWithValue("@LeaveType", leaveTypeValue);
                                    cmd.Connection = con;
                                    con.Open();
                                    reqId = Convert.ToInt32(cmd.ExecuteScalar());
                                    con.Close();

                                    if (reqId != 0)
                                    {
                                        _add = userId;
                                        if (_add != "")
                                        {
                                            var contractFlag = _leaveClass.ContractFlag(_uid);
                                            if (contractFlag == false)
                                            {
                                                _leaveClass.ContractUpdate(_add, chcSozlesme.Checked);
                                            }
                                        }
                                    }

                                    //If leave has any file that uploaded
                                    if (fuResim.HasFile)
                                    {
                                        _leaveClass.AddAttachment(reqId.ToString(), fuResim.FileName);
                                        fuResim.SaveAs(_leaveClass.AttachmentPath + @"\" + reqId + @"\" + fuResim.FileName);
                                    }
                                    else //If there is no any file that uploaded
                                    {
                                        _leaveClass.NoAttachment(reqId.ToString());
                                    }
                                }

                                /* MAIL */
                                string mailTitle;
                                string mailDesc;
                                if (Login.Language == "tr")
                                {
                                    mailDesc = "oluşturulmuştur";
                                    mailTitle = reqId + " Numaralı izin talebi oluşturulmuştur.";
                                }
                                else
                                {
                                    mailDesc = " has been created";
                                    mailTitle = "Leave " + reqId + " has been created.";
                                }

                                _appClass.SendMail(mailTitle, mailDesc, reqId.ToString(), 0);
                                /* MAIL */

                                Response.Redirect("~/LeaveDetails?id=" + reqId + "");
                                accordion.Visible = false;
                            }
                        }
                        else
                        {
                            Response.Write(Login.Language == "tr"
                                ? "<script lang='Javascript'>alert('İzin hakkınız bulunmadığı için izin alınamaz.');</script>"
                                : "<script lang='Javascript'>alert('Cannot get leave because you do not have any leave right.'); </script>");
                        }
                    }
                }
            }
            else //If leave is not suitable to take
            {
                Response.Write(Login.Language == "tr"
                    ? "<script lang='Javascript'>alert('2 haftadan uzun izin alınamaz. Lütfen izin bilgilerini düzenleyiniz.');</script>"
                    : "<script lang='Javascript'>alert('More than 2 weeks leave can not be taken. Please edit the leave period.'); </script>");
            }
        }
    }
}