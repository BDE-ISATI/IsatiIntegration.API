using IsatiIntegration.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Services.Interfaces
{
    public interface ITeamsService
    {
        Task<Team> GetTeam(string id);
        Task<List<Team>> GetTeams();

        Task<string> CreateTeam(Team toCreate);
        Task UpdateTeam(string id, Team toUpdate);

    }
}
