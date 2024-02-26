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
    public class FloorLocationBL
    {
        //View
        public SqlDataReader FloorLocation_View(string strFloorLocationCode, string strSearchQuery)
        {
            try
            {
                FloorLocationDA floorlocDA = new FloorLocationDA();
                return floorlocDA.FloorLocation_View(strFloorLocationCode, strSearchQuery);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Post
        public string FloorLocation_Post(FloorLocationBO floorlocBO, string strUserID)
        {
            try
            {
                FloorLocationDA floorlocDA = new FloorLocationDA();
                return floorlocDA.FloorLocation_Post(floorlocBO, strUserID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
