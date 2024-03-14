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
    public class TaxBL
    {
        //View
        public MySqlDataReader Tax_View(int intRecordID, string strSearchQuery)
        {
            try
            {
                TaxDA taxDA = new TaxDA();
                return taxDA.Tax_View(intRecordID, strSearchQuery);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string Tax_Post(TaxBO taxBO, string strUserID)
        {
            try
            {
                TaxDA taxDA = new TaxDA();
                return taxDA.Tax_Post(taxBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
