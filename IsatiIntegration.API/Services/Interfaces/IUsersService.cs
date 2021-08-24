using IsatiIntegration.API.Entities;
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
    }
}
