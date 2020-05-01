using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using log4net;

namespace DemoWebApp.classes
{
    public class LeavePolicyClass
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string GetPolicyValueByKey(string policyKey)
        {
            var leavePolicyValue = "";
            try
            {
                var conn = new SqlConnection(new ApplicationClass().Constr);
                var comm = new SqlCommand("select LeavePolicyValue from LeavePolicies where LeavePolicyKey = '"+policyKey+"'", conn);
                if (conn.State == ConnectionState.Closed) conn.Open();

                var dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    leavePolicyValue = dr["LeavePolicyValue"].ToString();
                }
                dr.Close();
                conn.Close();
            }
            catch (SqlException ex)
            {
                Log.Error(ex);
            }
            return leavePolicyValue;
        }

        public bool SetPolicyValueByKey(string policyKey, string policyValue)
        {
            var conn = new SqlConnection(new ApplicationClass().Constr);
            var comm = new SqlCommand("update LeavePolicies set LeavePolicyValue = '"+policyValue+"' where LeavePolicyKey = '"+ policyKey + "'", conn);

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