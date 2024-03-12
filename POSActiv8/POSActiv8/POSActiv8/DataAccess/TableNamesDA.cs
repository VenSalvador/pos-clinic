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
    public class TableNamesDA
    {
        //View
        public MySqlDataReader TableNames_View(string strTableCode, string strSearchQuery)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@strTableCode", strTableCode),
                    new MySqlParameter("@strSearchQuery", strSearchQuery)
                };

                return DBHelper.ExecuteParameterizedReader("TableNames_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string TableNames_Post(TableNamesBO tblnamesBO, string strUserID)
        {
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strTableCode", tblnamesBO.tablecode),
                new MySqlParameter("@strTableName", tblnamesBO.tablename),
                new MySqlParameter("@strFloorLocation", tblnamesBO.floorlocationcode),
                new MySqlParameter("@intRecordStatus", tblnamesBO.recordstatus),
                new MySqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("TableNames_Post", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
