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
    public class TableNamesDA
    {
        //View
        public SqlDataReader TableNames_View(string strTableCode, string strSearchQuery)
        {
            try
            {
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@strTableCode", strTableCode),
                    new SqlParameter("@strSearchQuery", strSearchQuery)
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
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strTableCode", tblnamesBO.tablecode),
                new SqlParameter("@strTableName", tblnamesBO.tablename),
                new SqlParameter("@strFloorLocation", tblnamesBO.floorlocationcode),
                new SqlParameter("@intRecordStatus", tblnamesBO.recordstatus),
                new SqlParameter("@strUserID", strUserID)
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
