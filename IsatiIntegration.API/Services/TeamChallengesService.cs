using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Models.TeamChallenges;
using IsatiIntegration.API.Services.Interfaces;
using IsatiIntegration.API.Settings.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Services
{
    public class TeamChallengesService : ITeamChallengesService
    {
        private readonly IMongoCollection<TeamChallenge> _teamChallenges;

        public TeamChallengesService(IMongoSettings mongoSettings)
        {
            var mongoClient = new MongoClient(mongoSettings.ConnectionString);
            var database = mongoClient.GetDatabase(mongoSettings.DatabaseName);

            _teamChallenges = database.GetCollection<TeamChallenge>(mongoSettings.TeamChallengesCollectionName);
        }

        public async Task<List<TeamChallenge>> GetChallengeFromAdmin()
        {
            var teamChallengesCursor = await _teamChallenges.FindAsync(
                dbTeamChallenge => true
            );

            return await teamChallengesCursor.ToListAsync();
        }

        public async Task<List<TeamChallenge>> GetChallengeFromPlayer()
        {
            DateTime now = DateTime.Now;

            var teamChallengesCursor = await _teamChallenges.FindAsync(dbTeamChallenge =>
                dbTeamChallenge.StartingDate <= now &&
                dbTeamChallenge.EndingDate >= now
            );

            return await teamChallengesCursor.ToListAsync();
        }

        public async Task<string> CreateChallenge(TeamChallengeModel toCreate)
        {
            TeamChallenge teamChallenge = new()
            {
                Title = toCreate.Title,
                Description = toCreate.Description,

                Value = toCreate.Value,
                NumberOfRepetitions = toCreate.NumberOfRepetitions,
                ShouldCountMembers = toCreate.ShouldCountMembers,

                StartingDate = toCreate.StartingDate,
                EndingDate = toCreate.EndingDate
            };

            await _teamChallenges.InsertOneAsync(teamChallenge);

            return teamChallenge.Id;

        }

        public async Task UpdateChallenge(string id, TeamChallengeModel toUpdate)
        {
            if (!TeamChallengeExist(id))
            {
                throw new ArgumentException("The challenge you want to update doesn't exist", nameof(id));
            }

            var update = Builders<TeamChallenge>.Update
                .Set(dbTeamChallenge => dbTeamChallenge.Title, toUpdate.Title)
                .Set(dbTeamChallenge => dbTeamChallenge.Description, toUpdate.Description)
                .Set(dbTeamChallenge => dbTeamChallenge.Value, toUpdate.Value)
                .Set(dbTeamChallenge => dbTeamChallenge.NumberOfRepetitions, toUpdate.NumberOfRepetitions)
                .Set(dbTeamChallenge => dbTeamChallenge.ShouldCountMembers, toUpdate.ShouldCountMembers)
                .Set(dbTeamChallenge => dbTeamChallenge.StartingDate, toUpdate.StartingDate)
                .Set(dbTeamChallenge => dbTeamChallenge.EndingDate, toUpdate.EndingDate);

            await _teamChallenges.UpdateOneAsync(dbTeamChallenge =>
                dbTeamChallenge.Id == id,
                update
            );
        }

        private bool TeamChallengeExist(string id)
        {
            return _teamChallenges.AsQueryable<TeamChallenge>().Any(dbTeamChallenge =>
                dbTeamChallenge.Id == id
            );
        }
    }
}
