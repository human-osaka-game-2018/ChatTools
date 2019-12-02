using ChatTool.Models.DomainObjects;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTool.Infrastructure.Database
{
    class UserDAO
    {
        public User[] UserList(User[] list,MySqlConnection? connection)
        {
            var cmd = new MySqlCommand("select * from m_user;", connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var user = new User();
                user.Id = DBNull.Value != reader["Id"] ? Convert.ToInt32(reader.GetString("Id")) : 0;
                user.Name = DBNull.Value != reader["user_name"] ? reader.GetString("user_name") : "";
                user.MailAdress = DBNull.Value != reader["mail_address"] ? reader.GetString("mail_address") : "";
                user.Password = DBNull.Value != reader["password"] ? reader.GetString("password") : "";
                Array.Resize(ref list, list.Length + 1);
                list[list.Length - 1] = user;
            }
            return list;
        }
        public User? User(string mailAdress, string password, MySqlConnection? connection)
        {
            var command = new StringBuilder();
            command.Append("select * from m_user where mail_address = @mail and password = @pass;");
            var cmd = new MySqlCommand(command.ToString(), connection);
            cmd.Parameters.Add(new MySqlParameter("@mail", mailAdress));
            cmd.Parameters.Add(new MySqlParameter("@pass", password));
            var reader = cmd.ExecuteReader();
            reader.Read();
            if (false == reader.HasRows) return null;
            var user = new User();
            user.Id = DBNull.Value != reader["Id"] ? Convert.ToInt32(reader.GetString("Id")) : 0;
            user.Name = DBNull.Value != reader["user_name"] ? reader.GetString("user_name") : "";
            user.MailAdress = DBNull.Value != reader["mail_address"] ? reader.GetString("mail_address") : "";
            user.Password = DBNull.Value != reader["password"] ? reader.GetString("password") : "";
            user.IsOnline = true;
            return user;
        }
        public void Online(User? user, MySqlConnection connection)
        {
            var command = new StringBuilder();
            command.Append("update m_user set is_online = @isOnline where id = @id;");
            var cmd = new MySqlCommand(command.ToString(), connection);
            cmd.Parameters.Add(new MySqlParameter("@isOnline", user.IsOnline));
            cmd.Parameters.Add(new MySqlParameter("@id", user.Id));

            cmd.ExecuteNonQuery();
        }
    }
}
