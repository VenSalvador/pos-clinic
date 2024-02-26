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
    public class SystemParametersBL
    {
        //View
        public SqlDataReader SystemParameters_View(int intParameterCode)
        {
            try
            {
                SystemParametersDA sysparamDA = new SystemParametersDA();
                return sysparamDA.SystemParameters_View(intParameterCode);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlDataReader Months_View()
        {
            try
            {
                SystemParametersDA sysparamDA = new SystemParametersDA();
                return sysparamDA.Months_View();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
