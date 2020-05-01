using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using log4net;

namespace DemoWebApp.classes
{
    public class RoleClass
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool AddRole(string roleName)
        {
            bool processResult;
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("insert into Roles (RoleName, EnabledFlag, CreationDate) values(@RoleName,1,GETDATE())", conn);
            comm.Parameters.Add("@RoleName", SqlDbType.NVarChar).Value = roleName;

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

        public bool RoleExistControl(string roleName)
        {
            var result = false;
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("Select RoleName from Roles where RoleName = @RoleName", conn);
            comm.Parameters.Add("@RoleName", SqlDbType.VarChar).Value = roleName;
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

        public bool RoleExistControlwithId(string roleId)
        {
            var result = false;
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("Select RoleName from Roles where RoleId = @RoleId", conn);
            comm.Parameters.Add("@RoleId", SqlDbType.VarChar).Value = roleId;
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

        public bool RoleUpdate(string roleId, string roleName)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            if (conn.State == ConnectionState.Closed) conn.Open();

            var comm = new SqlCommand
            (@"update Roles 
                set RoleName='" + roleName + "' " +
             @"where RoleId=" + roleId, conn);
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

        public bool RoleActivate(string roleId)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("update Roles set EnabledFlag=1 where RoleId='" + roleId + "'", conn);

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

        public bool RoleDisable(string roleId)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("update Roles set EnabledFlag=0 where RoleId='" + roleId + "'", conn);

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