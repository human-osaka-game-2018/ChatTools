using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace WPF_Core.Infrastructure.Database
{
    public static class UserDAO
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

        public static DataTable GetWithMailAddress(string mailAddress)
        {
            using var mySqlConnection = Connection.Connect();

            mySqlConnection.Open();

            using var cmd = mySqlConnection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM {TABLE} WHERE {MAIL_ADDRESS} = @{MAIL_ADDRESS};";

            var mailAddressParam = cmd.CreateParameter();
            mailAddressParam.ParameterName = MAIL_ADDRESS;
            mailAddressParam.MySqlDbType = MySqlDbType.VarChar;
            mailAddressParam.Direction = ParameterDirection.Input;
            mailAddressParam.Value = mailAddress;
            cmd.Parameters.Add(mailAddressParam);

            using var dataAdapter = new MySqlDataAdapter(cmd);
            using var ret = new DataTable();
            dataAdapter.Fill(ret);

            return ret;
        }

        public static void ChangeOnlineState(int id, bool isOnline)
        {
            using var mySqlConnection = Connection.Connect();

            mySqlConnection.Open();

            var onlineStateCode = isOnline ? 1 : 0;

            using var cmd = mySqlConnection.CreateCommand();
            cmd.CommandText = $"UPDATE {TABLE} SET is_online = {onlineStateCode} WHERE({ID} = {id});";
            using var dataAdapter = new MySqlDataAdapter(cmd);
        }

        private const string TABLE = "m_user";
        private const string ID = "id";
        private const string MAIL_ADDRESS = "mail_address";
    }
}
