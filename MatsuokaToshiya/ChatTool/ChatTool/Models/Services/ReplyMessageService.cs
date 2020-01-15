using ChatTool.Models.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTool.Models.Services
{
    static class ReplyMessageService
    {
        public static Action<Message?> SelectingMessageChanged = (_) => { };
        public static Action<int> EraseSelectingReply = (_) => { };
    }
}
