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
    public class GameXRegistrationDA
    {
        //View
        public MySqlDataReader GameXRegistrataion_View(int intRecordID, string strSearchQuery)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@intRecordID", intRecordID),
                    new MySqlParameter("@strSearchQuery", strSearchQuery)
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
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@intRecordID", gamexregBO.recordid),
                //new SqlParameter("@strFirstName", gamexregBO.firstname),
                //new SqlParameter("@strMiddleName", gamexregBO.middlename),
                //new SqlParameter("@strLastName", gamexregBO.lastname),
                new MySqlParameter("@strFullName", gamexregBO.fullname),
                new MySqlParameter("@strContactNumbers", gamexregBO.contactnumbers),
                new MySqlParameter("@strCompany", gamexregBO.company),
                new MySqlParameter("@strUserID", strUserID)
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
