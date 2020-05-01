<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DemoWebApp.Default" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DemoWebApp.classes" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
    <li class="active">
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
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
   <%=Global.Menu_dashboard %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
  <%=Global.Menu_dashboard %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%
        string rId;
        DateTime startDate;
        DateTime endDate;
        string status;
        string stdate;
        string edate;
        string leaveDuration;
        string leaveType;

        var username = new DemoWebApp.Login().User.Identity.Name;

        var appClass = new ApplicationClass();
        var isManager = appClass.IsManager(username);
    %>

     <div class="row">
          <div class="box box-warning box-solid">
            <div class="box-header with-border">
                <h3 class="box-title"><%=Global.Default_leave_table_th6 %></h3>

                <div class="box-tools pull-right">
                     <button type="button" class="btn btn-box-tool" data-widget="collapse" style="color: black;">
                        <i class="fa fa-minus"></i>
                    </button>
                </div>
            </div>
           <div class="box-body chart-responsive">

                <div class="col-xs-12">
                    <div class="box-body table-responsive no-padding">
                         <table class="table table-bordered table-striped dataTable" role="grid" aria-describedby="example1_info">
                                    <thead>
                                        <tr role="row">
                                            <th tabindex="0" aria-controls="example2" rowspan="1" colspan="1">
                                                <%=Global.Add_input1%>
                                            </th>
                                            <th tabindex="0" aria-controls="example2" rowspan="1" colspan="1">
                                                <%=Global.Team %>
                                            </th>
                                            <th tabindex="0" aria-controls="example2" rowspan="1" colspan="1">
                                                <%=Global.UserEdit_input3%>
                                            </th>
                                            <th tabindex="0" aria-controls="example2" rowspan="1" colspan="1">
                                                <%=Global.Default_leave_table_WorkStartDate%>
                                            </th>
                                            <th tabindex="0" aria-controls="example2" rowspan="1" colspan="1">
                                                <%=Global.Default_leave_table_th2%>
                                            </th>
                                            <th tabindex="0" aria-controls="example2" rowspan="1" colspan="1">
                                                <%=Global.Default_leave_table_th3%>
                                            </th>
                                            <th tabindex="0" aria-controls="example2" rowspan="1" colspan="1">
                                                <%=Global.Default_leave_table_th4%>
                                            </th>
                                            <th tabindex="0" aria-controls="example2" rowspan="1" colspan="1">
                                                <%=Global.Default_leave_table_th5%>
                                            </th>

                                        </tr>
                                    </thead>
                                
                                    <tbody>
                                        <%
                                            SqlConnection scon;
                                            SqlDataReader sreader;
                                            SqlCommand scmd;

                                            using (scon = new SqlConnection(appClass.Constr))
                                            {
                                                try
                                                {
                                                    scon.Open();
                                                    scmd = new SqlCommand(@"select * from OPT_LEAVE_SUMMARY where UserId =                          '"+username+"'", scon)
                                                    { CommandTimeout = 5};

                                                    sreader = scmd.ExecuteReader();

                                                    while (sreader.Read())
                                                    {
                                                        
                                                        var workStartDate = "";
                                                        var userFullName = sreader["FullName"].ToString();
                                                        var team = sreader["Team"].ToString();
                                                        var managerName = sreader["ManagerName"].ToString();

                                                        if (sreader["StartDate"] != null && sreader["StartDate"].ToString() != "")
                                                        {
                                                            workStartDate = Convert.ToDateTime(sreader["StartDate"]).ToString("dd-MM-yyyy");
                                                        }

                                                        var workYear = sreader["work_year"].ToString();
                                                        var totalLeaveRight = sreader["total_leave_right"].ToString();
                                                        var totalUsedLeave = sreader["total_used_leave"].ToString();
                                                        var totalLeaveLeft = sreader["total_leave_left"].ToString();
                                                            %>

                                                             <tr>
                                                                <td><%=userFullName%></td>
                                                                <td><%=team %></td>
                                                                <td><%=managerName %></td>
                                                                <td><%=workStartDate %></td>
                                                                <td><%=workYear%> </td>
                                                                <td><%=totalLeaveRight%></td>
                                                                <td><%=totalUsedLeave%></td>
                                                                <td><%=totalLeaveLeft%></td>
                                                            </tr>

                                                         <%
                                                    }
                                                    sreader.Close();
                                                    scmd.Dispose();
                                                 }

                                                catch (Exception)
                                                {
                                                    // ignored
                                                }
                                            }
                                             scon.Close();
                                        %>
                                       
                                    </tbody>
                                </table>
                        </div>
                </div>
            </div>
        </div>
    </div>


     <% if (isManager == "Y")
        { %>

    <div class="row">
        <div class="box box-warning box-solid">
            <div class="box-header with-border">
                <h3 class="box-title"><%=Global.Menu_approvals %></h3>

                <div class="box-tools pull-right">
                    <button type="button" class="btn btn-box-tool" data-widget="collapse" style="color: black;">
                        <i class="fa fa-minus"></i>
                    </button>
                </div>
            </div>
            <div class="box-body" style="display: block;">

                <div class="col-xs-12">
                    <div class="box">

                        <div class="box-body table-responsive no-padding">
                            <table class="table table-hover">
                                <tbody>
                                    <tr style="text-align: center;">
                                        <th><%=Global.Default_approvals_table_th1 %></th>
                                        <th><%=Global.Default_approvals_table_th2 %></th>
                                        <th><%=Global.Default_approvals_table_th3 %></th>
                                        <th><%=Global.Default_approvals_table_th4 %></th>
                                        <th><%=Global.Request_lbl1 %></th>
                                        <th><%=Global.Request_input5 %></th>
                                        <th><%=Global.Default_approvals_table_th7 %></th>
                                        <th><%=Global.Default_approvals_table_th5 %></th>
                                    </tr>

                                    <%
                                        var pendApprovalhasData = false;
                                        using (scon = new SqlConnection(appClass.Constr))
                                        {
                                            scon.Open();
                                            scmd = new SqlCommand("select r.Id ID, u.FullName, r.StartDate, r.EndDate, r.Day, lt.LeaveType, lt.LeaveTypeTr, case r.Status when 0 then '1' when 5 then '2' else '' end ApprovalLevel, r.Status from LeaveRequests r join Users u on u.UserId = r.UserId join LeaveTypes lt on lt.LeaveTypeId = r.LeaveTypeId where 1=1 and (u.SecondManager = '"+username+"' or r.ManagerId = '"+username+"') and (r.Status=5 or r.Status = 0)", scon);
                                            sreader = scmd.ExecuteReader();
                                            while (sreader.Read())
                                            {
                                                pendApprovalhasData = true;

                                                rId = sreader["ID"].ToString();
                                                startDate = Convert.ToDateTime(sreader["StartDate"]);
                                                endDate = Convert.ToDateTime(sreader["EndDate"]);
                                                var userId = sreader["FullName"].ToString();
                                                status = sreader["Status"].ToString();
                                                stdate =startDate.ToString("dd-MM-yyyy");
                                                edate = endDate.ToString("dd-MM-yyyy");
                                                leaveDuration = sreader["day"].ToString();
                                                leaveType = DemoWebApp.Login.Language == "tr" ? sreader["LeaveTypeTr"].ToString() : sreader["LeaveType"].ToString();
                                                var approvalLevel = sreader["ApprovalLevel"].ToString();
                                    %>
                                    <tr>
                                        <td><a href="LeaveDetails?id=<%=rId%>"><%=rId%></a></td>
                                        <td><%=userId%></td>
                                        <td><%=stdate%> </td>
                                        <td><%=edate%></td>
                                        <td><%=leaveDuration%></td>
                                        <td><%=leaveType%> </td>
                                        <td><%=approvalLevel%></td>
                                        <td><% 
                                        if (status == "0" || status == "5")
                                        {
                                            %><span class="label label-warning"><%=Global.Request_tbl_td3%></span><%
                                        }
                                        else if (status == "1")
                                        {
                                            %><span class="label label-success"><%=Global.Request_tbl_td1%></span><%
                                        }
                                        else if (status == "2")
                                        {
                                            %><span class="label label-danger"><%=Global.Request_tbl_td2%></span><%
                                        }
                                        else if (status == "3")
                                        {
                                            %><span class="label label-danger"><%=Global.Request_tbl_td4%></span><%
                                        }

                                            %>
                                        </td>
                                    </tr>
                                    <%
                                            }

                                            if (pendApprovalhasData == false)
                                            {
                                                %>
                                                    <tr style="text-align: center;">
                                                       <td colspan="8"><%=Global.PendingApprovalHasData%></td> 
                                                    </tr>
                                                <%
                                            }

                                        }

                                        sreader.Close();
                                        scmd.Dispose();
                                        scon.Close();
                                    %>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%  } %>

    <div class="row">
        <div class="box box-warning box-solid">
            <div class="box-header with-border">
                <h3 class="box-title"><%=Global.Default_request_table_title %></h3>

                <div class="box-tools pull-right">
                    <button type="button" class="btn btn-box-tool" data-widget="collapse" style="color: black;">
                        <i class="fa fa-minus"></i>
                    </button>
                </div>
            </div>
            <div class="box-body" style="display: block;">

                <div class="col-xs-12">
                    <div class="box">

                        <div class="box-body table-responsive no-padding">
                            <table class="table table-hover">
                                <tbody>
                                    <tr style="text-align: center;">
                                      <th><%=Global.Default_approvals_table_th1 %></th>
                                      <th><%=Global.Default_approvals_table_th3 %></th>
                                      <th><%=Global.Default_approvals_table_th4 %></th>
                                      <th><%=Global.Request_lbl1 %></th>
                                      <th><%=Global.Request_input5 %></th>
                                      <th><%=Global.Default_approvals_table_th5 %></th>
                                    </tr>

                                    <%
                                        var myLeavehasData = false;
                                        using (scon = new SqlConnection(appClass.Constr))
                                        {
                                            scon.Open();
                                            scmd = new SqlCommand("select r.Id ID, r.StartDate, r.EndDate, r.Day Duration, lt.LeaveType, lt.LeaveTypeTr, r.Status from LeaveRequests r join Users u on u.UserId = r.UserId join LeaveTypes lt on lt.LeaveTypeId = r.LeaveTypeId where u.UserId = '"+username+"' order by r.Id desc", scon);
                                            sreader = scmd.ExecuteReader();
                                            while (sreader.Read())
                                            {
                                                myLeavehasData = true;

                                                rId = sreader["ID"].ToString();
                                                startDate = Convert.ToDateTime(sreader["StartDate"]);
                                                endDate = Convert.ToDateTime(sreader["EndDate"]);
                                                status = sreader["Status"].ToString();
                                                stdate = startDate.ToString("dd-MM-yyyy");
                                                edate = endDate.ToString("dd-MM-yyyy");
                                                leaveDuration = sreader["Duration"].ToString();
                                                leaveType = DemoWebApp.Login.Language == "tr" ? sreader["LeaveTypeTr"].ToString() : sreader["LeaveType"].ToString();
                                                
                                    %>
                                    <tr>
                                        <td><a href="LeaveDetails?id=<%=rId%>"><%=rId%></a></td>
                                        <td><%=stdate%> </td>
                                        <td><%=edate%></td>
                                        <td><%=leaveDuration%></td>
                                        <td><%=leaveType%> </td>
                                        <td><% if (status == "0" || status == "5")
                                         {
                                            %><span class="label label-warning"><%=Global.Request_tbl_td3%></span><%
                                         }
                                          else if (status == "1")
                                          {
                                            %><span class="label label-success"><%=Global.Request_tbl_td1%></span><%
                                          }
                                          else if (status == "2")
                                          {
                                            %><span class="label label-danger"><%=Global.Request_tbl_td2%></span><%
                                          }
                                          else if (status == "3")
                                          {
                                            %><span class="label label-danger"><%=Global.Request_tbl_td4%></span><%
                                          }

                                        %></td>
                                    </tr>
                                    <%
                                            }

                                            if (myLeavehasData == false)
                                            {
                                                %>
                                                    <tr style="text-align: center;">
                                                       <td colspan="6"><%=Global.myLeaveHasData%></td> 
                                                    </tr>
                                                <%
                                            }
                                }
                                sreader.Close();
                                scmd.Dispose();
                                scon.Close();
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