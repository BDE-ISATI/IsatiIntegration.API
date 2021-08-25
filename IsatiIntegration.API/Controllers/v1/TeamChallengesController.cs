using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Models.TeamChallenges;
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
    [Route("v1/challenges/team")]
    [ApiController]
    public class TeamChallengesController : ControllerBase
    {
        private readonly ITeamChallengesService _teamChallengesService;

        public TeamChallengesController(ITeamChallengesService teamChallengesService)
        {
            _teamChallengesService = teamChallengesService;
        }

        /// <summary>
        /// Get a list of team challenges
        /// </summary>
        /// <response code="401">You must be logged in to get team challenges</response>
        /// <response code="200">Return the list of team challenges for your role</response>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<TeamChallenge>>> GetTeamChallenges()
        {
            if (User.IsInRole(Role.Admin) || User.IsInRole(Role.Captain))
            {
                var adminResult = await _teamChallengesService.GetChallengeFromAdmin();

                return Ok(adminResult);
            }

            var playerResult = await _teamChallengesService.GetChallengeFromPlayer();

            return Ok(playerResult);
        }

        /// <summary>
        /// Get the challenge image
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401">You must be logged in</response>
        /// <response code="404">The challenge doesn't have any image</response>
        /// <response code="200">Return the file bytes</response>
        [HttpGet("{id:length(24)}/image")]
        public async Task<ActionResult<byte[]>> GetImage(string id)
        {
            var file = await _teamChallengesService.GetChallengeImage(id);

            if (file == null)
            {
                return NotFound();
            }

            return Ok(file);
        }

        /// <summary>
        /// Create a team challenge
        /// </summary>
        /// <param name="creationModel"></param>
        /// <response code="401">Only admins can create team challenges</response>
        /// <response code="200">Return the newly created team challenge's id</response>
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<ActionResult<string>> CreateTeamChallenge([FromBody] TeamChallengeModel creationModel)
        {
            var teamChallengeId = await _teamChallengesService.CreateChallenge(creationModel);

            return Ok(teamChallengeId);
        }

        /// <summary>
        /// Update a team challenge
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateModel"></param>
        /// <response code="400">The challenge doesn't already exist (wrong id)</response>
        /// <response code="401">Only admins can update team challenges</response>
        /// <response code="200">The team challenges has successfully been updated</response>
        [Authorize(Roles = Role.Admin)]
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateTeamChallenge(string id, [FromBody] TeamChallengeModel updateModel)
        {
            try
            {
                await _teamChallengesService.UpdateChallenge(id, updateModel);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest($"Error during the request: {e.Message}");
            }
        }
    }
}
