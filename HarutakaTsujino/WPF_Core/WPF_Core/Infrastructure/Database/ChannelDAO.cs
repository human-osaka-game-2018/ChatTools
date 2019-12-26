using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WPF_Core.Infrastructure.Database
{
    public static class ChannelDAO
    {
        public static DataTable? Get(IEnumerable<int> ids)
        {
            if (ids.Count() <= 0) return null;

            using var mySqlConnection = Connection.Connect();

            if (mySqlConnection is null) return null;

            mySqlConnection.Open();

            using var dataAdapter = new MySqlDataAdapter(CreateCmd(ids, mySqlConnection));
            using var dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            mySqlConnection.Close();

            if (dataSet.Tables[0].Rows.Count == 0) return null;

            return dataSet.Tables[0];
        }

        private static MySqlCommand CreateCmd(IEnumerable<int> ids, MySqlConnection mySqlConnection)
        {
            using var cmd = mySqlConnection.CreateCommand();
            var commandTextSB = new StringBuilder();
            commandTextSB.Append("SELECT * FROM m_channel WHERE id ");

            if (ids.Count() > 1)
            {
                commandTextSB.Append("IN (");

                foreach (var id in ids)
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

                // 一番最後のカンマとスペースはいらない
                const int CHAR_NUM_CAMMA_AND_SPACE = 2;

                commandTextSB.Remove(commandTextSB.Length - CHAR_NUM_CAMMA_AND_SPACE, CHAR_NUM_CAMMA_AND_SPACE);
                commandTextSB.Append(");");
            }
            else
            {
                var channelIdParam = cmd.CreateParameter();
                channelIdParam.MySqlDbType = MySqlDbType.Int64;
                channelIdParam.Direction = ParameterDirection.Input;

                commandTextSB.Append($"= {CHANNEL_ID};");

                channelIdParam.ParameterName = CHANNEL_ID;
                channelIdParam.Value = ids.First();

                cmd.Parameters.Add(channelIdParam);
            }

            cmd.CommandText = commandTextSB.ToString();

            return cmd;
        }

        private const string CHANNEL_ID = "@channel_id";
    }
}
