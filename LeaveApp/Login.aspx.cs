using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using ASPSnippets.GoogleAPI;
using DemoWebApp.classes;
using log4net;

namespace DemoWebApp
{
    public partial class Login : Page
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string Language = "tr-TR";

        protected int UserId { get; set; }
        string _userEmail = "";

        protected override void InitializeCulture()
        {
            if (Request.Form["lblDil"] != null)
            {
                Language = Request.Form["lblDil"];
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Language);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language);
                base.InitializeCulture();
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Language);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language);
                base.InitializeCulture();
            }
            Session["selection_lang"] = Language;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.User.Identity.IsAuthenticated)
                {
                    Log.Warn("User "+ User.Identity.Name + " already logged in. Redirecting to login page.");
                    Response.Redirect("~/Dashboard", false);
                }
                
                if (!(Request.IsAuthenticated))
                {
                    GoogleConnect.ClientId = "947447230315-5lfekj88qber9m9pgq8g8t7oo7g4ih3h.apps.googleusercontent.com";
                    GoogleConnect.ClientSecret = "eSBeVilwnaDm7Rk8LDh-_SY4";
                    GoogleConnect.RedirectUri = Request.Url.AbsoluteUri.Split('?')[0];

                    if (!string.IsNullOrEmpty(Request.QueryString["code"]))
                    {
                        var code = Request.QueryString["code"];
                        var json = GoogleConnect.Fetch("me", code);
                        var profile = new JavaScriptSerializer().Deserialize<GoogleProfile>(json);
                        _userEmail = profile.Emails.Find(email => email.Type == "account").Value;
                        UserId = 0;
                        var strUserId = new UserClass().ValidateUser(_userEmail);

                        Log.Info("User [" + _userEmail + "] is not authenticated.");
                        Log.Info("User [" + _userEmail + "] login process is started.");
                        Log.Info("Google Auth Code: " + code);
                        Log.Info("Google Auth Redirect Uri: " + GoogleConnect.RedirectUri);

                        if (strUserId != "") { UserId = int.Parse(strUserId); }
                        
                        if (UserId != 0)
                        {
                            Log.Info("User Id: " + UserId);
                            new UserClass().UpdateLastLoginDate(UserId.ToString());
                            Log.Info("User ["+ _userEmail +"] Last Login Date is updated");
                            FormsAuthentication.SetAuthCookie(UserId.ToString(), false);
                            Response.Redirect("~/Dashboard", false);
                            Log.Info("User ["+ _userEmail + "] redirecting to Dashboard Page");
                        }
                        else
                        {
                            Log.Warn("User ["+ _userEmail + "] is not valid or active.");
                            var warningMessage = Language == "tr"
                                ? "[" + _userEmail + "] kullanıcısı geçersiz ya da aktif değil." 
                                : @"User [" + _userEmail + @"] is not valid or active.";
                            
                            WarningText.Text = warningMessage;

                            FormsAuthentication.SignOut();
                            Session.Abandon();
                        }
                    }
                    if (Request.QueryString["error"] == "access_denied")
                    {
                        Log.Warn("Google Auth Access Denied! " + Request.QueryString["error"]);
                        ClientScript.RegisterClientScriptBlock(GetType(), "alert", "alert('Access denied.')", true);
                    }
                }
            }

            catch (Exception ex)
            {
                Log.Error("Exception: " + ex);
            }
        }

        protected void ValidateUser(object sender, EventArgs e)
        {
            UserId = 0;
            var constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (var con = new SqlConnection(constr))
            {
                using (var cmd = new SqlCommand("Validate_User"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", Login1.UserName);
                    cmd.Connection = con;
                    con.Open();
                    UserId = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }

                switch (UserId)
                {
                    case -1:
                        break;
                    case -2:
                        break;
                    default:
                        FormsAuthentication.RedirectFromLoginPage(UserId.ToString(), true);
                        break;
                }
            }
        }

        protected void Logins(object sender, EventArgs e)
        {
            GoogleConnect.Authorize("profile", "email");
        }

        protected void Clear(object sender, EventArgs e)
        {
            GoogleConnect.Clear();
        }

        public class GoogleProfile
        {
            public string Id { get; set; }
            public string DisplayName { get; set; }
            public Image Image { get; set; }
            public List<Email> Emails { get; set; }
            public string Gender { get; set; }
            public string ObjectType { get; set; }
        }

        public class Email
        {
            public string Value { get; set; }
            public string Type { get; set; }
        }

        public class Image
        {
            public string Url { get; set; }
        }

        protected void ddlDil_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}