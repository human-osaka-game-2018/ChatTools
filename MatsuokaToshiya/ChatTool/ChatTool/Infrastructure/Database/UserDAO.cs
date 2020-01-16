using ChatTool.Models.DomainObjects;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTool.Infrastructure.Database
{
    class UserDAO : DAO
    {

        public static User? User(string mailAdress, string password)
        {
            var command = new StringBuilder();
            command.Append("select * from m_user where mail_address = @mail and password = @pass;");
            var cmd = new MySqlCommand(command.ToString(), Conection.ConnectDB());

            cmd.Parameters.Add(CreateParameter("@mail", mailAdress,MySqlDbType.VarChar,255));
            cmd.Parameters.Add(CreateParameter("@pass", password,MySqlDbType.VarChar,40));
            var reader = cmd.ExecuteReader();
            reader.Read();
            if (false == reader.HasRows) {
                Conection.DisConnectDB();
                return null; 
            }
            var user = new User();
            user.Id = DBNull.Value != reader["Id"] ? Convert.ToInt32(reader.GetString("Id")) : 0;
            user.IconId = DBNull.Value != reader["icon_id"] ? Convert.ToInt32(reader.GetString("icon_id")) : 0;
            user.Name = DBNull.Value != reader["user_name"] ? reader.GetString("user_name") : "";
            user.MailAddress = DBNull.Value != reader["mail_address"] ? reader.GetString("mail_address") : "";
            user.Password = DBNull.Value != reader["password"] ? reader.GetString("password") : "";
            user.IsOnline = true;
            user.IconPath = System.Configuration.ConfigurationManager.AppSettings[0]+"icon0"+user.IconId.ToString()+".png";
            Conection.DisConnectDB();

            return user;
        }

        public static void Online(User? user)
        {
            if (user == null) return;
            var command = new StringBuilder();
            command.Append("update m_user set is_online = @isOnline where id = @id;");
            var cmd = new MySqlCommand(command.ToString(), Conection.ConnectDB());
            cmd.Parameters.Add(CreateParameter("@isOnline", user.IsOnline,MySqlDbType.Byte,1));
            cmd.Parameters.Add(CreateParameter("@id", user.Id,MySqlDbType.Int32,16));
            cmd.ExecuteNonQuery();
            Conection.DisConnectDB();
        }

        public static string UserName(int userId)
        {
            var cmd = new MySqlCommand("select * from m_user where id = @user_id;", Conection.ConnectDB());
            cmd.Parameters.Add(CreateParameter("@user_id", userId, MySqlDbType.Int32, 10));
            var reader = cmd.ExecuteReader();
            reader.Read();
            var str =  DBNull.Value != reader["user_name"] ? reader.GetString("user_name") : "";
            Conection.DisConnectDB();
            return str;
        }

        public static int UserIconId(int userId)
        {
            var cmd = new MySqlCommand("select * from m_user where id = @user_id;", Conection.ConnectDB());
            cmd.Parameters.Add(CreateParameter("@user_id", userId, MySqlDbType.Int32, 10));
            var reader = cmd.ExecuteReader();
            reader.Read();
            int val = DBNull.Value != reader["icon_id"] ? Convert.ToInt32(reader.GetString("icon_id")) : 0;
            Conection.DisConnectDB();
            return val;
        }
    }
}
