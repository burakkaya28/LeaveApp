<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="LeavesBetweenDays.aspx.cs" Inherits="DemoWebApp.TwoDatesBetweenPermission" %>
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
    <%=Global.TwoDates_title%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <%=Global.TwoDates_title%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />

    <div class="row">
        <div class="col-md-6">
            <b><%=Global.Request_input3%>:</b>
            <div class="input-group date">
                <div class="input-group-addon">
                    <i class="fa fa-calendar"></i>
                </div>
                <input type="text" class="form-control pull-right" id="datepicker3" name="stdate" value="<%=Request.Form["stdate"] %>"/>
            </div>
        </div>
        <div class="col-md-6">
            <b><%=Global.Request_input4%>:</b>
            <div class="input-group date">
                <div class="input-group-addon">
                    <i class="fa fa-calendar"></i>
                </div>
                <input type="text" class="form-control pull-right" id="datepicker4" name="enddate" value="<%=Request.Form["enddate"] %>"/>
            </div>
        </div>
    </div>

    <br />
    <div class="form-group has-feedback">
        <asp:Button ID="Button1" runat="server" Text="<%$ Resources:Global, TwoDates_btn %>" Width="100%" class="btn btn-primary" />
    </div>

    <div class="box">
        
        <%
            if ((Request.Form["stdate"] != null && Request.Form["enddate"] != null) && (Request.Form["stdate"] != "" && Request.Form["enddate"] != ""))
            {
                %>
                    <div class="box-header">
                        <div style="float: right;" class="exportPC">
                            <asp:Button class="btn btn-block btn-primary" ID="Export" runat="server" Text="<%$ Resources:Global, ExportToExcelText %>" OnClick="Export_Click" />
                        </div>
                    </div>
                <%
            }
        %>

        <div class="box-body" id="tablebody" runat="server">
            <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">

                <div class="row">

                    <div class="col-sm-12">
                        
                        <div class="box">
                            <div class="box-body table-responsive no-padding">
                                <table class="table table-bordered table-striped dataTable" role="grid">
                                    <tbody>
                                        <tr style="text-align: center;">
                                            <th><%=Global.Default_approvals_table_th1 %></th>
                                            <th><%=Global.Default_approvals_table_th2 %></th>
                                            <th><%=Global.RoleText %></th>
                                            <th><%=Global.Team %></th>
                                            <th><%=Global.Default_approvals_table_th3 %></th>
                                            <th><%=Global.Default_approvals_table_th4 %></th>
                                            <th><%=Global.Request_lbl1 %></th>
                                            <th><%=Global.Request_input5 %></th>
                                            <th><%=Global.Default_approvals_table_th5 %></th>
                                        </tr>
                                        <%
                                            try
                                            {
                                                var startDate = Request.Form["stdate"];
                                                var endDate = Request.Form["enddate"];

                                                if ((startDate != null && endDate != null) && (startDate != "" && endDate != ""))
                                                {
                                                    startDate = Convert.ToDateTime(startDate).ToString("yyyy-MM-dd");
                                                    endDate = Convert.ToDateTime(endDate).ToString("yyyy-MM-dd");
                                                    var constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                                                    SqlConnection scon;
                                                    using (scon = new SqlConnection(constr))
                                                    {
                                                        scon.Open();
                                                        var scmd = new SqlCommand("select l.Id LeaveId, u.FullName, u.Role, t.TeamName, l.StartDate, l.EndDate, l.Day LeaveDuration, lt.LeaveType, l.Status from LeaveRequests l join Users u ON u.UserId = l.UserId join LeaveTypes lt on lt.LeaveTypeId = l.LeaveTypeId left join Teams t on t.TeamId = u.TeamId where (l.StartDate >= '"+startDate+"' and l.EndDate <= '"+endDate+"')", scon);
                                                        var sreader = scmd.ExecuteReader();

                                                        while (sreader.Read())
                                                        {
                                            var leaveId = sreader["LeaveId"].ToString();
                                            var fullName = sreader["FullName"].ToString();
                                            var roleName = sreader["Role"].ToString();;
                                            var teamName = sreader["TeamName"].ToString();;
                                            var _startDate = Convert.ToDateTime(sreader["StartDate"]).ToString("dd-MM-yyyy");
                                            var _endDate = Convert.ToDateTime(sreader["EndDate"]).ToString("dd-MM-yyyy");
                                            var leaveDuration = sreader["LeaveDuration"].ToString();
                                            var leaveType = sreader["LeaveType"].ToString();
                                            var status = sreader["Status"].ToString();

                                                            %>
                                                        <tr>
                                                            <td><a href="LeaveDetails?id=<%=leaveId %>"><%=leaveId %></a></td>
                                                            <td><%=fullName %></td>
                                                            <td><%=roleName %></td>
                                                            <td><%=teamName %></td>
                                                            <td><%=_startDate %></td>
                                                            <td><%=_endDate %></td>
                                                            <td><%=leaveDuration %></td>
                                                            <td><%=leaveType %></td>
                                                            <td>
                                                        <% 
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
                                                            }                                                                                        %>
                                                            </td>
                                                        </tr>
                                                            <%
                                                        }
                                                        scmd.Dispose();
                                                        sreader.Close();
                                                        scon.Close();
                                                    }
                                                }
                                                else
                                                {
                                                    %>
                                                        <tr style="text-align: center">
                                                            <td colspan="9"><%=Global.BetweenTwoDatesMsg %></td>
                                                        </tr>
                                                    <%
                                                }
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
</asp:Content>