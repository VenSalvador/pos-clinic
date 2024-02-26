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
    public class RegisterShiftBL
    {
        //View
        public SqlDataReader RegisterShift_View(string strControlNumber, string strSearchQuery)
        {
            try
            {
                RegisterShiftDA regshiftDA = new RegisterShiftDA();
                return regshiftDA.RegisterShift_View(strControlNumber, strSearchQuery);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlDataReader POS_XZReport_View(int intTransactionType, DateTime dteTransactionDate)
        {
            try
            {
                RegisterShiftDA regshiftDA = new RegisterShiftDA();
                return regshiftDA.POS_XZReport_View(intTransactionType, dteTransactionDate);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string RegisterShift_Post(RegisterShiftBO regshiftBO, string strUserID)
        {
            try
            {
                RegisterShiftDA regshiftDA = new RegisterShiftDA();
                return regshiftDA.RegisterShift_Post(regshiftBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
