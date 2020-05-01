using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using System.Web.UI;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class LeaveTypeEdit : Page
    {
        private readonly LeaveTypeClass _leaveTypeClass = new LeaveTypeClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            new ApplicationClass().AdminAuthorityCheck(Response);

            var leaveTypeId = Request.Params.Get("id");
            //If leave type does not exist
            if (_leaveTypeClass.LeaveTypeExistControlwithId(leaveTypeId) == false)
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
            var leaveTypeId = Request.Params.Get("id");
            var leaveType = Request.Form["leaveType"];
            var leaveTypeTr = Request.Form["leaveTypeTr"];

            if (leaveType != "" && leaveTypeTr != "")
            {
                try
                {
                    var leaveTypeUpdateFlag = _leaveTypeClass.LeaveTypeUpdate(leaveTypeId, leaveType, leaveTypeTr);
                    Response.Write(leaveTypeUpdateFlag
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
            var leaveTypeId = Request.Params.Get("id");
            try
            {
                var deActivateFlag = _leaveTypeClass.LeaveTypeDisable(leaveTypeId);
                Response.Write(deActivateFlag
                    ? "<script lang='Javascript'>alert('İzin tipi deaktif edilmiştir.');</script>"
                    : "<script lang='Javascript'>alert('İzin tipi deaktif etme işlemi başarısız.');</script>");
            }
            catch (SqlException ex)
            {
                Response.Write("<script lang='Javascript'>alert('Bir hata oluştu.! Lütfen sistem admin ile iletişime geçiniz. Hata Detayı: " + ex.Message + "');</script>");
            }
        }

        protected void btnActive_Click(object sender, EventArgs e)
        {
            var leaveTypeId = Request.Params.Get("id");
            try
            {
                var activateFlag = _leaveTypeClass.LeaveTypeActivate(leaveTypeId);
                Response.Write(activateFlag
                    ? "<script lang='Javascript'>alert('İzin tipi aktif edilmiştir.');</script>"
                    : "<script lang='Javascript'>alert('İzin tipi aktif etme işlemi başarısız.');</script>");
            }
            catch (SqlException ex)
            {
                Response.Write("<script lang='Javascript'>alert('Bir hata oluştu.! Lütfen sistem admin ile iletişime geçiniz. Hata Detayı: " + ex.Message + "');</script>");
            }
        }

        protected void BackButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/LeaveTypeManagement");
        }
    }
}