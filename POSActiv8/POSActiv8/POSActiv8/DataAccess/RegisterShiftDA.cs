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
    public class RegisterShiftDA
    {
        //View
        public SqlDataReader RegisterShift_View(string strControlNumber, string strSearchQuery)
        {
            try
            {
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@strControlNumber", strControlNumber),
                    new SqlParameter("@strSearchQuery", strSearchQuery)
                };

                return DBHelper.ExecuteParameterizedReader("RegisterShift_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlDataReader POS_XZReport_View(int intTransactionType, DateTime dteTransactionDate)
        {
            try
            {
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@intTransactionType", intTransactionType),
                    new SqlParameter("@dteTransactionDate", dteTransactionDate)
                };

                return DBHelper.ExecuteParameterizedReader("POS_XZReport_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string RegisterShift_Post(RegisterShiftBO regshiftBO, string strUserID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strControlNumber", regshiftBO.controlnumber),
                new SqlParameter("@intTransactionType", regshiftBO.transactiontype),
                new SqlParameter("@decOpeningAmount", regshiftBO.openingamount),
                new SqlParameter("@decClosingAmount", regshiftBO.closingamount),
                new SqlParameter("@strRemarks", regshiftBO.remarks),
                new SqlParameter("@intTransactionStatus", regshiftBO.transactionstatus),
                new SqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("RegisterShift_Post", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
