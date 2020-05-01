<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="PendingApprovals.aspx.cs" Inherits="DemoWebApp.ConfirmationRequestPermission" %>
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

        if (isManager == "Y" || isAdmin == "Y")
        {
        %>
        <li class="treeview active">
            <a href="#">
                <i class="fa fa-pie-chart"></i>
                <span><%=Global.Menu_reports%></span>
                <i class="fa fa-angle-left pull-right"></i>
            </a>
            <ul class="treeview-menu">
                <li>
                    <a href="AllLeaves">
                        <i class="fa fa-circle-o"></i><span><%=Global.Menu_all%></span>
                    </a>
                </li>

                <li class="active">
                    <a href="PendingApprovals">
                        <i class="fa fa-circle-o"></i><span><%=Global.Menu_approvals%></span>
                    </a>
                </li>
                <li><a href="LeaveSummary"><i class="fa fa-circle-o"></i><%=Global.Default_leave_table_th7%></a></li>
            </ul>
        </li>
        
        <% } %>
    
        <% if (isAdmin == "Y")
           { %>

        <li class="treeview">
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
    <%=Global.Menu_approvals %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <%=Global.Menu_approvals %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="box">
        <div class="box-header">
            <div style="float: right;" class="exportPC">
                 <asp:Button class="btn btn-block btn-primary" ID="Export" runat="server" Text="<%$ Resources:Global, ExportToExcelText %>" OnClick="Export_Click" />
            </div>
            <div style="float: right;" class="exportMobile">
                <asp:Button class="btn btn-block btn-primary" ID="ExportMobile" runat="server" Text="<%$ Resources:Global, ExportToExcelText %>" OnClick="ExportMobile_OnClick" />
            </div>
        </div>

        <div class="box-body chart-responsive">

        <div class="col-xs-12">
        <div class="box-body table-responsive no-padding" id="tablebody" runat="server">
          <table id="example1" class="table table-bordered table-striped dataTable" role="grid" aria-describedby="example1_info">
                 <thead>
                    <tr>
                        <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1"><%=Global.Default_approvals_table_th1%></th>
                        <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1"><%=Global.Default_approvals_table_th2%></th>
                        <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" >
                            <%=Global.RoleText%>
                        </th>
                        <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" >
                            <%=Global.Team%>
                        </th>
                        <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1"><%=Global.Default_approvals_table_th6%></th>
                        <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1"><%=Global.Default_approvals_table_th7%></th>
                        <th class="sorting_desc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="descending"><%=Global.Default_approvals_table_th3%></th>
                        <th class="sorting_desc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="descending"><%=Global.Default_approvals_table_th4%></th>
                        <th class="sorting_desc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="descending"><%=Global.LeaveDuration%></th>
                        <th class="sorting_desc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" aria-sort="descending"><%=Global.LeaveType%></th>
                        <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1"><%=Global.Confirmation_tbl_th1%></th>
                        <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" ><%=Global.Default_approvals_table_th5%></th>
                    </tr>
                </thead>

                <tbody>
                    <%
                        try
                        {
                            var scon = new SqlConnection(new ApplicationClass().Constr);
                            scon.Open();
                            var scmd = new SqlCommand(new ReportSqlQueries().PendingAppSql, scon);
                            var sreader = scmd.ExecuteReader();
                            while (sreader.Read())
                            {
                                var rId = sreader["ID"].ToString();
                                var startDate = Convert.ToDateTime(sreader["StartDate"]);
                                var endDate = Convert.ToDateTime(sreader["EndDate"]);
                                var fullName = sreader["FullName"].ToString();
                                var role = sreader["Role"].ToString();
                                var team = sreader["Team"].ToString();
                                var description = sreader["Description"].ToString();
                                var stdate = startDate.ToShortDateString();
                                var edate = endDate.ToShortDateString();
                                var leaveDuration = sreader["LeaveDuration"].ToString();
                                var leaveType = DemoWebApp.Login.Language == "tr" ? sreader["LeaveTypeTr"].ToString() : sreader["LeaveType"].ToString();
                                var approverLevel = sreader["AppLevel"].ToString();
                                var approver = sreader["WaitingApprover"].ToString();

                        %>
                        <tr>
                            <td><a href="LeaveDetails?id=<%=rId%>"><%=rId%></a></td>
                            <td><%=fullName%></td>
                            <td><%=role %></td>
                            <td><%=team %></td>
                            <td><%=approver%> </td>
                            <td><%=approverLevel%> </td>
                            <td><%=stdate%> </td>
                            <td><%=edate%></td>
                            <td><%=leaveDuration %></td>
                            <td><%=leaveType %></td>
                            <td><%=description%></td>
                            <td><%  %><span class="label label-warning"><%=Global.Request_tbl_td3%></span><%
                            %></td>
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

</asp:Content>