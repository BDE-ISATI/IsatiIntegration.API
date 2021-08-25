using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Models.TeamValidations;
using IsatiIntegration.API.Services.Interfaces;
using IsatiIntegration.API.Settings.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Services
{
    public class TeamValidationsService : ITeamValidationsService
    {
        private readonly IMongoCollection<TeamValidation> _validations;
        private readonly IMongoCollection<Team> _teams;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<TeamChallenge> _challenges;

        public TeamValidationsService(IMongoSettings mongoSettings)
        {
            var mongoClient = new MongoClient(mongoSettings.ConnectionString);
            var database = mongoClient.GetDatabase(mongoSettings.DatabaseName);

            _validations = database.GetCollection<TeamValidation>(mongoSettings.TeamValidationsCollectionName);
            _teams = database.GetCollection<Team>(mongoSettings.TeamsCollectionName);
            _users = database.GetCollection<User>(mongoSettings.UsersCollectionName);
            _challenges = database.GetCollection<TeamChallenge>(mongoSettings.TeamChallengesCollectionName);
        }

        public async Task<List<TeamValidation>> GetValidationsForTeam(string userId)
        {
            var user = await GetUserFromId(userId);

            if (user == null) throw new Exception("You appear to don't exist");

            var validationCursor = await _validations.FindAsync(dbValidation =>
                dbValidation.TeamId == user.TeamId 
            );

            return await validationCursor.ToListAsync();
        }

        public async Task<List<TeamValidation>> GetAllValidations()
        {
            var validationCursor = await _validations.FindAsync(dbValidation => true);

            return await validationCursor.ToListAsync();
        }

        public async Task ValidateSoloConfirmation(TeamValidationSubmitModel validateModel)
        {
            var challenge = await GetChallengeFromId(validateModel.ChallengeId);

            if (challenge == null) throw new Exception("The corresponding challenge doesn't exist");

            await _validations.InsertOneAsync(new()
            {
                ChallengeId = validateModel.ChallengeId,
                TeamId = validateModel.TeamId
            });

            var pointsToAdd = challenge.Value;

            if (challenge.ShouldCountMembers)
            {
                pointsToAdd *= validateModel.MembersCount;
            }

            // We must add points to team score
            var teamUpdate = Builders<Team>.Update
                .Set(dbTeam => dbTeam.Score, pointsToAdd + validateModel.ExtraPoints);

            await _teams.UpdateOneAsync(dbTeam => dbTeam.Id == validateModel.TeamId, teamUpdate);

        }

        private async Task<TeamChallenge> GetChallengeFromId(string id)
        {
            var challengeCursor = await _challenges.FindAsync(dbChallenge =>
                dbChallenge.Id == id
            );

            return await challengeCursor.FirstOrDefaultAsync();
        }

        private async Task<User> GetUserFromId(string id)
        {
            var userCursor = await _users.FindAsync(dbUser =>
                dbUser.Id == id
            );

            return await userCursor.FirstOrDefaultAsync();
        }

        private async Task<TeamValidation> GetValidationFromId(string id)
        {
            var validationCursor = await _validations.FindAsync(dbValiation =>
                dbValiation.Id == id
            );

            return await validationCursor.FirstOrDefaultAsync();
        }
    }
}
