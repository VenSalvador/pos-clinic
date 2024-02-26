using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using BusinessObject;
using DataAccess;

namespace BusinessLogic
{
    public class KitchenOrdersBL
    {
        //View
        public SqlDataReader POS_KitchenOrders_View(string strControlNumber, DateTime dteTransactionDate)
        {
            try
            {
                KitchenOrdersDA kitchordersDA = new KitchenOrdersDA();
                return kitchordersDA.POS_KitchenOrders_View(strControlNumber, dteTransactionDate);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string POS_KitchenOrders_Post(KitchenOrdersBO kitchenordersBO, string strUserID)
        {
            try
            {
                KitchenOrdersDA kitchordersDA = new KitchenOrdersDA();
                return kitchordersDA.POS_KitchenOrders_Post(kitchenordersBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
