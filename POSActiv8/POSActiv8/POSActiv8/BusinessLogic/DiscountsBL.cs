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
    public class DiscountsBL
    {
        //View
        public SqlDataReader Discounts_View(int intRecordID, string strSearchQuery)
        {
            try
            {
                DiscountsDA discDA = new DiscountsDA();
                return discDA.Discounts_View(intRecordID, strSearchQuery);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string Discounts_Post(DiscountsBO discBO, string strUserID)
        {
            try
            {
                DiscountsDA discDA = new DiscountsDA();
                return discDA.Discounts_Post(discBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
