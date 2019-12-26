using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace WPF_Core.Infrastructure.Database
{
    public static class UserDAO
    {
        public static DataTable? Get(string mailAddress)
        {
            using var mySqlConnection = Connection.Connect();

            if (mySqlConnection is null) return null;

            mySqlConnection.Open();

            using var cmd = mySqlConnection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM m_user WHERE mail_address = {MAIL_ADDRESS_PARAM_NAME};";

            var mailAddressParam = cmd.CreateParameter();
            mailAddressParam.ParameterName = MAIL_ADDRESS_PARAM_NAME;
            mailAddressParam.MySqlDbType = MySqlDbType.VarChar;
            mailAddressParam.Direction = ParameterDirection.Input;
            mailAddressParam.Value = mailAddress;
            cmd.Parameters.Add(mailAddressParam);

            using var dataAdapter = new MySqlDataAdapter(cmd);
            using var dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            mySqlConnection.Close();

            if (dataSet.Tables[0].Rows.Count == 0) return null;

            return dataSet.Tables[0];
        }

        public static void ChangeOnlineState(int id, bool isOnline)
        {
            using var mySqlConnection = Connection.Connect();

            if (mySqlConnection is null) return;

            mySqlConnection.Open();

            var onlineStateCode = isOnline ? 1 : 0;

            using var cmd = mySqlConnection.CreateCommand();
            cmd.CommandText = $"UPDATE m_user SET is_online = {onlineStateCode} WHERE(id = {id});";
            using var dataAdapter = new MySqlDataAdapter(cmd);

            mySqlConnection.Close();
        }

        private const string MAIL_ADDRESS_PARAM_NAME = "@mail_address";
    }
}
