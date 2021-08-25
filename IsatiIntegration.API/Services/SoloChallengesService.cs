using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Models.SoloChallenges;
using IsatiIntegration.API.Services.Interfaces;
using IsatiIntegration.API.Settings.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Services
{
    public class SoloChallengesService : ISoloChallengesService
    {
        private readonly IMongoCollection<SoloChallenge> _soloChallenges;

        private readonly IFilesService _filesService;

        public SoloChallengesService(IMongoSettings mongoSettings, IFilesService filesService)
        {
            var mongoClient = new MongoClient(mongoSettings.ConnectionString);
            var database = mongoClient.GetDatabase(mongoSettings.DatabaseName);

            _soloChallenges = database.GetCollection<SoloChallenge>(mongoSettings.SoloChallengesCollectionName);

            _filesService = filesService;
        }

        public async Task<List<SoloChallenge>> GetChallengeFromAdmin()
        {
            var soloChallengesCursor = await _soloChallenges.FindAsync(
                dbSoloChallenge => true
            );

            return await soloChallengesCursor.ToListAsync();
        }

        public async Task<List<SoloChallenge>> GetChallengeFromPlayer()
        {
            DateTime now = DateTime.Now;

            var soloChallengesCursor = await _soloChallenges.FindAsync(dbSoloChallenge =>
                dbSoloChallenge.StartingDate <= now &&
                dbSoloChallenge.EndingDate >= now
            );

            return await soloChallengesCursor.ToListAsync();
        }

        public async Task<byte[]> GetChallengeImage(string id)
        {
            var image = await _filesService.GetFileByName($"solo_challenge_{id}");

            if (image == null) return null;

            return image.Data;
        }

        public async Task<string> CreateChallenge(SoloChallengeModel toCreate)
        {
            SoloChallenge soloChallenge = new()
            {
                Title = toCreate.Title,
                Description = toCreate.Description,

                Value = toCreate.Value,
                NumberOfRepetitions = toCreate.NumberOfRepetitions,

                StartingDate = toCreate.StartingDate,
                EndingDate = toCreate.EndingDate
            };

            await _soloChallenges.InsertOneAsync(soloChallenge);

            if (toCreate.ChallengeImage != null)
            {
                await UpdateChallengeImage(soloChallenge.Id, toCreate.ChallengeImage);
            }

            return soloChallenge.Id;

        }

        public async Task UpdateChallenge(string id, SoloChallengeModel toUpdate)
        {
            if (!SoloChallengeExist(id))
            {
                throw new ArgumentException("The challenge you want to update doesn't exist", nameof(id));
            }

            var update = Builders<SoloChallenge>.Update
                .Set(dbSoloChallenge => dbSoloChallenge.Title, toUpdate.Title)
                .Set(dbSoloChallenge => dbSoloChallenge.Description, toUpdate.Description)
                .Set(dbSoloChallenge => dbSoloChallenge.Value, toUpdate.Value)
                .Set(dbSoloChallenge => dbSoloChallenge.NumberOfRepetitions, toUpdate.NumberOfRepetitions)
                .Set(dbSoloChallenge => dbSoloChallenge.StartingDate, toUpdate.StartingDate)
                .Set(dbSoloChallenge => dbSoloChallenge.EndingDate, toUpdate.EndingDate);

            await _soloChallenges.UpdateOneAsync(dbSoloChallenge =>
                dbSoloChallenge.Id == id,
                update
            );

            if (toUpdate.ChallengeImage != null)
            {
                await UpdateChallengeImage(id, toUpdate.ChallengeImage);
            }
        }

        private async Task UpdateChallengeImage(string id, byte[] data)
        {
            await _filesService.UploadFile($"solo_challenge_{id}", data);
        }

        private bool SoloChallengeExist(string id)
        {
            return _soloChallenges.AsQueryable<SoloChallenge>().Any(dbSoloChallenge =>
                dbSoloChallenge.Id == id
            );
        }
    }
}
