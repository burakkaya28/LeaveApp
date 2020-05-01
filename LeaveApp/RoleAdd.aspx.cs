using System;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using System.Web.UI;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class RoleAdd : Page
    {
        private readonly RoleClass _role = new RoleClass();

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
            var roleName = Request.Form["RoleName"];

            //If all fields are not null
            if (roleName != "")
            {
                var roleResult = _role.RoleExistControl(roleName);
                if (roleResult)
                {
                    Response.Write("<script>alert('Rol mevcut!');</script>");
                }
                else
                {
                    //Trying to add role to Database
                    Response.Write(_role.AddRole(roleName)
                        ? "<script lang='Javascript'>alert('Rol başarıyla oluşturulmuştur.'); </script>"
                        : "<script>alert('Rol oluşturma işlemi başarısız. Lütfen sistem admin ile iletişime geçiniz.'); </script>");
                }
            }
            else //If any value is missing
            {
                Response.Write(Login.Language == "tr"
                    ? "<script lang='Javascript'>alert('Tüm alanları doldurunuz.');</script>"
                    : "<script lang='Javascript'>alert('Fill in all fields.'); </script>");
            }
        }

        protected void BackButton_OnClicktton(object sender, EventArgs e)
        {
            Response.Redirect("~/RoleManagement");
        }
    }
}