using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Services.Interfaces;
using IsatiIntegration.API.Settings.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Services
{
    public class TeamsService : ITeamsService
    {
        private readonly IMongoCollection<Team> _teams;
        private readonly IMongoCollection<User> _users;

        public TeamsService(IMongoSettings mongoSettings)
        {
            var mongoClient = new MongoClient(mongoSettings.ConnectionString);
            var database = mongoClient.GetDatabase(mongoSettings.DatabaseName);

            _teams = database.GetCollection<Team>(mongoSettings.TeamsCollectionName);
            _users = database.GetCollection<User>(mongoSettings.UsersCollectionName);
        }

        public async Task<Team> GetTeam(string id)
        {
            var teamCursor = await _teams.FindAsync(dbTeam =>
                dbTeam.Id == id
            );

            return await teamCursor.FirstOrDefaultAsync();
        }

        public async Task<List<Team>> GetTeams()
        {
            var teamsCursor = await _teams.FindAsync(dbTeam => true);

            return await teamsCursor.ToListAsync();
        }

        public async Task<string> CreateTeam(Team toCreate)
        {
            await _teams.InsertOneAsync(toCreate);

            if (toCreate.CaptainId != null)
            {
                await UpdateCaptainData(toCreate.CaptainId, toCreate.Id);
            }

            return toCreate.Id;
        }

        public async Task UpdateTeam(string id, Team toUpdate)
        {
            await _teams.ReplaceOneAsync(dbTeam =>
                dbTeam.Id == id,
                toUpdate
            );

            if (toUpdate.CaptainId != null)
            {
                await UpdateCaptainData(toUpdate.CaptainId, id);
            }
        }

        private async Task UpdateCaptainData(string captainId, string teamId)
        {
            var userUpdate = Builders<User>.Update
                    .Set(dbUser => dbUser.Role, Role.Captain)
                    .Set(dbUser => dbUser.TeamId, teamId);

            await _users.UpdateOneAsync(dbUser =>
                dbUser.Id == captainId,
                userUpdate
            );
        }
    }
}
