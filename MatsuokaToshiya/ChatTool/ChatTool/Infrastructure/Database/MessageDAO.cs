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
        public static void MessageList(ObservableCollection<Message> list , int channelId)
        {
            var cmd = new MySqlCommand("select * from t_message where channel_id = @channel_id  order by time;", Conection.ConnectDB());
            cmd.Parameters.Add(CreateParameter("@channel_id", channelId, MySqlDbType.Int32, 16));

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var message = new Message();
                message.ChannelId = channelId;
                message.Id = DBNull.Value != reader["Id"] ? Convert.ToInt32(reader.GetString("Id")) : 0;
                message.Text = DBNull.Value != reader["text"] ? reader.GetString("text") : "";
                message.Time = DateTime.Parse( DBNull.Value != reader["time"] ? reader.GetString("time") : "");
                message.UserId = DBNull.Value != reader["user_id"] ? Convert.ToInt32(reader.GetString("user_id")) : 0;
                message.ParentId = DBNull.Value != reader["parent_message_id"] ? Convert.ToInt32(reader.GetString("parent_message_id")) : 0;
                if (message.ParentId != 0) continue;
                list.Add(message);
            }
            Conection.DisConnectDB();
            foreach (Message message in list)
            {
                GetChildMessages(message);
                message.UserName = UserDAO.UserName(message.UserId);
                int iconId = UserDAO.UserIconId(message.UserId);
                string iconNum = iconId.ToString();
                if (10 > iconId)
                {
                    iconNum = "0" + iconNum;
                }
                message.IconPath = System.Configuration.ConfigurationManager.AppSettings[0] + "icon" + iconNum + ".png";
                foreach (Message child in message.Child)
                {
                    int childIconId = UserDAO.UserIconId(child.UserId);
                    string childIconNum = childIconId.ToString();
                    if (10 > childIconId)
                    {
                        childIconNum = "0" + childIconNum;
                    }
                    child.UserName = UserDAO.UserName(child.UserId);
                    child.IconPath = System.Configuration.ConfigurationManager.AppSettings[0] + "icon" + childIconNum + ".png";

                }

            }
        }

        private static void GetChildMessages(Message message)
        {
            var cmd = new MySqlCommand("select * from t_message where parent_message_id = @parent_message_id order by time;", Conection.ConnectDB());
            cmd.Parameters.Add(CreateParameter("@parent_message_id", message.Id, MySqlDbType.Int32, 16));

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var childMessage = new Message();
                childMessage.Id = DBNull.Value != reader["Id"] ? Convert.ToInt32(reader.GetString("Id")) : 0;
                childMessage.Text = DBNull.Value != reader["text"] ? reader.GetString("text") : "";
                childMessage.Time = DateTime.Parse(DBNull.Value != reader["time"] ? reader.GetString("time") : "");
                childMessage.UserId = DBNull.Value != reader["user_id"] ? Convert.ToInt32(reader.GetString("user_id")) : 0;
                childMessage.ParentId = message.Id;
                message.Child.Add(childMessage);
                message.ExistChild = System.Windows.Visibility.Visible;
            }
            Conection.DisConnectDB();

        }

        public static void SendMessage(string inputText,int channelId)
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

        public static void SendReplyMessage(string inputText, int channelId,int parentId)
        {
            if (null == LoginService.User) return;
            var command = new StringBuilder();
            command.Append("insert into t_message( text, channel_id, user_id, time, parent_message_id) values ( @text, @channel_id, @user_id, @time, @parent_message_id);");
            MySqlCommand cmd = new MySqlCommand(command.ToString(), Conection.ConnectDB());

            cmd.Parameters.Add(CreateParameter("@text", inputText, MySqlDbType.VarChar, 280));
            cmd.Parameters.Add(CreateParameter("@channel_id", channelId, MySqlDbType.Int32, 16));
            cmd.Parameters.Add(CreateParameter("@user_id", LoginService.User.Id, MySqlDbType.Int32, 16));
            cmd.Parameters.Add(CreateParameter("@time", DateTime.Now, MySqlDbType.DateTime, 16));
            cmd.Parameters.Add(CreateParameter("@parent_message_id", parentId, MySqlDbType.Int32, 16));

            cmd.ExecuteNonQuery();

            Conection.DisConnectDB();

        }

    }
}
