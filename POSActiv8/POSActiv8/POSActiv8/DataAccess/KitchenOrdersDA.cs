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
    public class KitchenOrdersDA
    {
        //View
        public SqlDataReader POS_KitchenOrders_View(string strControlNumber, DateTime dteTransactionDate)
        {
            try
            {
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@strControlNumber", strControlNumber),
                    new SqlParameter("@dteTransactionDate", dteTransactionDate)
                };

                return DBHelper.ExecuteParameterizedReader("POS_KitchenOrders_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string POS_KitchenOrders_Post(KitchenOrdersBO kitchenordersBO, string strUserID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strControlNumber", kitchenordersBO.controlnumber),
                new SqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_KitchenOrders_Complete", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
