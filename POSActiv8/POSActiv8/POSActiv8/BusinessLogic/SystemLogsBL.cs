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
    public class SystemLogsBL
    {
        //View
        public SqlDataReader SystemLogs_View(int intCMD, string strReferenceDateFrom, string strReferenceDateTo)
        {
            try
            {
                SystemLogsDA slDA = new SystemLogsDA();
                return slDA.SystemLogs_View(intCMD, strReferenceDateFrom, strReferenceDateTo);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Save
        public string SystemLogs_Save(string strSourceFile, string strActivity, string strRemarks, string strUserID)
        {
            try
            {
                SystemLogsDA slDA = new SystemLogsDA();
                return slDA.SystemLogs_Save(strSourceFile, strActivity, strRemarks, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
