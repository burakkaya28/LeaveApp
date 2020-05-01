using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using log4net;

namespace DemoWebApp.classes
{
    public class UserClass
    {
        public string Text;

        public string Email { get; internal set; }
        public string FullName { get; internal set; }
        public string StartDate { get; internal set; }
        public int UserId { get; internal set; }
        public int TeamId { get; internal set; }
        public int ManagerId { get; internal set; }
        public string SecondManagerId { get; internal set; }
        public int LocationId { get; internal set; }
        public string Role { get; internal set; }

        public string AdminCheck { get; internal set; }
        public string ManagerCheck { get; internal set; }
        public string SecondManagerCheck { get; internal set; }
        public string FinancialUserCheck { get; internal set; }

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool AddUser(UserClass s)
        {
            bool processResult;
            s.SecondManagerId = s.SecondManagerId ?? "null";
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("insert into Users (Email,FullName,StartDate,Role,TeamId,ManagerId,SecondManager, LocationId) values(@Email,@FullName,@StartDate,@Role,@TeamId,@ManagerId,"+s.SecondManagerId+", @LocationId)", conn);
            comm.Parameters.Add("@Email", SqlDbType.NVarChar).Value = s.Email;
            comm.Parameters.Add("@FullName", SqlDbType.NVarChar).Value = s.FullName;
            comm.Parameters.Add("@StartDate", SqlDbType.Date).Value = s.StartDate;
            comm.Parameters.Add("@Role", SqlDbType.NVarChar).Value = s.Role;
            comm.Parameters.Add("@ManagerId", SqlDbType.Int).Value = s.ManagerId;
            comm.Parameters.Add("@TeamId", SqlDbType.Int).Value = s.TeamId;
            comm.Parameters.Add("@LocationId", SqlDbType.Int).Value = s.LocationId;

            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                processResult = Convert.ToBoolean(comm.ExecuteNonQuery());

                //Get New User ID...
                var newUserId = GetNewUserId();

                new ApplicationClass().SetUserAdminStatus(newUserId, s.AdminCheck);
                new ApplicationClass().SetUserManagerStatus(newUserId, s.ManagerCheck);
                new ApplicationClass().SetUserSecondManagerStatus(newUserId, s.SecondManagerCheck);
                new ApplicationClass().SetUserFinancialUserStatus(newUserId, s.FinancialUserCheck);
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
                return false;
            }
            finally { conn.Close(); }
            return processResult;
        }
        
        public bool UserUpdate(UserClass u)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            if (conn.State == ConnectionState.Closed) conn.Open();

            u.SecondManagerId = u.SecondManagerId ?? "null";
            var comm = new SqlCommand
                (@"update Users 
                set FullName='" + u.FullName + "'," +
                    @"StartDate='" + u.StartDate + "', " +
                    @"Role ='" + u.Role + "'," +
                    @"Email='" + u.Email + "'," +
                    @"ManagerId ="+ u.ManagerId + ","+
                    @"TeamId ="+ u.TeamId + "," +
                    @"LocationId =" + u.LocationId + "," +
                    @"SecondManager=" + u.SecondManagerId + " " +
                @"where UserId="+ u.UserId, conn);

            try
            {
                comm.ExecuteNonQuery();
                conn.Close();

                new ApplicationClass().SetUserAdminStatus(u.UserId.ToString(), u.AdminCheck);
                new ApplicationClass().SetUserManagerStatus(u.UserId.ToString(), u.ManagerCheck);
                new ApplicationClass().SetUserSecondManagerStatus(u.UserId.ToString(), u.SecondManagerCheck);
                new ApplicationClass().SetUserFinancialUserStatus(u.UserId.ToString(), u.FinancialUserCheck);

                return true;
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public bool UserDisable(string userId)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("update Users set EnabledFlag=1 where UserId='"+ userId + "'", conn);

            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                comm.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public bool UserActivate(string userId)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("update Users set EnabledFlag=0 where UserId='"+ userId + "'", conn);

            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                comm.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
                return false;
            }
        }
        
        public bool FullNameControl(string userName)
        {
            var result = false;
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("Select FullName from Users where FullName =@FullName and EnabledFlag=0", conn);
            comm.Parameters.Add("@FullName", SqlDbType.VarChar).Value = userName;
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                var dr = comm.ExecuteReader();
                if (dr.HasRows)
                {
                    result = true;
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            finally { conn.Close(); }
            return result;
        }

        public bool UserMailControl(string email)
        {
            var result = false;
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("Select Email from Users where Email=@Email and EnabledFlag=0", conn);
            comm.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                var dr = comm.ExecuteReader();
                if (dr.HasRows)
                {
                    result = true;
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            finally { conn.Close(); }
            return result;
        }

        public bool UserExistControl(string userId)
        {
            var result = false;
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("Select UserId from Users where UserId=@UserId", conn);
            comm.Parameters.Add("@UserId", SqlDbType.VarChar).Value = userId;
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                var dr = comm.ExecuteReader();
                if (dr.HasRows)
                {
                    result = true;
                }
                dr.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            finally { conn.Close(); }
            return result;
        }

        public int AdminControl(string userId)
        {
            var existResult = 0;
            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                if (conn.State != ConnectionState.Open) conn.Open();
                var comm = new SqlCommand("select UserId from Admins where UserId = " + userId, conn);

                var dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    existResult = Convert.ToInt32(dr["UserId"]);
                }

                dr.Close();
                conn.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return existResult;
        }

        public int ManagerControl(string userId)
        {
            var existResult = 0;
            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                var comm = new SqlCommand("select UserId from Managers where UserId = " + userId, conn);
                if (conn.State == ConnectionState.Closed) conn.Open();

                var dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    existResult = Convert.ToInt32(dr["UserId"]);
                }
                dr.Close();
                conn.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return existResult;
        }

        public string GetUserLocation(string userId)
        {
            var userLocation = "";
            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                var comm = new SqlCommand("select l.LocationName from Users u join Locations l on l.LocationId = u.LocationId where u.UserId = " + userId, conn);
                if (conn.State == ConnectionState.Closed) conn.Open();

                var dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    userLocation = dr["LocationName"].ToString();
                }
                dr.Close();
                conn.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return userLocation;
        }

        public string GetUserMail(string userId)
        {
            var userMail = "";
            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                var comm = new SqlCommand("select Email from Users where UserId = " + userId, conn);
                if (conn.State == ConnectionState.Closed) conn.Open();

                var dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    userMail = dr["Email"].ToString();
                }
                dr.Close();
                conn.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return userMail;
        }

        public string GetNewUserId()
        {
            var userId = "";
            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                var comm = new SqlCommand("select TOP 1 * from Users order by UserId DESC;", conn);
                if (conn.State == ConnectionState.Closed) conn.Open();

                var dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    userId = dr["UserId"].ToString();
                }
                dr.Close();
                conn.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return userId;
        }

        public bool CheckUserSecondManagerExist(string userId)
        {
            var result = false;
            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                var comm = new SqlCommand("select SecondManager from Users where UserId = " + userId + " and SecondManager is not null", conn);
                if (conn.State == ConnectionState.Closed) conn.Open();

                var dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    result = true;
                }
                dr.Close();
                conn.Close();

            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return result;
        }

        public string ValidateUser(string email)
        {
            var userId = "";
            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                var comm = new SqlCommand("SELECT UserId FROM Users WHERE Email = '"+email+"' and EnabledFlag=0", conn);
                if (conn.State == ConnectionState.Closed) conn.Open();

                var dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    userId = dr["UserId"].ToString();
                }
                dr.Close();
                conn.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return userId;
        }

        public bool UpdateLastLoginDate(string userId)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("update Users set LastLoginDate=GETDATE() where UserId='" + userId + "'", conn);

            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                comm.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
                return false;
            }
        }

        public string GetUserIdByLeaveRequest(string leaveId)
        {
            var userId = "";
            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                var comm = new SqlCommand("select UserId from LeaveRequests where Id = " + leaveId, conn);
                if (conn.State == ConnectionState.Closed) conn.Open();

                var dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    userId = dr["UserId"].ToString();
                }
                dr.Close();
                conn.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return userId;
        }

        public int GetUserLeaveLeft(string userId)
        {
            var leaveLeft = "";
            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                var comm = new SqlCommand("select total_leave_left from OPT_LEAVE_SUMMARY where UserId = " + userId, conn);
                if (conn.State == ConnectionState.Closed) conn.Open();

                var dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    leaveLeft = dr["total_leave_left"].ToString();
                }
                dr.Close();
                conn.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return int.Parse(leaveLeft);
        }
    }
}