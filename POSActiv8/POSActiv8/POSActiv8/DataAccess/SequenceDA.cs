using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using BusinessObject;

namespace DataAccess
{
    public class SequenceDA
    {
        //Invoices
        public string ControlNumber_Generate(int intSequenceID)
        {
            try
            {
                //Sequence Parameters
                System.Data.DataSet dsSEQ = new System.Data.DataSet();
                dsSEQ = DBHelper.GetData("SELECT SequenceID, SequenceDescription, SequenceCode, SequenceNumber FROM SequenceParameters WHERE SequenceID = '" + intSequenceID  + "'");

                int intSeqNum = Convert.ToInt32(dsSEQ.Tables[0].Rows[0]["SequenceNumber"]);
                string strSequenceCode = dsSEQ.Tables[0].Rows[0]["SequenceCode"].ToString();

                //Generate new sequence
                int intCodeLength = 10;
                int intSeq = 1;

                int intNewSeq = Convert.ToInt32(intSeqNum) + Convert.ToInt32(intSeq);
                string strSequence = "";

                //Determine the number of zeroes to include based on the length of intNewSeq
                int intSeqCounter = intCodeLength - Convert.ToString(intNewSeq).Length;

                for (int i = 0; i < intSeqCounter; i++)
                {
                    strSequence = strSequence + "0";
                }

                //New sequence number
                strSequence = strSequenceCode + strSequence + intNewSeq;
                string strControlID = strSequence;

                dsSEQ.Clear();

                //Update new sequence number
                System.Data.DataSet dsUPDATE = new System.Data.DataSet();
                dsUPDATE = DBHelper.GetData("UPDATE SequenceParameters SET SequenceNumber = SequenceNumber + 1 WHERE SequenceID = '" + intSequenceID + "'");
                dsUPDATE.Clear();

                return (strControlID);
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
                //Sequence Parameters
                System.Data.DataSet dsSEQ = new System.Data.DataSet();
                dsSEQ = DBHelper.GetData("SELECT SequenceID, SequenceDescription, SequenceCode, SequenceNumber FROM SequenceParameters WHERE SequenceID = 1");

                int intSeqNum = Convert.ToInt32(dsSEQ.Tables[0].Rows[0]["SequenceNumber"]);

                //Generate new sequence
                int intCodeLength = 10;
                int intSeq = 1;

                int intNewSeq = Convert.ToInt32(intSeqNum) + Convert.ToInt32(intSeq);
                string strSequence = "";

                //Determine the number of zeroes to include based on the length of intNewSeq
                int intSeqCounter = intCodeLength - Convert.ToString(intNewSeq).Length;

                for (int i = 0; i < intSeqCounter; i++)
                {
                    strSequence = strSequence + "0";
                }

                //New sequence number
                strSequence = strSequence + intNewSeq;
                string strControlID = strSequence;

                dsSEQ.Clear();

                //Update new sequence number
                System.Data.DataSet dsUPDATE = new System.Data.DataSet();
                dsUPDATE = DBHelper.GetData("UPDATE SequenceParameters SET SequenceNumber = SequenceNumber + 1 WHERE SequenceID = 1");
                dsUPDATE.Clear();

                return (strControlID);
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
                //Sequence Parameters
                System.Data.DataSet dsSEQ = new System.Data.DataSet();
                dsSEQ = DBHelper.GetData("SELECT SequenceID, SequenceDescription, SequenceCode, SequenceNumber FROM SequenceParameters WHERE SequenceID = 2");

                int intSeqNum = Convert.ToInt32(dsSEQ.Tables[0].Rows[0]["SequenceNumber"]);
                string strSequenceCode = dsSEQ.Tables[0].Rows[0]["SequenceCode"].ToString();

                //Generate new sequence
                int intCodeLength = 10;
                int intSeq = 1;

                int intNewSeq = Convert.ToInt32(intSeqNum) + Convert.ToInt32(intSeq);
                string strSequence = "";

                //Determine the number of zeroes to include based on the length of intNewSeq
                int intSeqCounter = intCodeLength - Convert.ToString(intNewSeq).Length;

                for (int i = 0; i < intSeqCounter; i++)
                {
                    strSequence = strSequence + "0";
                }

                //New sequence number
                strSequence = strSequenceCode + strSequence + intNewSeq;
                string strControlID = strSequence;

                dsSEQ.Clear();

                //Update new sequence number
                System.Data.DataSet dsUPDATE = new System.Data.DataSet();
                dsUPDATE = DBHelper.GetData("UPDATE SequenceParameters SET SequenceNumber = SequenceNumber + 1 WHERE SequenceID = 2");
                dsUPDATE.Clear();

                return (strControlID);
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
                //Sequence Parameters
                System.Data.DataSet dsSEQ = new System.Data.DataSet();
                dsSEQ = DBHelper.GetData("SELECT SequenceID, SequenceDescription, SequenceCode, SequenceNumber FROM SequenceParameters WHERE SequenceID = 3");

                int intSeqNum = Convert.ToInt32(dsSEQ.Tables[0].Rows[0]["SequenceNumber"]);
                string strSequenceCode = dsSEQ.Tables[0].Rows[0]["SequenceCode"].ToString();

                //Generate new sequence
                int intCodeLength = 10;
                int intSeq = 1;

                int intNewSeq = Convert.ToInt32(intSeqNum) + Convert.ToInt32(intSeq);
                string strSequence = "";

                //Determine the number of zeroes to include based on the length of intNewSeq
                int intSeqCounter = intCodeLength - Convert.ToString(intNewSeq).Length;

                for (int i = 0; i < intSeqCounter; i++)
                {
                    strSequence = strSequence + "0";
                }

                //New sequence number
                strSequence = strSequenceCode + strSequence + intNewSeq;
                string strControlID = strSequence;

                dsSEQ.Clear();

                //Update new sequence number
                System.Data.DataSet dsUPDATE = new System.Data.DataSet();
                dsUPDATE = DBHelper.GetData("UPDATE SequenceParameters SET SequenceNumber = SequenceNumber + 1 WHERE SequenceID = 3");
                dsUPDATE.Clear();

                return (strControlID);
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
                //Sequence Parameters
                System.Data.DataSet dsSEQ = new System.Data.DataSet();
                dsSEQ = DBHelper.GetData("SELECT SequenceID, SequenceDescription, SequenceCode, SequenceNumber FROM SequenceParameters WHERE SequenceID = 4");

                int intSeqNum = Convert.ToInt32(dsSEQ.Tables[0].Rows[0]["SequenceNumber"]);
                string strSequenceCode = dsSEQ.Tables[0].Rows[0]["SequenceCode"].ToString();

                //Generate new sequence
                int intCodeLength = 10;
                int intSeq = 1;

                int intNewSeq = Convert.ToInt32(intSeqNum) + Convert.ToInt32(intSeq);
                string strSequence = "";

                //Determine the number of zeroes to include based on the length of intNewSeq
                int intSeqCounter = intCodeLength - Convert.ToString(intNewSeq).Length;

                for (int i = 0; i < intSeqCounter; i++)
                {
                    strSequence = strSequence + "0";
                }

                //New sequence number
                strSequence = strSequenceCode + strSequence + intNewSeq;
                string strControlID = strSequence;

                dsSEQ.Clear();

                //Update new sequence number
                System.Data.DataSet dsUPDATE = new System.Data.DataSet();
                dsUPDATE = DBHelper.GetData("UPDATE SequenceParameters SET SequenceNumber = SequenceNumber + 1 WHERE SequenceID = 4");
                dsUPDATE.Clear();

                return (strControlID);
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
        //        //Sequence Parameters
        //        System.Data.DataSet dsSEQ = new System.Data.DataSet();
        //        dsSEQ = DBHelper.GetData("SELECT sequence_id, sequence_description, sequence_code, sequence_number FROM SequenceParameters WHERE sequence_id = 2");

        //        int intSeqNum = Convert.ToInt32(dsSEQ.Tables[0].Rows[0]["sequence_number"]);

        //        //Generate new sequence
        //        int intCodeLength = 10;
        //        int intSeq = 1;

        //        int intNewSeq = Convert.ToInt32(intSeqNum) + Convert.ToInt32(intSeq);
        //        string strSequence = "";

        //        //Determine the number of zeroes to include based on the length of intNewSeq
        //        int intSeqCounter = intCodeLength - Convert.ToString(intNewSeq).Length;

        //        for (int i = 0; i < intSeqCounter; i++)
        //        {
        //            strSequence = strSequence + "0";
        //        }

        //        //New sequence number
        //        strSequence = strSequence + intNewSeq;
        //        string strControlID = strSequence;

        //        dsSEQ.Clear();

        //        //Update new sequence number
        //        System.Data.DataSet dsUPDATE = new System.Data.DataSet();
        //        dsUPDATE = DBHelper.GetData("UPDATE SequenceParameters SET sequence_number = sequence_number + 1 WHERE sequence_id = 2");
        //        dsUPDATE.Clear();

        //        return (strControlID);
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
        //        //Sequence number
        //        System.Data.DataSet dsSEQ = new System.Data.DataSet();
        //        dsSEQ = DBHelper.GetDataRAPID("SELECT issuance_type, sequence_id, sequence_description, sequence_code, sequence_number FROM sequenceparameters WHERE issuance_type = '" + intIssuanceType + "' AND sequence_id = 2");

        //        string strSeqCode = dsSEQ.Tables[0].Rows[0]["sequence_code"].ToString();
        //        int itnSeqNum = Convert.ToInt32(dsSEQ.Tables[0].Rows[0]["sequence_number"]);

        //        //Generate new sequence
        //        int intCodeLength = 4;
        //        int intSeq = 1;

        //        int intNewSeq = Convert.ToInt32(itnSeqNum) + Convert.ToInt32(intSeq);
        //        string strSequence = "";

        //        //Determine the number of zeroes to include based on the length of intNewSeq
        //        int intSeqCounter = intCodeLength - Convert.ToString(intNewSeq).Length;

        //        for (int i = 0; i < intSeqCounter; i++)
        //        {
        //            strSequence = strSequence + "0";
        //        }

        //        //New sequence number
        //        //strSequence = strSeqCode + "-" + strSequence + intNewSeq;

        //        //For RLC
        //        strSequence = strSequence + intNewSeq;

        //        string strApplicationNumber = strSequence;
        //        dsSEQ.Clear();

        //        //Update new sequence number
        //        System.Data.DataSet dsUPDATE = new System.Data.DataSet();
        //        dsUPDATE = DBHelper.GetDataRAPID("UPDATE sequenceparameters SET sequence_number = sequence_number + 1 WHERE issuance_type = '" + intIssuanceType + "' AND sequence_id = 2");
        //        dsUPDATE.Clear();

        //        return (strApplicationNumber);
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}

