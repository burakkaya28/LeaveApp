using System;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using System.Web.UI;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class LeaveTypeAdd : Page
    {
        private readonly LeaveTypeClass _leaveTypeClass = new LeaveTypeClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            new ApplicationClass().AdminAuthorityCheck(Response);
        }

        protected override void InitializeCulture()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Login.Language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Login.Language);
            base.InitializeCulture();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var leaveType = Request.Form["LeaveType"];
            var leaveTypeTr = Request.Form["LeaveTypeTR"];

            //If all fields are not null
            if (leaveType != "" && leaveTypeTr != "")
            {
                var leaveTypeResult = _leaveTypeClass.LeaveTypeExistControl(leaveType);
                if (leaveTypeResult)
                {
                    Response.Write("<script>alert('İzin Tipi mevcut!');</script>");
                }
                else
                {
                    //Trying to add role to Database
                    Response.Write(_leaveTypeClass.AddLeaveType(leaveType, leaveTypeTr)
                        ? "<script lang='Javascript'>alert('İzin tipi başarıyla oluşturulmuştur.'); </script>"
                        : "<script>alert('İzin tipi oluşturma işlemi başarısız. Lütfen sistem admin ile iletişime geçiniz.'); </script>");
                }
            }
            else //If any value is missing
            {
                Response.Write(Login.Language == "tr"
                    ? "<script lang='Javascript'>alert('Tüm alanları doldurunuz.');</script>"
                    : "<script lang='Javascript'>alert('Fill in all fields.'); </script>");
            }
        }

        protected void BackButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/LeaveTypeManagement");
        }
    }
}