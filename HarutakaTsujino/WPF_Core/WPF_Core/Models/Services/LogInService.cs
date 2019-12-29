using System.Data;
using System.Linq;
using WPF_Core.Infrastructure.Database;
using WPF_Core.Models.DomainObjects;
using WPF_Core.Models.Services.Factories;

namespace WPF_Core.Models.Services
{
    static class LogInService
    {
        public static User? LogInUser { get; private set; }

        public static bool LogIn(string mailAddress, string password)
        {
            var userDataTable = UserDAO.GetWithMailAddress(mailAddress);
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

            LogInUser = UserFactory.Create(logInUser);

            return true;
        }
    }
}
