using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.Security.Cryptography;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf.fonts;

using BusinessObject;
using BusinessLogic;
using DataAccess;

namespace POSActiv8.Classes
{
    public class baseclass : System.Web.UI.Page
    {
        UserProfilesBL upBL = new UserProfilesBL();
        //usermatrixBL umBL = new usermatrixBL();

        public string Encrypt(string stringToEncrypt)
        {
            try
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                byte[] rgbIV = { 0x21, 0x43, 0x56, 0x87, 0x10, 0xfd, 0xea, 0x1c };
                byte[] key = { };

                key = System.Text.Encoding.UTF8.GetBytes("A0D1nX0Q");
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, rgbIV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }

            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string Decrypt(string EncryptedText)
        {
            try
            {
                EncryptedText = EncryptedText.Replace(" ", "+");
                byte[] inputByteArray = new byte[EncryptedText.Length + 1];
                byte[] rgbIV = { 0x21, 0x43, 0x56, 0x87, 0x10, 0xfd, 0xea, 0x1c };
                byte[] key = { };
                int mod = EncryptedText.Length % 4;

                if (mod > 0)
                {
                    EncryptedText += new string('=', 4 - mod);
                }

                key = System.Text.Encoding.UTF8.GetBytes("A0D1nX0Q");
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(EncryptedText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, rgbIV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }

            catch (Exception e)
            {
                return e.Message;
            }
        }

        public void Alert(string strMessage)
        {
            //String strScript = "alert('" + strMessage + "');";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "MyScript", strScript, true);

            string omsg = string.Format("<script>alert('{0}');</script>", strMessage);
            HttpContext.Current.Response.Write(omsg);
        }

        public string ComputeHash(string input, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }

        public string GetMethodType(string productType)
        {
            string method = string.Empty;
            string[] LpgProducts = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

            if (productType == "C")
            {
                method = "A";
            }
            //else if (productType == "B" || productType == "W")
            //{
            //    method = "B";
            //}
            else if (productType == "S")
            {
                method = "C";
            }
            else if (productType == "G")
            {
                method = "G";
            }
            else if (LpgProducts.Contains(productType))
            {
                method = "P";
            }
            else if (productType == "L")
            {
                method = "L"; // refer to table53e and 54e
            }
            else if (productType == "E")
            {
                method = "E"; // refer to table53e and 54e
            }
            else if (productType == "Q")
            {
                method = "Q"; // refer to table53e and 54e
            }
            else
            {
                method = "B";
            }

            return method;
        }

        public void UserInformation(string strUserID, int intUserLevel, int intUserRole)
        {
            strUserID = HttpContext.Current.Request.QueryString["userid"];

            if (strUserID == null || strUserID == string.Empty)
            {
                HttpContext.Current.Response.Redirect("Error.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "&cmd=" + "1");
            }

            else
            {
                //User Profiles
                using (SqlDataReader drUSERPROFILES = upBL.UserProfiles_View(strUserID, string.Empty))
                {
                    if (!drUSERPROFILES.HasRows)
                    {
                        HttpContext.Current.Response.Redirect("Error.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "&cmd=" + "2");
                    }
                }
            }
        }

        public void UserAccess(string strUserID, int intUserLevel, int intUserRole, int intSystemMenus)
        {
            ////User Matrix //Check if user has view access to the page
            //using (SqlDataReader drUSERMATRIX = umBL.UserMatrix_AccessRights(intUserLevel, intUserRole, intSystemMenus))
            //{
            //    if (!drUSERMATRIX.HasRows)
            //    {
            //        HttpContext.Current.Response.Redirect("error.aspx?userid=" + strUserID + "&userlevel=" + intUserLevel + "&userrole=" + intUserRole + "&cmd=" + "3" + "&pagename=" + Path.GetFileName(HttpContext.Current.Request.PhysicalPath).ToString().ToUpper().Replace(".ASPX", string.Empty));
            //    }
            //}
        }

        public bool ButtonControls(int intUserLevel, int intUserRole, int intSystemMenus, string strAction)
        {
            bool accessrights = false;

            ////User Matrix //Check if user access has button controls
            //using (SqlDataReader drUSERMATRIX = umBL.UserMatrix_AccessRights(intUserLevel, intUserRole, intSystemMenus))
            //{
            //    if (drUSERMATRIX.Read())
            //    {
            //        if (strAction == "View" && drUSERMATRIX["view_access"].ToString() == "1")
            //        {
            //            accessrights = true;
            //        }

            //        else if (strAction == "Create" && drUSERMATRIX["view_access"].ToString() == "1" && drUSERMATRIX["create_access"].ToString() == "1")
            //        {
            //            accessrights = true;
            //        }

            //        else if (strAction == "Update" && drUSERMATRIX["view_access"].ToString() == "1" && drUSERMATRIX["update_access"].ToString() == "1")
            //        {
            //            accessrights = true;
            //        }

            //        else if (strAction == "Upload" && drUSERMATRIX["view_access"].ToString() == "1" && drUSERMATRIX["upload_access"].ToString() == "1")
            //        {
            //            accessrights = true;
            //        }

            //        else if (strAction == "Export" && drUSERMATRIX["view_access"].ToString() == "1" && drUSERMATRIX["export_access"].ToString() == "1")
            //        {
            //            accessrights = true;
            //        }

            //        else
            //        {
            //            accessrights = false;
            //        }
            //    }
            //}

            return accessrights;
        }

        public string ReportsGeneratedPath(int intCMD)
        {
            //Reports Generated Path
            System.Data.DataSet dsREPORTPATH = new System.Data.DataSet();
            dsREPORTPATH = DBHelper.GetData("SELECT ParameterCode, ParameterValue, ParameterDescription FROM SystemParameters WHERE ParameterCode = '5' AND ParameterValue = '" + intCMD + "'");

            var reportpath = string.Empty;

            if (dsREPORTPATH.Tables[0].Rows.Count > 0)
            {
                reportpath = dsREPORTPATH.Tables[0].Rows[0]["ParameterDescription"].ToString();
            }

            return reportpath;
        }

        public string FilesUploadedPath(int intCMD)
        {
            //Reports Generated Path
            System.Data.DataSet dsFILESUPLOADEDPATH = new System.Data.DataSet();
            dsFILESUPLOADEDPATH = DBHelper.GetData("SELECT parameter_value, parameter_description FROM vwSystemParameters WHERE parameter_code = '25' AND parameter_value = '" + intCMD + "'");

            var filesuploadedpath = string.Empty;

            if (dsFILESUPLOADEDPATH.Tables[0].Rows.Count > 0)
            {
                filesuploadedpath = dsFILESUPLOADEDPATH.Tables[0].Rows[0]["parameter_description"].ToString();
            }

            return filesuploadedpath;
        }

        public string VehicleInventoryAttachmentsPath(string strAssetTag)
        {
            var fileattachmentpath = HttpContext.Current.Server.MapPath(string.Format("~/FileAttachments/VehicleInventory/") + strAssetTag);

            return fileattachmentpath;
        }

        public string MaintenanceRequisitionFileAttachmentsPath(string strControlNumber)
        {
            ////Reports Generated Path
            //System.Data.DataSet dsFILESUPLOADEDPATH = new System.Data.DataSet();
            //dsFILESUPLOADEDPATH = DBHelper.GetData("SELECT parameter_value, parameter_description FROM vwSystemParameters WHERE parameter_code = '25' AND parameter_value = '" + intCMD + "'");

            //var filesuploadedpath = string.Empty;

            //if (dsFILESUPLOADEDPATH.Tables[0].Rows.Count > 0)
            //{
            //    filesuploadedpath = dsFILESUPLOADEDPATH.Tables[0].Rows[0]["parameter_description"].ToString();
            //}

            var fileattachmentpath = HttpContext.Current.Server.MapPath(string.Format("~/FileAttachments/MaintenanceRequisition") + strControlNumber);

            return fileattachmentpath;
        }

        public string InvoiceJournalAttachmentsPath(string strControlNumber)
        {
            ////Reports Generated Path
            //System.Data.DataSet dsFILESUPLOADEDPATH = new System.Data.DataSet();
            //dsFILESUPLOADEDPATH = DBHelper.GetData("SELECT parameter_value, parameter_description FROM vwSystemParameters WHERE parameter_code = '25' AND parameter_value = '" + intCMD + "'");

            //var filesuploadedpath = string.Empty;

            //if (dsFILESUPLOADEDPATH.Tables[0].Rows.Count > 0)
            //{
            //    filesuploadedpath = dsFILESUPLOADEDPATH.Tables[0].Rows[0]["parameter_description"].ToString();
            //}

            var fileattachmentpath = HttpContext.Current.Server.MapPath(string.Format("~/FileAttachments/InvoiceJournal") + strControlNumber);

            return fileattachmentpath;
        }

        public decimal TemperatureConversion(int intConversionType, decimal decTankTemperature)
        {
            decimal decTemperature = 0;

            if (intConversionType == 1) //Celsius to Fahrenheit
            {
                decTemperature = Math.Round((decTankTemperature * 9 / 5) + 32, 0);
            }

            else //Fahrenheit to Celsius
            {

            }

            return decTemperature;
        }

        public string UserProfilesPath(int intCMD)
        {
            //User Profiles Path
            System.Data.DataSet dsUSERPROFILESPATH = new System.Data.DataSet();
            dsUSERPROFILESPATH = DBHelper.GetData("SELECT parameter_value, parameter_description FROM vwSystemParameters WHERE parameter_code = '36' AND parameter_value = '" + intCMD + "'");

            var userprofilespath = string.Empty;

            if (dsUSERPROFILESPATH.Tables[0].Rows.Count > 0)
            {
                userprofilespath = dsUSERPROFILESPATH.Tables[0].Rows[0]["parameter_description"].ToString();
            }

            return userprofilespath;
        }

        public PdfPCell getCell(String text, int alignment)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text));
            cell.Padding = 0;
            cell.HorizontalAlignment = alignment;
            cell.Border = PdfPCell.NO_BORDER;
            return cell;
        }

        //public decimal VCFLiters(string tanknumber, string tanktemperature, string tankdensity, string vaporpressure)
        //{
        //    decimal vcfliters = 0;

        //    //Tank Details
        //    using (SqlDataReader drTANKDETAILS = tanksBL.Tanks_View(1, tanknumber))
        //    {
        //        if (drTANKDETAILS.Read())
        //        {
        //            //LPG and PROPYLENE products
        //            if (drTANKDETAILS["product_description"].ToString() == "LPG" || drTANKDETAILS["product_description"].ToString() == "PROPYLENE")
        //            {
        //                decimal vcflitersfrom = 0;
        //                decimal vcflitersto = 0;

        //                //Check if inputted tank temperature is out of range in ASTM Table34 (-20 to 120)
        //                System.Data.DataSet dsASTMTABLE34Temperature = new System.Data.DataSet();
        //                dsASTMTABLE34Temperature = DBHelper.GetData("SELECT distinct tank_temperature from ASTMTable34 WHERE tank_temperature = '" + TemperatureConversion(1, Convert.ToDecimal(tanktemperature)) + "' ");

        //                if (dsASTMTABLE34Temperature.Tables[0].Rows.Count == 0)
        //                {
        //                    //this.lblValidationTankValues.Text = "Tank temperature &deg;c is out of range (Observed temperature &deg;f is from -20 to 120 only).";
        //                    //this.lblValidationTankValues.Attributes["style"] = "color:Red;";
        //                    //this.txtTankTemperature.Focus();
        //                    //this.popuptankvalues.Show();
        //                }

        //                else
        //                {
        //                    //Check if actual density is out of range in ASTM Table34, then use interpolation method to get the value of the VCF
        //                    System.Data.DataSet dsASTMTABLE34 = new System.Data.DataSet();
        //                    dsASTMTABLE34 = DBHelper.GetData("SELECT vcf_value from ASTMTable34 WHERE tank_temperature = '" + TemperatureConversion(1, Convert.ToDecimal(tanktemperature)) + "' AND '" + ((Convert.ToDecimal(tankdensity)) / 1000) + "' BETWEEN density_from AND density_to");

        //                    if (dsASTMTABLE34.Tables[0].Rows.Count == 0)
        //                    {
        //                        //Density From
        //                        decimal densityfrom = Decimal.Truncate(Convert.ToDecimal(tankdensity)); //Remove decimal
        //                        densityfrom = (densityfrom / 1000);

        //                        using (SqlDataReader drDENSITYFROM = tankinvBL.Table34VCF_View(TemperatureConversion(1, Convert.ToDecimal(tanktemperature)), densityfrom))
        //                        {
        //                            if (drDENSITYFROM.Read())
        //                            {
        //                                vcflitersfrom = Convert.ToDecimal(drDENSITYFROM["vcf_value"]);
        //                            }
        //                        }

        //                        //Density To
        //                        decimal densityto = (densityfrom + Convert.ToDecimal(0.010));
        //                        using (SqlDataReader drDENSITYTO = tankinvBL.Table34VCF_View(TemperatureConversion(1, Convert.ToDecimal(tanktemperature)), densityto))
        //                        {
        //                            if (drDENSITYTO.Read())
        //                            {
        //                                vcflitersto = Convert.ToDecimal(drDENSITYTO["vcf_value"]);
        //                            }
        //                        }

        //                        //Interpolation Formula
        //                        vcfliters = ((vcflitersfrom - vcflitersto) / ((densityfrom * 1000) - (densityto * 1000)));
        //                        vcfliters = (vcfliters * (Convert.ToDecimal(tankdensity) - (densityto * 1000)));
        //                        vcfliters = vcfliters + vcflitersto;
        //                    }

        //                    dsASTMTABLE34.Clear();
        //                }

        //                dsASTMTABLE34Temperature.Clear();
        //            }

        //            else //Other Products
        //            {
        //                string[] strEvenNumbers = new string[] { "0", "2", "4", "6", "8" };
        //                string[] strDecimalNumbers = new string[] { "25", "50", "75" };
        //                string[] strTemperaturesValidEnding = new string[] { "00", "25", "50", "75" };

        //                //Get VCF values using calculator program (ISTOCK.DLMC)
        //                dlmcparamobject.product = drTANKDETAILS["product_code"].ToString();
        //                dlmcparamobject.productType = drTANKDETAILS["product_type"].ToString();
        //                dlmcparamobject.method = GetMethodType(drTANKDETAILS["product_type"].ToString());
        //                dlmcparamobject.volume = double.Parse("1000");
        //                dlmcparamobject.baseTemp = 15;
        //                dlmcparamobject.sampleTemp = 15;
        //                dlmcparamobject.tankTemp = double.Parse(tanktemperature);
        //                dlmcparamobject.pressure = double.Parse(vaporpressure);

        //                //Check if inputted density is below 654, then use extrapolation formula //Except BTX, LPG, and PROPYLENE tanks
        //                if ((drTANKDETAILS["product_code"].ToString() != "2491" || drTANKDETAILS["product_code"].ToString() != "2492" || drTANKDETAILS["product_code"].ToString() != "2271") && float.Parse(tankdensity) < 654)
        //                {
        //                    //Start at 656 and 654 density
        //                    double densityfrom = 656;
        //                    double densityto = 654;

        //                    dlmcparamobject.density = double.Parse(densityfrom.ToString());
        //                    dlmcobject = dlmccalculate.DLMCCalculate(dlmcparamobject);
        //                    decimal vcf_from = Math.Round(Convert.ToDecimal(dlmcobject.vcfAt15.ToString()), 4); //VCF rounded to 4 decimal places

        //                    //To compensate 0.0001 difference in temp 30 between density 658 and 656 computation in DLL vs. Table54 hardcopy
        //                    if ((tanktemperature == "30" || tanktemperature == "30.00") && (densityfrom.ToString() == "658" || densityfrom.ToString() == "656"))
        //                    {
        //                        vcf_from = (vcf_from + Convert.ToDecimal("0.0001"));
        //                    }

        //                    dlmcparamobject.density = double.Parse(densityto.ToString());
        //                    dlmcobject = dlmccalculate.DLMCCalculate(dlmcparamobject);
        //                    decimal vcf_to = Math.Round(Convert.ToDecimal(dlmcobject.vcfAt15.ToString()), 4); //VCF rounded to 4 decimal places

        //                    //03102022 density with decimal
        //                    decimal intCount = Decimal.Truncate(Convert.ToDecimal(tankdensity)); //Convert.ToInt32(Math.Round(Convert.ToDecimal(this.txtDensity15.Text), 3));
        //                    int intCounter;

        //                    //Loop into density by 2s; start at density 652
        //                    for (intCounter = 652; intCounter >= intCount; intCounter -= 2)
        //                    {
        //                        //Extrapolation Formula
        //                        vcfliters = (vcf_to - (Convert.ToDecimal(densityto) - intCounter) / (Convert.ToDecimal(densityfrom) - Convert.ToDecimal(densityto)) * (vcf_from - vcf_to));
        //                        vcf_from = vcf_to;
        //                        vcf_to = vcfliters;
        //                        densityfrom = densityto;
        //                        densityto = intCounter;
        //                    }

        //                    //If inputted density is not divisible by 2 or have decimal 
        //                    if (strEvenNumbers.Contains(tankdensity.Substring(tankdensity.ToString().Length - 1)) == false)
        //                    {
        //                        vcfliters = (vcf_to - (Convert.ToDecimal(densityto) - Convert.ToDecimal(tankdensity)) / (Convert.ToDecimal(densityfrom) - Convert.ToDecimal(densityto)) * (vcf_from - vcf_to));
        //                        vcfliters = Math.Round(vcfliters, 4);
        //                    }
        //                }

        //                else
        //                {
        //                    //Benzene, Toluene, and Mixed Xylene (BTX) products; use 5 decimal places
        //                    if (drTANKDETAILS["product_code"].ToString() == "2491" || drTANKDETAILS["product_code"].ToString() == "2492" || drTANKDETAILS["product_code"].ToString() == "2271")
        //                    {
        //                        dlmcparamobject.density = double.Parse(tankdensity);
        //                        dlmcobject = dlmccalculate.DLMCCalculate(dlmcparamobject);
        //                        vcfliters = Convert.ToDecimal(dlmcobject.vcfAt15.ToString());
        //                    }

        //                    else
        //                    {
        //                        decimal tankdensitya = Math.Round(Convert.ToDecimal(tankdensity), 0);
        //                        decimal densityfrom = Decimal.Truncate(Convert.ToDecimal(tankdensity));

        //                        //Check if last digit of actual density is an even number
        //                        if (strEvenNumbers.Contains(densityfrom.ToString().Substring(densityfrom.ToString().Length - 1)))
        //                        {
        //                            densityfrom = (densityfrom + 2);
        //                        }

        //                        else
        //                        {
        //                            densityfrom = (densityfrom - 1) + 2;
        //                        }

        //                        decimal densityto = 0;

        //                        //Check if last digit of tank temperature is divisible 0.25
        //                        decimal d25 = Convert.ToDecimal("0.25");
        //                        //if (strTemperaturesValidEnding.Contains(this.txtTankTemperature.Text.Substring(this.txtTankTemperature.Text.Length - 2)))
        //                        //{
        //                        if (Convert.ToDecimal(tanktemperature) % d25 == 0)
        //                        {
        //                            dlmcparamobject.density = double.Parse(densityfrom.ToString());
        //                            dlmcobject = dlmccalculate.DLMCCalculate(dlmcparamobject);
        //                            decimal vcflitersfrom = Math.Round(Convert.ToDecimal(dlmcobject.vcfAt15.ToString()), 4);

        //                            densityto = densityfrom - 2;
        //                            dlmcparamobject.density = double.Parse(densityto.ToString());
        //                            dlmcobject = dlmccalculate.DLMCCalculate(dlmcparamobject);
        //                            decimal vcflitersto = Math.Round(Convert.ToDecimal(dlmcobject.vcfAt15.ToString()), 4);

        //                            vcfliters = ((vcflitersfrom - vcflitersto) / (densityfrom - densityto));
        //                            //vcfliters = (vcfliters * (Convert.ToDecimal(this.txtDensity15.Text) - densityto));
        //                            vcfliters = (vcfliters * (tankdensitya - densityto));
        //                            vcfliters = vcfliters + vcflitersto;
        //                            vcfliters = Math.Round(vcfliters, 4, MidpointRounding.AwayFromZero);
        //                        }

        //                        else
        //                        {
        //                            //Interpolation needed in Temperature and Density
        //                            decimal tanktempfrom = 0;
        //                            decimal tanktempto = 0;

        //                            //Check if last digit of tank temperature is divisible 0.25
        //                            if (strDecimalNumbers.Contains(tanktemperature.ToString().Substring(tanktemperature.ToString().Length - 2)) == false)
        //                            {
        //                                tanktempfrom = Math.Round(Convert.ToDecimal(tanktemperature), 0);
        //                                tanktempto = (tanktempfrom - Convert.ToDecimal(0.25));
        //                            }

        //                            //Tank Temp From
        //                            dlmcparamobject.tankTemp = double.Parse(tanktempfrom.ToString());
        //                            dlmcparamobject.density = double.Parse(densityfrom.ToString());
        //                            dlmcobject = dlmccalculate.DLMCCalculate(dlmcparamobject);
        //                            decimal vcflitersfrom1 = Math.Round(Convert.ToDecimal(dlmcobject.vcfAt15.ToString()), 4);

        //                            densityto = densityfrom - 2;
        //                            dlmcparamobject.density = double.Parse(densityto.ToString());
        //                            dlmcobject = dlmccalculate.DLMCCalculate(dlmcparamobject);
        //                            decimal vcflitersto1 = Math.Round(Convert.ToDecimal(dlmcobject.vcfAt15.ToString()), 4);

        //                            vcfliters = ((vcflitersfrom1 - vcflitersto1) / (densityfrom - densityto));
        //                            vcfliters = (vcfliters * (Convert.ToDecimal(tankdensity) - densityto));
        //                            vcfliters = vcfliters + vcflitersfrom1;
        //                            vcfliters = Math.Round(vcfliters, 4, MidpointRounding.AwayFromZero);
        //                            var test1 = vcfliters;

        //                            //Tank Temp To
        //                            dlmcparamobject.tankTemp = double.Parse(tanktempto.ToString());
        //                            dlmcparamobject.density = double.Parse(densityfrom.ToString());
        //                            dlmcobject = dlmccalculate.DLMCCalculate(dlmcparamobject);
        //                            decimal vcflitersfrom2 = Math.Round(Convert.ToDecimal(dlmcobject.vcfAt15.ToString()), 4);

        //                            densityto = densityfrom - 2;
        //                            dlmcparamobject.density = double.Parse(densityto.ToString());
        //                            dlmcobject = dlmccalculate.DLMCCalculate(dlmcparamobject);
        //                            decimal vcflitersto2 = Math.Round(Convert.ToDecimal(dlmcobject.vcfAt15.ToString()), 4);

        //                            vcfliters = 0;
        //                            vcfliters = ((vcflitersfrom2 - vcflitersto2) / (densityfrom - densityto));
        //                            vcfliters = (vcfliters * (Convert.ToDecimal(tankdensity) - densityto));
        //                            vcfliters = vcfliters + vcflitersto2;
        //                            vcfliters = Math.Round(vcfliters, 4, MidpointRounding.AwayFromZero);
        //                            var test2 = vcfliters;

        //                            //
        //                            vcfliters = 0;
        //                            vcfliters = ((test2 - test1) / (tanktempto - tanktempfrom));
        //                            vcfliters = (vcfliters * (Convert.ToDecimal(tanktemperature) - tanktempfrom));
        //                            vcfliters = vcfliters + test1;
        //                            vcfliters = Math.Round(vcfliters, 4, MidpointRounding.AwayFromZero);
        //                            var test3 = vcfliters;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        //To handle unverified operation //For LPG and Propylene products
        //        if (vcfliters.ToString() == "NaN")
        //        {
        //            vcfliters = 0;
        //        }
        //    }

        //    return vcfliters;
        //}

    }
}