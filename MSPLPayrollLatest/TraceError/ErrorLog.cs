using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace TraceError
{
    /// <summary>
    /// To handle the error and trace the error
    /// </summary>
    public static class ErrorLog
    {
        /// <summary>
        /// initialize the object
        /// </summary>
        static ErrorLog()
        {

        }

        /// <summary>
        /// initialize the object with provided values
        /// </summary>
        /// <param name="ex"></param>
        public static void Log(Exception ex)
        {
            if (GetErrorLogIn() == "DataBase")
            {
                LogInDB(ex);
            }
            else
            {
                // ErrorLogOldFileDelete(System.AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\");
                LogInFile(ex);
            }

        }

        /// <summary>
        /// Delete the erroe log file which is created very older
        /// </summary>
        public static void DeleteOldLogFile()
        {
            try
            {
                string tempDay = GetErrorLogLivingDay();
                int day = 5;
                if (!int.TryParse(tempDay, out day))
                {
                    day = 30;
                }
                string directoryPath = GetLocation();
                var directory = new DirectoryInfo(directoryPath).GetDirectories("**");
                foreach (var dir in directory.Where(dir => DateTime.UtcNow - dir.CreationTimeUtc > TimeSpan.FromDays(day)))
                {
                    dir.Delete(true);
                }
                var files = new DirectoryInfo(directoryPath).GetFiles("*.*");
                foreach (var file in files.Where(file => DateTime.UtcNow - file.CreationTimeUtc > TimeSpan.FromDays(day)))
                {
                    file.Delete();
                }

            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }
        public const string EXCEPTION_URL = "ExceptionUrl";
        public const string EXCEPTION_MESSAGE = "ExceptionMessage";
        public const string EXCEPTION_STACK_TRACE = "ExceptionStackTrace";
        /// <summary>
        /// save the error in specified file
        /// </summary>
        /// <param name="ex"></param>
        public static void LogInFile(Exception ex)
        {
            try
            {
                string strPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\";    //GetLocation();
                                                                                               //  strPath = strPath.Substring(0, strPath.IndexOf("\\GoForService")) + "\\Logs\\";
                if (!Directory.Exists(strPath))
                    Directory.CreateDirectory(strPath);
                StreamWriter objFile = new StreamWriter(strPath + "\\ErrorLog " + DateTime.Now.ToString("dd-MM-yyyy") + ".txt", true);
                StackTrace stkTrace = new StackTrace();
                StackFrame stkFrame = stkTrace.GetFrame(1);
                MethodBase mtdBase = stkFrame.GetMethod();
                objFile.WriteLine(DateTime.Now.ToString());
                objFile.WriteLine(mtdBase.Name);
                objFile.WriteLine(ex.TargetSite.Name);
                objFile.WriteLine(ex.Message);
                objFile.WriteLine(ex.StackTrace);
                objFile.WriteLine("=======================================================================");
                objFile.WriteLine(ex.InnerException);
                objFile.WriteLine("=======================================================================");
                objFile.Close();
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
        public static void LogTestWrite(string msg)
        {
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\";    //GetLocation();
            //  strPath = strPath.Substring(0, strPath.IndexOf("\\GoForService")) + "\\Logs\\";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            StreamWriter objFile = new StreamWriter(strPath + "\\ErrorLog " + DateTime.Now.ToString("dd-MM-yyyy") + ".txt", true);
            StackTrace stkTrace = new StackTrace();
            StackFrame stkFrame = stkTrace.GetFrame(1);
            MethodBase mtdBase = stkFrame.GetMethod();
            objFile.WriteLine(DateTime.Now.ToString());
            objFile.WriteLine(mtdBase.Name);
            objFile.WriteLine(msg);

            objFile.WriteLine("=======================================================================");
            objFile.Close();
        }
        public static void ChangePwd(string msg)
        {
            string strPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\";    //GetLocation();
            //  strPath = strPath.Substring(0, strPath.IndexOf("\\GoForService")) + "\\Logs\\";
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            StreamWriter objFile = new StreamWriter(strPath + "\\ChangePwd " + DateTime.Now.ToString("dd-MM-yyyy") + ".txt", true);
            StackTrace stkTrace = new StackTrace();
            StackFrame stkFrame = stkTrace.GetFrame(1);
            MethodBase mtdBase = stkFrame.GetMethod();
            objFile.WriteLine(DateTime.Now.ToString());
            objFile.WriteLine(mtdBase.Name);
            objFile.WriteLine(msg);

            objFile.WriteLine("=======================================================================");
            objFile.Close();
        }

        /// <summary>
        /// Save the errors in specofied data base
        /// </summary>
        /// <param name="ex"></param>
        private static void LogInDB(Exception ex)
        {
            using (SqlConnection odpCon = new SqlConnection(GetSqlConnection()))
            {
                try
                {
                    odpCon.Open();
                    SqlCommand sqlCommand = new SqlCommand("Sp_Name");
                    sqlCommand.Connection = odpCon;
                    sqlCommand.ExecuteNonQuery();
                }
                catch
                {

                }
                finally
                {
                    if (odpCon.State == ConnectionState.Open) odpCon.Close();
                }
            }
        }

        /// <summary>
        /// Get the data base instance to save save the error
        /// </summary>
        /// <returns></returns>
        private static string GetSqlConnection()
        {
            return ConfigurationManager.ConnectionStrings["ErrorConnection"].ToString();
        }

        /// <summary>
        /// get the location to store the error
        /// </summary>
        /// <returns></returns>
        private static string GetLocation()
        {
            return ConfigurationManager.AppSettings["ErrorLocation"].ToString();
        }

        /// <summary>
        /// get the error log in 
        /// </summary>
        /// <returns></returns>
        private static string GetErrorLogIn()
        {
            return ConfigurationManager.AppSettings["ErrorLogIn"].ToString();
        }

        /// <summary>
        /// get the error log living day
        /// </summary>
        /// <returns></returns>
        private static string GetErrorLogLivingDay()
        {
            return Convert.ToString(ConfigurationManager.AppSettings["ErrorLogLivingDay"]);
        }
        public static void ErrorLogOldFileDelete()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\";
            var direcList = Directory.GetDirectories(path);
            foreach (var dirName in direcList)
            {
                string[] files = Directory.GetFiles(dirName);
                foreach (string tempfile in files)
                {
                    FileInfo fi = new FileInfo(tempfile);
                    if (fi.LastAccessTime < DateTime.Now.AddMonths(-1))
                        fi.Delete();
                }

            }

        }
    }
}
