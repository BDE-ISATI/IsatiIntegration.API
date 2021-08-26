using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Services.Interfaces
{
    public interface IUsersService
    {
        Task<User> GetFullUser(string id);
        Task<User> GetSensitiveUser(string id);
        Task<List<User>> GetAllUsers(bool removeSensitiveInformation = true);

        Task<byte[]> GetProfilePicture(string id);

        Task UpdateUserFromAdmin(string id, UserUpdateModel updateModel);
        Task UpdateUserFromSelf(string id, UserUpdateModel updateModel);

        Task UpdateProfilePicture(string id, byte[] data);
        bool UserExist(string email);

    }
}
