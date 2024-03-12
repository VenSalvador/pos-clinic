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
    public class ItemMasterDA
    {
        //View
        public MySqlDataReader ItemMaster_View(string strItemCode, string strSearchQuery)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@strItemCode", strItemCode),
                    new MySqlParameter("@strSearchQuery", strSearchQuery)
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
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strItemCode", itemmastBO.itemcode),
                new MySqlParameter("@strItemName", itemmastBO.itemname),
                new MySqlParameter("@strItemDescription", itemmastBO.itemdescription),
                new MySqlParameter("@strItemCategory", itemmastBO.itemcategory),
                new MySqlParameter("@strItemSubCategory", itemmastBO.itemsubcategory),
                new MySqlParameter("@decItemPrice", itemmastBO.itemprice),
                new MySqlParameter("@intItemStatus", itemmastBO.itemstatus),
                new MySqlParameter("@strUserID", strUserID)
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
