using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace WPF_Core.Infrastructure.Database
{
    public static class ChannelDAO
    {
        public static DataTable GetJoinedUser(int userId)
        {
            var channelIdsJoinedUser = GetIdsJoinedUser(userId);

            using var mySqlConnection = Connection.Connect();

            mySqlConnection.Open();

            DataTable channelTable = null;

            foreach (DataRow channelIdRow in channelIdsJoinedUser.Rows)
            {
                using var cmd = mySqlConnection.CreateCommand();
                cmd.CommandText = $"SELECT * FROM m_channel WHERE id = {CHANNEL_ID};";

                var channelIdParam = cmd.CreateParameter();
                channelIdParam.ParameterName = CHANNEL_ID;
                channelIdParam.MySqlDbType = MySqlDbType.Int64;
                channelIdParam.Direction = ParameterDirection.Input;
                channelIdParam.Value = channelIdRow[0];
                cmd.Parameters.Add(channelIdParam);

                using var dataAdapter = new MySqlDataAdapter(cmd);
                using var dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                if (channelTable == null)
                {
                    channelTable = dataSet.Tables[0].Clone();
                }

                var dataRowTmp = channelTable.NewRow();
                dataRowTmp.ItemArray = dataSet.Tables[0].Rows[0].ItemArray;
                channelTable.Rows.Add(dataRowTmp);
            }

            mySqlConnection.Close();

            return channelTable;
        }

        private static DataTable GetIdsJoinedUser(int userId)
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
            using var dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            mySqlConnection.Close();

            return dataSet.Tables[0];
        }

        private const string USER_ID = "@user_id";
        private const string CHANNEL_ID = "@channel_id";
    }
}
