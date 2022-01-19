using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace mysql_demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var executions = 100;
            for (var i = 0; i < executions; i++)
            {
                insert();
            }
            Console.WriteLine("complete!");
            Console.Read();
        }

        static void insert()
        {
            var dummyTable = new List<string>();
            var rowCount = 20000;
            for (var i = 0; i < rowCount; i++)
            {
                dummyTable.Add("INSERT INTO users (first_name, last_name, email) VALUES ('alec', 'holland', 'swampy@avatarofthegreen.org');");
            }
            Console.WriteLine("ok, dummy data created");

            var connString = "server=127.0.0.1;uid=root;pwd=password;database=connection_test";
            using (var conn = new MySqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("it's hammer time");
                    using (MySqlTransaction tran = conn.BeginTransaction(System.Data.IsolationLevel.Serializable))
                    {
                        for (var i = 0; i < rowCount; i++)
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = tran;
                                cmd.CommandText = dummyTable[i];
                                cmd.ExecuteNonQuery();
                            }
                        }
                        tran.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                conn.Close();
            }
        }

    }
}
