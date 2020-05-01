using System;
using System.Web.Security;
using System.Web.UI;
using DemoWebApp.classes;

namespace DemoWebApp
{

    public partial class Site1 : MasterPage
    {

        protected string Role { get; set; }
        protected string MyName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            var username = new Login().User.Identity.Name;

            if (username != "")
            {
                var appClass = new ApplicationClass();
                MyName = appClass.GetUserFullName(Convert.ToInt32(username));
                Role = appClass.GetUserRole(Convert.ToInt32(username));
            }
        }


        protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            FormsAuthentication.RedirectToLoginPage();
            //Response.Redirect("~/Login.aspx");
        }

       protected void l_turkish_Click(object sender, ImageClickEventArgs e)
        {

            Login.Language = "tr";
            Session["selection_lang"] = Login.Language;

            Response.Redirect(Request.RawUrl);
        }

        protected void l_english_Click(object sender, ImageClickEventArgs e)
        {
            Login.Language = "en";
            Session["selection_lang"] = Login.Language;

            Response.Redirect(Request.RawUrl);
        }
    }
}