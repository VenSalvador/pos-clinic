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
    public class RegisterShiftDA
    {
        //View
        public MySqlDataReader RegisterShift_View(string strControlNumber, string strSearchQuery)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@strControlNumber", strControlNumber),
                    new MySqlParameter("@strSearchQuery", strSearchQuery)
                };

                return DBHelper.ExecuteParameterizedReader("RegisterShift_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MySqlDataReader POS_XZReport_View(int intTransactionType, DateTime dteTransactionDate)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@intTransactionType", intTransactionType),
                    new MySqlParameter("@dteTransactionDate", dteTransactionDate)
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
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strControlNumber", regshiftBO.controlnumber),
                new MySqlParameter("@intTransactionType", regshiftBO.transactiontype),
                new MySqlParameter("@decOpeningAmount", regshiftBO.openingamount),
                new MySqlParameter("@decClosingAmount", regshiftBO.closingamount),
                new MySqlParameter("@strRemarks", regshiftBO.remarks),
                new MySqlParameter("@intTransactionStatus", regshiftBO.transactionstatus),
                new MySqlParameter("@strUserID", strUserID)
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
