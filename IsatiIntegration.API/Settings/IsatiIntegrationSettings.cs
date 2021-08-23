using IsatiIntegration.API.Settings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Settings
{
    public class IsatiIntegrationSettings : IIsatiIntegrationSettings
    {   
        public string ApiSecret { get; set; }
    }
}
