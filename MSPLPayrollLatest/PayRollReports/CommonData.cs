// -----------------------------------------------------------------------
// <copyright file="CommonData.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace PayRollReports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SQLDBOperation;
    using System.Data.SqlClient;
    using System.Data;
    using System.Configuration;
    using System.Reflection;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommonData
    {

        public string GetRdlcPath()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["RdlcPath"]);

        }

        /// <summary>
        /// Description : Assign PDF storing path
        /// Created by  :
        /// Created on  : 10/March/2015
        /// </summary>
        /// <returns>Location for save current executional file</returns>
        public string GetReportSaveLocation()
        {
            var con = ConfigurationManager.AppSettings["ReportSaveLocation"];
            if (con == null)
                return @"D:\Projects\GoForService\GoForServiceReport\GeneratedReport";
            else
                return con.ToString();
        }

        /// <summary>
        /// Description : Save PDF file with Unique id.
        /// Created by  :
        /// Created on  : 10/March/2015
        /// </summary>
        /// <returns>File Name</returns>
        public string GetReportSaveFile()
        {
            string location = GetReportSaveLocation();
            string fileName = location + "\\" + Guid.NewGuid().ToString();
            return fileName;
        }
        /// <summary>
        /// Convert List to Datatable
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ConvertListToTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();
            if (list.Count > 0)
            {

                PropertyInfo[] properties = list[0].GetType().GetProperties();
                List<string> columns = new List<string>();
                foreach (PropertyInfo pi in properties)
                {
                    dt.Columns.Add(pi.Name);
                    columns.Add(pi.Name);
                }
                foreach (T item in list)
                {
                    object[] cells = getValues(columns, item);
                    dt.Rows.Add(cells);
                }
            }
            return dt;
        }
        private static object[] getValues(List<string> columns, object instance)
        {
            object[] ret = new object[columns.Count];
            for (int n = 0; n < ret.Length; n++)
            {
                PropertyInfo pi = instance.GetType().GetProperty(columns[n]);
                object value = pi.GetValue(instance, null);
                ret[n] = value;
            }
            return ret;

        }


    }
}
