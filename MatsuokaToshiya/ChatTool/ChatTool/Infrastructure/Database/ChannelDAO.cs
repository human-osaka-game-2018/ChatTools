using ChatTool.Models.DomainObjects;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChatTool.Infrastructure.Database
{
    class ChannelDAO : DAO
    {
        public void ChannelList(ObservableCollection<Channel> list)
        {
            var cmd = new MySqlCommand("select * from m_channel;", Conection.ConnectDB());
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var channel = new Channel();
                channel.Id = DBNull.Value != reader["Id"] ? Convert.ToInt32(reader.GetString("Id")) : 0;
                channel.Name = DBNull.Value != reader["channel_name"] ? reader.GetString("channel_name") : "";
                list.Add(channel);
            }
            Conection.DisConnectDB();
        }

        public void ParticipatedUser(ObservableCollection<Channel> list,int id)
        {
            var cmd = new MySqlCommand("select * from m_channel_member where user_id = @user_id;", Conection.ConnectDB());
            cmd.Parameters.Add(CreateParameter("@user_id", id, MySqlDbType.Int32, 16));
            var reader = cmd.ExecuteReader();
            ObservableCollection<ChannelMember> channelMembers = new ObservableCollection<ChannelMember>();
            while (reader.Read())
            {
                var channelMember = new ChannelMember();
                channelMember.Id = DBNull.Value != reader["Id"] ? Convert.ToInt32(reader.GetString("Id")) : 0;
                channelMember.ChannelId = DBNull.Value != reader["channel_id"] ? Convert.ToInt32(reader.GetString("channel_id")) : 0;
                channelMembers.Add(channelMember);
            }
            Conection.DisConnectDB();
            
            var command = new StringBuilder();
            command.Append("select * from m_channel where id in( ");
            for (int i=0;i< channelMembers.Count;i++) {
                if (i == 0)
                {
                    command.Append($"{channelMembers[i].ChannelId}");
                }
                command.Append("," + $"{channelMembers[i].ChannelId}");
            }
            command.Append(");");
            cmd = new MySqlCommand(command.ToString(), Conection.ConnectDB());
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var channel = new Channel();
                channel.Id = DBNull.Value != reader["Id"] ? Convert.ToInt32(reader.GetString("Id")) : 0;
                channel.Name = DBNull.Value != reader["channel_name"] ? reader.GetString("channel_name") : "";
                list.Add(channel);
            }
            Conection.DisConnectDB();

        }
    }
}
