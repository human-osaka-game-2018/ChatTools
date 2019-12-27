using ChatTool.Models.DomainObjects;
using ChatTool.Models.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChatTool.Infrastructure.Database
{
    class MessageDAO : DAO
    {
        public void MessageList(ObservableCollection<Message> list , int channelId)
        {
            var cmd = new MySqlCommand("select * from t_message where channel_id = @channel_id;", Conection.ConnectDB());
            cmd.Parameters.Add(CreateParameter("@channel_id", channelId, MySqlDbType.Int32, 16));

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var message = new Message();
                message.Id = DBNull.Value != reader["Id"] ? Convert.ToInt32(reader.GetString("Id")) : 0;
                message.Text = DBNull.Value != reader["text"] ? reader.GetString("text") : "";
                message.Time = DateTime.Parse( DBNull.Value != reader["time"] ? reader.GetString("time") : "");
                message.UserId = DBNull.Value != reader["user_id"] ? Convert.ToInt32(reader.GetString("user_id")) : 0;
                list.Add(message);
            }
            Conection.DisConnectDB();
            foreach (Message message in list)
            {
                var userDao = new UserDAO();
                message.UserName = userDao.UserName(message.UserId);
            }
        }


        public void SendMessage(string inputText,int channelId)
        {
            if (null == LoginService.User) return;
            var command = new StringBuilder();
            command.Append("insert into t_message( text, channel_id, user_id, time) values ( @text, @channel_id, @user_id, @time);");
            MySqlCommand cmd = new MySqlCommand(command.ToString(), Conection.ConnectDB());

            cmd.Parameters.Add(CreateParameter("@text", inputText, MySqlDbType.VarChar, 280));
            cmd.Parameters.Add(CreateParameter("@channel_id", channelId, MySqlDbType.Int32, 16));
            cmd.Parameters.Add(CreateParameter("@user_id", LoginService.User.Id, MySqlDbType.Int32, 16));
            cmd.Parameters.Add(CreateParameter("@time",DateTime.Now, MySqlDbType.DateTime, 16));

            cmd.ExecuteNonQuery();

            Conection.DisConnectDB();

        }
    }
}
