using System;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using System.Web.UI;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class LeaveTypeManagement : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            new ApplicationClass().AdminAuthorityCheck(Response);
        }

        protected void Export_Click(object sender, EventArgs e)
        {
            new ApplicationClass().ExportToExcel(tablebody, Response);
        }

        protected override void InitializeCulture()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Login.Language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Login.Language);
            base.InitializeCulture();
        }

        protected void AddLeaveType_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AddLeaveType");
        }
    }
}