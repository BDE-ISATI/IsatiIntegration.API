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
    public class TeamsController : ControllerBase
    {
        private readonly ITeamsService _teamsService;

        public TeamsController(ITeamsService teamsService)
        {
            _teamsService = teamsService;
        }
 
        /// <summary>
        /// Get all teams
        /// </summary>
        /// <response code="401">You must be logged in</response>
        /// <response code="200">Return a list of the teams</response>
        [HttpGet]
        public async Task<ActionResult<List<Team>>> GetTeams()
        {
            var teams = await _teamsService.GetTeams();

            return Ok(teams);
        }

        /// <summary>
        /// Get all teams ordered by score
        /// </summary>
        /// <response code="401">You must be logged in to see ranking</response>
        /// <response code="200">Return the teams ordered by score</response>
        [HttpGet("ranked")]
        public async Task<ActionResult<List<Team>>> GetRankedTeams()
        {
            List<Team> teams;

            if (User.IsInRole(Role.Player))
            {
                teams = await _teamsService.GetRankedTeamsForUser();
            }
            else
            {
                teams = await _teamsService.GetRankedTeamsForAdmin();
            }

            return teams;
        }

        /// <summary>
        /// Get a team
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401">You must be logged in</response>
        /// <response code="404">The team doesn't exist</response>
        /// <response code="200">Return the team</response>
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Team>> GetTeam(string id)
        {
            var team = await _teamsService.GetTeam(id);

            if (team != null)
            {
                return Ok(team);
            }

            return NotFound();
        }

        /// <summary>
        /// Create a team
        /// </summary>
        /// <param name="toCreate"></param>
        /// <response code="401">You must be logged in</response>
        /// <response code="403">Only admin can create teams</response>
        /// <response code="200">Return the newly created team's id</response>
        [HttpPost]
        public async Task<ActionResult<string>> CreateTeam([FromBody] Team toCreate)
        {
            if (User.IsInRole(Role.Admin))
            {
                string id = await _teamsService.CreateTeam(toCreate);

                return Ok(id);
            }

            return Forbid();
        }

        /// <summary>
        /// Update a team
        /// </summary>
        /// <param name="id"></param>
        /// <param name="toUpdate"></param>
        /// <reponse code="401">You must be logged in</reponse>
        /// <response code="403">Only admin can update teams</response>
        /// <response code="200">The team has been successfully updated</response>
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateTeam(string id, [FromBody] Team toUpdate)
        {
            toUpdate.Id = id;

            if (User.IsInRole(Role.Admin))
            {
                await _teamsService.UpdateTeam(id, toUpdate);

                return Ok();
            }

            return Forbid();
        }
    }
}
