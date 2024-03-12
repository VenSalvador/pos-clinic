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
    public class SystemLogsDA
    {
        //View
        public MySqlDataReader SystemLogs_View(int intCMD, string strReferenceDateFrom, string strReferenceDateTo)
        {
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@intCMD", intCMD),
                new MySqlParameter("@strReferenceDateFrom", strReferenceDateFrom),
                new MySqlParameter("@strReferenceDateTo", strReferenceDateTo)
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
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strSourceFile", strSourceFile),
                new MySqlParameter("@strActivity", strActivity),
                new MySqlParameter("@strRemarks", strRemarks),
                new MySqlParameter("@strUserID", strUserID)
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
