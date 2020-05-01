using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Web;
using log4net;

namespace DemoWebApp.classes
{
    public class LeaveClass
    {
        public readonly string AttachmentPath = HttpContext.Current.Request.PhysicalApplicationPath + @"\Attachments";
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string StatusYellow = "#f7f700";
        private const string StatusGreen = "#49d549";

        public int ContractControl(string userId)
        {
            var result = 0;
            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                var comm = new SqlCommand("select ContractFlag from Users where UserId=@UserId ", conn);
                comm.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                if (conn.State == ConnectionState.Closed) conn.Open();

                result = Convert.ToInt32(comm.ExecuteScalar());

                conn.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return result;
        }

        public bool ContractFlag(int t)
        {
            var result = false;

            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                var comm = new SqlCommand("SELECT ContractFlag FROM Users where UserId= @UserId and EnabledFlag=0   ", conn);
                comm.Parameters.Add("@UserId", SqlDbType.Int).Value = t;
                if (conn.State == ConnectionState.Closed) conn.Open();

                var dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    if (dr["ContractFlag"] != DBNull.Value)
                    {
                        result = Convert.ToBoolean(dr["ContractFlag"]);
                    }
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

        public int LeaveRequestControl(string fullname, string id)
        {
            var result = 0;
            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                var comm = new SqlCommand("select UserId from LeaveRequests where UserId =@username and Id=@ID ", conn);
                comm.Parameters.Add("@username", SqlDbType.VarChar).Value = fullname;
                comm.Parameters.Add("@ID", SqlDbType.VarChar).Value = id;
                if (conn.State == ConnectionState.Closed) conn.Open();

                var dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    if (dr["UserId"] != DBNull.Value)
                    {
                        result = Convert.ToInt32(dr["UserId"]);
                    }
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

        public string LeaveUpdate(string requestId, string state)
        {
            try
            {
                using (var con = new SqlConnection(new ApplicationClass().Constr))
                {
                    using (var cmd = new SqlCommand("UPDATE LeaveRequests SET Status = '" + state + "' where Id = '" + requestId + "'"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return "0";
        }

        public bool ContractUpdate(string userid, bool undertak)
        {
            var updateResult = false;
            try
            {
                using (var con = new SqlConnection(new ApplicationClass().Constr))
                {
                    using (var cmd = new SqlCommand("UPDATE Users SET ContractFlag =  '" + undertak + "' where UserId = '" + userid + "'"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        updateResult = Convert.ToBoolean(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return updateResult;
        }

        public int GetLeaveManagerId(string leaveRequestId)
        {
            var managerId = 0;
            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                var comm = new SqlCommand("select d.userId ID from LeaveRequests d where d.Id='" + leaveRequestId + "'", conn);
                comm.Parameters.Add("@Id", SqlDbType.Int).Value = leaveRequestId;
                if (conn.State == ConnectionState.Closed) conn.Open();

                var dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    if (dr["ID"] != DBNull.Value)
                    {
                        managerId = Convert.ToInt32(dr["ID"].ToString());
                    }
                }

                dr.Close();
                conn.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return managerId;
        }

        public string GetLeaveManagerFullName(string userId)
        {
            var managerFullName = "";
            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                var comm = new SqlCommand("select u.FullName name  from Users u where u.UserId='" + userId + "'", conn);
                if (conn.State == ConnectionState.Closed) conn.Open();

                var dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    if (dr["name"] != DBNull.Value)
                    {
                        managerFullName = dr["name"].ToString();
                    }
                }

                dr.Close();
                conn.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return managerFullName;
        }

        public string GetLeaveStatus(string leaveId)
        {
            var status = "";
            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                var comm = new SqlCommand("select l.status from LeaveRequests l where l.Id ='" + leaveId + "'", conn);
                if (conn.State == ConnectionState.Closed) conn.Open();

                var dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    if (dr["status"] != DBNull.Value)
                    {
                        status = dr["status"].ToString();
                    }
                }

                dr.Close();
                conn.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return status;
        }

        public string GetManagerColor(string leaveStatus)
        {
            var color = "background: ";
            switch (leaveStatus)
            {
                case "1":
                case "5":
                    color += StatusGreen;
                    break;
                case "0":
                    color += StatusYellow;
                    break;
            }
            return color;
        }

        public string GetSecondManagerColor(string leaveStatus)
        {
            var color = "background: ";
            switch (leaveStatus)
            {
                case "1":
                    color += StatusGreen;
                    break;
                case "5":
                    color += StatusYellow;
                    break;
            }
            return color;
        }

        public bool CreateManagerialLeave(string userId, string leaveDate)
        {
            var createResult = false;
            try
            {
                using (var con = new SqlConnection(new ApplicationClass().Constr))
                {
                    
                    using (var cmd = new SqlCommand("insert into LeaveRequests (StartDate, EndDate, UserId, ManagerId, Status, Description, CreatedOn, Day, LeaveLanguage, LeaveTypeId) values ('" + leaveDate + "', '" + leaveDate + "', '" + userId + "', (select ManagerId from Users where UserId = '" + userId + "'), 1, 'Managerial Leave - İdari İzin', GETDATE(), 1, 'tr', (select LeaveTypeId from LeaveTypes where LeaveType = 'Managerial Leave'))"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        createResult = Convert.ToBoolean(cmd.ExecuteNonQuery());
                        con.Close();
                    }

                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return createResult;
        }

        public bool AddAttachment(string leaveId, string attachmentName)
        {
            try
            {
                if (attachmentName.Length > 0)
                {
                    bool updateResult;
                    using (var con = new SqlConnection(new ApplicationClass().Constr))
                    {

                        using (var cmd = new SqlCommand("update LeaveRequests set ReportImage = '" + attachmentName + "' where Id = '" + leaveId + "'"))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = con;
                            con.Open();
                            updateResult = Convert.ToBoolean(cmd.ExecuteNonQuery());
                            con.Close();
                        }
                    }

                    //Check attachment path and if it does not exist
                    if (Directory.Exists(AttachmentPath) == false)
                    {
                        Directory.CreateDirectory(AttachmentPath); //Create attachment path
                    }

                    //Check leave attachment path and if it does not exist
                    if (Directory.Exists(AttachmentPath + @"\" + leaveId) == false)
                    {
                        Directory.CreateDirectory(AttachmentPath + @"\" + leaveId); //Create leave attachment path
                    }

                    return updateResult;
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return false;
        }
        public bool NoAttachment(string leaveId)
        {
            var updateResult = false;
            try
            {
                using (var con = new SqlConnection(new ApplicationClass().Constr))
                {

                    using (var cmd = new SqlCommand("update LeaveRequests set ReportImage = NULL where Id = '" + leaveId + "'"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        updateResult = Convert.ToBoolean(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return updateResult;
        }
    }
}