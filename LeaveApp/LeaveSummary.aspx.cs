using System;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class LeaveSummary : ApplicationClass
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            new ApplicationClass().AdminAndManagerAuthorityCheck(Response);
        }

        protected override void InitializeCulture()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Login.Language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Login.Language);
            base.InitializeCulture();
        }

        protected void Export_Click(object sender, EventArgs e)
        {
            new ApplicationClass().ExportToExcel(exportTable, Response);
        }

        protected void ExportMobile_OnClickMobile(object sender, EventArgs e)
        {
            var sql = "";
            var userId = new Login().User.Identity.Name;
            var isManager = new ApplicationClass().IsManager(userId);
            var isAdmin = new ApplicationClass().IsAdmin(userId);

            if (isManager == "Y")
                sql = @"select * from OPT_LEAVE_SUMMARY where ManagerId = '" + userId + "'";

            if (isAdmin == "Y")
                sql = @"select * from OPT_LEAVE_SUMMARY order by FullName";

            new ReportMail().LeaveSummaryReportviaMail(
                new UserClass().GetUserMail(new Login().User.Identity.Name),
                "Leave Summary Leaves Report",
                sql,
                "7"
            );
        }
    }
}