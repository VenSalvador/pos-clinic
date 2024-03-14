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
    public class ErrorLogsBL
    {
        //View
        public MySqlDataReader ErrorLogs_View(int intCMD, string strReferenceDateFrom, string strReferenceDateTo)
        {
            try
            {
                ErrorLogsDA elDA = new ErrorLogsDA();
                return elDA.ErrorLogs_View(intCMD, strReferenceDateFrom, strReferenceDateTo);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Save
        public string ErrorLogs_Save(string strSourceFile, string strSectionName, string strErrorDescription, string strUserID)
        {
            try
            {
                ErrorLogsDA errorlogsDA = new ErrorLogsDA();
                return errorlogsDA.ErrorLogs_Save(strSourceFile, strSectionName, strErrorDescription, strUserID);
            }

            catch (Exception ex)
            {
                
                throw ex;
            }
        }

    }
}
