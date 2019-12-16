using ChatTool.Models.DomainObjects.Channel;
using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace ChatTool.Infrastructure.Database.ChannelDAO
{
    public static class ChannelDAO
    {
        public static List<Channel> GetChannels()
        {
            List<Channel> channels = new List<Channel>();

            using(var con = Connection.Connection.OpenDB())
            {
                string sql = "select * from m_channel;";
                var command = new MySqlCommand(sql.ToString(), con);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    channels.Add(new Channel()
                    {
                        Id = (int)reader["id"],
                        Name = (string)reader["channel_name"]
                    });
                }
            }

            return channels;
        }

        public static List<Channel> GetLoginUserChannels(int userId)
        {
            List<Channel> channels = new List<Channel>();

            using (var con = Connection.Connection.OpenDB())
            {
                var sql = new StringBuilder();
                sql.Append("select m_channel.id, m_channel.channel_name, m_channel_member.user_id from m_channel_member ");
                sql.Append("left join m_channel on ");
                sql.Append("m_channel_member.channel_id = m_channel.id ");
                sql.Append("where user_id in (@userId);");

                var command = new MySqlCommand(sql.ToString(), con);
                command.Parameters.Add(new MySqlParameter("userId", userId));

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    channels.Add(new Channel()
                    {
                        Id = (int)reader["id"],
                        Name = (string)reader["channel_name"]
                    });
                }
            }

            return channels;
        }
    }
}
