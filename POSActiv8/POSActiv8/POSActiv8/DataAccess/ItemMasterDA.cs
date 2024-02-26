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
    public class ItemMasterDA
    {
        //View
        public SqlDataReader ItemMaster_View(string strItemCode, string strSearchQuery)
        {
            try
            {
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@strItemCode", strItemCode),
                    new SqlParameter("@strSearchQuery", strSearchQuery)
                };

                return DBHelper.ExecuteParameterizedReader("ItemMaster_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string ItemMaster_Post(ItemMasterBO itemmastBO, string strUserID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strItemCode", itemmastBO.itemcode),
                new SqlParameter("@strItemName", itemmastBO.itemname),
                new SqlParameter("@strItemDescription", itemmastBO.itemdescription),
                new SqlParameter("@strItemCategory", itemmastBO.itemcategory),
                new SqlParameter("@strItemSubCategory", itemmastBO.itemsubcategory),
                new SqlParameter("@decItemPrice", itemmastBO.itemprice),
                new SqlParameter("@intItemStatus", itemmastBO.itemstatus),
                new SqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("ItemMaster_Post", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
