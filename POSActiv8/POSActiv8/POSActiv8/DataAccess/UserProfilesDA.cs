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
    public class UserProfilesDA
    {
        //View
        public SqlDataReader UserProfiles_View(string strUserID, string strSearchQuery)
        {
            try
            {
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@strUserID", strUserID),
                    new SqlParameter("@strSearchQuery", strSearchQuery)
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
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@intRecordID", upBO.recordid),
                new SqlParameter("@strUserID", upBO.networkid),
                new SqlParameter("@strFullName", upBO.fullname),
                new SqlParameter("@strEmailAddress", upBO.emailaddress),
                new SqlParameter("@strUserDepartment", upBO.department),
                new SqlParameter("@intUserLevel", upBO.userlevel),
                new SqlParameter("@intUserRole", upBO.userrole),
                new SqlParameter("@intUserStatus", upBO.userstatus),
                new SqlParameter("@intLoginStatus", upBO.loginstatus),
                new SqlParameter("@strPostedBy", strUserID)
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
