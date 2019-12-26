using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
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

            if (userDataTable is null) return false;

            var existsUser = ExtractLogInUser(mailAddress, password, userDataTable);

            if (existsUser)
            {
                if (LogInUser is null) return false;

                UserDAO.ChangeOnlineState(LogInUser.Id, true);

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
            // 引数のメアドとパスに対応するユーザを取得
            var logInUsers = userDataTable.AsEnumerable()
                .Where(x => mailAddress == x.Field<string>("mail_address") &&
                            password    == x.Field<string>("password"))
                .Select(x =>
                {
                    return new User(
                                x.Field<int>("id"),
                                x.Field<string>("mail_address"),
                                x.Field<string>("password"),
                                x.Field<string>("user_name"));
                });

            // 対応するユーザは一人しかいないはずなので
            foreach (var logInUser in logInUsers)
            {
                LogInUser = logInUser;

                return true;
            }

            return false;
        }
    }
}
