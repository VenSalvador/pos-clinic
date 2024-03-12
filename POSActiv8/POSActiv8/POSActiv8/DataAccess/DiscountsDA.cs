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
    public class DiscountsDA
    {
        //View
        public MySqlDataReader Discounts_View(int intRecordID, string strSearchQuery)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@intRecordID", intRecordID),
                    new MySqlParameter("@strSearchQuery", strSearchQuery)
                };

                return DBHelper.ExecuteParameterizedReader("Discounts_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string Discounts_Post(DiscountsBO discBO, string strUserID)
        {

            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@intRecordID", discBO.recordid),
                new MySqlParameter("@strDiscountName", discBO.discountname),
                new MySqlParameter("@strDiscountAmount", discBO.discountamount),
                new MySqlParameter("@intRecordStatus", discBO.recordstatus),
                new MySqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("Discounts_Post", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
