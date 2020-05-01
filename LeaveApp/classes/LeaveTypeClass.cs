using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using log4net;

namespace DemoWebApp.classes
{
    public class LeaveTypeClass
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool AddLeaveType(string leaveTypeEng, string leaveTypeTr)
        {
            bool processResult;
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("insert into LeaveTypes (LeaveType, LeaveTypeTr, EnabledFlag, CreationDate)values(@leaveTypeENG, @leaveTypeTR, 1, GETDATE())", conn);
            comm.Parameters.Add("@leaveTypeENG", SqlDbType.NVarChar).Value = leaveTypeEng;
            comm.Parameters.Add("@leaveTypeTR", SqlDbType.NVarChar).Value = leaveTypeTr;

            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                processResult = Convert.ToBoolean(comm.ExecuteNonQuery());
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
                return false;
            }
            finally { conn.Close(); }
            return processResult;
        }

        public bool LeaveTypeExistControl(string leaveTypeName)
        {
            var result = false;
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("Select LeaveTypeId from LeaveTypes where LeaveType = @leaveType", conn);
            comm.Parameters.Add("@leaveType", SqlDbType.VarChar).Value = leaveTypeName;
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
                result = false;
            }
            finally { conn.Close(); }
            return result;
        }

        public bool LeaveTypeExistControlwithId(string leaveTypeId)
        {
            var result = false;
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("Select LeaveTypeId from LeaveTypes where LeaveTypeId = @leaveTypeId", conn);
            comm.Parameters.Add("@leaveTypeId", SqlDbType.VarChar).Value = leaveTypeId;
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
                result = false;
            }
            finally { conn.Close(); }
            return result;
        }

        public bool LeaveTypeUpdate(string leaveTypeId, string leaveTypeEng, string leaveTypeTr)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            if (conn.State == ConnectionState.Closed) conn.Open();

            var comm = new SqlCommand
            ("update LeaveTypes set LeaveType='" + leaveTypeEng + "', LeaveTypeTr='"+leaveTypeTr+"' where LeaveTypeId=" + leaveTypeId, conn);
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

        public bool LeaveTypeActivate(string leaveTypeId)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("update LeaveTypes set EnabledFlag=1 where LeaveTypeId='" + leaveTypeId + "'", conn);

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

        public bool LeaveTypeDisable(string leaveTypeId)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("update LeaveTypes set EnabledFlag=0 where LeaveTypeId='" + leaveTypeId + "'", conn);

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
    }
}