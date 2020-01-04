using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using ChatTool.Models.DomainObjects.User;
using ChatTool.Models.Services.Main;

namespace ChatTool.Infrastructure.Database.UserDAO
{
    public static class UserDAO
    {
        /// <summary>
        /// ユーザー全員の取得
        /// </summary>
        /// <returns>ユーザーのList</returns>
        public static List<User> GetUsers()
        {
            var users = new List<User>();
            using (var con = Connection.Connection.OpenDB())
            {
                string sql = "select * from m_user;";
                var command = new MySqlCommand(sql.ToString(), con);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User()
                    {
                        Id = (int)reader["id"],
                        Name = (string)reader["user_name"],
                        IconId = (int)reader["icon_id"],
                        IsOnline = (bool)reader["is_online"],
                        Password = (string)reader["password"],
                        MailAddress = (string)reader["mail_address"],
                    });
                }
            }

            return users;
        }

        /// <summary>
        /// ユーザー個人の取得
        /// </summary>
        /// <param name="id">取得したいユーザーのID</param>
        /// <returns>ユーザーの情報</returns>
        public static User GetUser(int id)
        {
            var user = new User();
            using (var con = Connection.Connection.OpenDB())
            {
                var sql = new StringBuilder();

                sql.Append("select * from m_user ");
                sql.Append("where id = ");
                sql.Append(id.ToString() + ";");

                var command = new MySqlCommand(sql.ToString(), con);

                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user.Id = id;
                    user.Name = (string)reader["user_name"];
                    user.IconId = (int)reader["icon_id"];
                    user.IsOnline = ((sbyte)reader["is_online"] == 1) ? true : false;
                    user.Password = (string)reader["password"];
                    user.MailAddress = (string)reader["mail_address"];

                    user.IconPath = IconPathFactory.Create(user.IconId);
                }              
            }

            return user;
        }

        /// <summary>
        /// ログインに必要な情報を取ってくる
        /// </summary>
        /// <returns>メールアドレスがキーになっているDictionary</returns>
        public static Dictionary<string,Tuple<string?, int>> GetLoginInfos()
        {
            var loginInfos = new Dictionary<string, Tuple<string?, int>>();
            using (var con = Connection.Connection.OpenDB())
            {
                string sql = "select id, mail_address, password from m_user;";
                var command = new MySqlCommand(sql, con);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    loginInfos.Add((string)reader["mail_address"], new Tuple<string?, int>((string)reader["password"],(int)reader["id"]));
                }
            }
            return loginInfos;
        }

        /// <summary>
        /// ユーザーのログイン情報を更新する
        /// </summary>
        /// <param name="userId">ユーザーのidを更新する</param>
        /// <param name="isOnline">ログイン状態にするかどうか</param>
        public static void UpdateOnline(int userId, bool isOnline)
        {
            using(var con = Connection.Connection.OpenDB())
            {
                var sql = new StringBuilder();
                sql.Append("update m_user ");
                sql.Append("set is_online = @currentState ");
                sql.Append("where id = @userId;");

                var command = new MySqlCommand(sql.ToString(), con);
                
                var state = (isOnline) ? 1 : 0;
                command.Parameters.Add(new MySqlParameter("currentState",state ));
                command.Parameters.Add(new MySqlParameter("userId", userId));

                command.ExecuteNonQuery();
            }
        }
    }
}
