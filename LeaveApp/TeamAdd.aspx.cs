using System;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using System.Web.UI;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class TeamAdd : Page
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

        protected void AddButton_OnClick(object sender, EventArgs e)
        {
            var teamName = Request.Form["TeamName"];
            var manager = Convert.ToInt32(ManagerDDL.SelectedItem.Value);

            //If all fields are not null
            if (teamName != "" && manager != 0)
            {
                var teamResult = new TeamClass().TeamExistControl(teamName);
                if (teamResult)
                {
                    Response.Write(Login.Language == "tr"
                        ? "<script lang='Javascript'>alert('Ekip sistemde mevcut!');</script>"
                        : "<script lang='Javascript'>alert('Team exists!'); </script>");
                }
                else
                {
                    //Trying to add team to Database
                    Response.Write(new TeamClass().AddTeam(teamName, manager)
                        ? "<script lang='Javascript'>alert('Ekip başarıyla oluşturulmuştur.'); </script>"
                        : "<script>alert('Ekip oluşturma işlemi başarısız. Lütfen sistem admin ile iletişime geçiniz.'); </script>");
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
            Response.Redirect("~/TeamManagement", false);
        }
    }
}