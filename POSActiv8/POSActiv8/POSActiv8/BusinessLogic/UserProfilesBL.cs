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
    public class UserProfilesBL
    {
        //View
        public MySqlDataReader UserProfiles_View(string strUserID, string strSearchQuery)
        {
            try
            {
                UserProfilesDA upDA = new UserProfilesDA();
                return upDA.UserProfiles_View(strUserID, strSearchQuery);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string UserProfiles_Post(UserProfilesBO upBO, string strUserID)
        {
            try
            {
                UserProfilesDA upDA = new UserProfilesDA();
                return upDA.UserProfiles_Post(upBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
