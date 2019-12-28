using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace WPF_Core.Infrastructure.Database
{
    public static class MessageDAO
    {
        public static DataTable Get(int channelId)
        {
            using var mySqlConnection = Connection.Connect();

            mySqlConnection.Open();

            using var cmd = mySqlConnection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM t_message WHERE channel_id = {CHANNEL_ID};";

            var channelIdParam = cmd.CreateParameter();
            channelIdParam.ParameterName = CHANNEL_ID;
            channelIdParam.MySqlDbType = MySqlDbType.Int64;
            channelIdParam.Direction = ParameterDirection.Input;
            channelIdParam.Value = channelId;
            cmd.Parameters.Add(channelIdParam);

            using var dataAdapter = new MySqlDataAdapter(cmd);
            using var ret = new DataTable();
            dataAdapter.Fill(ret);

            return ret;
        }

        private const string CHANNEL_ID = "@channel_id";
    }
}
