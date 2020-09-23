using Admin.Models.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace Admin.App_Code
{
    ///=======================================
    /// <summary>
    /// DB Connection Class
    /// </summary>
    ///=======================================
    public class DBInitConn
    {
        private static SqlConnectionStringBuilder builder = null;

        #region DB Connection Initialize
        public DBInitConn()
        {
            builder = new SqlConnectionStringBuilder();

            builder.DataSource     = "10.10.120.150";
            builder.UserID         = "sa";
            builder.Password       = "rhdahwjs12#$";
            builder.InitialCatalog = "BPM_DB";
        }
        #endregion

        ///---------------------------------------
        /// <summary>
        /// Executing SQL command with no value to return
        /// </summary>
        /// <param name="sql"></param>
        ///---------------------------------------
        public void SqlExecute(string sql)
        {
            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        ///---------------------------------------
        /// <summary>
        /// Executing SQL command with a single value to return
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        ///---------------------------------------
        public object SqlReturnValue(string sql)
        {
            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);

                object result = (object)cmd.ExecuteScalar();

                return result;
            }
        }

        ///---------------------------------------
        /// <summary>
        /// Executing SQL command with a table to return
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        ///--------------------------------------- 
        public DataTable SqlReturnDataTable(string sql)
        {
            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Connection.Open();

                DataTable result = new DataTable();

                result.Load(cmd.ExecuteReader());

                return result;
            }
        }

        ///---------------------------------------
        /// <summary>
        /// Executing Stored Procedure with a table to return
        /// </summary>
        /// <param name="StoredProcedure"></param>
        /// <param name="ParamName"></param>
        /// <param name="ParamValue"></param>
        /// <returns></returns>
        ///---------------------------------------
        public Result<DataTable> SqlExecuteSPReturnTable(string StoredProcedure, string[] ParamName, object[] ParamValue)
        {
            Result<DataTable> objResult = null;

            try
            {
                SqlConnection conn = new SqlConnection(builder.ConnectionString);
                SqlCommand    cmd  = new SqlCommand(StoredProcedure, conn);
                objResult          = new Result<DataTable>();

                cmd.CommandType = CommandType.StoredProcedure;

                for(int i = 0; i < ParamName.Length; i++)
                {
                    cmd.Parameters.Add(new SqlParameter(ParamName[i], ParamValue[i].ToString()));
                }

                cmd.Connection.Open();

                DataTable result = new DataTable();

                result.Load(cmd.ExecuteReader());

                objResult.RetVal = 0;
                objResult.data   = result;

            }
            catch(Exception ex)
            {
                objResult.RetVal = 999999;
                objResult.data   = null;
            }

            return objResult;
        }

        ///---------------------------------------
        /// <summary>
        /// Executing Stored Procedure with values to return
        /// </summary>
        /// <param name="StoredProcedure"></param>
        /// <param name="ParamName"></param>
        /// <param name="ParamValue"></param>
        /// <returns></returns>
        ///---------------------------------------
        public Result SqlExecuteSPReturnValue(string StoredProcedure, string[] ParamName, object[] ParamValue)
        {
            SqlParameter SqlRetVal = null;
            SqlParameter SqlErrMsg = null;
            Result       objResult = null;

            try
            {
                SqlConnection conn = new SqlConnection(builder.ConnectionString);
                SqlCommand    cmd  = new SqlCommand(StoredProcedure, conn);
                objResult          = new Result();

                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < ParamName.Length; i++)
                {
                    if (ParamName[i].Equals("@po_intRetVal"))
                    {
                        SqlRetVal           = new SqlParameter(ParamName[i], SqlDbType.Int);
                        SqlRetVal.Value     = ParamValue[i];
                        SqlRetVal.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(SqlRetVal);

                    }
                    else if (ParamName[i].Equals("@po_strErrMsg"))
                    {
                        SqlErrMsg           = new SqlParameter(ParamName[i], SqlDbType.VarChar);
                        SqlErrMsg.Size      = -1;
                        SqlErrMsg.Value     = ParamValue[i];
                        SqlErrMsg.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(SqlErrMsg);
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter(ParamName[i], ParamValue[i].ToString()));
                    }
                }

                cmd.Connection.Open();

                cmd.ExecuteNonQuery();

                objResult.RetVal = Convert.ToInt32(cmd.Parameters["@po_intRetVal"].Value);
                objResult.ErrMsg = cmd.Parameters["@po_strErrMsg"].Value.ToString();
            }
            catch(Exception ex)
            {
                objResult.RetVal = 999998;
               objResult.ErrMsg = string.Format("Exception in SqlExecuteSPReturnValue : {0}", ex.Message);
            }

            return objResult;
        }
    }
}
