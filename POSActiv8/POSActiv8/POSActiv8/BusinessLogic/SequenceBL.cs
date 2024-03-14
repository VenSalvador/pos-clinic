using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

using BusinessObject;
using DataAccess;

namespace BusinessLogic
{
    public class SequenceBL
    {
        public string ControlNumber_Generate(int intSequenceID)
        {
            try
            {
                SequenceDA seqDA = new SequenceDA();
                return seqDA.ControlNumber_Generate(intSequenceID);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Vehicle Profile
        public string VehicleControlNumber_Generate()
        {
            try
            {
                SequenceDA seqDA = new SequenceDA();
                return seqDA.VehicleControlNumber_Generate();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Vehicle Requisition
        public string VehicleRequisitionControlID_Generate()
        {
            try
            {
                SequenceDA seqDA = new SequenceDA();
                return seqDA.VehicleRequisitionControlID_Generate();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Maintenance Requisition
        public string MaintenanceRequisitionControlID_Generate()
        {
            try
            {
                SequenceDA seqDA = new SequenceDA();
                return seqDA.MaintenanceRequisitionControlID_Generate();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Vehicle Assignment
        public string VehicleAssignmentControlID_Generate()
        {
            try
            {
                SequenceDA seqDA = new SequenceDA();
                return seqDA.VehicleAssignmentControlID_Generate();
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        ////Stags Number COC
        //public string StagsNumberCOC_Generate()
        //{
        //    try
        //    {
        //        SequenceDA seqDA = new SequenceDA();
        //        return seqDA.StagsNumberCOC_Generate();
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ////Application Number
        //public string ApplicationNumber_Generate(int intIssuanceType)
        //{
        //    try
        //    {
        //        sequenceDA seqDA = new sequenceDA();
        //        return seqDA.ApplicationNumber_Generate(intIssuanceType);
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}
