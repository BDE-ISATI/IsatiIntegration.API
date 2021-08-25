using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Entities.SoloValidation;
using IsatiIntegration.API.Models.SoloValidations;
using IsatiIntegration.API.Services.Interfaces;
using IsatiIntegration.API.Settings.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Services
{
    public class SoloValidationsService : ISoloValidationsService
    {
        private readonly IMongoCollection<SoloValidation> _validations;
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Team> _teams;
        private readonly IMongoCollection<SoloChallenge> _challenges;

        private readonly IFilesService _filesService;

        public SoloValidationsService(IMongoSettings mongoSettings, IFilesService filesService)
        {
            var mongoClient = new MongoClient(mongoSettings.ConnectionString);
            var database = mongoClient.GetDatabase(mongoSettings.DatabaseName);

            _validations = database.GetCollection<SoloValidation>(mongoSettings.SoloValidationsCollectionName);
            _users = database.GetCollection<User>(mongoSettings.UsersCollectionName);
            _teams = database.GetCollection<Team>(mongoSettings.TeamsCollectionName);
            _challenges = database.GetCollection<SoloChallenge>(mongoSettings.SoloChallengesCollectionName);

            _filesService = filesService;
        }

        public async Task<List<SoloValidation>> GetValidationsForUser(string userId)
        {
            var validtionsCursor = await _validations.FindAsync(dbValidation =>
                dbValidation.UserId == userId && 
                dbValidation.Status != SoloValidationStatus.Rejected 
            );

            return await validtionsCursor.ToListAsync();
        }

        public async Task<List<SoloValidation>> GetValidationsForTeam(string userId)
        {
            var captain = await GetUserFromId(userId);

            if (captain == null) throw new Exception("The captain doesn't exist");

            var validationCursor = await _validations.FindAsync(dbValidation =>
                dbValidation.TeamId == captain.TeamId &&
                dbValidation.Status == SoloValidationStatus.Waiting
            );

            return await validationCursor.ToListAsync();
        }

        public async Task<List<SoloValidation>> GetAllValidations()
        {
            var validationCursor = await _validations.FindAsync(dbValidation => 
                dbValidation.Status == SoloValidationStatus.Waiting 
            );

            return await validationCursor.ToListAsync();
        }

        public async Task<byte[]> GetSoloValidationFile(string fileId)
        {
            var file = await _filesService.GetFileById(fileId);

            return file.Data;
        }

        public async Task<string> SubmitValidation(string userId, SoloValidationSubmitModel submitModel)
        {
            User user = await GetUserFromId(userId);

            if (user == null) throw new Exception("The user doesn't exist");

            Team team = await GetTeamFromId(user.TeamId);

            if (team == null) throw new Exception("The team doesn't exist");

            SoloValidation validation = new()
            {
                ChallengeId = submitModel.ChallengeId,
                UserId = user.Id,
                TeamId = team.Id,
                Status = SoloValidationStatus.Waiting,
                FilesIds = new List<String>()
            };

            int index = 1;
            foreach (var fileBytes in submitModel.Files)
            {
                string fileId = await _filesService.UploadFile($"validation_{user.Id}_{index}", fileBytes, false);
                validation.FilesIds.Add(fileId);

                ++index;
            }

            await _validations.InsertOneAsync(validation);

            return validation.Id;
        }

        public async Task ValidateSoloConfirmation(string id, SoloValidationValidateModel validateModel)
        {
            var validation = await GetValidationFromId(id);
            if (validation == null) throw new Exception("The validation doesn't exist in the database");

            var challenge = await GetChallengeFromId(validation.ChallengeId);
            if (challenge == null) throw new Exception("The corresponding challenge doesn't exist");

            var user = await GetUserFromId(validation.UserId);
            if (user == null) throw new Exception("The user doesn't exist");

            var team = await GetTeamFromId(validation.TeamId);
            if (team == null) throw new Exception("The team doesn't exist");

            validation.Status = SoloValidationStatus.Validated;

            await _validations.ReplaceOneAsync(dbValidation => dbValidation.Id == id, validation);

            // We must add points to the user and team score
            var userUpdate = Builders<User>.Update
                .Set(dbUser => dbUser.Score, user.Score + challenge.Value + validateModel.ExtraPoints);
            var teamUpdate = Builders<Team>.Update
                .Set(dbTeam => dbTeam.Score, team.Score + challenge.Value + validateModel.ExtraPoints);

            await _users.UpdateOneAsync(dbUser => dbUser.Id == validation.UserId, userUpdate);
            await _teams.UpdateOneAsync(dbTeam => dbTeam.Id == validation.TeamId, teamUpdate);
            
        }

        public async Task ValidationRejectConfirmation(string id)
        {
            var validation = await GetValidationFromId(id);

            if (validation == null) throw new Exception("The validation doesn't exist in the database");

            validation.Status = SoloValidationStatus.Rejected;

            await _validations.ReplaceOneAsync(dbValidation => dbValidation.Id == id, validation);
        }

        private async Task<User> GetUserFromId(string id)
        {
            var userCursor = await _users.FindAsync(dbUser =>
                dbUser.Id == id
            );

            return await userCursor.FirstOrDefaultAsync();
        }

        private async Task<Team> GetTeamFromId(string id)
        {
            var teamCursor = await _teams.FindAsync(dbTeam =>
                dbTeam.Id == id
            );

            return await teamCursor.FirstOrDefaultAsync();
        }

        private async Task<SoloChallenge> GetChallengeFromId(string id)
        {
            var challengeCursor = await _challenges.FindAsync(dbChallenge =>
                dbChallenge.Id == id
            );

            return await challengeCursor.FirstOrDefaultAsync();
        }

        private async Task<SoloValidation> GetValidationFromId(string id)
        {
            var validationCursor = await _validations.FindAsync(dbValiation =>
                dbValiation.Id == id
            );

            return await validationCursor.FirstOrDefaultAsync();
        }
    }
}
