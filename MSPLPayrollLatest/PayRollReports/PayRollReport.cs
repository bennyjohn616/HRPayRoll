using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SQLDBOperation;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace PayRollReports
{
    public class PayRollReport
    {

        /// Modified By : Pandiyan 
        /// Modified On : 16th April 2015
        public string DoPDFJobInstalled(string StartDate, string EndDate, string ReqDateFrom, string ReqDateTo, string ClientId)
        {
            try
            {
                string rdlcName = string.Empty;
                PDFGenerator PDFGenerator = new PDFGenerator();
                DataTable JobInstalled = GetJobInstalled(StartDate, EndDate, ReqDateFrom, ReqDateTo, ClientId);
                rdlcName = "JobsInstalled.rdlc";
                return PDFGenerator.DownloadReport(JobInstalled, "dsJobInstalled", rdlcName, "JobInstalledList");

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// Description : Find tracking permit for the client
        /// Created by  : Pandiyan
        /// Created on  : April 28 2015
        /// </summary>
        public string DoPDFCommonForReportModule(string ReportType, string InstallDateFrom, string InstallDateTo, string PermitType, string Status, string CustomerName, string HomeOwnerName, string CustomerJobNo, string SoftwareJobNo, string Town, string State)
        {
            try
            {
                string rdlcName = string.Empty;
                string dataSet = string.Empty;
                string ReportName = string.Empty;
                PDFGenerator PDFGenerator = new PDFGenerator();
                DataTable TrackingPermitList = GetCommonReportModule(ReportType, InstallDateFrom, InstallDateTo, PermitType, Status, CustomerName, HomeOwnerName, CustomerJobNo, SoftwareJobNo, Town, State);

                if (TrackingPermitList.Rows.Count > 0)
                {

                    if (ReportType.Equals("TrackHours"))
                    {
                        rdlcName = "TrackingPermits.rdlc";
                        dataSet = "dsTrackingPermit";
                        ReportName = "TrackPermitsForEachClient";
                    }
                    else if (ReportType.Equals("JobInstalled"))
                    {
                        rdlcName = "JobsInstalled.rdlc";
                        dataSet = "dsJobInstalled";
                        ReportName = "ViewPermitsByInstallDate";
                    }
                    else if (ReportType.Equals("PermitReceivedAndCompleted"))
                    {
                        rdlcName = "PermitStatusInformation.rdlc";
                        dataSet = "dsPermitStatus";
                        ReportName = "TrackAverageCompletionTime";
                    }
                    else if (ReportType.Equals("TrackTimeFrame"))
                    {
                        rdlcName = "TrackTimeFrame.rdlc";
                        dataSet = "dsTrackTimeFrame";
                        ReportName = "ViewPermitsExceedingCompletion";
                    }
                    else
                    {
                        rdlcName = "TrackInstallDates.rdlc";
                        dataSet = "dsTrackInstall";
                        ReportName = "TrackInstallDates";
                    }
                    return PDFGenerator.DownloadReport(TrackingPermitList, dataSet, rdlcName, ReportName);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// Created By Pandiyan
        /// On 08-04-2015
        /// To Bring the Rdlc File 
        /// </summary>
        /// <returns></returns>
        public string DoPDFPrintAverageDays(string StartDate, string EndDate, string ReqDateFrom, string ReqDateTo, string ClientId)
        {
            string rdlcFileName = string.Empty;
            PDFGenerator pdfGenerator = new PDFGenerator();
            DataTable dt = GetAvgDaysDetails(StartDate, EndDate, ReqDateFrom, ReqDateTo, ClientId);
            rdlcFileName = "PermitStatusInformation.rdlc";
            return pdfGenerator.DownloadReport(dt, "dsPermitStatus", rdlcFileName, "PermitStatusInformations");
        }

        /// <summary>
        /// Description : Get Service Agent Conatct List Details
        /// Created by  : 
        /// Created on  : 10/March/2015
        /// </summary>
        /// <returns></returns>
        public string DoPDFServiceAgentContactList()
        {
            try
            {
                string rdlcName = string.Empty;
                PDFGenerator PDFGenerator = new PDFGenerator();
                DataTable ServiceAgentContactList = GetServiceAgent(0);
                rdlcName = "ServiceAgentContact.rdlc";
                return PDFGenerator.DownloadReport(ServiceAgentContactList, "dsServiceAgentContact", rdlcName, "ServiceAgentContactList");

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        /// <summary>
        /// Description : Get Service Agent Conatct List Details
        /// Created by  : 
        /// Created on  : March 23 2015
        /// </summary>
        /// <returns></returns>
        public string DoPDFClientInformation()
        {
            try
            {
                string rdlcName = string.Empty;
                PDFGenerator PDFGenerator = new PDFGenerator();
                DataTable ClietnInformation = GetClientInformation(0);
                rdlcName = "ClientInformation.rdlc";
                return PDFGenerator.DownloadReport(ClietnInformation, "dsClientInformation", rdlcName, "ClientInformations");

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }


        /// <summary>
        /// Created By Pandiyan
        /// On 06-04-2015
        /// for Get the Permit Details
        /// </summary>
        /// <returns></returns>
        public string DoPDFPermitDetails()
        {
            try
            {
                string rdlcFileName = string.Empty;
                PDFGenerator pdfgenerator = new PDFGenerator();
                DataTable dtPermitDetails = GetDoPDFPermitDetails();
                rdlcFileName = "PermitInformation.rdlc";
                return pdfgenerator.DownloadReport(dtPermitDetails, "dsPermitInformation", rdlcFileName, "PermitInformations");
            }
            catch (Exception e)
            {

                return e.ToString();
            }

        }

        /// <summary>
        /// Created By  : Pandiyan
        /// Created On  : 16 April 2015
        /// Description : Prepare Check 
        /// </summary>
        /// <returns></returns>
        public string DoPDFCheckPreparation()
        {
            try
            {
                string rdlcFileName = string.Empty;
                PDFGenerator pdfgenerator = new PDFGenerator();
                DataTable dtPermitDetails = GetDoPDFPermitDetails();
                rdlcFileName = "CheckPreparation.rdlc";
                return pdfgenerator.DownloadReport(dtPermitDetails, "dsPermitInformation", rdlcFileName, "Check");
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }


        /// <summary>
        /// Created By  : Pandiyan
        /// Created On  : 16 April 2015
        /// Description : Prepare Check 
        /// </summary>
        /// <returns></returns>
        public string DoPDFVoidCheckPreparation(int permitId)
        {
            try
            {
                string rdlcFileName = string.Empty;
                PDFGenerator pdfgenerator = new PDFGenerator();
                DataTable dtPermitDetails = GetDoPDFPrintCheck(permitId);
                rdlcFileName = "VoidCheckPreparation.rdlc";
                return pdfgenerator.DownloadReport(dtPermitDetails, "dsPermitInformation", rdlcFileName, "VoidCheck");
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        /// <summary>
        /// Created By Pandiyan 
        /// On 24th April 2015
        /// </summary>
        /// <returns></returns>
        public string DoPDFInvoiceDetails(string invoiceNumber, string clientNumber)
        {
            try
            {
                string rdlcFileName = string.Empty;
                PDFGenerator pdfgenerator = new PDFGenerator();
                DataTable dtInvoiceDetails = GetDoPDFInvoiceDetails(invoiceNumber, clientNumber);
                rdlcFileName = "InvoiceDetails.rdlc";
                return pdfgenerator.DownloadReport(dtInvoiceDetails, "dsInvoiceNumbers", rdlcFileName, "InvoiceDetails");
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// Description : To get service agent records 
        /// Created by  : 
        /// Created on  : 10/March/2015
        /// </summary>
        /// <returns></returns>
        private DataTable GetServiceAgent(int id)
        {
            DBOperation dbOperation = new DBOperation();
            SqlCommand sqlCommand = new SqlCommand("ServiceAgent_SelectAll");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            return dbOperation.GetTableData(sqlCommand);
        }


        /// <summary>
        /// Description : To get Client information details
        /// Created by  : 
        /// Created on  : March 23 2015
        /// </summary>
        /// <returns></returns>
        private DataTable GetClientInformation(int id)
        {
            DBOperation dbOperation = new DBOperation();
            SqlCommand sqlCommand = new SqlCommand("Client_Generate_Report");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            return dbOperation.GetTableData(sqlCommand);
        }

        /// <summary>
        /// Created By : Thereshan.K
        /// Created On : 29 April 2015
        /// </summary>
        /// <param name="permitId"></param>
        /// <returns></returns>
        public DataTable GetDoPDFPrintCheck(int permitId)
        {
            DBOperation dbOperation = new DBOperation();
            SqlCommand sqlCommand = new SqlCommand("Permit_Select");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@PermitId", permitId);
            return dbOperation.GetTableData(sqlCommand);

        }

        /// <summary>
        /// Created By Pandiyan 
        /// On 06-04-2015
        /// </summary>
        /// <returns></returns>
        public DataTable GetDoPDFPermitDetails()
        {
            DBOperation dbopertion = new DBOperation();
            SqlCommand cmd = new SqlCommand("SelectAll_Permit");
            cmd.CommandType = CommandType.StoredProcedure;
            return dbopertion.GetTableData(cmd);

        }
        /// <summary>
        /// Created By Pandiyan
        /// On 24th april 2015
        /// To Bring the Invoice Details
        /// </summary>
        /// <returns></returns>
        public DataTable GetDoPDFInvoiceDetails(string invoiceNumber, string clientNumber)
        {
            DBOperation dboperation = new DBOperation();
            SqlCommand cmd = new SqlCommand("Invoic_Select");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@InvoiceNumber", invoiceNumber);
            cmd.Parameters.AddWithValue("@ClientId", clientNumber);
            return dboperation.GetTableData(cmd);
        }

        /// <summary>
        /// Created By Pandiyan
        /// On 08-04-2015
        /// To Bring the Request and Complete Days
        /// </summary>
        /// <returns></returns>
        public DataTable GetAvgDaysDetails(string StartDate, string EndDate, string ReqDateFrom, string ReqDateTo, string ClientId)
        {
            DBOperation dbopertion = new DBOperation();
            SqlCommand cmd = new SqlCommand("Rpt_PermitCompleteDate");
            cmd.Parameters.AddWithValue("@DateFrom", StartDate);
            cmd.Parameters.AddWithValue("@DateTo", EndDate);
            cmd.Parameters.AddWithValue("@ReqDateFrom", ReqDateFrom);
            cmd.Parameters.AddWithValue("@ReqDateTo", ReqDateTo);
            cmd.Parameters.AddWithValue("@ClientId", ClientId);
            cmd.CommandType = CommandType.StoredProcedure;
            return dbopertion.GetTableData(cmd);
        }

        /// <summary>
        /// Description : To get Tracking Permit for the client
        /// Created by  : Pandiyan
        /// Created on  : April 28 2015
        /// </summary>
        private DataTable GetCommonReportModule(string ReportType, string InstallDateFrom, string InstallDateTo, string PermitType, string Status, string CustomerName, string HomeOwnerName, string CustomerJobNo, string SoftwareJobNo, string Town, string State)
        {
            string SqlArugument = string.Empty;

            if (ReportType.Equals("TrackHours"))
                SqlArugument = "Rpt_TrackingPermits";

            else if (ReportType.Equals("JobInstalled"))
                SqlArugument = "Rpt_JobInstalled";

            else if (ReportType.Equals("PermitReceivedAndCompleted"))
                SqlArugument = "Rpt_PermitCompleteDate";

            else if (ReportType.Equals("TrackTimeFrame"))
                SqlArugument = "Rpt_TrackTimeFrame";
            else
                SqlArugument = "Rpt_TrackingInstallDates";
           // string status = "";

            DBOperation dbOperation = new DBOperation();
            SqlCommand sqlCommand = new SqlCommand(SqlArugument);
            sqlCommand.Parameters.AddWithValue("@DateFrom", InstallDateFrom);
            sqlCommand.Parameters.AddWithValue("@DateTo", InstallDateTo);
            sqlCommand.Parameters.AddWithValue("@PermitType", PermitType);
            sqlCommand.Parameters.AddWithValue("@CustomerJobNo", CustomerJobNo);
            sqlCommand.Parameters.AddWithValue("@SoftwareJobNo", SoftwareJobNo);
            sqlCommand.Parameters.AddWithValue("@Status", Status);
            sqlCommand.Parameters.AddWithValue("@CustomerName", CustomerName);
            sqlCommand.Parameters.AddWithValue("@HomeOwnerName", HomeOwnerName);
            sqlCommand.Parameters.AddWithValue("@Town", Town);
            sqlCommand.Parameters.AddWithValue("@State", State);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            return dbOperation.GetTableData(sqlCommand);
        }

        /// <summary>
        /// Description : To get Job Installation for the client
        /// Created by  : Pandiyan
        /// Created on  : April 17 2015
        /// </summary>
        private DataTable GetJobInstalled(string StartDate, string EndDate, string ReqDateFrom, string ReqDateTo, string ClientId)
        {
            DBOperation dbOperation = new DBOperation();
            SqlCommand sqlCommand = new SqlCommand("Rpt_JobInstalled");
            sqlCommand.Parameters.AddWithValue("@DateFrom", StartDate);
            sqlCommand.Parameters.AddWithValue("@DateTo", EndDate);
            sqlCommand.Parameters.AddWithValue("@ReqDateFrom", ReqDateFrom);
            sqlCommand.Parameters.AddWithValue("@ReqDateTo", ReqDateTo);
            sqlCommand.Parameters.AddWithValue("@ClientId", ClientId);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            return dbOperation.GetTableData(sqlCommand);
        }

        public string PermitListPDF()
        {
            try
            {
                string rdlcName = string.Empty;
                PDFGenerator PDFGenerator = new PDFGenerator();
                DataTable PermitList = GetPermitDetail();
                rdlcName = "PermitList.rdlc";
                return PDFGenerator.DownloadReport(PermitList, "dsPermitDetailList", rdlcName, "PermitList");

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private DataTable GetPermitDetail()
        {
            DBOperation dbOperation = new DBOperation();
            SqlCommand sqlCommand = new SqlCommand("PermitList_ReportSelect");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            return dbOperation.GetTableData(sqlCommand);
        }





        /// <summary>
        /// Created By : Thereshan.K
        /// Created On : 12 March 2015
        /// Description: Get Job Details
        /// </summary>
        /// <param name="jobNumber"></param>
        /// <returns></returns>
        private DataTable GetJobDetails(string jobNumber)
        {
            DBOperation dbOperation = new DBOperation();
            SqlCommand sqlcommand = new SqlCommand("Rpt_JobDetails");
            sqlcommand.Parameters.AddWithValue("@JobNumber", jobNumber);
            sqlcommand.CommandType = CommandType.StoredProcedure;
            return dbOperation.GetTableData(sqlcommand);
        }


        /// <summary>
        /// Created By : Thereshan.K
        /// Created On : 12 March 2015
        /// Description:GetPDFjobDetails
        /// </summary>
        /// <returns></returns>
        public string PermitJobDetailsPDF(string jobDetails)
        {
            try
            {
                string rdlcName = string.Empty;
                string[] datasetName = new string[3];
                PDFGenerator PDFGenerator = new PDFGenerator();
                DataTable[] dataTable = new DataTable[3];
                dataTable[0] = GetJobDetails(jobDetails);
                dataTable[1] = GetPermitJobSummary(Convert.ToInt32(jobDetails));
                
                DataTable dtClientPermitAddressTemp = GetDoPDFPrintCheck(Convert.ToInt32(jobDetails));
                DataTable dtClientPermitAddress = GetDoPDFPrintCheck(Convert.ToInt32(jobDetails));
                dtClientPermitAddress.Clear();
                DataRow[] dr = dtClientPermitAddressTemp.Select("AddressType =2");
               
                foreach (DataRow row in dr)
                {
                    dtClientPermitAddress.ImportRow(row);
                }
                dataTable[2] = dtClientPermitAddress;
                //DataTable dtJobDetails = GetJobDetails(jobDetails);
                rdlcName = "JobDetails.rdlc";
                datasetName[0] = "dsJobDetail";
                datasetName[1] = "dsJobSummary";
                datasetName[2] = "dsJobPermitAddress";
                
                return DownloadSubReport(dataTable, datasetName, rdlcName, "JobDetails");

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        /// <summary>
        /// Created By : Thereshan.K
        /// Created On : 12 March 2015
        /// Description: Get permit Details
        /// </summary>
        /// <param name="permitId"></param>
        /// <param name="jobNumber"></param>
        /// <returns></returns>
        private DataTable GetPermitsDetail(int permitId,int jobNumber)
        {
            DBOperation dbOperation = new DBOperation();
            SqlCommand sqlcommand = new SqlCommand("AdditionalPermits_SelectAll");
            sqlcommand.Parameters.AddWithValue("@Id", permitId);
            sqlcommand.Parameters.AddWithValue("@PermitId",jobNumber);
            sqlcommand.CommandType = CommandType.StoredProcedure;
            return dbOperation.GetTableData(sqlcommand);
        }

        /// <summary>
        /// Created By : Thereshan.K
        /// Created On : 12 March 2015
        /// Description:Get pemitDetails PDF
        /// </summary>
        /// <param name="permitId"></param>
        /// <param name="jobNumber"></param>
        /// <returns></returns>
        public string PermitDetailsPDF(int permitId, int jobNumber)
        {
            try
            {
                string rdlcName = string.Empty;
                string[] dataSetName = new string[1];
                DataTable[] dataTable = new DataTable[1];
                dataSetName[0] = "dsPermitDetails";
                dataTable[0] = GetPermitsDetail(permitId, jobNumber);
                PDFGenerator PDFGenerator = new PDFGenerator();
                rdlcName = "PermitDetails.rdlc";
                return DownloadSubReport(dataTable, dataSetName, rdlcName, "PermitDetails");
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        /// <summary>
        /// Get permit details info
        /// </summary>
        /// <param name="permitId"></param>
        /// <returns></returns>
        private DataTable GetPermitsDetailinfo(int permitId)
        {
            DBOperation dbOperation = new DBOperation();
            SqlCommand sqlcommand = new SqlCommand("Rpt_PermitDetailInfo");
            sqlcommand.Parameters.AddWithValue("@PermitId", permitId);
            sqlcommand.CommandType = CommandType.StoredProcedure;
            return dbOperation.GetTableData(sqlcommand);
        }

        /// <summary>
        /// Created By : Thereshan
        /// Created On : 14 May 2015
        /// Description: Get Job Summary 
        /// </summary>
        /// <param name="PermitId"></param>
        /// <returns></returns>
        private DataTable GetPermitJobSummary(int PermitId)
        {
            DBOperation dbOperation = new DBOperation();
            SqlCommand sqlCommand = new SqlCommand("GetPermitJobSummary");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@PermitId", PermitId);
            return dbOperation.GetTableData(sqlCommand);
        }

        /// <summary>
        /// Created By : Thereshan
        /// Created On : 14 May 2015
        /// Description: Get Permit Checks 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="additionalPermitId"></param>
        /// <returns></returns>
        private DataTable GetPermitChecks(int id, int additionalPermitId)
        {
            DBOperation dbOperation = new DBOperation();
            SqlCommand sqlCommand = new SqlCommand("PermitChecks_SelectAll");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@AdditionalPermitId", additionalPermitId);
            return dbOperation.GetTableData(sqlCommand);
        }

        /// <summary>
        /// Created By : Thereshan
        /// Created On : 14 May 2015
        /// Description: Get Permit Checks 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="additionalPermitId"></param>
        /// <returns></returns>
        private DataTable GetPermitReceiptNumber(int id, int additionalPermitId)
        {
            DBOperation dbOperation = new DBOperation();
            SqlCommand sqlCommand = new SqlCommand("PermitReceiptNumber_SelectAll");
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Id", id);
            sqlCommand.Parameters.AddWithValue("@AdditionalPermitId", additionalPermitId);
            return dbOperation.GetTableData(sqlCommand);
        }   


        /// <summary>
        /// Download permitdetails info 
        /// </summary>
        /// <param name="DataSource"></param>
        /// <param name="ReportName"></param>
        /// <param name="rdlcName"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public string DownloadSubReport(DataTable [] DataSource, string [] ReportName, string rdlcName, string FileName)
        {
            try
            {
                CommonData commonData = new CommonData();
                string rdlcpath = commonData.GetRdlcPath();
                if (rdlcpath[rdlcpath.Length - 1] != '\\' && rdlcpath[rdlcpath.Length - 1] != '/')
                    rdlcpath = rdlcpath + "\\";
                rdlcpath = rdlcpath + rdlcName;

                for (int icount = 0; icount < ReportName.Length; icount++)
                {
                    DataSource[icount].TableName = ReportName[icount].ToString();
                    DataSource[icount].TableName = ReportName[icount].ToString();
                }
                
               
                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.EnableExternalImages = true;

                ReportDataSource rds;
                viewer.ProcessingMode = ProcessingMode.Local;

                LocalReport rep = viewer.LocalReport;
                rep.Refresh();
                rep.ReportPath = rdlcpath;
               

                for (int icount = 0; icount < DataSource.Length; icount++)
                {
                    rds = new ReportDataSource();
                    rds.Name = ReportName[icount].ToString();
                    rds.Value = DataSource[icount];
                    rep.DataSources.Add(rds);
                }

                rep.SubreportProcessing += new SubreportProcessingEventHandler(rep_SubreportProcessing);

                FileName = (String.IsNullOrEmpty(FileName.Trim()) == true) ? ReportName[0] : FileName;
                byte[] content = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                string PDFFilePath = string.Empty;

                PDFFilePath = commonData.GetReportSaveFile();
                if (!Directory.Exists(PDFFilePath))
                    Directory.CreateDirectory(PDFFilePath);
                if (PDFFilePath[PDFFilePath.Length - 1] != '\\' && PDFFilePath[PDFFilePath.Length - 1] != '/')
                    PDFFilePath = PDFFilePath + "\\";
                PDFFilePath = PDFFilePath + FileName + ".pdf";
                using (FileStream fs = File.Create(PDFFilePath))
                {
                    fs.Write(content, 0, (int)content.Length);
                }
                return PDFFilePath;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        /// Created By : Thereshan.K
        /// Created On : 15 May 2015
        /// It will call sub report datasets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void rep_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            DataTable dtPermitDetailInfo = GetPermitsDetailinfo(Convert.ToInt32(e.Parameters[0].Values[0]));
            DataTable dtPermitChecks = GetPermitChecks(0, Convert.ToInt32(e.Parameters[0].Values[0]));
            DataTable dtPermitReceipt = GetPermitReceiptNumber(0, Convert.ToInt32(e.Parameters[0].Values[0]));
            e.DataSources.Add(new ReportDataSource("dsJobDetailSub",dtPermitDetailInfo));
            e.DataSources.Add(new ReportDataSource("dsPermitChecks", dtPermitChecks));
            e.DataSources.Add(new ReportDataSource("dsPermitReceiptNumber", dtPermitReceipt));
        }
    }
}
