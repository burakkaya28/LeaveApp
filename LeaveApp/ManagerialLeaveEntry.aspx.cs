using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class ManagerialLeaveEntry : ApplicationClass
    {
        private static readonly LeaveClass LeaveClass = new LeaveClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            new ApplicationClass().AdminAuthorityCheck(Response);

            var scon = new SqlConnection(new ApplicationClass().Constr);
            scon.Open();

            if (LocationDDL.Items.Count == 0)
            {
                //All Location options have been adding by sql
                var scmd = new SqlCommand("select LocationId, LocationName, LocationNameTR from Locations where EnabledFlag = 1", scon);
                var sreader = scmd.ExecuteReader();
                while (sreader.Read())
                {
                    var locationsId = sreader["LocationId"].ToString();
                    var locations = Login.Language == "tr"
                        ? sreader["LocationNameTR"].ToString()
                        : sreader["LocationName"].ToString();
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
            try
            {
                var locationId = 0;
                var leaveDate = Request.Form["leaveDate"];

                if (!string.IsNullOrEmpty(LocationDDL.SelectedValue))
                    locationId = int.Parse(LocationDDL.SelectedValue);

                if (leaveDate != "")
                {
                    leaveDate = SetDateFormat(leaveDate);
                    var conn = new SqlConnection(Constr);
                    var comm = new SqlCommand("select UserId from Users where EnabledFlag=0 and Role NOT IN ('Founder, CEO', 'Chairman') and LocationId = " + locationId, conn);
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["UserId"] != DBNull.Value)
                        {
                            LeaveClass.CreateManagerialLeave(dr["UserId"].ToString(), leaveDate);
                        }
                    }

                    dr.Close();
                    conn.Close();

                    Response.Write(Login.Language == "tr"
                        ? "<script lang='Javascript'>alert('İdari izinler başarıyla oluşturulmuştur.');</script>"
                        : "<script lang='Javascript'>alert('Managerial leaves have been created.'); </script>");
                }
                else
                {
                    Response.Write(Login.Language == "tr"
                        ? "<script lang='Javascript'>alert('Tüm alanları doldurunuz.');</script>"
                        : "<script lang='Javascript'>alert('Fill in all fields.'); </script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(Login.Language == "tr"
                    ? "<script lang='Javascript'>alert('Hata: "+ ex + "');</script>"
                    : "<script lang='Javascript'>alert('Error: " + ex + "'); </script>");
            }
        }
    }
}