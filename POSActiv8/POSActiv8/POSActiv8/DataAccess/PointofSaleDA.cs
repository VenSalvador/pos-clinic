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
    public class PointofSaleDA
    {
        //View
        public MySqlDataReader POS_MyOrders_View(string strTableCode, string strUserID)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@strTableCode", strTableCode),
                    new MySqlParameter("@strUserID", strUserID)
                };

                return DBHelper.ExecuteParameterizedReader("POS_MyOrders_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MySqlDataReader POS_Orders_View(DateTime dteReferenceDate)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@dteReferenceDate", dteReferenceDate)
                };

                return DBHelper.ExecuteParameterizedReader("POS_Orders_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //
        public MySqlDataReader POS_Orders_View(string strControlNumber, string strTableCode, string strItemCode, string strUserID)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@strControlNumber", strControlNumber),
                    new MySqlParameter("@strTableCode", strTableCode),
                    new MySqlParameter("@strItemCode", strItemCode),
                    new MySqlParameter("@strUserID", strUserID)
                };

                return DBHelper.ExecuteParameterizedReader("POS_Orders_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MySqlDataReader POS_OrdersTotal_View(string strControlNumber, string strUserID)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@strControlNumber", strControlNumber),
                    new MySqlParameter("@strUserID", strUserID)
                };

                return DBHelper.ExecuteParameterizedReader("POS_OrdersTotal_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MySqlDataReader POS_SalesInvoice_View(int intCMD, string strControlNumber, DateTime dteReferenceDateFrom, DateTime dteReferenceDateTo)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@intCMD", intCMD),
                    new MySqlParameter("@strControlNumber", strControlNumber),
                    new MySqlParameter("@dteReferenceDateFrom", dteReferenceDateFrom),
                    new MySqlParameter("@dteReferenceDateTo", dteReferenceDateTo)
                };

                return DBHelper.ExecuteParameterizedReader("POS_SalesInvoice_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MySqlDataReader POS_XZReport_View(int intTransactionType, DateTime dteTransactionDate, string strUserID)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@intTransactionType", intTransactionType),
                    new MySqlParameter("@dteTransactionDate", dteTransactionDate),
                    new MySqlParameter("@strUserID", strUserID)
                };

                return DBHelper.ExecuteParameterizedReader("POS_XZReport_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MySqlDataReader POS_VoidItems_View(DateTime dteTransactionDate)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@dteTransactionDate", dteTransactionDate)
                };

                return DBHelper.ExecuteParameterizedReader("POS_VoidItems_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MySqlDataReader POS_CancelledOrders_View(DateTime dteTransactionDate)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@dteTransactionDate", dteTransactionDate)
                };

                return DBHelper.ExecuteParameterizedReader("POS_CancelledOrders_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string POS_Orders_Select(PointofSaleBO posBO, string strUserID)
        {
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strTableCode", posBO.tablecode),
                new MySqlParameter("@strCategoryCode", posBO.categorycode),
                new MySqlParameter("@strItemCode", posBO.itemcode),
                new MySqlParameter("@decItemPrice", posBO.itemprice),
                new MySqlParameter("@intItemQuantity", posBO.itemquantity),
                new MySqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_Select", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_Post(PointofSaleBO posBO, string strUserID)
        {
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strControlNumber", posBO.controlnumber),
                new MySqlParameter("@strTableCode", posBO.tablecode),
                new MySqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_Post", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_Discount(PointofSaleBO posBO, string strUserID)
        {
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strControlNumber", posBO.controlnumber),
                new MySqlParameter("@strItemCode", posBO.itemcode),
                new MySqlParameter("@intDiscountType", posBO.discounttype),
                new MySqlParameter("@decDiscountAmount", posBO.discountamount),
                new MySqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_Discount", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_Quantity(PointofSaleBO posBO, string strUserID)
        {
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strControlNumber", posBO.controlnumber),
                new MySqlParameter("@strTableCode", posBO.tablecode),
                new MySqlParameter("@strItemCode", posBO.itemcode),
                new MySqlParameter("@intItemQuantity", posBO.itemquantity),
                new MySqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_Quantity", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_ServiceCharge(PointofSaleBO posBO, string strUserID)
        {
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strControlNumber", posBO.controlnumber),
                new MySqlParameter("@decServiceCharge", posBO.servicecharge),
                new MySqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_ServiceCharge", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_Payment(PointofSaleBO posBO, string strUserID)
        {
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strControlNumber", posBO.controlnumber),
                new MySqlParameter("@strTableCode", posBO.tablecode),
                new MySqlParameter("@intPaymentType", posBO.paymenttype),
                new MySqlParameter("@decPaymentAmount", posBO.paymentamount),
                new MySqlParameter("@decChangeDue", posBO.changedue),
                new MySqlParameter("@strCardType", posBO.cardtype),
                new MySqlParameter("@strCardNumber", posBO.cardnumber),
                new MySqlParameter("@strAccountName", posBO.accountname),
                new MySqlParameter("@dteExpiryDate", posBO.expirydate),
                new MySqlParameter("@strGCashReferenceNumber", posBO.gcashreferencenumber),
                new MySqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_Payment", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string POS_Orders_Void(PointofSaleBO posBO, string strUserID)
        {
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strControlNumber", posBO.controlnumber),
                new MySqlParameter("@strItemCode", posBO.itemcode),
                new MySqlParameter("@strVoidAuthorizedBy", posBO.voidauthorizedby),
                new MySqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_Void", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string POS_Orders_Cancel(PointofSaleBO posBO, string strUserID)
        {
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strControlNumber", posBO.controlnumber),
                new MySqlParameter("@strTableCode", posBO.tablecode),
                new MySqlParameter("@intCancelType", posBO.canceltype),
                new MySqlParameter("@strCancelAuthorizedBy", posBO.cancelauthorizedby),
                new MySqlParameter("@strCancelRemarks", posBO.cancelremarks),
                new MySqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("POS_Orders_Cancel", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
