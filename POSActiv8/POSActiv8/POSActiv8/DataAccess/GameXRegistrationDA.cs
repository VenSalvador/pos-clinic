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
    public class GameXRegistrationDA
    {
        //View
        public SqlDataReader GameXRegistrataion_View(int intRecordID, string strSearchQuery)
        {
            try
            {
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@intRecordID", intRecordID),
                    new SqlParameter("@strSearchQuery", strSearchQuery)
                };

                return DBHelper.ExecuteParameterizedReader("GameXRegistration_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string GameXRegistration_Post(GameXRegistrationBO gamexregBO, string strUserID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@intRecordID", gamexregBO.recordid),
                //new SqlParameter("@strFirstName", gamexregBO.firstname),
                //new SqlParameter("@strMiddleName", gamexregBO.middlename),
                //new SqlParameter("@strLastName", gamexregBO.lastname),
                new SqlParameter("@strFullName", gamexregBO.fullname),
                new SqlParameter("@strContactNumbers", gamexregBO.contactnumbers),
                new SqlParameter("@strCompany", gamexregBO.company),
                new SqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("GameXRegistration_Post", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
