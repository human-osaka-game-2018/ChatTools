using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTool.Infrastructure.Database
{
    class DAO
    {
        public static MySqlParameter CreateParameter(string paramName, object value, MySqlDbType dbType, int size)
        {
            var mysqlParam = new MySqlParameter(paramName, dbType, size);
            mysqlParam.Value = value;
            return mysqlParam;
        }
    }
}
