using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections.Concurrent;
using Payroll.Controllers;

namespace Payroll.Helpers
{
    public class HubClient : BaseController
    {
        public static ConcurrentDictionary<string, string> clients = new ConcurrentDictionary<string, string>();
        public static ConcurrentDictionary<string, string> Deletedclients = new ConcurrentDictionary<string, string>();

        public string Login(string id,string username)

        {
            string retval = "";
            clients.TryGetValue(id, out retval);
            if (retval != "")
            {
                clients.TryRemove(id, out retval);
            }

           clients.TryAdd(id,username);
           return username;
        }

        public string Delete(string id)

        {
            string retval = "";
            clients.TryGetValue(id,out retval);
            if (retval != "")
            {
                Deletedclients.TryAdd(id, retval);
            }
            string val1 = "";
            clients.TryRemove(id, out val1);
            return null;
        }

        public string Reconnect(string id)

        {
            string retval = "";
            Deletedclients.TryGetValue(id, out retval);
            if (retval != "")
            {
                clients.TryAdd(id, retval);
            }
            return null;
        }


    }
}