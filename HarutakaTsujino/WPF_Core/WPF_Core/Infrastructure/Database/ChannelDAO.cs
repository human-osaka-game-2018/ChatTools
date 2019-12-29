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
        public static DataTable Get(int id)
        {
            using var mySqlConnection = Connection.Connect();

            mySqlConnection.Open();

            using var dataAdapter = new MySqlDataAdapter(CreateCmdWithId(id, mySqlConnection));
            using var ret = new DataTable();
            dataAdapter.Fill(ret);

            return ret;
        }

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

            cmd.CommandText = $"SELECT * FROM m_channel WHERE {ID} = {id};";

            return cmd;
        }

        private static MySqlCommand CreateCmdWithIds(IEnumerable<int> ids, MySqlConnection mySqlConnection)
        {
            using var cmd = mySqlConnection.CreateCommand();

            var commandTextSB = new StringBuilder();
            commandTextSB.Append($"SELECT * FROM m_channel WHERE {ID} IN(");

            foreach (var id in ids)
            {
                commandTextSB.Append($"{id}, ");
            }

            // 一番最後のカンマとスペースはいらない
            const int CHAR_NUM_CAMMA_AND_SPACE = 2;

            commandTextSB.Remove(commandTextSB.Length - CHAR_NUM_CAMMA_AND_SPACE, CHAR_NUM_CAMMA_AND_SPACE);
            commandTextSB.Append(");");

            cmd.CommandText = commandTextSB.ToString();

            return cmd;
        }

        private const string ID = "id";
    }
}
