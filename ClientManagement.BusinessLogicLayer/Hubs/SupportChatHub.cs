using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientManagement.Presentation.Models;
using ClientManagement.Presentation.Models.SupportChat.Constants;
using Microsoft.AspNetCore.SignalR;

namespace ClientManagement.BusinessLogicLayer.Hubs
{
    public class SupportChatHub: Hub
    {
        public async Task SendMessage(string senderUsername, string message)
        {
           await Clients.All.SendAsync(Events.MessageSent ,senderUsername, message);
        }
    }
}
