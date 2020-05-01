using System;
using System.Reflection;
using System.Web.Security;
using DemoWebApp.classes;
using log4net;

namespace DemoWebApp
{
    public partial class Default: ApplicationClass
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            Log.Info("User: " + User.Identity.Name.ToString());
            if (!Page.User.Identity.IsAuthenticated)
            {
                Log.Warn("User is not authenticated. Redirected to Login Page!");
                FormsAuthentication.RedirectToLoginPage();
            }
        }
    }
}