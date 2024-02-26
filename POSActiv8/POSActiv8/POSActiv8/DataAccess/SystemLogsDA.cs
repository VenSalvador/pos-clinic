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
    public class SystemLogsDA
    {
        //View
        public SqlDataReader SystemLogs_View(int intCMD, string strReferenceDateFrom, string strReferenceDateTo)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@intCMD", intCMD),
                new SqlParameter("@strReferenceDateFrom", strReferenceDateFrom),
                new SqlParameter("@strReferenceDateTo", strReferenceDateTo)
            };

            try
            {
                return DBHelper.ExecuteParameterizedReader("SystemLogs_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Save
        public string SystemLogs_Save(string strSourceFile, string strActivity, string strRemarks, string strUserID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strSourceFile", strSourceFile),
                new SqlParameter("@strActivity", strActivity),
                new SqlParameter("@strRemarks", strRemarks),
                new SqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("SystemLogs_Save", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
