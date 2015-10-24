using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace BlueDiamond.Hubs
{
    public class EventHub : Hub
    {
        public void Send(string incident, string lastname, string firstname, string signintime)
        {
            Clients.Group(incident).login(lastname, firstname, signintime);
            // Call the addNewMessageToPage method to update clients.
            //Clients.All.login(lastname, firstname, signintime);
        }

        public Task JoinIncident(Guid incidentID)
        {
            return Groups.Add(Context.ConnectionId, incidentID.ToString());
        }

        public Task LeaveIncident(Guid incidentID)
        {
            return Groups.Remove(Context.ConnectionId, incidentID.ToString());
        }
    }
}