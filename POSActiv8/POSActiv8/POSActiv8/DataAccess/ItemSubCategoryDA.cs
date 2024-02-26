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
    public class ItemSubCategoryDA
    {
        //View
        public SqlDataReader ItemSubCategory_View(int intRecordID, string strSearchQuery)
        {
            try
            {
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@intRecordID", intRecordID),
                    new SqlParameter("@strSearchQuery", strSearchQuery)
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
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strSubCategoryCode", itemsubcatBO.subcategorycode),
                new SqlParameter("@strSubCategoryName", itemsubcatBO.subcategoryname),
                new SqlParameter("@intItemCategoryCode", itemsubcatBO.itemcategorycode),
                new SqlParameter("@intRecordStatus", itemsubcatBO.recordstatus),
                new SqlParameter("@strUserID", strUserID)
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
