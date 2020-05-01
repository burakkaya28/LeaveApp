using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class ReportTopRequest : ApplicationClass
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            new ApplicationClass().AdminAndManagerAuthorityCheck(Response);

            GetYearDdLdata();
        }

        /// <summary>
        /// This method is used to get current years and 2 year below and above
        /// </summary>
        public void GetYearDdLdata()
        {
            //If year list does not include any value in list context
            if (YearDDL.Items.Count == 0)
            {
                using (var con = new SqlConnection(Constr))
                {
                    //Get years
                    using (var cmd = new SqlCommand("SELECT YEAR(GETDATE()) as YearNum UNION SELECT YEAR(GETDATE()) - 1 as YearNum UNION SELECT YEAR(GETDATE()) - 2 as YearNum ORDER BY YearNum DESC"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        if(con.State != ConnectionState.Open) con.Open();
                        var sreader = cmd.ExecuteReader();
                        while (sreader.Read())
                        {
                            //Add years in to the list...
                            YearDDL.Items.Add(sreader["YearNum"].ToString());
                        }
                        sreader.Close();
                        con.Close();
                    }
                }

                //This select the current year in the list
                YearDDL.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Language Property
        /// </summary>
        protected override void InitializeCulture()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Login.Language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Login.Language);
            base.InitializeCulture();
        }

        /// <summary>
        /// Export the excel method
        /// </summary>
        protected void Export_Click(object sender, EventArgs e)
        {
            new ApplicationClass().ExportToExcel(tablebody, Response);
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}