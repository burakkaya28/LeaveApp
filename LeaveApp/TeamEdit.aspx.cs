using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class TeamEdit : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            new ApplicationClass().AdminAuthorityCheck(Response);

            var teamId = Request.Params.Get("id");

            //If team does not exist
            if (new TeamClass().TeamExistControlwithId(teamId) == false)
            {
                //Go to Dashboard page
                Response.Redirect("~/Dashboard", false);
            }

            var managerId = new TeamClass().GetManagerIdByTeam(teamId);
            var scon = new SqlConnection(new ApplicationClass().Constr);

            if (ManagerDDL.Items.Count != 0) return;
            scon.Open();
            var scmd = new SqlCommand(
                "select u.UserId,u.FullName ManagerName from Users u inner join Managers on u.UserId=Managers.UserId order by 2",
                scon);
            var sreader = scmd.ExecuteReader();
            while (sreader.Read())
            {
                ManagerDDL.Items.Add(new ListItem(sreader["ManagerName"].ToString(), sreader["UserId"].ToString()));
            }

            sreader.Close();
            ManagerDDL.Items.FindByValue(managerId).Selected = true;

            var isActive = new TeamClass().GetTeamStatus(teamId);

            if (isActive != "Y")
            {
                ManagerDDL.Enabled = false;
                ManagerDDL.BackColor = Color.FromName("#eee");
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
            var teamId = Request.Params.Get("id");
            var teamName = Request.Form["teamName"];

            if (teamName != "")
            {
                try
                {
                    var teamUpdateFlag = new TeamClass().TeamUpdate(teamId, teamName, 52);
                    Response.Write(teamUpdateFlag
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
            var teamId = Request.Params.Get("id");
            try
            {
                var deActivateFlag = new TeamClass().TeamDisable(teamId);
                Response.Write(deActivateFlag
                    ? "<script lang='Javascript'>alert('Ekip deaktif edilmiştir.');</script>"
                    : "<script lang='Javascript'>alert('Ekip deaktif etme işlemi başarısız.');</script>");

                ManagerDDL.Enabled = false;
                ManagerDDL.BackColor = Color.FromName("#eee");
            }
            catch (SqlException ex)
            {
                Response.Write("<script lang='Javascript'>alert('Bir hata oluştu.! Lütfen sistem admin ile iletişime geçiniz. Hata Detayı: " + ex.Message + "');</script>");
            }
        }

        protected void btnActive_Click(object sender, EventArgs e)
        {
            var teamId = Request.Params.Get("id");
            try
            {
                var activateFlag = new TeamClass().TeamActivate(teamId);
                Response.Write(activateFlag
                    ? "<script lang='Javascript'>alert('Ekip aktif edilmiştir.');</script>"
                    : "<script lang='Javascript'>alert('Ekip aktif etme işlemi başarısız.');</script>");

                ManagerDDL.Enabled = true;
                ManagerDDL.BackColor = Color.Transparent;
            }
            catch (SqlException ex)
            {
                Response.Write("<script lang='Javascript'>alert('Bir hata oluştu.! Lütfen sistem admin ile iletişime geçiniz. Hata Detayı: " + ex.Message + "');</script>");
            }
        }

        protected void BackButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/TeamManagement", false);
        }
    }
}