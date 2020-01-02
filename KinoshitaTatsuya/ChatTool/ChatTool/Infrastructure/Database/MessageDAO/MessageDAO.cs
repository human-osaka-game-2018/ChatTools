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

        public static void SendMessage(Message message)
        {
            using (var con = Connection.Connection.OpenDB())
            {
                var sql = new StringBuilder();
                sql.Append("insert into t_message (channel_id, user_id, text, time, parent_message_id, displays_to_channel) ");
                sql.Append("values (");
                sql.Append("@channel_id, @user_id, @text, @time, @parent_message_id, @displays_to_channel)");

                var command = new MySqlCommand(sql.ToString(), con);
                command.Parameters.Add(new MySqlParameter("channel_id", message.ChannelId));
                command.Parameters.Add(new MySqlParameter("user_id", message.User?.Id));
                command.Parameters.Add(new MySqlParameter("text", message.Text));
                command.Parameters.Add(new MySqlParameter("time", message.Time));
                command.Parameters.Add(new MySqlParameter("parent_message_id", message.ParentMessageId));
                command.Parameters.Add(new MySqlParameter("displays_to_channel", message.DisplaysToChannel));

                command.ExecuteNonQuery();
            }
        }
    }
}
