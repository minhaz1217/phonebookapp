using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PhonebookRead
{
    public class ConfigReader
    {
        private static ConfigReader instance = null;
        private static IConfigurationRoot config = null;

        private ConfigReader() { }
        public static ConfigReader Instance()
        {
            if(instance == null)
            {
                instance = new ConfigReader();
            }
            if(config == null)
            {
                config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build();
            }
            return instance;
        }
        public static T GetValue<T>(string name)
        {
            Instance();
            T val =  config.GetValue<T>(name);
            return val;
        }
    }
}
