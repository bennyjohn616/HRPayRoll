using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
namespace Payroll.Helpers
{
    public class HelperCls
    {
        public void ImportOldDirectoryDelete(string path)
        {
            if (Directory.Exists(path))
            {
                var direcList = Directory.GetDirectories(path);
                foreach (var dirName in direcList)
                {
                    var dir = new DirectoryInfo(dirName);
                    if (dir.LastAccessTime < DateTime.Now.AddDays(-1))
                        dir.Delete(true);
                }
            }

        }

        public void ErrorLogFileDelete(string path)
        {
            if (Directory.Exists(path))
            {
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
        public void DirectoryDeletewithfile(string path)
        {
            if (Directory.Exists(path))
            {
                var direcList = Directory.GetDirectories(path);
                foreach (var dirName in direcList)
                {
                    string[] files = Directory.GetFiles(dirName);

                    foreach (string tempfile in files)
                    {
                        FileInfo fi = new FileInfo(tempfile);
                        if (fi.LastAccessTime < DateTime.Now.AddDays(-1))
                            fi.Delete();
                    }
                    var dir = new DirectoryInfo(dirName);
                    if (dir.LastAccessTime < DateTime.Now.AddDays(-1))
                        dir.Delete(true);
                }
            }
        }


    }
}