<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DemoWebApp.Login" %>
<%@ Import Namespace="Resources" %>
<!DOCTYPE html>
<html>
<head>
  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <link rel ="shortcut icon" type="image/x-icon" href="images/schedule.png" />
   <title>OPTiiM | Leave App</title>
  <!-- Tell the browser to be responsive to screen width -->
  <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
  <!-- Bootstrap 3.3.6 -->
  <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css">
  <link rel="stylesheet" href="dist/css/AdminLTE.min.css">
  <link rel="stylesheet" href="plugins/iCheck/square/blue.css">
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.5.0/css/font-awesome.min.css">

</head>
<body class="hold-transition login-page">
<div class="login-box">
  <div class="login-logo">
    <a href="#"><b>OPT<span style="color:gray;">ii</span>M</b></a>
  </div>

  <div class="login-box-body">
    <p class="login-box-msg"><%=Global.Login_lbl %></p>

    <form id="form1" runat="server">
          <!--<asp:Login ID="Login1" runat="server" OnAuthenticate="ValidateUser"   Font-Names="Verdana" Font-Size="0.8em" Height="200px" >
        <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
        <LayoutTemplate>
            <table cellpadding="4" cellspacing="0" style="border-collapse:collapse;">
                <tr>
                    <td>
                        <table cellpadding="0" style="height:200px;width:320px;">
                           
                            <tr>
                                <td >
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="UserName" runat="server" Font-Size="13px" Width="80%" Height="21px" ></asp:TextBox>
                                
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="Password" runat="server" Font-Size="13px" TextMode="Password" Width="80%" Height="21px"></asp:TextBox>
                               
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="RememberMe" runat="server" Text="&nbsp;Remember me" Font-Bold="True" Checked="True" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color:Red;">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="LoginButton" runat="server" BackColor="White" BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px" CommandName="Login" Font-Names="Verdana"  Width="80%" Height="25px" Font-Size="14px" ForeColor="#284E98" Text="Log In" ValidationGroup="Login1" />
                                  
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">OR<br /> </td>
                            </tr>
                            <tr>
                                 <td align="center" colspan="2">
                                   
                          
                                 </td>
                                </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <LoginButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" />
        <TextBoxStyle Font-Size="0.8em" />
        <TitleTextStyle BackColor="#507CD1" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
    </asp:Login>-->
        <div class="container-fluid col-md-offset-1">
            <div class="form-group">
           
             <label><%=Global.Login_language_lbl %></label>
            
              <div class="row">
                 <div class="col-md-11">
                     <asp:DropDownList ID="lblDil" runat="server" class="form-control"  AutoPostBack="True"  onselectedindexchanged="ddlDil_SelectedIndexChanged">  
                     <asp:ListItem Text="<%$ Resources:Global, Login_language_tr %>" Value="<%$ Resources:Global, Login_language_tr_value %>"></asp:ListItem> 
                     <asp:ListItem Text="<%$ Resources:Global, Login_language_en %>" Value="<%$ Resources:Global, Login_language_en_value %>"></asp:ListItem> 
                    </asp:DropDownList>
                </div>
              </div>

             </div>

             <div class="form-group">
                   <div class="row">
		              <div class="col-md-11">
                          <asp:linkbutton runat="server" class="btn btn-block btn-social btn-google" OnClick="Logins">
                                 <i class="fa fa-google-plus"></i> <%=Global.Login_btn%>
		                  </asp:linkbutton>
			           </div>
			      </div>
              </div>


        </div>

    </form>

     <%
         if(ConfigurationManager.AppSettings["AppMode"] == "MAINTENANCE") {
             %>
                <p class="text-danger text-center">Leave application is under maintenance and not available for a while.</p>
             <%
         }
     %>   
     
     <p class="text-danger text-center"><asp:Label ID="WarningText" runat="server" Text=""></asp:Label></p>
     <p class="text-info text-center"><%=Global.LeaveAppSupport%>: <a href="mailto:leave.support@optiim.com">leave.support@optiim.com</a></p>
  </div>
</div>


<script src="bootstrap/js/bootstrap.min.js"></script>
<script src="plugins/jQuery/jQuery-2.2.0.min.js"></script>
<script src="plugins/google/platform.js" async defer></script>
<script>

function onSuccessG(googleUser) {
        var profile = googleUser.getBasicProfile();
        console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
        console.log('Name: ' + profile.getName());
        console.log('Image URL: ' + profile.getImageUrl());
        console.log('Email: ' + profile.getEmail());
}
function onFailureG(error) {
    console.log(error);
}
function renderGmail() {

  gapi.signin2.render('my-signin2', {
    'scope': 'https://www.googleapis.com/auth/plus.login',
    'width': 0,
    'height': 0,
    'longtitle': true,
    'theme': 'dark',
    'onsuccess': onSuccessG,
    'onfailure': onFailureG
  });
}
</script>
<!-- iCheck -->
<script src="plugins/iCheck/icheck.min.js"></script>
<script>
  $(function () {
    $('input').iCheck({
      checkboxClass: 'icheckbox_square-blue',
      radioClass: 'iradio_square-blue',
      increaseArea: '20%' // optional
    });
  });
</script>
</body>
</html>