using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace OrmLiteDbConnectionTest
{
    class Program
    {
        static string connString = "Uid=root;Password=root;Server=localhost;Port=3307;Database=;SslMode=None;convert zero datetime=True";
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int nbr = 10;
            // await DoTestWithoutUsing(nbr);
            // await DoTestWithUsing(nbr);
            DoTestWithUsingInThreadPool(nbr);
            await Task.Delay(-1);
        }

        static void DoTestWithoutUsing(int nbr)
        {
            for (int i = 0; i < nbr; i++)
            {
                IDbConnection db = new MySqlConnection(connString);
                db.Open();
                var cmd = db.CreateCommand();
                cmd.CommandText = "SELECT 1";
                var reader = cmd.ExecuteReader();
                reader.Read();
                reader.GetInt32(0);
            }
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}\tDoTestWithoutUsing: DONE");
        }

        static void DoTestWithUsing(int nbr)
        {
            for (int i = 0; i < nbr; i++)
            {
                using IDbConnection db = new MySqlConnection(connString);
                db.Open();
                var cmd = db.CreateCommand();
                cmd.CommandText = "SELECT 1";
                var reader = cmd.ExecuteReader();
                reader.Read();
                reader.GetInt32(0);
            }
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}\tDoTestWithUsing: DONE");
        }

        static void DoTestWithUsingInThreadPool(int nbr)
        {
            for (int i = 0; i < nbr; i++)
            {
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    using IDbConnection db = new MySqlConnection(connString);
                    db.Open();
                    var cmd = db.CreateCommand();
                    cmd.CommandText = "SELECT 1";
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    reader.GetInt32(0);
                    reader.Close();
                    db.Close();
                });
            }
            Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff}\tDoTestWithUsingInThreadPool: DONE");
        }
    }
}
