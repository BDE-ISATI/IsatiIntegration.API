using IsatiIntegration.API.Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Settings
{
    public class MongoSettings : IMongoSettings
    {
        // The collections 
        public string UsersCollectionName { get; set; }


        // The database informations
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
