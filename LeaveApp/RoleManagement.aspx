﻿<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="RoleManagement.aspx.cs" Inherits="DemoWebApp.RoleManagement" EnableEventValidation="false" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="System.Data.SqlClient" %>
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
       </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <%=Global.Menu_RoleManagement %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <%=Global.Menu_RoleManagement %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="box top80" runat="server">
        <div class="box-body chart-responsive">
             <div class="box">
                <div class="box-header">
                    
                    <div style="float: right;" class="exportPC">
                        <asp:Button class="btn btn-block btn-primary" ID="Export" runat="server" Text="<%$ Resources:Global, ExportToExcelText %>" OnClick="Export_Click" />
                    </div>
                    
                    <div style="float: right; margin-right: 1%;">
                        <asp:Button ID="AddRole" class="btn btn-block btn-primary" runat="server" Text="<%$Resources:Global,AddRole%>" OnClick="AddRole_Click" />
                    </div>

                </div>

                <!-- /.box-header -->
                <div class="box-body" id="tablebody" runat="server">

                    <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">

                        <div class="row">
                            <div class="col-sm-12">
                                <div class="box-body table-responsive no-padding">
                                <table id="example1" class="table table-bordered table-striped dataTable" role="grid" aria-describedby="example1_info">
                                    <thead>
                                        <tr role="row">
                                            <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1"><%=Global.RoleText%></th>
                                            <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1"><%=Global.StatusText %></th>
                                        </tr>
                                    </thead>
                                
                                    <tbody>
                                        <%
                                            try
                                            {
                                                var scon = new SqlConnection(new ApplicationClass().Constr);
                                                scon.Open();

                                                var scmd = new SqlCommand("select RoleId, RoleName, EnabledFlag from Roles", scon);
                                                var sreader = scmd.ExecuteReader();
                                                while (sreader.Read())
                                                {
                                                    var roleId = sreader["RoleId"].ToString();
                                                    var roleName = sreader["RoleName"].ToString();
                                                    var enabledFlag = sreader["EnabledFlag"].ToString();
                                            %>
                                            <tr>
                                                <td><a href="EditRole?id=<%=roleId%>"><%=roleName%></a></td>
                                                <td><% 
                                                     if (enabledFlag == "0")
                                                     {
                                                        %><span class="label label-danger"><%=Global.Add_tbl_h5%></span><%
                                                     }
                                                     else 
                                                     {
                                                        %><span class="label label-success"><%=Global.Add_tbl_h4%></span>
                                                   <%
                                                     }
                                                   %>
                                                </td>
                                            </tr>
                                            <%
                                                }
                                                sreader.Close();
                                                scmd.Dispose();
                                                scon.Close();

                                            }
                                            catch (Exception ex)
                                            {
                                                Response.Write(ex);
                                                throw;
                                            }
                                        %>
                                    </tbody>
                                </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </div>
</asp:Content>