namespace DemoWebApp.classes
{
    public class ReportSqlQueries
    {
        public string AllLeavesSql = "select r.Id, u.FullName, u.Role, t.TeamName Team, r.Description, r.StartDate, r.EndDate, r.Day Duration, lt.LeaveType, r.Status from LeaveRequests r join Users u ON u.UserId = r.UserId join LeaveTypes lt ON lt.LeaveTypeId = r.LeaveTypeId left join Teams t on t.TeamId = u.TeamId order by 1 desc";

        public string PendingAppSql =
            "select lr.ID, u.FullName, lr.UserId, u.Role, t.TeamName Team, lr.StartDate, lr.EndDate, lr.Day LeaveDuration, lt.LeaveType, lt.LeaveTypeTr, lr.Description, lr.Status, case lr.Status when 0 then 1 when 5 then 2 else 0 end AppLevel, case lr.Status when 0 then (select m.FullName from Users uu join Users m ON m.UserId = uu.ManagerId where uu.UserId = u.UserId) when 5 then (select m.FullName from Users uu join Users m ON m.UserId = uu.SecondManager where uu.UserId = u.UserId) else '' end WaitingApprover from Users u join LeaveRequests lr on u.UserId= lr.UserId join LeaveTypes lt on lt.LeaveTypeId = lr.LeaveTypeId left join Teams t on t.TeamId = u.TeamId where (lr.Status=0 or lr.Status=5)";

        public string ApprovedLeaveSql =
            "select r.Id Id, u.FullName, u.Role, t.TeamName Team, r.description, r.StartDate, r.EndDate, r.Day Duration, lt.LeaveType, r.Status from LeaveRequests r join Users u ON u.UserId = r.UserId join LeaveTypes lt ON lt.LeaveTypeId = r.LeaveTypeId left join Teams t on t.TeamId = u.TeamId where status=1 order by 1 desc";

        public string RejectedLeaveSql =
            "select r.Id Id, u.FullName, u.Role, t.TeamName Team, r.description, r.StartDate, r.EndDate, r.Day Duration, lt.LeaveType, r.Status from LeaveRequests r join Users u ON u.UserId = r.UserId join LeaveTypes lt ON lt.LeaveTypeId = r.LeaveTypeId left join Teams t on t.TeamId = u.TeamId where status=2 order by 1 desc";
    }
}