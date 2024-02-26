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
    public class FloorLocationDA
    {
        //View
        public SqlDataReader FloorLocation_View(string strFloorLocationCode, string strSearchQuery)
        {
            try
            {
                SqlParameter[] myparams = new SqlParameter[]
                {
                    new SqlParameter("@strFloorLocationCode", strFloorLocationCode),
                    new SqlParameter("@strSearchQuery", strSearchQuery)
                };

                return DBHelper.ExecuteParameterizedReader("FloorLocation_View", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string FloorLocation_Post(FloorLocationBO floorlocBO, string strUserID)
        {
            SqlParameter[] myparams = new SqlParameter[]
            {
                new SqlParameter("@strFloorLocationCode", floorlocBO.floorlocationcode),
                new SqlParameter("@strFloorName", floorlocBO.floorname),
                new SqlParameter("@strFloorColor", floorlocBO.floorcolor),
                //new SqlParameter("@strFloorDescription", floorlocBO.floordescription),
                new SqlParameter("@intRecordStatus", floorlocBO.recordstatus),
                new SqlParameter("@strUserID", strUserID)
            };

            try
            {
                return DBHelper.ExecuteNonQueryParam("FloorLocation_Post", CommandType.StoredProcedure, myparams);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
