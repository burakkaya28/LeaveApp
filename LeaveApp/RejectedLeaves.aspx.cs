using System;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class RejectedRequests : ApplicationClass
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
            new ApplicationClass().ExportToExcel(tablebody, Response);
        }

        protected void ExportMobile_OnClick(object sender, EventArgs e)
        {
            new ReportMail().RejectedLeavesReportviaMail(
                new UserClass().GetUserMail(new Login().User.Identity.Name),
                "Rejected Leaves Report",
                new ReportSqlQueries().RejectedLeaveSql,
                "8"
            );
        }
    }
}