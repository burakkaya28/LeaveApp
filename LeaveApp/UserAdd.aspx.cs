using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class Add : ApplicationClass
    {
        private readonly UserClass _add = new UserClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            new ApplicationClass().AdminAuthorityCheck(Response);

            var userId = new Login().User.Identity.Name;
            var locationId = 0;
            var locationName = string.Empty;

            var scon = new SqlConnection(new ApplicationClass().Constr);
            scon.Open();

            var scmd = new SqlCommand("select l.LocationId, l.LocationName, l.LocationNameTR from Users u join Users m ON m.UserId = u.ManagerId join Users sm ON sm.UserId = u.SecondManager join Locations l ON l.LocationId = u.LocationId where u.UserId= '" + userId + "'", scon);
            var sreader = scmd.ExecuteReader();
            while (sreader.Read())
            {
                locationId = Convert.ToInt32(sreader["LocationId"].ToString());
                locationName = Login.Language == "tr"
                    ? sreader["LocationNameTR"].ToString()
                    : sreader["LocationName"].ToString();
            }
            sreader.Close();

            if (LocationDDL.Items.Count == 0)
            {
                //Location has been added to related fields.
                LocationDDL.Items.Insert(0, new ListItem(locationName, locationId.ToString()));

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
                        LocationDDL.Items.Add(new ListItem(locations, locationsId));
                }
                sreader.Close();
            }

            scon.Close();
        }

        protected override void InitializeCulture()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Login.Language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Login.Language);
            base.InitializeCulture();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var fullname = Request.Form["FullName"]; //Getting Full Name from Textbox
            var email = Request.Form["email"]; //Getting Email from Textbox
            var startDate = Request.Form["StartDate"]; //Getting StartDate from Textbox
            var role = "";
            var roleId = 0;
            var teamId = 0;
            var managerId = 0;
            var secondManagerId=0;
            var locationId = 0;

            var adminCheck = Request.Form["IsAdmin"];
            var managerCheck = Request.Form["IsManager"];
            var secondManagerCheck = Request.Form["IsSecondManager"];
            var financialCheck = Request.Form["IsFinancialUser"];

            /* Dropdown Controls */
            if (!string.IsNullOrEmpty(ddlUnvan.SelectedValue))
            {
                role = ddlUnvan.SelectedItem.ToString();
                roleId = int.Parse(ddlUnvan.SelectedItem.Value);
            }

            if(!string.IsNullOrEmpty(TeamDDL.SelectedValue))
                teamId = int.Parse(TeamDDL.SelectedValue);

            if (!string.IsNullOrEmpty(ddlEmployees.SelectedValue))
                 managerId = int.Parse(ddlEmployees.SelectedValue);

            if (!string.IsNullOrEmpty(secondManager.SelectedValue))
                 secondManagerId = int.Parse(secondManager.SelectedValue);

            if (!string.IsNullOrEmpty(LocationDDL.SelectedValue))
                locationId = int.Parse(LocationDDL.SelectedValue);
            /* Dropdown Controls - Finish */


            //If all fields are not bull
            if (fullname != "" && email != "" && roleId != 0 && managerId != 0 && teamId != 0)
            {
                //Check mail if it exists in the system
                var emailResult = _add.UserMailControl(email);
                if (emailResult) //If it exists
                {
                    Response.Write("<script>alert('Email adresi mevcut!');</script>");
                }

                //All values send to User Class
                var newUser = new UserClass
                {
                    FullName = fullname,
                    StartDate = SetDateFormat(startDate),
                    Email = email,
                    Role = role,
                    TeamId = teamId,
                    ManagerId = managerId,
                    SecondManagerId = secondManagerId != 0 ? secondManagerId.ToString() : null,
                    LocationId = locationId,
                    AdminCheck = adminCheck,
                    ManagerCheck = managerCheck,
                    SecondManagerCheck = secondManagerCheck,
                    FinancialUserCheck = financialCheck
                };

                if (emailResult == false) //If mail does not exist in system
                {
                    //Trying to add user to Database
                    Response.Write(_add.AddUser(newUser)
                        ? "<script lang='Javascript'>alert('Kullanıcı başarıyla oluşturulmuştur.'); window.location = 'AddUser'</script>"
                        : "<script>alert('Kullanıcı oluşturma işlemi başarısız. Lütfen sistem admin ile iletişime geçiniz.'); window.location = 'AddUser'</script>");
                }
            }
            else //If any value is missing
            {
                Response.Write(Login.Language == "tr"
                    ? "<script lang='Javascript'>alert('Tüm alanları doldurunuz.'); window.location = 'AddUser'</script>"
                    : "<script lang='Javascript'>alert('Fill in all fields.'); window.location = 'AddUser'</script>");
            }
        }

        protected void BackButton_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/UserManagement", false);
        }
    }
}