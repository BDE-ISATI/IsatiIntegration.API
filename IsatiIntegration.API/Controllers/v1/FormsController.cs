using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Entities.Form;
using IsatiIntegration.API.Models.Form;
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
    [Route("v1/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly IFormsService _formsService;

        public FormsController(IFormsService formsService)
        {
            _formsService = formsService;
        } 

        /// <summary>
        /// Get the form of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <response code="401">You must be logged in</response>
        /// <response code="403">Only admins can see forms</response>
        /// <response code="200">Return the form</response>
        [Authorize(Roles = Role.Admin)]
        [HttpGet("users/{userId:length(24)}")]
        public async Task<ActionResult<Form>> GetFormForUser(string userId)
        {
            var form = await _formsService.GetFormForUser(userId);

            if (form == null)
            {
                return NotFound();
            }

            return Ok(form);
        }

        /// <summary>
        /// Get a drawing from a form
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401">You must be logged in</response>
        /// <response code="403">Only admins can view drawing</response>
        /// <response code="200">Return the drawing</response>
        [Authorize(Roles = Role.Admin)]
        [HttpGet("drawing/{id:length(24)}")]
        public async Task<ActionResult<byte[]>> GetDrawing(string id)
        {
            var drawing = await _formsService.GetDrawing(id);

            if (drawing == null)
            {
                return NotFound();
            }

            return drawing;
        }

        /// <summary>
        /// Submit a form
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="submitModel"></param>
        /// <response code="200">Your form has been submitted</response>
        [HttpPost("users/{userId:length(24)}")]
        public async Task<ActionResult<string>> SubmitForm(string userId, [FromBody] FormSubmitModel submitModel)
        {
            var formId = await _formsService.SubmitForm(userId, submitModel);

            return Ok(formId);
        }
    }
}
