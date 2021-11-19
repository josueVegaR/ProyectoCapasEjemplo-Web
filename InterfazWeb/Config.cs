using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace InterfazWeb
{
    public static class Config
    {
        public static string getConnectionString
        {
            get
            {
                return ConfigurationManager.AppSettings["ConnectionString"];
            }
        }
    }
}