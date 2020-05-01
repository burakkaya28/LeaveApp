using DemoWebApp.classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DemoWebApp
{
    public partial class SysLocationResponsibles : System.Web.UI.Page
    {
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

        protected void ExportButton_Click(object sender, EventArgs e)
        {
            new ApplicationClass().ExportToExcel(exportTable, Response);
        }
    }
}