
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
    public class POSSessionsDA
    {
        //View
        public MySqlDataReader POS_Sessions_View(string strSessionCode, string strSearchQuery)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@strSessionCode", strSessionCode),
                    new MySqlParameter("@strSearchQuery", strSearchQuery)
                };

                return DBHelper.ExecuteParameterizedReader("POS_Sessions_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string POS_Sessions_Post(POSSessionsBO possessBO, string strUserID)
        {
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strSessionCode", possessBO.sessioncode),
                new MySqlParameter("@intSessionStatus", possessBO.sessionstatus),
                new MySqlParameter("@strTerminalName", possessBO.terminalname),
                new MySqlParameter("@dteTransactionDate", possessBO.transactiondate),
                new MySqlParameter("@decOpeningAmount", possessBO.openingamount),
                new MySqlParameter("@decClosingAmount", possessBO.closingamount),
                new MySqlParameter("@strRemarks", possessBO.remarks),
                new MySqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Sessions_Post", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
