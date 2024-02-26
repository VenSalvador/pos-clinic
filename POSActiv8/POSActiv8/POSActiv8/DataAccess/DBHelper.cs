using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace DataAccess
{
    public class DBHelper
    {
        public static DataSet GetData(string SQLQuery)
        {
            string POS = ConfigurationManager.ConnectionStrings["POS"].ConnectionString;

            System.Data.DataSet dsCONN = new System.Data.DataSet();
            SqlConnection SQLConn = new SqlConnection(POS);
            SqlDataAdapter SQLDataAdapt = new SqlDataAdapter();

            try
            {
                SQLConn.Open();
                SQLDataAdapt.SelectCommand = new SqlCommand(SQLQuery, SQLConn);
                SQLDataAdapt.Fill(dsCONN);

                return dsCONN;
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                SQLConn.Close();
            }
        }

        public static SqlDataReader ExecuteReader(string CommandName, CommandType CmdType)
        {
            string POS = ConfigurationManager.ConnectionStrings["POS"].ConnectionString;

            SqlConnection SQLConn = new SqlConnection(POS);

            try
            {
                //Open connection
                SQLConn.Open();

                SqlCommand cmd = new SqlCommand(CommandName, SQLConn);
                cmd.CommandType = CommandType.StoredProcedure;

                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SqlDataReader ExecuteParameterizedReader(string CommandName, CommandType CmdType, SqlParameter[] param)
        {
            string POS = ConfigurationManager.ConnectionStrings["POS"].ConnectionString;

            SqlConnection SQLConn = new SqlConnection(POS);

            try
            {
                //Open connection
                SQLConn.Open();

                SqlCommand cmd = new SqlCommand(CommandName, SQLConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(param);

                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        static internal string ExecuteNonQueryParam(string CommandName, CommandType cmdType, SqlParameter[] param)
        {
            string POS = ConfigurationManager.ConnectionStrings["POS"].ConnectionString;

            var result = 0;
            System.Data.DataSet dsCONN = new System.Data.DataSet();
            SqlConnection SQLConn = new SqlConnection(POS);
            SQLConn.Open();

            try
            {
                SqlCommand cmd = new SqlCommand(CommandName, SQLConn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddRange(param);

                result = cmd.ExecuteNonQuery();

                return result.ToString();
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                SQLConn.Close();
            }
        }

    }
}
