using SQLDBOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayrollBO.SuperAdmin
{
    public class SACompCreation
    {
        public bool CheckExistDB(string dbName)
        {            
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.CheckDatabaseExists(dbName);           
            return status;
        }

        public bool CreateNewDB(string dbName)
        {
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.CreateNewDB(dbName);
            return status;
        }

        public bool ExcuteNewDBScript(string dbName)
        {
            DBOperation dbOperation = new DBOperation();
            string outValue = string.Empty;
            bool status = dbOperation.ExcuteNewDBScript(dbName);
            return status;
        }
    }
}
