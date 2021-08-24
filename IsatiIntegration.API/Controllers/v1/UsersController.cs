using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Controllers.v1
{
    [Authorize]
    [Route("v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        /// <summary>
        /// Get all users of the application
        /// </summary>
        /// <response code="401">You must be logged in</response>
        /// <response code="200">Return the list of application's users</response>
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _usersService.GetAllUsers(!User.IsInRole(Role.Admin));

            return Ok(users);
        }

        /// <summary>
        /// Get a specific user
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401">You must be logged in</response>
        /// <response code="404">The user doesn't exist</response>
        /// <response code="200">Return the user corresponding to the given id</response>
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            string currentUserId = User.Identity.Name;
            User user;

            if (User.IsInRole(Role.Admin) || currentUserId == id)
            {
                user = await _usersService.GetFullUser(id);
            }
            else
            {
                user = await _usersService.GetSensitiveUser(id);
            }

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }
    }
}
