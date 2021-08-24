using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> LoginAsync(string email, string password);
        Task<string> RegisterAsync(RegisterModel registerModel);
    }
}
