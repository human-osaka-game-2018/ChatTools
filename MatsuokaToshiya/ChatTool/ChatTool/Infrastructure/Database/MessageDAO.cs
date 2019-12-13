using ChatTool.Models.DomainObjects;
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

    }
}
