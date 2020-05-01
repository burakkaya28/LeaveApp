using System;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class ApprovedRequests : ApplicationClass
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
            new ReportMail().ApprovedLeavesReportviaMail(
                new UserClass().GetUserMail(new Login().User.Identity.Name),
                "Approved Leaves Report",
                new ReportSqlQueries().ApprovedLeaveSql,
                "8"
            );
        }
    }
}