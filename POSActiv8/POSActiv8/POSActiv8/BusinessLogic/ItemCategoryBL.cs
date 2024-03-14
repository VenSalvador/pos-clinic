using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

using BusinessObject;
using DataAccess;

namespace BusinessLogic
{
    public class ItemCategoryBL
    {
        //View
        public MySqlDataReader ItemCategory_View(int intRecordID, string strSearchQuery)
        {
            try
            {
                ItemCategoryDA itemcatDA = new ItemCategoryDA();
                return itemcatDA.ItemCategory_View(intRecordID, strSearchQuery);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string ItemCategory_Post(ItemCategoryBO itemcatBO, string strUserID)
        {
            try
            {
                ItemCategoryDA itemcatDA = new ItemCategoryDA();
                return itemcatDA.ItemCategory_Post(itemcatBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
