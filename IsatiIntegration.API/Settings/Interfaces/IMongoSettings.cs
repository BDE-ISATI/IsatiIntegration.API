using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Settings.Interfaces
{
    public interface IMongoSettings
    {
        // The collections 
        string UsersCollectionName { get; set; }
        string TeamsCollectionName { get; set; }
        string SoloChallengesCollectionName { get; set; }

        // The database informations
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
