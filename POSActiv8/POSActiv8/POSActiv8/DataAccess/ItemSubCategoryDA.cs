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
    public class ItemSubCategoryDA
    {
        //View
        public MySqlDataReader ItemSubCategory_View(int intRecordID, string strSearchQuery)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@intRecordID", intRecordID),
                    new MySqlParameter("@strSearchQuery", strSearchQuery)
                };

                return DBHelper.ExecuteParameterizedReader("ItemSubCategory_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string ItemSubCategory_Post(ItemSubCategoryBO itemsubcatBO, string strUserID)
        {
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strSubCategoryCode", itemsubcatBO.subcategorycode),
                new MySqlParameter("@strSubCategoryName", itemsubcatBO.subcategoryname),
                new MySqlParameter("@intItemCategoryCode", itemsubcatBO.itemcategorycode),
                new MySqlParameter("@intRecordStatus", itemsubcatBO.recordstatus),
                new MySqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("ItemSubCategory_Post", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
