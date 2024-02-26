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
    public class SystemParametersDA
    {
        //View
        public SqlDataReader SystemParameters_View(int intParameterCode)
        {
            try
            {
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@intParameterCode", intParameterCode),
                };

                return DBHelper.ExecuteParameterizedReader("SystemParameters_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlDataReader Months_View()
        {
            try
            {
                return DBHelper.ExecuteReader("Months_View", CommandType.StoredProcedure);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
