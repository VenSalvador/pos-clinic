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
    public class TableNamesBL
    {
        //View
        public MySqlDataReader TableNames_View(string strTableCode, string strSearchQuery)
        {
            try
            {
                TableNamesDA tblnamesDA = new TableNamesDA();
                return tblnamesDA.TableNames_View(strTableCode, strSearchQuery);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string TableNames_Post(TableNamesBO tblnamesBO, string strUserID)
        {
            try
            {
                TableNamesDA tblnamesDA = new TableNamesDA();
                return tblnamesDA.TableNames_Post(tblnamesBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
