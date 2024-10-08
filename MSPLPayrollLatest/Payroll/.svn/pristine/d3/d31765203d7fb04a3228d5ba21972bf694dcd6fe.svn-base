using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Payroll.Hubs;
using PayrollBO;

namespace Payroll.Helpers
{
    public static class SignalRCls
    {

        public static void SendProgress(string username, string progressMessage, int progressCount, int totalItems)
        {
            //IN ORDER TO INVOKE SIGNALR FUNCTIONALITY DIRECTLY FROM SERVER SIDE WE MUST USE THIS
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();

            //CALCULATING PERCENTAGE BASED ON THE PARAMETERS SENT
            var percentage = (progressCount * 100) / totalItems;

            //PUSHING DATA TO ALL CLIENTS
            // hubContext.Clients.Client(username).AddProgress(progressMessage, percentage + "%");
            hubContext.Clients.Client(username).AddProgress(progressMessage, percentage + "%");
        }
    }
}
