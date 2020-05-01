using System;
using System.Globalization;
using System.Threading;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DemoWebApp.classes;

namespace DemoWebApp
{
    public partial class LeavePolicies : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            new ApplicationClass().AdminAuthorityCheck(Response);

            //Allow Leave If Zero parameter
            if (AllowLeaveIfZeroDDL.Items.Count == 0)
            {
                var allowLeaveIfZero = new LeavePolicyClass().GetPolicyValueByKey("ALLOW_GET_LEAVE_IF_RESOURCE_NO_LEAVE");
                AllowLeaveIfZeroDDL.Items.Add(new ListItem(Resources.Global.DropdownValue_Yes, "1"));
                AllowLeaveIfZeroDDL.Items.Add(new ListItem(Resources.Global.DropdownValue_No, "0"));

                if (allowLeaveIfZero == "1" || allowLeaveIfZero == "0")
                {
                    AllowLeaveIfZeroDDL.Items.FindByValue(allowLeaveIfZero).Selected = true;
                }
            }
        }

        protected void SaveButton_OnClickon(object sender, EventArgs e)
        {
            try
            {
                //If leave duration field is not null
                if (Request.Form["leaveDuration"] != null)
                {
                    var leaveDuration = Request.Form["leaveDuration"];
                    int n;
                    var isNumeric = int.TryParse(leaveDuration, out n);

                    if (isNumeric)
                    {
                        //Allowed Leave Duration for Before
                        new LeavePolicyClass().SetPolicyValueByKey("ALLOWED_LEAVE_DURATION_FOR_BEFORE", leaveDuration);

                        //Allow Leave If Resource has not got any leave right
                        new LeavePolicyClass().SetPolicyValueByKey("ALLOW_GET_LEAVE_IF_RESOURCE_NO_LEAVE",
                            AllowLeaveIfZeroDDL.SelectedItem.Value);

                        Response.Write(
                            Login.Language == "tr" ?
                                "<script lang='Javascript'>alert('Başarıyla kaydedilmiştir.');</script>"
                                : "<script lang='Javascript'>alert('Saved successfully.');</script>");
                    }
                }
                else
                {
                    Response.Write(
                        Login.Language == "tr" ?
                            "<script lang='Javascript'>alert('İzin süresi alanı boş bırakılamaz.'); window.location = 'Dashboard'</script>"
                            : "<script lang='Javascript'>alert('Leave Duration field cannot be empty.'); window.location = 'Dashboard'</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
        }

        protected override void InitializeCulture()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Login.Language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Login.Language);
            base.InitializeCulture();
        }
    }
}