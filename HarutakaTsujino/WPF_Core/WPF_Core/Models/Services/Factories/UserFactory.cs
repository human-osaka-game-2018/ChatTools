using System.Data;
using WPF_Core.Models.DomainObjects;

namespace WPF_Core.Models.Services.Factories
{
    static class UserFactory
    {
        public static User Create(DataRow dataRow)
        {
            return new User(
                dataRow.Field<int>("id"),
                dataRow.Field<string>("mail_address"),
                dataRow.Field<string>("password"),
                dataRow.Field<string>("user_name"));
        }
    }
}
