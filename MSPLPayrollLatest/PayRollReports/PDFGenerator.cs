// -----------------------------------------------------------------------
// <copyright file="PDFGenerator.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace PayRollReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using Microsoft.Reporting.WebForms;
    using System.Web;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PDFGenerator
    {

        /// <summary>
        /// Description : Download Report using PDF format
        /// Created by  : Pandiyan 
        /// Created on  : 10/March/2015
        /// </summary>
        /// <param name="DataSource">DataTable</param>
        /// <param name="ReportName">DataSet Name</param>
        /// <param name="rdlcName">Location for RDLC report</param>
        /// <param name="FileName">FileName for user identification</param>
        public string DownloadReport(DataTable DataSource, string ReportName, string rdlcName, string FileName)
        {
            try
            {
                DeleteOldGeneratedFile();//Delete the file which we generated for report earliar days or hours
                CommonData commonData = new CommonData();
                string rdlcpath = commonData.GetRdlcPath();
                if (rdlcpath[rdlcpath.Length - 1] != '\\' && rdlcpath[rdlcpath.Length - 1] != '/')
                    rdlcpath = rdlcpath + "\\";
                //rdlcpath = rdlcpath + "\\" + rdlcName;
                rdlcpath = rdlcpath + rdlcName;

                // The DataTable Name should be same as DataSetName of Tablix
                DataSource.TableName = ReportName;
                Warning[] warnings;
                string[] streamIds;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                ReportViewer viewer = new ReportViewer();
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.EnableExternalImages = true;

                ReportDataSource rds = new ReportDataSource();
                viewer.ProcessingMode = ProcessingMode.Local;

                LocalReport rep = viewer.LocalReport;
                rep.Refresh();
                rep.ReportPath = rdlcpath;
                rds.Name = ReportName;
                rds.Value = DataSource;
                rep.DataSources.Add(rds);
                FileName = (String.IsNullOrEmpty(FileName.Trim()) == true) ? ReportName : FileName;
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
        /// delete the file which we generated file early hours
        /// </summary>
        public void DeleteOldGeneratedFile()
        {
            try
            {
                CommonData commonData = new CommonData();
                string directoryPath = commonData.GetReportSaveLocation();

                var directory = new DirectoryInfo(directoryPath).GetDirectories("*.*");
                foreach (var dir in directory.Where(dir => DateTime.UtcNow - dir.CreationTimeUtc > TimeSpan.FromHours(1)))
                {
                    dir.Delete(true);
                }

                var files = new DirectoryInfo(directoryPath).GetFiles("*.*");
                foreach (var file in files.Where(file => DateTime.UtcNow - file.CreationTimeUtc > TimeSpan.FromHours(1)))
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                string st = ex.Message;
            }
        }

    }
}
