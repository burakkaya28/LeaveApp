<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="UserEdit.aspx.cs" Inherits="DemoWebApp.UserEdit" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
      <%=Global.UserEdit_msj%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
      <%=Global.UserEdit_msj%>
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

            var email = "";
            var fullName = "";
            var isActive = "";
            var startDate = "";

            //Authority
            var isAdmin = new ApplicationClass().IsAdmin(id);
            var isManager = new ApplicationClass().IsManager(id);
            var isSecondManager = new ApplicationClass().IsSecondManager(id);
            var isFinancialUser = new ApplicationClass().IsFinancialUser(id);

            var scon = new SqlConnection(new ApplicationClass().Constr);
            scon.Open();
            var scmd = new SqlCommand("select UserId,ManagerId,FullName,Email, StartDate from Users where UserId= '" + id + "';", scon);
            var sreader = scmd.ExecuteReader();
            while (sreader.Read())
            {
                fullName = sreader["FullName"].ToString();
                email = sreader["Email"].ToString();

                if (sreader["StartDate"] != null)
                {
                    if (sreader["StartDate"].ToString() != "")
                    {
                        startDate = Convert.ToDateTime(sreader["StartDate"]).ToString("dd-MM-yyyy");
                    }
                }
            }

            sreader.Close();

            scmd = new SqlCommand("select 'Y' IsActive from Users where UserId = '" + id + "' and EnabledFlag=0 ", scon);
            sreader = scmd.ExecuteReader();
            while (sreader.Read())
            {
                isActive = sreader["IsActive"].ToString();
            }
            sreader.Close();

            var adminCheck = isAdmin == "Y" ? "checked" : "";
            var managerCheck = isManager == "Y" ? "checked" : "";
            var secondManagerCheck = isSecondManager == "Y" ? "checked" : "";
            var financialCheck = isFinancialUser == "Y" ? "checked" : "";
            var authorityStatus = isActive == "Y" ? "" : "disabled";

        %>  
      
     <div class="col-md-12">
        <div class="box box-<% if(isActive == "Y") { %>success<% } else { %>danger<% } %>">
                
            <div class="box-header">
                <div style="float: right; margin-right: 1%;">
                    <asp:Button ID="BackButton" class="btn btn-block btn-primary" runat="server" Text="<%$Resources:Global, BackButton%>" OnClick="BackButton_OnClick" />
                </div>
            </div>

            <div class="box-body">
                
                <!-- Full Name -->
                <div class="form-group">
                    <label><%=Global.Add_input1%>:</label>

                    <div class="input-group date">
                        <div class="input-group-addon">
                        <i class="fa fa-fw fa-user"></i>
                        </div>
                    
                        <input type="text" class="form-control pull-right" id="createdby" name ="createdby" value="<%=fullName%>" <% if(isActive != "Y") { %> readonly="readonly" <% } %>/>
             
                    </div>
                </div>

                
                <!-- Email -->
                <div class="form-group">
                    <label><%=Global.Add_input3%>:</label>
                    <div class="input-group date">
                        <div class="input-group-addon">
                            <i class="fa fa-envelope"></i>
                        </div>
                        <input  type="text" class="form-control pull-right" id="email" name ="email" value="<%=email%>"  
                                <% if(isActive != "Y") { %> readonly="readonly" <% } %>/>
                
                    </div>
                </div>
                
                <!-- Work Start Date-->
                <div class="form-group">
                    <label><%=Global.Default_leave_table_WorkStartDate %>:</label>

                    <div class="input-group date">
                        <div class="input-group-addon">
                            <i class="fa fa-calendar"></i>
                        </div>
                        <input type="text" class="form-control pull-right" id="dateStartDate" value="<%=startDate%>" name="StartDate" 
                               <% if(isActive != "Y") { %> readonly="readonly" <% } %>/>
                    </div>
                </div>

                <!-- Role -->
                 <div class="form-group">
                    <label><%=Global.UserEdit_input2%>:</label>
                    <div class="input-group date">
                          <div class="input-group-addon">
                              <i class="fa fa-fw fa-eyedropper"></i>
                          </div>

                        <asp:DropDownList ID="ddlUnvan" class="form-control" runat="server" Width="100%" Height="33px">
                        </asp:DropDownList>
                    </div>
                 </div>
                
                <!-- Team -->
                <div class="form-group">
                    <label><%=Global.Team %>:</label>
                    <div class="input-group date">
                        <div class="input-group-addon">
                            <i class="fa fa-fw fa-users"></i>
                        </div>

                        <asp:DropDownList ID="TeamDDL" class="form-control" runat="server" Width="100%" Height="33px">
                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Manager -->
                 <div class="form-group">
                   <label><%=Global.UserEdit_input3%>:</label>
                       <div class="input-group date">
                      <div class="input-group-addon">
                       <i class="fa fa-fw fa-user"></i>
                       </div>

                    <asp:DropDownList ID="ddlEmployees" class="form-control" runat="server" Width="100%" Height="33px"
                        DataTextField="NAME" DataValueField="ID" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </div>
                     </div>

                <!-- Second Manager -->
                 <div class="form-group">
                   <label><%=Global.UserEdit_input4%>:</label>
                     <div class="input-group date">
                       <div class="input-group-addon">
                           <i class="fa fa-fw fa-user"></i>
                       </div>
                        <asp:DropDownList ID="secondManagerDDL" class="form-control" runat="server" Width="100%" Height="33px"
                            DataTextField="NAME" DataValueField="ID" AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </div>
                 </div>
                
                <!-- Location -->
                <div class="form-group">
                    <label><%=Global.Location%>:</label>
                    <div class="input-group date">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-map-marker"></i>
                        </div>
                        <asp:DropDownList ID="locationDDL" class="form-control" runat="server" Width="100%" Height="33px" DataTextField="NAME" DataValueField="ID" AppendDataBoundItems="true">
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
                                <label><input type="checkbox" name="IsAdmin" id="IsAdmin" <%=adminCheck %> <%=authorityStatus %>/><%=Global.AdminText%></label>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <!-- Manager -->
                            <div class="checkbox">
                                <label><input type="checkbox" name="IsManager" id="IsManager" <%=managerCheck %> <%=authorityStatus %>/><%=Global.ManagerText%></label>
                            </div>
                        </div>
                        
                        <div class="col-md-3">
                            <!-- Second Manager -->
                            <div class="checkbox">
                                <label><input type="checkbox" name="IsSecondManager" id="IsSecondManager" <%=secondManagerCheck %> <%=authorityStatus %>/><%=Global.SecondManagerText%></label>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <!-- Financial User-->
                            <div class="checkbox">
                                <label><input type="checkbox" name="IsFinancialUser" id="IsFinancialUser" <%=financialCheck %> <%=authorityStatus %>/><%=Global.FinancialUserText%></label>
                            </div>
                        </div>
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