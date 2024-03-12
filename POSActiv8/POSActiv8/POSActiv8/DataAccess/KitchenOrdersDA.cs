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
    public class KitchenOrdersDA
    {
        //View
        public MySqlDataReader POS_KitchenOrders_View(string strControlNumber, DateTime dteTransactionDate)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@strControlNumber", strControlNumber),
                    new MySqlParameter("@dteTransactionDate", dteTransactionDate)
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
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strControlNumber", kitchenordersBO.controlnumber),
                new MySqlParameter("@strUserID", strUserID)
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
