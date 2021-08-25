using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Entities.SoloValidation;
using IsatiIntegration.API.Models.SoloValidations;
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
    [Route("v1/validations/solo")]
    [ApiController]
    public class SoloValidationsController : ControllerBase
    {
        private readonly ISoloValidationsService _soloValidationsService;

        public SoloValidationsController(ISoloValidationsService soloValidationsService)
        {
            _soloValidationsService = soloValidationsService;
        }

        /// <summary>
        /// Get validations for solo challenge
        /// </summary>
        /// <response code="400">Their was an error during the request</response>
        /// <response code="401">You must be logged in</response>
        /// <response code="200">Return the validations accessible to the user</response>
        [HttpGet]
        public async Task<ActionResult<List<SoloValidation>>> GetValidations()
        {
            string currentUserId = User.Identity.Name;

            try
            {
                List<SoloValidation> validations;

                if (User.IsInRole(Role.Admin))
                {
                    validations = await _soloValidationsService.GetAllValidations();
                }
                else if (User.IsInRole(Role.Captain))
                {
                    validations = await _soloValidationsService.GetValidationsForTeam(currentUserId);
                }
                else
                {
                    validations = await _soloValidationsService.GetValidationsForUser(currentUserId);
                }

                return Ok(validations);
            }
            catch (Exception e)
            {
                return BadRequest($"Can't get the validatons: {e.Message}");
            }
        }

        /// <summary>
        /// Get a file for a solo challenge
        /// </summary>
        /// <param name="fileId"></param>
        /// <response code="401">You must be logged in</response>
        /// <response code="403">Only captains and admin can view proofs</response>
        /// <response code="200">Return the file bytes</response>
        [Authorize(Roles = Role.Admin + "," + Role.Captain)]
        [HttpGet("files/{fileId:length(24)}")]
        public async Task<ActionResult<byte[]>> GetFile(string fileId)
        {
            var file = await _soloValidationsService.GetSoloValidationFile(fileId);

            return Ok(file);
        }

        /// <summary>
        /// Submit a validation for a solo challenge
        /// </summary>
        /// <param name="submitModel"></param>
        /// <response code="400">The validation is invalid</response>
        /// <response code="401">You must be logged in</response>
        /// <response code="200">The validation has been submitted</response>
        [HttpPost]
        public async Task<ActionResult<string>> SubmitValidation([FromBody] SoloValidationSubmitModel submitModel)
        {
            string currentUserId = User.Identity.Name;

            try
            {
                string validationId = await _soloValidationsService.SubmitValidation(currentUserId, submitModel);

                return Ok(validationId);
            }
            catch (Exception e)
            {
                return BadRequest($"Can't submit the validation : {e.Message}");
            }
        }

        /// <summary>
        /// Mark a solo validation as accepted
        /// </summary>
        /// <param name="id"></param>
        /// <param name="acceptModel"></param>
        /// <response code="400">Their was an error during the validation</response>
        /// <response code="401">You must be logged in</response>
        /// <response code="200">The validation is now accepted</response>
        [HttpPost("{id:length(24)}/validate")]
        public async Task<IActionResult> AcceptValidation(string id, [FromBody] SoloValidationValidateModel acceptModel)
        {
            try
            {
                await _soloValidationsService.ValidateSoloConfirmation(id, acceptModel);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest($"Can't accept validation : {e.Message}");
            }

        }

        /// <summary>
        /// Mark a solo validation as rejected
        /// </summary>
        /// <param name="id"></param>
        /// <response code="400">Their was an error during the validation</response>
        /// <response code="401">You must be logged in</response>
        /// <response code="200">The validation is now rejected</response>
        [HttpPost("{id:length(24)}/reject")]
        public async Task<IActionResult> RejectValidation(string id)
        {
            try
            {
                await _soloValidationsService.ValidationRejectConfirmation(id);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest($"Can't reject ealidation : {e.Message}");
            }

        }
    }
}
