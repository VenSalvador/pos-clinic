using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using BusinessObject;
using DataAccess;

namespace BusinessLogic
{
    public class GameXRegistrationBL
    {
        //View
        public SqlDataReader GameXRegistrataion_View(int intRecordID, string strSearchQuery)
        {
            try
            {
                GameXRegistrationDA gamexregDA = new GameXRegistrationDA();
                return gamexregDA.GameXRegistrataion_View(intRecordID, strSearchQuery);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string GameXRegistration_Post(GameXRegistrationBO gamexregBO, string strUserID)
        {
            try
            {
                GameXRegistrationDA gamexregDA = new GameXRegistrationDA();
                return gamexregDA.GameXRegistration_Post(gamexregBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
