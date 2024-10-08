using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Payroll.Helpers;

namespace Payroll.Hubs
{
    public class ProgressHub : Hub
    {
        public void AddUser(string name)

        {
            string id = Context.ConnectionId;
            HubClient hubs = new HubClient();
            hubs.Login(id,name);
        }

        public override Task OnConnected()
        {
            string id = Context.ConnectionId;
            HubClient hubs = new HubClient();
            hubs.Reconnect(id);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            HubClient hubs = new HubClient();
            string id = Context.ConnectionId;
            hubs.Delete(id);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            HubClient hubs = new HubClient();
            string id = Context.ConnectionId;
            hubs.Reconnect(id);
            return base.OnReconnected();
        }

    }

}