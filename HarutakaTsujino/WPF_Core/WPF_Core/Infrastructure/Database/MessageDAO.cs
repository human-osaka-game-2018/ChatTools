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
            cmd.CommandText = $"SELECT * FROM t_message WHERE channel_id = @{CHANNEL_ID};";

            var channelIdParam = cmd.CreateParameter();
            channelIdParam.ParameterName = $"@{CHANNEL_ID}";
            channelIdParam.MySqlDbType = MySqlDbType.Int64;
            channelIdParam.Direction = ParameterDirection.Input;
            channelIdParam.Value = channelId;
            cmd.Parameters.Add(channelIdParam);

            using var dataAdapter = new MySqlDataAdapter(cmd);
            using var ret = new DataTable();
            dataAdapter.Fill(ret);

            return ret;
        }

        public static void Post(string text, int userId, int channelId, int? parentMessageId = null)
        {
            using var mySqlConnection = Connection.Connect();

            using var cmd = mySqlConnection.CreateCommand();
            cmd.Connection.Open();

            cmd.CommandText = $"INSERT INTO t_message " +
                $"({CHANNEL_ID}, {USER_ID}, {TEXT}, {TIME}, {PARENT_MESSAGE_ID}) " +
                $"VALUES (@{CHANNEL_ID}, @{USER_ID}, @{TEXT}, @{TIME}, @{PARENT_MESSAGE_ID});";

            var channelIdParam = cmd.CreateParameter();
            channelIdParam.ParameterName = $"@{CHANNEL_ID}";
            channelIdParam.MySqlDbType = MySqlDbType.Int64;
            channelIdParam.Direction = ParameterDirection.Input;
            channelIdParam.Value = channelId;
            cmd.Parameters.Add(channelIdParam);

            var userIdParam = cmd.CreateParameter();
            userIdParam.ParameterName = $"@{USER_ID}";
            userIdParam.MySqlDbType = MySqlDbType.Int64;
            userIdParam.Direction = ParameterDirection.Input;
            userIdParam.Value = userId;
            cmd.Parameters.Add(userIdParam);

            var textParam = cmd.CreateParameter();
            textParam.ParameterName = $"@{TEXT}";
            textParam.MySqlDbType = MySqlDbType.VarChar;
            textParam.Direction = ParameterDirection.Input;
            textParam.Value = text;
            cmd.Parameters.Add(textParam);

            var timeParam = cmd.CreateParameter();
            timeParam.ParameterName = $"@{TIME}";
            timeParam.MySqlDbType = MySqlDbType.DateTime;
            timeParam.Direction = ParameterDirection.Input;
            timeParam.Value = DateTime.Now;
            cmd.Parameters.Add(timeParam);

            var parentMessageIdParam = cmd.CreateParameter();
            parentMessageIdParam.ParameterName = $"@{PARENT_MESSAGE_ID}";
            parentMessageIdParam.MySqlDbType = MySqlDbType.Int64;
            parentMessageIdParam.Direction = ParameterDirection.Input;
            parentMessageIdParam.Value = parentMessageId;
            cmd.Parameters.Add(parentMessageIdParam);

            cmd.ExecuteNonQuery();
        }

        private const string CHANNEL_ID = "channel_id";
        private const string USER_ID = "user_id";
        private const string TEXT = "text";
        private const string TIME = "time";
        private const string PARENT_MESSAGE_ID = "parent_message_id";
    }
}
