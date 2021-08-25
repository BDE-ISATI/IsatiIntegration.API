using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Models.SoloChallenges;
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
    [Route("v1/challenges/solo")]
    [ApiController]
    public class SoloChallengesController : ControllerBase
    {
        private readonly ISoloChallengesService _soloChallengesService;

        public SoloChallengesController(ISoloChallengesService soloChallengesService)
        {
            _soloChallengesService = soloChallengesService;
        }

        /// <summary>
        /// Get a list of solo challenges
        /// </summary>
        /// <response code="401">You must be logged in to get solo challenges</response>
        /// <response code="200">Return the list of solo challenges for your role</response>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<SoloChallenge>>> GetSoloChallenges()
        {
            if (User.IsInRole(Role.Admin) || User.IsInRole(Role.Captain))
            {
                var adminResult = await _soloChallengesService.GetChallengeFromAdmin();

                return Ok(adminResult);
            }

            var playerResult = await _soloChallengesService.GetChallengeFromPlayer();

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
            var file = await _soloChallengesService.GetChallengeImage(id);

            if (file == null)
            {
                return NotFound();
            }

            return Ok(file);
        }

        /// <summary>
        /// Create a solo challenge
        /// </summary>
        /// <param name="creationModel"></param>
        /// <response code="401">Only admins can create solo challenges</response>
        /// <response code="200">Return the newly created solo challenge's id</response>
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        public async Task<ActionResult<string>> CreateSoloChallenge([FromBody] SoloChallengeModel creationModel)
        {
            var soloChallengeId = await _soloChallengesService.CreateChallenge(creationModel);

            return Ok(soloChallengeId);
        }

        /// <summary>
        /// Update a solo challenge
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateModel"></param>
        /// <response code="400">The challenge doesn't already exist (wrong id)</response>
        /// <response code="401">Only admins can update solo challenges</response>
        /// <response code="200">The solo challges has successfully been updated</response>
        [Authorize(Roles = Role.Admin)]
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateSoloChallenge(string id, [FromBody] SoloChallengeModel updateModel)
        {
            try
            {
                await _soloChallengesService.UpdateChallenge(id, updateModel);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest($"Error during the request: {e.Message}");
            }
        }
    }
}
