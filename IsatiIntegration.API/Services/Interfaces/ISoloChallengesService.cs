using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Models.SoloChallenges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Services.Interfaces
{
    public interface ISoloChallengesService
    {
        Task<List<SoloChallenge>> GetChallengeFromAdmin();
        Task<List<SoloChallenge>> GetChallengeFromPlayer();

        Task<string> CreateChallenge(SoloChallengeModel toCreate);

        Task UpdateChallenge(string id, SoloChallengeModel toUpdate);
    }
}
