using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Settings.Interfaces
{
    public interface IIsatiIntegrationSettings
    {
        string ApiSecret { get; set; }
    }
}
