using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Models.TeamChallenges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Services.Interfaces
{
    public interface ITeamChallengesService
    {
        Task<List<TeamChallenge>> GetChallengeFromAdmin();
        Task<List<TeamChallenge>> GetChallengeFromPlayer();

        Task<string> CreateChallenge(TeamChallengeModel toCreate);

        Task UpdateChallenge(string id, TeamChallengeModel toUpdate);
    }
}
