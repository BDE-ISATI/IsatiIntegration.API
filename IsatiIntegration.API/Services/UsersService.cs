using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Models.Users;
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

        private readonly IFilesService _filesService;

        public UsersService(IMongoSettings mongoSettings, IFilesService filesService)
        {
            var mongoClient = new MongoClient(mongoSettings.ConnectionString);
            var database = mongoClient.GetDatabase(mongoSettings.DatabaseName);

            _users = database.GetCollection<User>(mongoSettings.UsersCollectionName);

            _filesService = filesService;
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

        public async Task<byte[]> GetProfilePicture(string id)
        {
            var image = await _filesService.GetFileByName($"profile_picture_{id}");

            if (image == null) return null;

            return image.Data;
        }

        public async Task UpdateUserFromAdmin(string id, UserUpdateModel updateModel)
        {
            var user = await UpdatedUser(id, updateModel);

            user.TeamId = updateModel.TeamId;
            user.Role = updateModel.Role;
            user.Score = updateModel.Score;

            await _users.ReplaceOneAsync(dbUser => dbUser.Id == id, user);
        }

        public async Task UpdateUserFromSelf(string id, UserUpdateModel updateModel)
        {
            var user = await UpdatedUser(id, updateModel);

            await _users.ReplaceOneAsync(dbUser => dbUser.Id == id, user);
        }

        public async Task UpdateProfilePicture(string id, byte[] data)
        {
            await _filesService.UploadFile($"profile_picture_{id}", data);
        }

        public bool UserExist(string email)
        {
            return _users.AsQueryable().Any(dbUser =>
                dbUser.Email == email.ToLower()
            );
        }

        private async Task<User> UpdatedUser(string id, UserUpdateModel updateModel)
        {
            var user = await GetFullUser(id);

            if (user == null) throw new Exception("The user doesn't exist");

            user.FirstName = updateModel.FirstName;
            user.LastName = updateModel.LastName;

            if (updateModel.Password != null)
            {
                AuthenticationService.CreatePasswordHash(updateModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = Convert.ToBase64String(passwordHash);
                user.PasswordSalt = passwordSalt;
            }

            if (user.Email != updateModel.Email)
            {
                if (UserExist(updateModel.Email))
                {
                    throw new Exception("The email address is already in use");
                }

                user.Email = updateModel.Email;
            }

            if (updateModel.ProfilePicture != null)
            {
                await UpdateProfilePicture(id, updateModel.ProfilePicture);
            }

            return user;

        }
    }
}
