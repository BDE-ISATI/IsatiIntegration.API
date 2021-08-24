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
    public class UsersService : IUsersService
    {
        private readonly IMongoCollection<User> _users;


        public UsersService(IMongoSettings mongoSettings)
        {
            var mongoClient = new MongoClient(mongoSettings.ConnectionString);
            var database = mongoClient.GetDatabase(mongoSettings.DatabaseName);

            _users = database.GetCollection<User>(mongoSettings.UsersCollectionName);
        }

        public async Task<User> GetFullUser(string id)
        {
            var userCursor = await _users.FindAsync(dbUser =>
                dbUser.Id == id
            );

            return await userCursor.FirstOrDefaultAsync();
        }
        public async Task<User> GetSensitiveUser(string id)
        {
            var fullUser = await GetFullUser(id);

            // We need to remove sensitive field
            fullUser.Email = null;

            return fullUser;
        }

        public async Task<List<User>> GetAllUsers(bool removeSensitiveInformations = true)
        {
            var usersCursor = await _users.FindAsync(dbUser => true);
            var users = await usersCursor.ToListAsync();

            if (removeSensitiveInformations)
            {
                foreach (var user in users)
                {
                    user.Email = null;
                }
            }

            return users;
        }
    }
}
