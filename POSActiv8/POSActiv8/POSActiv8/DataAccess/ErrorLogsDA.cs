using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

using BusinessObject;

namespace DataAccess
{
    public class ErrorLogsDA
    {
        //View
        public SqlDataReader ErrorLogs_View(int intCMD, string strReferenceDateFrom, string strReferenceDateTo)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@intCMD", intCMD),
                new SqlParameter("@strReferenceDateFrom", strReferenceDateFrom),
                new SqlParameter("@strReferenceDateTo", strReferenceDateTo)
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
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strSourceFile", strSourceFile),
                new SqlParameter("@strErrorName", strSectionName),
                new SqlParameter("@strErrorDescription", strErrorDescription),
                new SqlParameter("@strUserID", strUserID)
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
