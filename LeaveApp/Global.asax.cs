using System;
using System.Web;
using System.Web.Routing;
using log4net.Config;

namespace DemoWebApp
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            XmlConfigurator.Configure();
            Routing(RouteTable.Routes);
        }

        private static void Routing(RouteCollection route)
        {
            //route.MapPageRoute("", "", "~/test.aspx");
            
            //DASHBOARD & LEAVES
            route.MapPageRoute("Default", "Dashboard", "~/Default.aspx");
            route.MapPageRoute("LeaveDetails", "LeaveDetails", "~/LeaveDetails.aspx");
            route.MapPageRoute("LeaveRequest", "LeaveRequest", "~/LeaveRequest.aspx");

            //REPORTS
            route.MapPageRoute("AllLeaves", "AllLeaves", "~/AllLeaves.aspx");
            route.MapPageRoute("ApprovedLeaves", "ApprovedLeaves", "~/ApprovedLeaves.aspx");
            route.MapPageRoute("LeavesBasedYear", "LeavesBasedYear", "~/LeavesBasedYear.aspx");
            route.MapPageRoute("LeavesBetweenDays", "LeavesBetweenDays", "~/LeavesBetweenDays.aspx");
            route.MapPageRoute("LeaveSummary", "LeaveSummary", "~/LeaveSummary.aspx");
            route.MapPageRoute("PendingApprovals", "PendingApprovals", "~/PendingApprovals.aspx");
            route.MapPageRoute("RejectedLeaves", "RejectedLeaves", "~/RejectedLeaves.aspx");

            //MANAGEMENT
            //MANAGEMENT - Definitions
            route.MapPageRoute("UserAdd", "AddUser", "~/UserAdd.aspx");
            route.MapPageRoute("UserEdit", "EditUser", "~/UserEdit.aspx");
            route.MapPageRoute("UserManagement", "UserManagement", "~/UserManagement.aspx");
            route.MapPageRoute("TeamAdd", "AddTeam", "~/TeamAdd.aspx");
            route.MapPageRoute("TeamEdit", "EditTeam", "~/TeamEdit.aspx");
            route.MapPageRoute("TeamManagement", "TeamManagement", "~/TeamManagement.aspx");
            route.MapPageRoute("RoleAdd", "AddRole", "~/RoleAdd.aspx");
            route.MapPageRoute("RoleEdit", "EditRole", "~/RoleEdit.aspx");
            route.MapPageRoute("RoleManagement", "RoleManagement", "~/RoleManagement.aspx");
            route.MapPageRoute("LeaveTypeAdd", "AddLeaveType", "~/LeaveTypeAdd.aspx");
            route.MapPageRoute("LeaveTypeEdit", "EditLeaveType", "~/LeaveTypeEdit.aspx");
            route.MapPageRoute("LeaveTypeManagement", "LeaveTypeManagement", "~/LeaveTypeManagement.aspx");
            route.MapPageRoute("ManagerialLeaveEntry", "ManagerialLeaveEntry", "~/ManagerialLeaveEntry.aspx");
            route.MapPageRoute("Locations", "Locations", "~/Locations.aspx");
            route.MapPageRoute("LeavePolicies", "LeavePolicies", "~/LeavePolicies.aspx");
            route.MapPageRoute("LocationResponsibleManagement", "LocationResponsibleManagement", "~/LocationResponsibleManagement.aspx");

            //MANAGEMENT - System Roles
            route.MapPageRoute("SysAdmins", "SysAdmins", "~/SysAdmins.aspx");
            route.MapPageRoute("SysManagers", "SysManagers", "~/SysManagers.aspx");
            route.MapPageRoute("SysSecondManagers", "SysSecondManagers", "~/SysSecondManagers.aspx");
            route.MapPageRoute("SysFinancialTeam", "SysFinancialTeam", "~/SysFinancialTeam.aspx");
        }
    }
}