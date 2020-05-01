<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="RoleEdit.aspx.cs" Inherits="DemoWebApp.RoleEdit" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DemoWebApp.classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
     <li>
        <a href="Dashboard">
            <i class="fa fa-dashboard"></i><span><%=Global.Menu_dashboard %></span>
        </a>
    </li>

    <li>
        <a href="LeaveRequest">
          <i class="fa fa-calendar-minus-o"></i><span><%=Global.Menu_request %></span>
        </a>
    </li>
     <%
        var appClass = new ApplicationClass();
        var userId = new DemoWebApp.Login().User.Identity.Name;
        var isAdmin = appClass.IsAdmin(userId);
        var isManager = appClass.IsManager(userId);

        if (isManager == "Y" && isAdmin != "Y")
        {

        %>
       <li class="treeview">
        <a href="#">
            <i class="fa fa-pie-chart"></i>
            <span><%=Global.Menu_reports%></span>
            <i class="fa fa-angle-left pull-right"></i>
        </a>
        <ul class="treeview-menu" style="display: none;">
            <li><a href="LeaveSummary"><i class="fa fa-circle-o"></i><%=Global.Default_leave_table_th7%></a></li>
           
        </ul>
    </li>

    <% } %>


    <% if (isAdmin == "Y")
        { %>

       <li class="treeview">
            <a href="#">
                <i class="fa fa-pie-chart"></i>
                <span><%=Global.Menu_reports%></span>
                <i class="fa fa-angle-left pull-right"></i>
            </a>
            <ul class="treeview-menu" style="display: none;">
                    <li>
                       <a href="AllLeaves">
                            <i class="fa fa-circle-o"></i><span><%=Global.Menu_all%></span>
                      </a>
                    </li>

                 <li>
                     <a href="PendingApprovals">
                         <i class="fa fa-circle-o"></i><span><%=Global.Menu_approvals%></span>
                     </a>
                </li>
                <li><a href="LeaveSummary"><i class="fa fa-circle-o"></i><%=Global.Default_leave_table_th7%></a></li>
            </ul>
        </li>
        
        <li class="treeview active">
            <a href="#">
                <i class="fa fa-user-plus"></i>
                <span><%=Global.Menu_management%></span>
                <i class="fa fa-angle-left pull-right"></i>
            </a>
            <ul class="treeview-menu">
                <li class="treeview active">
                    <a href="#">
                        <i class="fa fa-circle-o"></i><%=Global.Menu_Definitions%>
                        <i class="fa fa-angle-left pull-right"></i>
                    </a>
                    <ul class="treeview-menu">
                        <li>
                            <a href="UserManagement">
                                <i class="fa fa-circle-o"></i><%=Global.Menu_UserManagement%>
                            </a>
                        </li>
                        <li>
                            <a href="TeamManagement">
                                <i class="fa fa-circle-o"></i><%=Global.Menu_TeamManagement%>
                            </a>
                        </li>
                        <li class="active">
                            <a href="RoleManagement">
                                <i class="fa fa-circle-o"></i><%=Global.Menu_RoleManagement%>
                            </a>
                        </li>
                        <li>
                            <a href="LeaveTypeManagement">
                                <i class="fa fa-circle-o"></i><%=Global.Menu_LeaveTypeManagement%>
                            </a>
                        </li>
                        <li>
                            <a href="LocationResponsibleManagement">
                                <i class="fa fa-circle-o"></i><%=Global.Menu_LocationResponsibles %>
                            </a>
                        </li>
                    </ul>
                </li>
                <li>
                    <a href="ManagerialLeaveEntry">
                        <i class="fa fa-circle-o"></i><%=Global.Menu_ManagerialLeaveEntry%>
                    </a>
                </li>
                <li>
                    <a href="LeavePolicies">
                        <i class="fa fa-circle-o"></i><%=Global.Menu_LeavePolicies%>
                    </a>
                </li>
                <li>
                    <a href="Locations">
                        <i class="fa fa-circle-o"></i><%=Global.Locations%>
                    </a>
                </li>
                <li class="treeview">
                    <a href="#">
                        <i class="fa fa-circle-o"></i><%=Global.Menu_SystemRoles %>
                        <i class="fa fa-angle-left pull-right"></i>
                    </a>
                    <ul class="treeview-menu">
                        <li>
                            <a href="SysAdmins">
                                <i class="fa fa-circle-o"></i><%=Global.Menu_Admins %>
                            </a>
                        </li>
                        <li>
                            <a href="SysManagers">
                                <i class="fa fa-circle-o"></i><%=Global.Menu_Managers %>
                            </a>
                        </li>
                        <li>
                            <a href="SysSecondManagers">
                                <i class="fa fa-circle-o"></i><%=Global.Menu_SecondManagers %>
                            </a>
                        </li>
                        <li>
                            <a href="SysFinancialTeam">
                                <i class="fa fa-circle-o"></i><%=Global.Menu_FinancialTeam %>
                            </a>
                        </li>
                    </ul>
                </li>
            </ul>
        </li>

    <% }
    %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
      <%=Global.EditRole%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
      <%=Global.EditRole%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <div class="row">
        <%
            var id = "";
            try
            {
                id = Request.Params.Get("id");
                id = Regex.Match(id, @"\d+").Value;
            }
            catch {
                Response.Redirect("~/Dashboard");
            }

            var roleName = "";
            var isActive = "";

            var scon = new SqlConnection(new ApplicationClass().Constr);
            scon.Open();
            var scmd = new SqlCommand("select RoleName from Roles where RoleId= '" + id + "';", scon);
            var sreader = scmd.ExecuteReader();
            while (sreader.Read())
            {
                roleName = sreader["RoleName"].ToString();
            }

            sreader.Close();

            scmd = new SqlCommand("select 'Y' IsActive from Roles where RoleId = '" + id + "' and EnabledFlag=1 ", scon);
            sreader = scmd.ExecuteReader();
            while (sreader.Read())
            {
                isActive = sreader["IsActive"].ToString();
            }
            sreader.Close();


        %>  <div class="col-md-12">
           
        <div class="box box-<% if(isActive == "Y") { %>success<% } else { %>danger<% } %>">
            
            <div class="box-header">
                <div style="float: right; margin-right: 1%;">
                    <asp:Button ID="BackButton" class="btn btn-block btn-primary" runat="server" Text="<%$Resources:Global, BackButton%>" OnClick="BackButton_OnClick" />
                </div>
            </div>
   
            <div class="box-body">
                
              <!-- Role -->
              <div class="form-group">
                <label><%=Global.RoleText%>:</label>

                <div class="input-group date">
                  <div class="input-group-addon">
                    <i class="fa fa-fw fa-user-secret"></i>
                  </div>
                    
                  <input  type="text" class="form-control pull-right" id="roleName" name ="roleName" value="<%=roleName%>" <% if(isActive != "Y") { %> readonly="readonly" <% } %>/>
             
                </div>
              </div>

                  <div class="row">
                      <%
                          if (isActive == "Y")
                          {
                              %>
                          <div class="col-sm-6">
                              <asp:Button class=" btn btn-success fa fa-envelope  btn-block" ID="btnUpdate" runat="server" Text="<%$ Resources:Global, UserEdit_input %>" OnClick="btnUpdate_Click"  />
                          </div>
                          <div class="col-sm-6">
                              <asp:Button class=" btn-block btn btn-danger fa fa-envelope" ID="btnDelete" runat="server" Text="<%$ Resources:Global, Add_tbl_h5 %>" OnClick="btnDelete_Click" />
                          </div>
                              <%
                          }
                          else
                          {
                             %>
                          <div class="col-sm-12">
                              <asp:Button class=" btn btn-success fa fa-envelope  btn-block" ID="btnActive" runat="server" Text="<%$ Resources:Global, UserActivate %>" OnClick="btnActive_Click"  />
                          </div>
                             <%
                          }
                      %>
      
                  </div>
            </div>
          </div>
         </div>
     </div>
</asp:Content>