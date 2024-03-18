using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

using BusinessObject;

namespace DataAccess
{
    public class ErrorLogsDA
    {
        //View
        public MySqlDataReader ErrorLogs_View(int intCMD, string strReferenceDateFrom, string strReferenceDateTo)
        {
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@intCMD", intCMD),
                new MySqlParameter("@strReferenceDateFrom", strReferenceDateFrom),
                new MySqlParameter("@strReferenceDateTo", strReferenceDateTo)
            };

            try
            {
                return DBHelper.ExecuteParameterizedReader("ErrorLogs_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Save
        public string ErrorLogs_Save(string strSourceFile, string strSectionName, string strErrorDescription, string strUserID)
        {
            MySqlParameter[] myparams = new MySqlParameter[]
            {
              new MySqlParameter("@strSourceFile", strSourceFile),
              new MySqlParameter("@strErrorName", strSectionName),
              new MySqlParameter("@strErrorDescription", strErrorDescription),
              new MySqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("ErrorLogs_Save", CommandType.StoredProcedure, myparams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
