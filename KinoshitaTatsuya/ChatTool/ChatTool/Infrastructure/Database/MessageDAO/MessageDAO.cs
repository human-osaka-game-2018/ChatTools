using ChatTool.Models.DomainObjects.Message;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChatTool.Infrastructure.Database.MessageDAO
{
    public static class MessageDAO
    {
        public static ObservableCollection<Message> GetMessages(int channelId)
        {
            ObservableCollection<Message> messages = new ObservableCollection<Message>();

            using (var con = Connection.Connection.OpenDB())
            {                
                string sql = "select * from t_message where channel_id = @channelId";
                
                var command = new MySqlCommand(sql, con);
                command.Parameters.Add(new MySqlParameter("channelId", channelId));

                MySqlDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    var message = new Message()
                    {
                        Id = (int)reader["id"],                        
                        ChannelId = (int)reader["channel_id"],
                        Text = (string)reader["text"],
                        Time = (DateTime)reader["time"],
                        ParentMessageId = (int)reader["parent_message_id"],
                        DisplaysToChannel = ((sbyte)reader["displays_to_channel"] == 1) ? true : false
                    };

                    message.User = UserDAO.UserDAO.GetUser((int)reader["user_id"]);

                    messages.Add(message);
                }
            }

            return messages;
        }
    }
}
