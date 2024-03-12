using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using BusinessObject;
using MySql.Data.MySqlClient;


namespace DataAccess
{
    public class ChargesDA
    {
        //View
        public MySqlDataReader Charges_View(int intRecordID, string strSearchQuery)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@intRecordID", intRecordID),
                    new MySqlParameter("@strSearchQuery", strSearchQuery)
                };

                return DBHelper.ExecuteParameterizedReader("Charges_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string Charges_Post(ChargesBO chargesBO, string strUserID)
        {
            try
            {
                 MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@intRecordID", chargesBO.recordid),
                    new MySqlParameter("@strChargeName", chargesBO.chargename),
                    new MySqlParameter("@decChargeAmount", chargesBO.chargeamount),
                    new MySqlParameter("@intRecordStatus", chargesBO.recordstatus),
                    new MySqlParameter("@strUserID", strUserID)
                };

                return DBHelper.ExecuteNonQueryParam("Charges_Post", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
