
using ClientManagement.AI.StateMachines.UserAssistant.Triggers.Interfaces.Base;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Triggers
{
    public  class UserIncomingMessageTrigger(UserAssitantContext context) : Base.TriggerBase<UserIncomingMessageTrigger, ChatMessage> , ITriggerBase<UserIncomingMessageTrigger,ChatMessage>
    {
        public void UserTextInputSignalListener(object? sender, ChatMessage userMessage)
        {
            Fire(this, userMessage);
        }
    }
}
