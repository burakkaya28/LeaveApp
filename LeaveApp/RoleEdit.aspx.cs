using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using System.Web.UI;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class RoleEdit : Page
    {
        private readonly RoleClass _roleClass = new RoleClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            new ApplicationClass().AdminAuthorityCheck(Response);

            var roleId = Request.Params.Get("id");

            //If role does not exist
            if (_roleClass.RoleExistControlwithId(roleId) == false)
            {
                //Go to Dashboard page
                Response.Redirect("~/Dashboard");
            }
        }
        protected override void InitializeCulture()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Login.Language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Login.Language);
            base.InitializeCulture();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            var roleId = Request.Params.Get("id");
            var roleName = Request.Form["roleName"];

            if (roleName != "")
            {
                try
                {
                    var roleUpdateFlag = _roleClass.RoleUpdate(roleId, roleName);
                    Response.Write(roleUpdateFlag
                        ? "<script lang='Javascript'>alert('Güncelleme İşlemi Başarılı İle Tamamlandı!'); </script>"
                        : "<script lang='Javascript'>alert('Güncelleme İşlemi Başarısız!); </script>");
                }
                catch (Exception ex)
                {
                    Response.Write("<script lang='Javascript'>alert('Bir hata oluştu.! Lütfen sistem admin ile iletişime geçiniz. Hata Detayı: " + ex.Message + "');</script>");
                }
            }
            else
            {
                Response.Write("<script lang='Javascript'>alert('Lütfen tüm alanları doldurunuz');</script>");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var roleId = Request.Params.Get("id");
            try
            {
                var deActivateFlag = _roleClass.RoleDisable(roleId);
                Response.Write(deActivateFlag
                    ? "<script lang='Javascript'>alert('Rol deaktif edilmiştir.');</script>"
                    : "<script lang='Javascript'>alert('Rol deaktif etme işlemi başarısız.');</script>");
            }
            catch (SqlException ex)
            {
                Response.Write("<script lang='Javascript'>alert('Bir hata oluştu.! Lütfen sistem admin ile iletişime geçiniz. Hata Detayı: " + ex.Message + "');</script>");
            }
        }

        protected void btnActive_Click(object sender, EventArgs e)
        {
            var roleId = Request.Params.Get("id");
            try
            {
                var activateFlag = _roleClass.RoleActivate(roleId);
                Response.Write(activateFlag
                    ? "<script lang='Javascript'>alert('Rol aktif edilmiştir.');</script>"
                    : "<script lang='Javascript'>alert('Rol aktif etme işlemi başarısız.');</script>");
            }
            catch (SqlException ex)
            {
                Response.Write("<script lang='Javascript'>alert('Bir hata oluştu.! Lütfen sistem admin ile iletişime geçiniz. Hata Detayı: " + ex.Message + "');</script>");
            }
        }

        protected void BackButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/RoleManagement");
        }
    }
}