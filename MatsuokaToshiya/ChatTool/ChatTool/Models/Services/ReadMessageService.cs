using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChatTool.Models.Services
{
    delegate void RefleshMessageLogEvent();
    static class ReadMessageService
    {
        public static RefleshMessageLogEvent RefleshMessageLog = ()=> { };
    }
}
