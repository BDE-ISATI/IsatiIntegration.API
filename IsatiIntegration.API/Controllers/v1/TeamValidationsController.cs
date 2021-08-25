using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Models.TeamValidations;
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
    [Route("v1/validations/team")]
    [ApiController]
    public class TeamValidationsController : ControllerBase
    {
        private readonly ITeamValidationsService _teamValidationsService;

        public TeamValidationsController(ITeamValidationsService teamValidationsService)
        {
            _teamValidationsService = teamValidationsService;
        }

        /// <summary>
        /// Get validations for team challenge
        /// </summary>
        /// <response code="400">Their was an error during the request</response>
        /// <response code="401">You must be logged in</response>
        /// <response code="200">Return the validations accessible to the user</response>
        [HttpGet]
        public async Task<ActionResult<List<TeamValidation>>> GetValidations()
        {
            string currentUserId = User.Identity.Name;

            try
            {
                List<TeamValidation> validations;

                if (User.IsInRole(Role.Admin))
                {
                    validations = await _teamValidationsService.GetAllValidations();
                }
                else
                {
                    validations = await _teamValidationsService.GetValidationsForTeam(currentUserId);
                }

                return Ok(validations);
            }
            catch (Exception e)
            {
                return BadRequest($"Can't get the validatons: {e.Message}");
            }
        }

        /// <summary>
        /// Mark a solo validation as accepted
        /// </summary>
        /// <param name="acceptModel"></param>
        /// <response code="400">Their was an error during the validation</response>
        /// <response code="401">You must be logged in</response>
        /// <response code="200">The validation is now accepted</response>
        [HttpPost()]
        public async Task<IActionResult> AcceptValidation([FromBody] TeamValidationSubmitModel acceptModel)
        {
            try
            {
                await _teamValidationsService.ValidateSoloConfirmation(acceptModel);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest($"Can't accept validation : {e.Message}");
            }

        }
    }
}
