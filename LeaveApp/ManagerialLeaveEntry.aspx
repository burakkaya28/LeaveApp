﻿<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="ManagerialLeaveEntry.aspx.cs" Inherits="DemoWebApp.ManagerialLeaveEntry" EnableEventValidation="false" %>
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
                <li class="treeview">
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
                <li class="active">
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
    <%=Global.Menu_ManagerialLeaveEntry %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <%=Global.Menu_ManagerialLeaveEntry %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <div class="col-md-6">

            <div class="box box-primary">
                <center> <asp:Label ID="Label3" runat="server" Text="" ForeColor ="Red"></asp:Label></center>
                <div class="box-body">
                    
                    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
                    <form action="" method="post">
                    
                    <!-- Leave Date -->
                    <div class="form-group">
                        <label><%=Global.LeaveDate %>:</label>

                        <div class="input-group date">
                            <div class="input-group-addon">
                                <i class="fa fa-calendar"></i>
                            </div>
                            <input type="text" class="form-control" id="leaveDate" value="" name="leaveDate" />
                  
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
                    </form>
                </div>
                
                <!-- Add Button -->
                <div class="form-group has-feedback" style="margin-left:2%; margin-right:2%;">
                    <asp:Button ID="CreateButton" runat="server" Text="<%$Resources:Global,Create%>" Width="100%" class="btn btn-primary" OnClick="Button1_Click" />
                </div>
                <br />
            </div>
            <!-- /.box-body -->
        </div>
    </div>
    
</asp:Content>