using System.Data;
using WPF_Core.Infrastructure.Database;
using WPF_Core.Models.DomainObjects;

namespace WPF_Core.Models.Services.Factories
{
    static class UserFactory
    {
        public static User Create(DataRow dataRow)
        {
            const string ICON_ID = "icon_id";

            return new User(
                dataRow.Field<int>("id"),
                dataRow.Field<int>(ICON_ID),
                IconDAO.GetPath(dataRow.Field<int>(ICON_ID)),
                dataRow.Field<string>("mail_address"),
                dataRow.Field<string>("password"),
                dataRow.Field<string>("user_name"));
        }
    }
}
