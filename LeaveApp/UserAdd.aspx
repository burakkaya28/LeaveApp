<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="UserAdd.aspx.cs" Inherits="DemoWebApp.Add" EnableEventValidation="false" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="DemoWebApp.classes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
   <ul class="sidebar-menu">
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

       <li class="treeview ">
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
                        <li class="active">
                            <a href="UserManagement">
                                <i class="fa fa-circle-o"></i><%=Global.Menu_UserManagement%>
                            </a>
                        </li>
                        <li>
                            <a href="TeamManagement">
                                <i class="fa fa-circle-o"></i><%=Global.Menu_TeamManagement%>
                            </a>
                        </li>
                        <li>
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
  </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <%=Global.Menu_staff %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <%=Global.Menu_staff %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <div class="col-md-6">

            <div class="box box-primary">

                <div class="box-body">
                    
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <form action="" method="post">
                    
                    <!-- Full Name -->
                    <div class="form-group">
                        <label><%=Global.Add_input1 %>:</label>

                        <div class="input-group date">
                            <div class="input-group-addon">
                                <i class="fa fa-fw fa-user"></i>
                            </div>
                            <input type="text" class="form-control" name="FullName"/>
                  
                        </div>
                    </div>
                    
                    <!-- Email -->
                    <div class="form-group">
                        <label><%=Global.Add_input3 %>:</label>

                        <div class="input-group date">
                            <div class="input-group-addon">
                                <i class="fa fa-fw fa-envelope"></i>
                            </div>
                            <input type="text" class="form-control" name="email"/>
                        </div>
                    </div>
                
                    <!-- Work Start Date-->
                    <div class="form-group">
                        <label><%=Global.Default_leave_table_WorkStartDate %>:</label>

                        <div class="input-group date">
                            <div class="input-group-addon">
                                <i class="fa fa-calendar"></i>
                            </div>
                            <input type="text" class="form-control pull-right" id="dateStartDate" value="<%=DateTime.Now.ToString("dd/MM/yyyy")%>" name="StartDate" />
                        </div>
                    </div>
                    
                  <!-- Role -->
                  <div class="form-group">
                      <label><%=Global.RoleText %>:</label>
                       <div class="input-group date">
                           <div class="input-group-addon">
                                <i class="fa fa-fw fa-user-secret"></i>
                           </div>

                            <asp:DropDownList ID="ddlUnvan" class="form-control" runat="server" Width="100%" Height="33px" DataSourceID="RoleDataSource" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true">
                                <asp:ListItem Text="<%$Resources:Global,Add_input4%>" Value="" />

                            </asp:DropDownList>

                            <asp:SqlDataSource ID="RoleDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:constr %>"
                            SelectCommand="select RoleId ID, RoleName Name from Roles where EnabledFlag=1 order by 2">
                            </asp:SqlDataSource>
                        </div>
                    </div>
                        
                    <!-- Team -->
                    <div class="form-group">
                        <label><%=Global.Team %>:</label>
                        <div class="input-group date">
                            <div class="input-group-addon">
                                <i class="fa fa-fw fa-users"></i>
                            </div>

                            <asp:DropDownList ID="TeamDDL" class="form-control" runat="server" Width="100%" Height="33px" DataSourceID="TeamDataSource" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true">
                                <asp:ListItem Text="<%$Resources:Global, SelectTeam%>" Value="" />

                            </asp:DropDownList>

                            <asp:SqlDataSource ID="TeamDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:constr %>"
                                                SelectCommand="select TeamId ID, TeamName Name from Teams where EnabledFlag=1 order by 2">
                            </asp:SqlDataSource>
                        </div>
                    </div>

                    <!-- Manager -->
                    <div class="form-group">
                        <label><%=Global.ManagerText %>:</label>
                         <div class="input-group date">
                               <div class="input-group-addon">
                                    <i class="fa fa-fw fa-user"></i>
                               </div>
                            <asp:DropDownList ID="ddlEmployees" class="form-control" runat="server" DataSourceID="ManagersDataSource" Width="100%" Height="33px"
                                DataTextField="NAME" DataValueField="ID" AppendDataBoundItems="true">
                                <asp:ListItem Text="<%$Resources:Global,Add_input5%>" Value="" />
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="ManagersDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:constr %>"
                                SelectCommand="select u.UserId ID ,u.FullName  NAME  from  Users u inner join Managers on u.UserId=Managers.UserId order by 2">
                            </asp:SqlDataSource>
                        </div>
                    </div>

                      <!-- Second Manager -->  
                     <div class="form-group">
                         <label><%=Global.SecondManagerText %>:</label>
                          <div class="input-group date">
                                   <div class="input-group-addon">
                                        <i class="fa fa-fw fa-user"></i>
                                   </div>
                                <asp:DropDownList ID="secondManager" class="form-control" runat="server" DataSourceID="SecondManagerDataSource" Width="100%" Height="33px"
                                    DataTextField="NAME" DataValueField="ID" AppendDataBoundItems="true">
                                    <asp:ListItem Text="<%$Resources:Global,Add_input6%>" Value="" />
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SecondManagerDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:constr %>"
                                    SelectCommand="select u.UserId ID ,u.FullName  NAME  from  Users u inner join SecondManagers on u.UserId=SecondManagers.UserId order by 2">
                                </asp:SqlDataSource>
                        </div>
                      </div>
                        
                        <!-- Location -->  
                        <div class="form-group">
                            <label><%=Global.Location %>:</label>
                            <div class="input-group date">
                                <div class="input-group-addon">
                                    <i class="glyphicon glyphicon-map-marker"></i>
                                </div>
                                <asp:DropDownList ID="LocationDDL" class="form-control" runat="server" Width="100%" Height="33px" DataTextField="NAME">
                                </asp:DropDownList>
                            </div>
                        </div>
                        
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <h3 class="panel-title"><%=Global.Authorization %></h3>
                            </div>
                            <div class="panel-body">
                                <div class="col-md-3">
                                    <!-- Admin -->
                                    <div class="checkbox">
                                        <label><input type="checkbox" name="IsAdmin" id="IsAdmin"/><%=Global.AdminText%></label>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <!-- Manager -->
                                    <div class="checkbox">
                                        <label><input type="checkbox" name="IsManager" id="IsManager"/><%=Global.ManagerText%></label>
                                    </div>
                                </div>
                        
                                <div class="col-md-3">
                                    <!-- Second Manager -->
                                    <div class="checkbox">
                                        <label><input type="checkbox" name="IsSecondManager" id="IsSecondManager"/><%=Global.SecondManagerText%></label>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <!-- Financial User-->
                                    <div class="checkbox">
                                        <label><input type="checkbox" name="IsFinancialUser" id="IsFinancialUser"/><%=Global.FinancialUserText%></label>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </form>
                </div>
                
                <!-- Add Buton -->
                <div class="form-group has-feedback" style="margin-left:2%; margin-right:2%;">
                    <asp:Button ID="Button1" runat="server" Text="<%$Resources:Global,Add_btn%>" Width="100%" class="btn btn-primary" OnClick="Button1_Click" />
                </div>
                <!-- Back Buton -->
                <div class="form-group has-feedback" style="margin-left:2%; margin-right:2%;">
                    <asp:Button ID="BackButton" runat="server" Text="<%$Resources:Global,BackButton%>" Width="100%" class="btn btn-primary" OnClick="BackButton_OnClick" />
                </div>
                <br />
            </div>
        </div>
    </div>
    
</asp:Content>