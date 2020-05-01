<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="LeaveRequest.aspx.cs" Inherits="DemoWebApp.RequestDayOff" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DemoWebApp.classes" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
<script src="plugins/jQuery/jquery-1.9.1.min.js"></script>

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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
    <%=Global.Request_form_title %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <%=Global.Request_form_title %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">

        <%
            var userId = new DemoWebApp.Login().User.Identity.Name;
            var fullName = "";
            var managername = "";
            var secondmanager = "";
            var scon = new SqlConnection(new ApplicationClass().Constr);

            scon.Open();
            var scmd = new SqlCommand("select u.FullName CreatedBy, u2.FullName Manager, u3.FullName SecondManager, l.LocationId From Users u join Users u2 on u2.UserId = u.ManagerId left join Users u3 on u3.UserId=u.SecondManager join Locations l on l.LocationId = u.LocationId where u.UserId = '" + userId + "'", scon);
            var sreader = scmd.ExecuteReader();

            while (sreader.Read())
            {
                fullName = sreader["CreatedBy"].ToString();
                managername = sreader["Manager"].ToString();
                secondmanager = sreader["SecondManager"].ToString();
            }

        %>
        <div class="col-md-6">

            <div class="box box-primary">
                <center> <asp:Label ID="Label1" runat="server" Text="" ForeColor ="Red"></asp:Label></center>
                <div class="box-body">
                    
                    <input type="hidden" id="AllowLeaveDurationForBefore" name="AllowLeaveDurationForBefore" value="<%=new LeavePolicyClass().GetPolicyValueByKey("ALLOWED_LEAVE_DURATION_FOR_BEFORE") %>" />

                    <!-- Created By -->
                    <div class="form-group">
                        <label><%= Global.Request_input1 %>:</label>

                        <div class="input-group date">
                            <div class="input-group-addon">
                                <i class="fa fa-fw fa-user"></i>
                            </div>
                            <input type="text" class="form-control pull-right" id="createdby" name="createdby" value="<%= fullName %>" readonly="readonly" />
                        </div>
                    </div>
                    
                    <!-- Manager -->
                    <div class="form-group">
                        <label><%= Global.Request_input2 %>:</label>

                        <div class="input-group date">
                            <div class="input-group-addon">
                                <i class="fa fa-fw fa-user"></i>
                            </div>
                            <input type="text" class="form-control pull-right" id="manager" name="manager" value="<%= managername %>" disabled />
                        </div>
                    </div>
                    
                    <%
                        if (new UserClass().CheckUserSecondManagerExist(userId))
                        {
                    %>
                        <!-- Second Manager -->
                        <div class="form-group">
                            <label><%= Global.UserEdit_input4 %>:</label>

                            <div class="input-group date">
                                <div class="input-group-addon">
                                    <i class="fa fa-fw fa-user"></i>
                                </div>
                                <input type="text" class="form-control pull-right" id="secondmanager" name="secondmanager" value="<%= secondmanager %>" disabled />
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
                            <asp:DropDownList ID="LeaveType" class="form-control" AppendDataBoundItems="true" runat="server" Width="100%" Height="33px"> 
                            </asp:DropDownList>
                         </div>
                    </div>
                    
                    <!-- Start Date -->
                    <div class="form-group">
                        <label id="start_lbl"><%=Global.Request_input3 %>:</label>

                        <div class="input-group date">
                            <div class="input-group-addon">
                                <i class="fa fa-calendar"></i>
                            </div>
                            <input type="text" class="form-control pull-right" id="datepicker" value="<%=DateTime.Now.ToString("dd/MM/yyyy")%>" name="stdate" />
                        </div>
                    </div>
                    
                    <!-- End Date -->
                    <div class="form-group" id="end_date">
                        <label><%=Global.Request_input4 %>:</label>

                        <div class="input-group date">
                            <div class="input-group-addon">
                                <i class="fa fa-calendar"></i>
                            </div>
                            <input type="text" class="form-control pull-right" id="datepicker2" value="<%=DateTime.Now.ToString("dd/MM/yyyy")%>" name="enddate" />
                        </div>
                    </div>
                    
                    <!-- Half Day -->
                    <div class="form-group" id="half_form">
                        <label title="<%=Global.Request_chkbox1%>">
                            <input type="checkbox" class="minimal" name="halfday" id="halfday" title="<%=Global.Request_chkbox1%>"/>
                            <%=Global.Request_href3%>
                        </label>
            
                    </div>
                    
                    <!-- Add Half Day -->
                    <div class="form-group" id="halfadd_form">
                        <label title="<%=Global.Request_chkbox%>">
                            <input type="checkbox" class="minimal" name="halfday_add" id="halfday_add" title="<%=Global.Request_chkbox%>" />
                             <%=Global.Request_href1%>
                        </label>
            
                     </div>

                        <script>
                            $(document).ready(function () {

                                $("#leaveDuration").text(1);
                                var difference = 1;
                                var language = "<%= RequestDay %>";
                                var contract = "<%= ContractFlag %>";

                                $('#datepicker2').on('changeDate', function () {

                                    var isChecked = $('#halfday:checkbox:checked').length > 0;
                                    var isCheckedHalf = $('#halfday_add:checkbox:checked').length > 0;

                                    if (isChecked === false) {

                                        var start = $('#datepicker').datepicker('getDate');
                                        var end = $('#datepicker2').datepicker('getDate');
                                        var diffDay = calcBusinessDays(start, end) + 1;
                                        var diffrence = diffDay;

                                        if (isCheckedHalf === true) {
                                            diffrence = diffDay + 0.5;
                                        }

                                        $("#leaveDuration").text(diffrence);

                                    }
                             
                                });

                            
                                $("#halfday_add").change(function () {

                                    var isChecked = $('#halfday:checkbox:checked').length > 0;

                                    if (isChecked === false) {
                                        var start = $('#datepicker').datepicker('getDate');
                                        var end = $('#datepicker2').datepicker('getDate');
                                        var diffDay = calcBusinessDays(start, end) + 1;

                                        if (this.checked) {
                                            difference = diffDay + 0.5;
                                        }

                                        else {
                                            difference = diffDay;

                                            if (difference === 0)
                                                difference = 1;
                                        }
                                    }

                                    $("#leaveDuration").text(difference);
                                });

                                $("#halfday").change(function () {
                                    if (this.checked) {
                                        difference = 0.5;

                                        if (language === "tr")
                                            $("#start_lbl").text("İzin Tarihi:");
                                        else
                                            $("#start_lbl").text("Leave Date:");

                                        $('#halfadd_form').css('visibility', 'hidden');
                                        $('#end_date').css('visibility', 'hidden');
                                        $('#half_form').css('margin-top', "-15%");
                                        $('#report_add').css('margin-top', "-8.5%");

                                        if (contract === true) {
                                            $('#report_accordion').css('margin-top', "-8.5%");
                                            $('#report_add').css('margin-top', "0px");
                                        }
                                        else {
                                            $('#report_accordion').css('margin-top', "-8.5%");
                                            $('#report_add').css('margin-top', "0px");
                                        }

                                        var startDate= $("#datepicker").datepicker('getDate');
                                        $('#datepicker2').datepicker("setDate", startDate);
                                    } 

                                    else
                                    {
                                        $('#halfadd_form').css('visibility', 'visible');
                                        $('#end_date').css('visibility', 'visible');
                                        $('#half_form').css('margin-top', "0px");
                                        $('#report_add').css('margin-top', "0px");

                                        if (language === "tr")
                                            $("#start_lbl").text("Başlangıç Tarihi:");
                                        else
                                            $("#start_lbl").text("Start Date:");

                                        if (contract === true) {
                                            $('#report_accordion').css('margin-top', "0px");
                                            $('#report_add').css('margin-top', "0px");
                                        }
                                        else {
                                            $('#report_accordion').css('margin-top', "0px");
                                            $('#report_add').css('margin-top', "0px");
                                        }

                                        var start = $('#datepicker').datepicker('getDate');
                                        var end = $('#datepicker2').datepicker('getDate');
                                        var date = (end - start) / 1000 / 60 / 60 / 24;
                                        difference = date + 1;

                                        var isChecked = $('#halfday_add:checkbox:checked').length > 0;

                                        if (isChecked === true)
                                            difference = difference + 0.5;

                                    }
                                    $("#leaveDuration").text(difference);
                                });
                            });
                        </script>
                    
                        <script>
                            function calcBusinessDays(start, end) {
                                // This makes no effort to account for holidays
                                // Counts end day, does not count start day

                                // make copies we can normalize without changing passed in objects    
                                start = new Date(start);
                                end = new Date(end);

                                // initial total
                                var totalBusinessDays = 0;

                                // normalize both start and end to beginning of the day
                                start.setHours(0,0,0,0);
                                end.setHours(0,0,0,0);

                                var current = new Date(start);
                                current.setDate(current.getDate() + 1);
                                var day;
                                // loop through each day, checking
                                while (current <= end) {
                                    day = current.getDay();
                                    if (day >= 1 && day <= 5) {
                                        ++totalBusinessDays;
                                    }
                                    current.setDate(current.getDate() + 1);
                                }
                                return totalBusinessDays;
                            }
                        </script>

                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     
                    <!-- Contract -->
                    <div class="form-group" id="report_accordion">
                        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true" runat="server" visible="false">


                            <div class="panel panel-default">
                                <div class="panel-heading" role="tab" id="headingThree">
                                    <h4 class="panel-title">
                                        <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="false" aria-controls="collapseThree"><%=Global.Request_msj2%>
                                        </a>
                                    </h4>
                                </div>
                                <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                                    <div class="panel-body">
                                        <div class="bs-example bs-example-standalone" data-example-id="dismissible-alert-js">

                                            <div class="alert alert-danger alert-dismissible fade in" role="alert">

                                                <h4><%=Global.Request_msj%></h4>
                                                <%=Global.Request_msj1%>

                                                <div class="row">
                                                    <div class="col-lg-6">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">
                                                                <input type="checkbox" runat="server" id="chcSozlesme"/>
                                                                <label><%=Global.Request_msj3%> </label>
                                                            </span>

                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <!-- Attachment -->
                 <asp:UpdatePanel ID="UpdatePanel2" runat="server">

                    <ContentTemplate>

                        <div class="form-group" style="margin-left:2%">
                            <div class="panel-group" id="Div3" role="tablist" aria-multiselectable="true" runat="server">
                                <label><%=Global.Request_file%></label>
                                <asp:FileUpload ID="fuResim" runat="server" Width="162px" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <!-- Description -->
                <div class="form-group" style=" margin-left:2%; margin-right:2%; ">
                  <label><%=Global.Request_lbl %>:</label>
                    <textarea class="form-control" style="max-width: 100%; min-width: 100%;" name="description" rows="3" data-parsley-required="true"></textarea>
                </div>
                
                <!-- Leave Duration -->
                <div class="form-group" style=" margin-left:2%; margin-right:2%; ">  
                    <label><%=Global.Request_lbl1 %>:</label>
                    <label id="leaveDuration" style="font-weight:normal;font-size:small">0</label>
                    <label style="font-weight:normal;font-size:small"><%=Global.Request_lbl2 %></label>
                </div>
                
                <!-- Send Button -->
                <div  style=" margin-left:2%; margin-right:2%; ">
                    <asp:Button class="btn btn-block btn-primary btn-flat" ID="Button1" OnClick="Button1_Click" runat="server"  Command="RequestDayOff" ValidationGroup="RequestDayOff" Text="<%$ Resources:Global, Request_btn %>" />
                </div>
                <br />

            </div>
        </div>
    </div>

</asp:Content>