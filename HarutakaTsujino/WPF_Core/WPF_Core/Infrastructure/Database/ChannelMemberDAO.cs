using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace WPF_Core.Infrastructure.Database
{
    public static class ChannelMemberDAO
    {
        public static DataTable GetIdsJoinedUser(int userId)
        {
            using var mySqlConnection = Connection.Connect();

            mySqlConnection.Open();

            using var cmd = mySqlConnection.CreateCommand();
            cmd.CommandText = $"SELECT channel_id FROM m_channel_member WHERE user_id = {USER_ID};";

            var userIdParam = cmd.CreateParameter();
            userIdParam.ParameterName = USER_ID;
            userIdParam.MySqlDbType = MySqlDbType.Int64;
            userIdParam.Direction = ParameterDirection.Input;
            userIdParam.Value = userId;
            cmd.Parameters.Add(userIdParam);

            using var dataAdapter = new MySqlDataAdapter(cmd);
            using var ret = new DataTable();
            dataAdapter.Fill(ret);

            return ret;
        }

        private const string USER_ID = "@user_id";
    }
}
