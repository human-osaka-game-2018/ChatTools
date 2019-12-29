using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace WPF_Core.Infrastructure.Database
{
    public static class MessageDAO
    {
        public static DataTable GetWithId(int id)
        {
            using var mySqlConnection = Connection.Connect();

            mySqlConnection.Open();

            using var cmd = mySqlConnection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM {TABLE} WHERE {ID} = {id};";

            using var dataAdapter = new MySqlDataAdapter(cmd);
            using var ret = new DataTable();
            dataAdapter.Fill(ret);

            return ret;
        }

        public static DataTable GetWithChannelId(int channelId)
        {
            using var mySqlConnection = Connection.Connect();

            mySqlConnection.Open();

            using var cmd = mySqlConnection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM {TABLE} WHERE {CHANNEL_ID} = {channelId};";

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

            cmd.CommandText = $"INSERT INTO {TABLE} " +
                $"({CHANNEL_ID}, {USER_ID}, {TEXT}, {TIME}, {PARENT_MESSAGE_ID}) " +
                $"VALUES ({channelId}, {userId}, @{TEXT}, @{TIME}, @{PARENT_MESSAGE_ID});";

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

        private const string TABLE = "t_message";
        private const string ID = "id";
        private const string CHANNEL_ID = "channel_id";
        private const string USER_ID = "user_id";
        private const string TEXT = "text";
        private const string TIME = "time";
        private const string PARENT_MESSAGE_ID = "parent_message_id";
    }
}
