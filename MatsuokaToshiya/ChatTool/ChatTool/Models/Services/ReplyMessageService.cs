using ChatTool.Models.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTool.Models.Services
{
    delegate void SelectingMessageChangedEvent(Message message);
    static class ReplyMessageService
    {
        public static SelectingMessageChangedEvent ChangedSelectingMessage = (_) => { };

    }
}
