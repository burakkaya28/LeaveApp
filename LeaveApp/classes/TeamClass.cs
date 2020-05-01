using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using log4net;

namespace DemoWebApp.classes
{
    public class TeamClass
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public bool AddTeam(string teamName, int managerId)
        {
            bool processResult;
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("insert into Teams (TeamName, ManagerId, EnabledFlag, CreationDate) values(@TeamName, @ManagerId, 1, GETDATE())", conn);
            comm.Parameters.Add("@TeamName", SqlDbType.NVarChar).Value = teamName;
            comm.Parameters.Add("@ManagerId", SqlDbType.NVarChar).Value = managerId;

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

        public bool TeamExistControl(string teamName)
        {
            var result = false;
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("Select TeamName from Teams where TeamName = @TeamName", conn);
            comm.Parameters.Add("@TeamName", SqlDbType.VarChar).Value = teamName;
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

        public bool TeamExistControlwithId(string teamId)
        {
            var result = false;
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("Select TeamId from Teams where TeamId = @teamId", conn);
            comm.Parameters.Add("@teamId", SqlDbType.VarChar).Value = teamId;
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

        public bool TeamUpdate(string teamId, string teamName, int managerId)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            if (conn.State == ConnectionState.Closed) conn.Open();

            var comm = new SqlCommand
                ("update Teams set TeamName = '"+teamName+"', ManagerId = "+managerId+" where TeamId = '"+teamId+"'", conn);
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

        public bool TeamActivate(string teamId)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("update Teams set EnabledFlag=1 where TeamId='" + teamId + "'", conn);

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

        public bool TeamDisable(string teamId)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("update Teams set EnabledFlag=0 where TeamId='" + teamId + "'", conn);

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

        public string GetManagerIdByTeam(string teamId)
        {
            string managerId = null;
            try
            {
                using (var con = new SqlConnection(new ApplicationClass().Constr))
                {
                    using (var cmd = new SqlCommand("select ManagerId from Teams where TeamId ='" + teamId + "'"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        managerId = Convert.ToString(cmd.ExecuteScalar());
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return managerId;
        }

        public string GetTeamStatus(string teamId)
        {
            string isActive = null;
            try
            {
                using (var con = new SqlConnection(new ApplicationClass().Constr))
                {
                    using (var cmd = new SqlCommand("select 'Y' IsActive from Teams where TeamId = '" + teamId + "' and EnabledFlag=1"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        isActive = Convert.ToString(cmd.ExecuteScalar());
                        con.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return isActive;
        }
    }
}