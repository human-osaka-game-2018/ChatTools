using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace WPF_Core.Infrastructure.Database
{
    public static class ChannelDAO
    {
        public static DataTable Get(IReadOnlyList<int> Ids)
        {
            if (Ids.Count <= 0) return null;

            using var mySqlConnection = Connection.Connect();

            mySqlConnection.Open();

            using var cmd = mySqlConnection.CreateCommand();
            var commandTextSB = new StringBuilder();
            commandTextSB.Append("SELECT * FROM m_channel WHERE id ");

            if (Ids.Count > 1)
            {
                commandTextSB.Append("IN (");

                foreach (var id in Ids)
                {
                    var channelIdParam = cmd.CreateParameter();
                    channelIdParam.MySqlDbType = MySqlDbType.Int64;
                    channelIdParam.Direction = ParameterDirection.Input;

                    var commandName = "@channel_id_" + id.ToString();

                    commandTextSB.Append($"{commandName}, ");

                    channelIdParam.ParameterName = commandName;
                    channelIdParam.Value = id;

                    cmd.Parameters.Add(channelIdParam);
                }

                const int CHAR_NUM_CAMMA_AND_SPACE = 2;

                commandTextSB.Remove(commandTextSB.Length - CHAR_NUM_CAMMA_AND_SPACE, CHAR_NUM_CAMMA_AND_SPACE);
                commandTextSB.Append(");");
            }
            else
            {
                var channelIdParam = cmd.CreateParameter();
                channelIdParam.MySqlDbType = MySqlDbType.Int64;
                channelIdParam.Direction = ParameterDirection.Input;

                commandTextSB.Append($"= { CHANNEL_ID};");

                channelIdParam.ParameterName = CHANNEL_ID;
                channelIdParam.Value = Ids[0];

                cmd.Parameters.Add(channelIdParam);
            }

            cmd.CommandText = commandTextSB.ToString();

            using var dataAdapter = new MySqlDataAdapter(cmd);
            using var dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            mySqlConnection.Close();

            return dataSet.Tables[0];
        }

        private const string CHANNEL_ID = "@channel_id";
    }
}
