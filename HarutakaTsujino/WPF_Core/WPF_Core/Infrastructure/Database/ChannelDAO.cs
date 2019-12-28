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
            if (!ids.Any()) return null;

            using var mySqlConnection = Connection.Connect();

            mySqlConnection.Open();

            using var dataAdapter = new MySqlDataAdapter(CreateCmd(ids, mySqlConnection));
            using var ret = new DataTable();
            dataAdapter.Fill(ret);

            return ret;
        }

        private static MySqlCommand CreateCmd(IEnumerable<int> ids, MySqlConnection mySqlConnection)
        {
            return ids.Count() > 1 ?
                CreateCmdWithIds(ids, mySqlConnection) :
                CreateCmdWithId(ids.First(), mySqlConnection);
        }

        private static MySqlCommand CreateCmdWithId(int id, MySqlConnection mySqlConnection)
        {
            using var cmd = mySqlConnection.CreateCommand();

            cmd.CommandText = $"SELECT * FROM m_channel WHERE id = {CHANNEL_ID};";

            var channelIdParam = cmd.CreateParameter();
            channelIdParam.MySqlDbType = MySqlDbType.Int64;
            channelIdParam.Direction = ParameterDirection.Input;

            channelIdParam.ParameterName = CHANNEL_ID;
            channelIdParam.Value = id;

            cmd.Parameters.Add(channelIdParam);

            return cmd;
        }

        private static MySqlCommand CreateCmdWithIds(IEnumerable<int> ids, MySqlConnection mySqlConnection)
        {
            using var cmd = mySqlConnection.CreateCommand();

            var commandTextSB = new StringBuilder();
            commandTextSB.Append("SELECT * FROM m_channel WHERE id IN(");

            foreach (var id in ids)
            {
                var channelIdParam = cmd.CreateParameter();
                channelIdParam.MySqlDbType = MySqlDbType.Int64;
                channelIdParam.Direction = ParameterDirection.Input;

                var commandName = $"{CHANNEL_ID}_{id.ToString()}";

                commandTextSB.Append($"{commandName}, ");

                channelIdParam.ParameterName = commandName;
                channelIdParam.Value = id;

                cmd.Parameters.Add(channelIdParam);
            }

            // 一番最後のカンマとスペースはいらない
            const int CHAR_NUM_CAMMA_AND_SPACE = 2;

            commandTextSB.Remove(commandTextSB.Length - CHAR_NUM_CAMMA_AND_SPACE, CHAR_NUM_CAMMA_AND_SPACE);
            commandTextSB.Append(");");

            cmd.CommandText = commandTextSB.ToString();

            return cmd;
        }

        private const string CHANNEL_ID = "@channel_id";
    }
}
