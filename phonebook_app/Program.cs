using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using phonebook_app.Persistence;
using phonebook_app_read;

namespace phonebook_app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string dapperConnection = ConfigReader.GetValue<string>("DapperConnectionString");
            var db = new DBRepository(dapperConnection);
            if (!db.TableExists("phonebook"))
            {
                db.CreateTable("CREATE TABLE phonebook(id text,name text,number text, PRIMARY KEY(id)); ");
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
