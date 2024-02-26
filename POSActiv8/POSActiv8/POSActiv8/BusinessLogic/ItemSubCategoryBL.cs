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
    public class ItemSubCategoryBL
    {
        //View
        public SqlDataReader ItemSubCategory_View(int intRecordID, string strSearchQuery)
        {
            try
            {
                ItemSubCategoryDA itemsubcatDA = new ItemSubCategoryDA();
                return itemsubcatDA.ItemSubCategory_View(intRecordID, strSearchQuery);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string ItemSubCategory_Post(ItemSubCategoryBO itemsubcatBO, string strUserID)
        {
            try
            {
                ItemSubCategoryDA itemsubcatDA = new ItemSubCategoryDA();
                return itemsubcatDA.ItemSubCategory_Post(itemsubcatBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
