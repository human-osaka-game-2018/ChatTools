using System.Data;
using WPF_Core.Models.DomainObjects;

namespace WPF_Core.Models.Services.Factories
{
    static class ChannelFactory
    {
        public static Channel Create(DataRow dataRow)
        {
            return new Channel(
                dataRow.Field<int>("id"),
                dataRow.Field<string>("channel_name"));
        }
    }
}
