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

        public SoloChallengesService(IMongoSettings mongoSettings)
        {
            var mongoClient = new MongoClient(mongoSettings.ConnectionString);
            var database = mongoClient.GetDatabase(mongoSettings.DatabaseName);

            _soloChallenges = database.GetCollection<SoloChallenge>(mongoSettings.SoloChallengesCollectionName);
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
                dbSoloChallenge.StartingDate >= now &&
                dbSoloChallenge.EndingDate <= now
            );

            return await soloChallengesCursor.ToListAsync();
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
        }

        private bool SoloChallengeExist(string id)
        {
            return _soloChallenges.AsQueryable<SoloChallenge>().Any(dbSoloChallenge =>
                dbSoloChallenge.Id == id
            );
        }
    }
}
