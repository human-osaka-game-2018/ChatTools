using System.Data;
using System.Linq;
using WPF_Core.Infrastructure.Database;
using WPF_Core.Models.DomainObjects;

namespace WPF_Core.Models.Services
{
    static class LogInService
    {
        public static User? LogInUser { get; private set; }

        public static bool LogIn(string mailAddress, string password)
        {
            var userDataTable = UserDAO.Get(mailAddress);
            var existsUser = ExtractLogInUser(mailAddress, password, userDataTable);

            if (existsUser)
            {
                UserDAO.ChangeOnlineState(LogInUser!.Id, true);

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool LogOut()
        {
            if (LogInUser is null)
            {
                return false;
            }
            else
            {
                UserDAO.ChangeOnlineState(LogInUser.Id, false);

                LogInUser = null;

                return true;
            }
        }

        private static bool ExtractLogInUser(string mailAddress, string password, DataTable userDataTable)
        {
            var logInUser = userDataTable.AsEnumerable()
                .SingleOrDefault(x => mailAddress == x.Field<string>("mail_address") &&
                                         password == x.Field<string>("password"));

            if (logInUser is null) return false;

            LogInUser = new User(
                logInUser.Field<int>("id"),
                logInUser.Field<string>("mail_address"),
                logInUser.Field<string>("password"),
                logInUser.Field<string>("user_name"));

            return true;
        }
    }
}
