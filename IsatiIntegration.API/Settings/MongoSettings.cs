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
        public string FormsCollectionName { get; set; }
        public string TeamsCollectionName { get; set; }
        public string SoloChallengesCollectionName { get; set; }
        public string SoloValidationsCollectionName { get; set; }
        public string TeamChallengesCollectionName { get; set; }
        public string TeamValidationsCollectionName { get; set; }
        public string AppSettingsCollectionName { get; set; }


        // The database informations
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
