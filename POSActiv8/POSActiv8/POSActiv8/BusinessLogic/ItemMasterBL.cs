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
    public class ItemMasterBL
    {
        //View
        public MySqlDataReader ItemMaster_View(string strItemCode, string strSearchQuery)
        {
            try
            {
                ItemMasterDA itemmastDA = new ItemMasterDA();
                return itemmastDA.ItemMaster_View(strItemCode, strSearchQuery);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string ItemMaster_Post(ItemMasterBO itemmastBO, string strUserID)
        {
            try
            {
                ItemMasterDA equipDA = new ItemMasterDA();
                return equipDA.ItemMaster_Post(itemmastBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
