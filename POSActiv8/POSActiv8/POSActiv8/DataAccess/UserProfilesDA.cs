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
    public class UserProfilesDA
    {
        //View
        public MySqlDataReader UserProfiles_View(string strUserID, string strSearchQuery)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    new MySqlParameter("@strUserID", strUserID),
                    new MySqlParameter("@strSearchQuery", strSearchQuery)
                };

                return DBHelper.ExecuteParameterizedReader("UserProfiles_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string UserProfiles_Post(UserProfilesBO upBO, string strUserID)
        {
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@intRecordID", upBO.recordid),
                new MySqlParameter("@strUserID", upBO.networkid),
                new MySqlParameter("@strFullName", upBO.fullname),
                new MySqlParameter("@strEmailAddress", upBO.emailaddress),
                new MySqlParameter("@strUserDepartment", upBO.department),
                new MySqlParameter("@intUserLevel", upBO.userlevel),
                new MySqlParameter("@intUserRole", upBO.userrole),
                new MySqlParameter("@intUserStatus", upBO.userstatus),
                new MySqlParameter("@intLoginStatus", upBO.loginstatus),
                new MySqlParameter("@strPostedBy", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("UserProfiles_Post", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
