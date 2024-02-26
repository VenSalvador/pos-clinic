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
    public class POSSessionsDA
    {
        //View
        public SqlDataReader POS_Sessions_View(string strSessionCode, string strSearchQuery)
        {
            try
            {
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@strSessionCode", strSessionCode),
                    new SqlParameter("@strSearchQuery", strSearchQuery)
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
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strSessionCode", possessBO.sessioncode),
                new SqlParameter("@intSessionStatus", possessBO.sessionstatus),
                new SqlParameter("@strTerminalName", possessBO.terminalname),
                new SqlParameter("@dteTransactionDate", possessBO.transactiondate),
                new SqlParameter("@decOpeningAmount", possessBO.openingamount),
                new SqlParameter("@decClosingAmount", possessBO.closingamount),
                new SqlParameter("@strRemarks", possessBO.remarks),
                new SqlParameter("@strUserID", strUserID)
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
