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
    public class POSSessionsBL
    {
        //View
        public MySqlDataReader POS_Sessions_View(string strSessionCode, string strSearchQuery)
        {
            try
            {
                POSSessionsDA possessDA = new POSSessionsDA();
                return possessDA.POS_Sessions_View(strSessionCode, strSearchQuery);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string POS_Sessions_Post(POSSessionsBO possessBO, string strUserID)
        {
            try
            {
                POSSessionsDA possessDA = new POSSessionsDA();
                return possessDA.POS_Sessions_Post(possessBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
