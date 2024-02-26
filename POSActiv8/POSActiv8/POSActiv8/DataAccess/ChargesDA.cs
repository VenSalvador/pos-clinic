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
    public class ChargesDA
    {
        //View
        public SqlDataReader Charges_View(int intRecordID, string strSearchQuery)
        {
            try
            {
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@intRecordID", intRecordID),
                    new SqlParameter("@strSearchQuery", strSearchQuery)
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
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@intRecordID", chargesBO.recordid),
                    new SqlParameter("@strChargeName", chargesBO.chargename),
                    new SqlParameter("@decChargeAmount", chargesBO.chargeamount),
                    new SqlParameter("@intRecordStatus", chargesBO.recordstatus),
                    new SqlParameter("@strUserID", strUserID)
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
