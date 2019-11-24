using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var useDb = Boolean.Parse(config["UseDatabase"]);
            Client client = new Client(useDb);
            client.AddStores();
            client.AddProducts();
            client.AddConsignment();
        }
    }
}