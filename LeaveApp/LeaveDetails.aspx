<%@ page title="" language="C#" masterpagefile="~/App.Master" autoeventwireup="true" codebehind="LeaveDetails.aspx.cs" inherits="DemoWebApp.Details" enableeventvalidation="true" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DemoWebApp.classes" %>


<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder4" runat="server">
        <li>
        <a href="Dashboard">
            <i class="fa fa-dashboard"></i><span><%=Global.Menu_dashboard %></span>
        </a>
    </li>

    <li class="active">
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
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder3" runat="server">
    <% var number = "";
       try
       {
           number = Request.Params.Get("id");
           number = Regex.Match(number, @"\d+").Value;
       }
       catch {
           Response.Redirect("~/Dashboard");
       }%>
    <%=Global.Details_title %> (#<%=number%>)
</asp:content>
<asp:content id="Content3" contentplaceholderid="ContentPlaceHolder2" runat="server">
    <%=Global.Details_title %>
</asp:content>
<asp:content id="Content4" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div class="row">
        <%
            string id = "";
            try
            {
                id = Request.Params.Get("id");
                id = Regex.Match(id, @"\d+").Value;
            }
            catch {
                Response.Redirect("~/Dashboard");
            }
            var constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            var leaveClass = new LeaveClass();

            var userId = 0;
            var status = "";
            var description = "";
            var leaveTypes = "";

            var stdate = "";
            var edate = "";
            var managerName = "";
            var secondManager = "";
            var managerId = 0;
            var secondManagerId = "";
            var fullName = "";
            var day = "";
            var attahmentName = "";

            var scon = new SqlConnection(constr);
            scon.Open();
            var scmd = new SqlCommand("select r.Id ID, r.StartDate, r.EndDate, r.Day, u.FullName, u.UserId, r.Status, r.Description, u2.FullName ManagerName, u3.FullName SecondManager, u2.UserId ManagerId, u3.UserId SecondManagerId, lt.LeaveType, lt.LeaveTypeTr, r.ReportImage AttachmentName from LeaveRequests r join LeaveTypes lt on lt.LeaveTypeId=r.LeaveTypeId join Users u on u.UserId = r.UserId join Users u2 on u2.UserId = r.ManagerId left join Users u3 on u3.UserId=u.SecondManager where r.Id = '" + id + "'", scon);
            var sreader = scmd.ExecuteReader();
            while (sreader.Read())
            {
                userId = Convert.ToInt32(sreader["UserId"].ToString());
                var startDate = Convert.ToDateTime(sreader["StartDate"]);
                var endDate = Convert.ToDateTime(sreader["EndDate"]);
                day= sreader["Day"].ToString();
                fullName = sreader["FullName"].ToString();
                managerName = sreader["ManagerName"].ToString();
                secondManager = sreader["SecondManager"].ToString();
                managerId = Convert.ToInt32(sreader["ManagerId"].ToString());
                secondManagerId = sreader["SecondManagerId"].ToString();
                status = sreader["Status"].ToString();
                description = sreader["Description"].ToString();
                stdate = startDate.ToString("dd MMMM yyyy");
                edate = endDate.ToString("dd MMMM yyyy");
                attahmentName = sreader["AttachmentName"].ToString();
               
                leaveTypes = LeaveType=="tr" ? sreader["LeaveTypeTr"].ToString() : sreader["LeaveType"].ToString();
                
            }
            sreader.Close();

             %> <div class="col-md-12">
           
<div class="box box-primary">
        <center> <asp:Label ID="Label1" runat="server" Text="" ForeColor ="Red"></asp:Label></center> 
            <div class="box-body">
                 <div class="form-group" style ="margin-bottom: 0;">
                <label><%=Global.Default_approvals_table_th5%>:</label>

                     <% if (status == "0" || status == "5" )
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
                
              </div>
                <hr style ="margin-top:3px;margin-bottom:10px;"/>
                
               <!-- Created By -->
               <div class="form-group">
                    <label><%=Global.Request_input1%>:</label>

                    <div class="input-group date">
                        <div class="input-group-addon">
                        <i class="fa fa-fw fa-user"></i>
                        </div>
                        <input type="text" class="form-control pull-right" id="createdby" name ="createdby" value="<%=fullName%>" disabled/>
                
                    </div>
                </div>

               <!-- Manager -->
               <div class="form-group">
                  <label><%=Global.Request_input2%>:</label>

                    <div class="input-group date">
                      <div class="input-group-addon" style="<%=leaveClass.GetManagerColor(status)%>">
                        <i class="fa fa-fw fa-user"></i>
                      </div>
                        <input type="text" class="form-control pull-right" id="manager" name ="manager" value="<%=managerName %>" disabled/>
                
                    </div>
                </div>
               
                <%
                    if (new UserClass().CheckUserSecondManagerExist(userId.ToString()))
                    {
                        %>

                    <!-- Second Manager -->
                    <div class="form-group">
                        <label><%=Global.UserEdit_input4 %>:</label>

                        <div class="input-group date">
                            <div class="input-group-addon" style="<%=leaveClass.GetSecondManagerColor(status)%>">
                                <i class="fa fa-fw fa-user"></i>
                            </div>
                            <input type="text" class="form-control pull-right" id="manager" name="manager" value="<%=secondManager%>" disabled/>
                        </div>
                    </div>

                        <%
                    }
                %>
                
                
                <!-- Leave Type -->
                <div class="form-group">
                    <label><%= Global.Request_input5 %>:</label>
                    <div class="input-group date">
                        <div class="input-group-addon">
                            <i class="fa fa-fw fa-clock-o"></i>
                        </div>
                        <input type="text" class="form-control pull-right" id="leavetype" name="leavetype" value="<%= leaveTypes %>" disabled/>
                    </div>
                </div>


                <!-- Start Date -->
                <div class="form-group">
                    <label><%=Global.Default_approvals_table_th3%>:</label>

                    <div class="input-group date">
                        <div class="input-group-addon">
                        <i class="fa fa-calendar"></i>
                        </div>
                        <input type="text" class="form-control pull-right" id="datepicker" name ="stdate" value="<%=stdate%>" disabled/>
                 
                    </div>
                
                </div>
                
                <!-- End Date -->
                <div class="form-group">
                    <label><%=Global.Default_approvals_table_th4%>:</label>

                    <div class="input-group date">
                      <div class="input-group-addon">
                        <i class="fa fa-calendar"></i>
                      </div>
                        <input type="text" class="form-control pull-right" id="datepicker2" name ="enddate" value="<%=edate%>" disabled/>
                
                    </div>
                </div>
                
                <!-- Attachment -->
                <div class="form-group">
                    <label><%=Global.Attachment%>:</label>

                    <div class="input-group date">
                        <div class="input-group-addon">
                            <i class="glyphicon glyphicon-file"></i>
                        </div>
                        <div class="form-control pull-right" style="background: #eee;">
                            <a href="<%=HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/") + "Attachments/" + id + "/" + attahmentName %>" target="_blank"><%=attahmentName %></a>
                        </div>
                
                    </div>
                </div>

            
                <!-- Leave Duration -->
                <div class="form-group" >  
                    <label><%=Global.Request_lbl1 %>:</label>
                    <label id="leaveDuration" style="font-weight:normal;font-size:small"><%=day%></label>
                    <label style="font-weight:normal;font-size:small"><%=Global.Request_lbl2 %></label>
                </div>
                
                <!-- Description -->
                <div class="form-group">
                  <label><%=Global.Request_lbl%>:</label>
                  <textarea class="form-control" style="max-width: 100%; min-width: 100%;" name="description" rows="3" disabled><%=description%></textarea>
                </div>

              
                <center>
                    <div>
                       
                    <%  if (status == "0" || status == "5" )
                        {
                            var auth = 0;
                            var l1 = new DemoWebApp.Login();
                            try
                            {
                                auth = Convert.ToInt32(l1.User.Identity.Name);
                            }
                            catch
                            {
                                Response.Redirect("~/Dashboard");
                            }

                                  if ((auth == managerId && status=="0") || (auth.ToString() == secondManagerId && status=="5") )

                                  {
                    %>   
                      <div style="float:left; margin-right:3px;width:49%;"> <asp:Button ID ="Approve" runat="server" Text="<%$ Resources:Global, Details_input1%>" class="btn btn-block btn-success btn-sm" OnClick="Approve_Click"></asp:Button>
                          
                      </div>
              <div style="float:left;margin-right:3px;width:49%;"> <asp:Button ID ="Reject" runat="server" Text="<%$ Resources:Global, Details_input2%>" class="btn btn-block btn-danger btn-sm" OnClick="Reject_Click"></asp:Button> </div>
                   
                         <% 
                              }

                              if (auth == userId)
                              { %>
            
             
              <div style="float:left;margin-top:5px;margin-right:3px;width:98.4%;">  <asp:Button  ID ="Cancel" runat="server" Text="<%$ Resources:Global, Details_input3%>" class="btn btn-block btn-danger btn-sm" OnClick="Cancel_Click"></asp:Button></div>
                  <% }
                 %>
                </div>
                    </center>
            </div>
          </div>
         </div>
        <%
            }
             %>
         </div>
</asp:content>