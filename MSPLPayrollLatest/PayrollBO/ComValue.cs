using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollBO
{
    public static class ComValue
    {
        public static string SalaryTable = "Salary";
        public static string EMPADDLINFOTable = "AdditionalInfo";
        public static string EmployeeTable = "Employee";
        public static string SupplementaryDays = "SUPPDAYS";
        public static string SupplementaryLOPDays = "SUPPLOPDAYS";
        public static string LOPCreditDays = "LOPCREDITDAYS";
        public static string PAIDDAYS = "PD";
        public static string[] payrollProcessStatus = new string[4] { "Processed", "Imported", "Not Processed", "Error" };

        public static string GetXLOldebConnection(string sourceFile)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sourceFile + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            if (sourceFile.Contains(".xlsx"))
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sourceFile + ";Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\"";
            return strConn;
        }
    }
}
