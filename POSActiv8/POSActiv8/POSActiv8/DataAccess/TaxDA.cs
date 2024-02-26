﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

using BusinessObject;

namespace DataAccess
{
    public class TaxDA
    {
        //View
        public SqlDataReader Tax_View(int intRecordID, string strSearchQuery)
        {
            try
            {
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@intRecordID", intRecordID),
                    new SqlParameter("@strSearchQuery", strSearchQuery)
                };

                return DBHelper.ExecuteParameterizedReader("Tax_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string Tax_Post(TaxBO taxBO, string strUserID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@intRecordID", taxBO.recordid),
                new SqlParameter("@strTaxName", taxBO.taxname),
                new SqlParameter("@decTaxAmount", taxBO.taxamount),
                new SqlParameter("@intRecordStatus", taxBO.recordstatus),
                new SqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("Tax_Post", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}