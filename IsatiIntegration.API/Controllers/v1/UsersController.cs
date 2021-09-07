using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Models.Users;
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
        /// Get all users ordered by score
        /// </summary>
        /// <response code="401">You must be logged in to see ranking</response>
        /// <response code="200">Return the users ordered by score</response>
        [HttpGet("ranked")]
        public async Task<ActionResult<List<User>>> GetRankedUsers()
        {
            List<User> users;

            if (User.IsInRole(Role.Player))
            {
                users = await _usersService.GetRankedUsersForUser();
            }
            else
            {
                users = await _usersService.GetRankedUsersForAdmin();
            }

            return users;
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

        /// <summary>
        /// Get the user's profile picture
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401">You must be logged in</response>
        /// <response code="404">The user doesn't have any profile pictue</response>
        /// <response code="200">Return the file bytes</response>
        [HttpGet("{id:length(24)}/profile_picture")]
        public async Task<ActionResult<byte[]>> GetProfilePicture(string id)
        {
            var file = await _usersService.GetProfilePicture(id);

            if (file == null)
            {
                return NotFound();
            }

            return Ok(file);
        }

        /// <summary>
        /// Update a iser
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateModel"></param>
        /// <response code="400">You can't update this user this way</response>
        /// <response code="401">You must be logged in</response>
        /// <response code="403">You don't have the rights to update this user</response>
        /// <response code="200">The user has successfully been updated</response>
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateModel updateModel)
        {
            try
            {
                if (User.IsInRole(Role.Admin))
                {
                    await _usersService.UpdateUserFromAdmin(id, updateModel);
                }
                else if (id == User.Identity.Name)
                {
                    await _usersService.UpdateUserFromSelf(id, updateModel);
                }
                else
                {
                    return Forbid();
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest($"Error during the request: {e.Message}");
            }
        }
    }
}
