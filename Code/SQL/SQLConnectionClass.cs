using System;
using System.Data;
using System.Data.SqlClient;

namespace template.SQL
{
    public class SQLConnectionClass
    {
        public static string StaticConnectionString { get; set; }


        public static SqlConnection SQLConnect()
        {
            try
            {
                SqlConnection SQLCon = new SqlConnection(StaticConnectionString);
                SQLCon.Open();
                return SQLCon;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
