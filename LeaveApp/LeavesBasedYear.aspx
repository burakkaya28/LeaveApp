<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="LeavesBasedYear.aspx.cs" Inherits="DemoWebApp.ReportTopRequest" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DemoWebApp.classes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
    <li>
        <a href="Dashboard">
             <i class="fa fa-dashboard"></i><span><%=Global.Menu_dashboard%></span>
        </a>
    </li>

    <li>
        <a href="LeaveRequest">
            <i class="fa fa-calendar-minus-o"></i><span><%=Global.Menu_request%></span>
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

                <li>
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
    <%=Global.Report_title%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <%=Global.Report_title%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="box">
        <div class="box-header">
            <div style="float: right;" class="exportPC">
                <asp:Button class="btn btn-block btn-primary" ID="Export" runat="server" Text="<%$ Resources:Global, ExportToExcelText %>" OnClick="Export_Click" />
            </div>
            <br />

            <div style="float: left;">
                <b><%=Global.Year %>:</b>
                <asp:DropDownList ID="YearDDL" runat="server" OnSelectedItemChanged="fncsSelect()" AutoPostBack="True">
                    
                </asp:DropDownList>
            </div>
        </div>

        <div class="box-body" id="tablebody" runat="server">

            <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">

                <div class="row">
                    <div class="col-sm-12">

                        <div class="box-body table-responsive no-padding">
                        <table id="example1" class="table table-bordered table-striped dataTable" role="grid" aria-describedby="example1_info">
                            <thead>
                                <tr role="row">
                                    <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1">
                                        <%=Global.Default_approvals_table_th2%>
                                    </th>
                                <th class="sorting_desc" tabindex="0" aria-controls="example2" rowspan="1" colspan="1">
                                    <%=Global.Request_input2%>
                                </th>
                                <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" >
                                    <%=Global.RoleText%>
                                </th>
                                <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" >
                                    <%=Global.Team %>
                                </th>
                                <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" >
                                    <%=Global.LeaveType%>
                                </th>   
                                <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" >
                                    <%=Global.Report_tbl_th1%>
                                </th>
                                <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" >
                                    <%=Global.Request_tbl_td3%>
                                </th>
                                <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" >
                                    <%=Global.Request_tbl_td1%>
                                </th>
                                <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" >
                                    <%=Global.Request_tbl_td2%>
                                </th>
                                <th class="sorting" tabindex="0" aria-controls="example2" rowspan="1" colspan="1" >
                                    <%=Global.Request_tbl_td4%>
                                </th>
                                </tr>
                            </thead>
                            <tbody>
                             <script type="text/javascript">
                                 
                                 function fncsSelect() {
                                     if (<%= YearDDL.SelectedItem.Value!=""%>) {
                             <%
                                 try
                                 {
                                     var scon = new SqlConnection(new ApplicationClass().Constr);
                                     scon.Open();
                                     var scmd = new SqlCommand("select l.UserId, l.FullName, l.Manager, l.Role, l.Team, l.LeaveTypeId, l.LeaveType, l.LeaveYear, ( select count(*) from LeaveRequests where UserId = l.UserId and DATEPART(yyyy, StartDate) = l.LeaveYear and LeaveTypeId = l.LeaveTypeId ) totalLeaves, ( select count(*) from LeaveRequests where UserId = l.UserId and DATEPART(yyyy, StartDate) = l.LeaveYear and LeaveTypeId = l.LeaveTypeId and status IN (0, 5) ) Pending, ( select count(*) from LeaveRequests where UserId = l.UserId and DATEPART(yyyy, StartDate) = l.LeaveYear and LeaveTypeId = l.LeaveTypeId and status = 1 ) Approved, ( select count(*) from LeaveRequests where UserId = l.UserId and DATEPART(yyyy, StartDate) = l.LeaveYear and LeaveTypeId = l.LeaveTypeId and status = 2 ) Rejected, ( select count(*) from LeaveRequests where UserId = l.UserId and DATEPART(yyyy, StartDate) = l.LeaveYear and LeaveTypeId = l.LeaveTypeId and status = 3 ) Cancelled from ( select DISTINCT u.UserId, u.FullName, m.FullName Manager, u.Role, t.TeamName Team, lt.LeaveTypeId, lt.LeaveType, DATEPART(yyyy, lr.StartDate) LeaveYear from Users u join LeaveRequests lr on lr.UserId = u.UserId join LeaveTypes lt on lt.LeaveTypeId = lr.LeaveTypeId join Users m on m.UserId = u.ManagerId and DATEPART(yyyy, lr.StartDate) = '"+YearDDL.SelectedItem.Value+"' left join Teams t on t.TeamId = u.TeamId ) l where l.FullName NOT IN ('Hakan Turgut', 'Levent Özalp') order by l.FullName", scon);
                                     var sreader = scmd.ExecuteReader();

                                     while (sreader.Read())
                                     {
                                         var userId = sreader["FullName"].ToString();
                                         var managerName = sreader["Manager"].ToString();
                                         var role = sreader["Role"].ToString();
                                         var team = sreader["Team"].ToString();
                                         var leaveType = sreader["LeaveType"].ToString();
                                         var totalLeave = sreader["totalLeaves"].ToString();
                                         var pendingLeave = sreader["Pending"].ToString();
                                         var approvedLeave = sreader["Approved"].ToString();
                                         var rejectedLeave = sreader["Rejected"].ToString();
                                         var cancelledLeave = sreader["Cancelled"].ToString();
                                     
                    %>    </script>
                                    <tr>
                                        <td><%=userId%></td>
                                        <td><%=managerName%> </td>
                                        <td><%=role%></td>
                                        <td><%=team %></td>
                                        <td><%=leaveType %></td>
                                        <td><%=totalLeave%></td>
                                        <td><%=pendingLeave%></td>
                                        <td><%=approvedLeave%> </td>
                                        <td><%=rejectedLeave%> </td>
                                        <td><%=cancelledLeave%></td>

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
</asp:Content>