using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

using BusinessObject;

namespace DataAccess
{
    public class FloorLocationDA
    {
        //View
        public MySqlDataReader FloorLocation_View(string strFloorLocationCode, string strSearchQuery)
        {
            try
            {
                MySqlParameter[] myparams = new MySqlParameter[]
                {
                    
                    new MySqlParameter("@strFloorLocationCode", strFloorLocationCode),
                    new MySqlParameter("@strSearchQuery", strSearchQuery)
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
            MySqlParameter[] myparams = new MySqlParameter[]
            {
                new MySqlParameter("@strFloorLocationCode", floorlocBO.floorlocationcode),
                new MySqlParameter("@strFloorName", floorlocBO.floorname),
                new MySqlParameter("@strFloorColor", floorlocBO.floorcolor),
                //new SqlParameter("@strFloorDescription", floorlocBO.floordescription),
                new MySqlParameter("@intRecordStatus", floorlocBO.recordstatus),
                new MySqlParameter("@strUserID", strUserID)
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
