using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class UserEdit : ApplicationClass
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var userId = Request.Params.Get("id");
            if (!Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            //If user does not exist
            if (new UserClass().UserExistControl(userId) == false)
            {
                //Go to Dashboard page
                Response.Redirect("~/Dashboard");
            }

            new ApplicationClass().AdminAuthorityCheck(Response);

            var managerId = 0;
            var teamId = "";
            var teamName = string.Empty;
            var secondManagerId = "";
            var userRole = "";
            var managerName = "";
            var locationId = 0;
            var locationName = string.Empty;
            var isActive = "";

            var scon = new SqlConnection(new ApplicationClass().Constr);
            scon.Open();
            var scmd = new SqlCommand("select u.Role, u.TeamId, t.TeamName, u.ManagerId, m.FullName Manager, u.SecondManager SecondManagerId, sm.FullName SecondManager, l.LocationId, l.LocationName, l.LocationNameTR from Users u join Users m ON m.UserId = u.ManagerId left join Users sm ON sm.UserId = u.SecondManager left join Teams t ON t.TeamId = u.TeamId join Locations l ON l.LocationId = u.LocationId where u.UserId= '" + userId + "'", scon);
            var sreader = scmd.ExecuteReader();
            while (sreader.Read())
            {
                managerId = Convert.ToInt32(sreader["ManagerId"].ToString());
                teamId = sreader["TeamId"].ToString();
                teamName = sreader["TeamName"].ToString();
                secondManagerId = sreader["SecondManagerId"].ToString();
                userRole = sreader["Role"].ToString();
                managerName = sreader["Manager"].ToString();
                locationId = Convert.ToInt32(sreader["LocationId"].ToString());
                locationName = Login.Language == "tr"
                    ? sreader["LocationNameTR"].ToString()
                    : sreader["LocationName"].ToString();
            }
            sreader.Close();

            scmd = new SqlCommand("select 'Y' IsActive from Users where UserId = '" + userId + "' and EnabledFlag=0 ", scon);
            sreader = scmd.ExecuteReader();
            while (sreader.Read())
            {
                isActive = sreader["IsActive"].ToString();
            }
            sreader.Close();

            if (ddlUnvan.Items.Count == 0)
            {
                //User's role has been added to Role field
                ddlUnvan.Items.Insert(0, new ListItem(userRole, userRole));

                //All roles options have been adding by sql
                scmd = new SqlCommand("select RoleId ID, RoleName from Roles where EnabledFlag=1 order by 2", scon);
                sreader = scmd.ExecuteReader();
                while (sreader.Read())
                {
                    var role = sreader["RoleName"].ToString();
                    ddlUnvan.Items.Add(new ListItem(role, role));
                }
                sreader.Close();
            }

            if (TeamDDL.Items.Count == 0)
            {
                TeamDDL.Items.Insert(0, new ListItem(teamName, teamId));

                scmd = new SqlCommand("select TeamId, TeamName from Teams where EnabledFlag = 1 order by 2", scon);
                sreader = scmd.ExecuteReader();
                while (sreader.Read())
                {
                    if (teamName != sreader["TeamName"].ToString())
                        TeamDDL.Items.Add(new ListItem(sreader["TeamName"].ToString(), sreader["TeamId"].ToString()));
                }
                sreader.Close();
            }

            if (ddlEmployees.Items.Count == 0)
            {
                //Manager has been added to related fields.
                ddlEmployees.Items.Insert(0, new ListItem(managerName, managerId.ToString()));

                //All manager options have been adding by sql
                scmd = new SqlCommand("select u.UserId ID, u.FullName NAME from Users u inner join Managers on u.UserId = Managers.UserId order by 2", scon);
                sreader = scmd.ExecuteReader();
                while (sreader.Read())
                {
                    if (managerName != sreader["NAME"].ToString())
                        ddlEmployees.Items.Add(new ListItem(sreader["NAME"].ToString(), sreader["ID"].ToString()));
                }
                sreader.Close();
            }

            if (secondManagerDDL.Items.Count == 0)
            {
                //Free line was added...
                secondManagerDDL.Items.Insert(0, new ListItem("", "0"));

                //All second manager options have been adding by sql
                scmd = new SqlCommand("select u.UserId ID ,u.FullName NAME from Users u inner  join SecondManagers on u.UserId=SecondManagers.UserId order by 2", scon);
                sreader = scmd.ExecuteReader();
                while (sreader.Read())
                {
                    secondManagerDDL.Items.Add(new ListItem(sreader["NAME"].ToString(), sreader["ID"].ToString()));
                }

                if (secondManagerId != "")
                {
                    //Select the second manager
                    secondManagerDDL.Items.FindByValue(secondManagerId).Selected = true;
                }
            }

            if (locationDDL.Items.Count == 0)
            {
                //Location has been added to related fields.
                locationDDL.Items.Insert(0, new ListItem(locationName, locationId.ToString()));

                //All Location options have been adding by sql
                scmd = new SqlCommand("select LocationId, LocationName, LocationNameTR from Locations where EnabledFlag = 1", scon);
                sreader = scmd.ExecuteReader();
                while (sreader.Read())
                {
                    var locationsId = sreader["LocationId"].ToString();
                    var locations = Login.Language == "tr"
                        ? sreader["LocationNameTR"].ToString()
                        : sreader["LocationName"].ToString();
                    if (locationName != locations)
                        locationDDL.Items.Add(new ListItem(locations, locationsId));
                }
            }

            //If user to be edit is not active
            if (isActive != "Y")
            {
                ddlUnvan.Enabled = false;
                ddlEmployees.Enabled = false;
                secondManagerDDL.Enabled = false;
                locationDDL.Enabled = false;
                TeamDDL.Enabled = false;
                
                ddlUnvan.BackColor = Color.FromName("#eee");
                ddlEmployees.BackColor = Color.FromName("#eee");
                secondManagerDDL.BackColor = Color.FromName("#eee");
                locationDDL.BackColor = Color.FromName("#eee");
                TeamDDL.BackColor = Color.FromName("#eee");
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
            try
            {
                var startDate = Request.Form["StartDate"];
                var userIDe = Request.Params.Get("id");
                var fullnameE = Request.Form["createdby"];
                var emailE = Request.Form["email"];
                var userRole = ddlUnvan.SelectedItem.ToString();
                var teamId = Convert.ToInt32(TeamDDL.SelectedValue);
                var manager = Convert.ToInt32(ddlEmployees.SelectedValue);
                var secondManager = Convert.ToInt32(secondManagerDDL.SelectedValue);
                var locationIdValue = Convert.ToInt32(locationDDL.SelectedValue);

                var adminCheck = Request.Form["IsAdmin"];
                var managerCheck = Request.Form["IsManager"];
                var secondManagerCheck = Request.Form["IsSecondManager"];
                var financialCheck = Request.Form["IsFinancialUser"];

                if (Request.Form["StartDate"] != null && Request.Form["StartDate"] != "")
                {
                    startDate = SetDateFormat(Request.Form["StartDate"]);
                }
                
                var userClass = new UserClass
                {
                    FullName = fullnameE,
                    StartDate = startDate,
                    Email = emailE,
                    UserId = Convert.ToInt32(userIDe),
                    ManagerId = manager,
                    SecondManagerId = secondManager != 0 ? secondManager.ToString() : null,
                    Role = userRole,
                    TeamId = teamId,
                    LocationId = locationIdValue,
                    AdminCheck = adminCheck,
                    ManagerCheck = managerCheck,
                    SecondManagerCheck = secondManagerCheck,
                    FinancialUserCheck = financialCheck
                };

                if (fullnameE != "" && emailE != "" && userIDe != "" && startDate != "" && manager != 0 && userRole != "")
                {
                    try
                    {
                        var userUpdateFlag = userClass.UserUpdate(userClass);
                        Response.Write(userUpdateFlag
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
            catch (Exception exception)
            {
                Response.Write("Exception:" + exception);
                throw;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var userId = Request.Params.Get("id");
            var userClass = new UserClass();

            try
            {
                var deActivateFlag = userClass.UserDisable(userId);
                if (deActivateFlag)
                {
                    Response.Write("<script lang='Javascript'>alert('Kullanıcı deaktif edilmiştir.');</script>");

                    ddlUnvan.Enabled = false;
                    ddlEmployees.Enabled = false;
                    secondManagerDDL.Enabled = false;
                    locationDDL.Enabled = false;
                    TeamDDL.Enabled = false;

                    ddlUnvan.BackColor = Color.FromName("#eee");
                    ddlEmployees.BackColor = Color.FromName("#eee");
                    secondManagerDDL.BackColor = Color.FromName("#eee");
                    locationDDL.BackColor = Color.FromName("#eee");
                    TeamDDL.BackColor = Color.FromName("#eee");
                }
                else
                {
                    Response.Write("<script lang='Javascript'>alert('Kullanıcı deaktif etme işlemi başarısız.');</script>");
                }
            }
            catch (SqlException ex)
            {
                Response.Write("<script lang='Javascript'>alert('Bir hata oluştu.! Lütfen sistem admin ile iletişime geçiniz. Hata Detayı: " + ex.Message + "');</script>");
            }
        }

        protected void btnActive_Click(object sender, EventArgs e)
        {
            var userId = Request.Params.Get("id");
            var userClass = new UserClass();
            try
            {
                var activateFlag = userClass.UserActivate(userId);
                if (activateFlag)
                {
                    Response.Write("<script lang='Javascript'>alert('Kullanıcı aktif edilmiştir.');</script>");
                    ddlUnvan.Enabled = true;
                    ddlEmployees.Enabled = true;
                    secondManagerDDL.Enabled = true;
                    locationDDL.Enabled = true;
                    TeamDDL.Enabled = true;

                    ddlUnvan.BackColor = Color.Transparent;
                    ddlEmployees.BackColor = Color.Transparent;
                    secondManagerDDL.BackColor = Color.Transparent;
                    locationDDL.BackColor = Color.Transparent;
                    TeamDDL.BackColor = Color.Transparent;
                }
                else
                {
                    Response.Write("<script lang='Javascript'>alert('Kullanıcı aktif etme işlemi başarısız.');</script>");
                }
            }
            catch (SqlException ex)
            {
                Response.Write("<script lang='Javascript'>alert('Bir hata oluştu.! Lütfen sistem admin ile iletişime geçiniz. Hata Detayı: " + ex.Message + "');</script>");
            }
        }

        protected void BackButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/UserManagement", false);
        }
    }
}