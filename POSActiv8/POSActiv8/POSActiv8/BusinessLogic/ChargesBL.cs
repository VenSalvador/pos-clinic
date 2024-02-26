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
    public class ChargesBL
    {
        //View
        public SqlDataReader Charges_View(int intRecordID, string strSearchQuery)
        {
            try
            {
                ChargesDA chargesDA= new ChargesDA();
                return chargesDA.Charges_View(intRecordID, strSearchQuery);
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
                ChargesDA chargesDA = new ChargesDA();
                return chargesDA.Charges_Post(chargesBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
