using IsatiIntegration.API.Entities;
using IsatiIntegration.API.Models.TeamValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Services.Interfaces
{
    public interface ITeamValidationsService
    {
        Task<List<TeamValidation>> GetValidationsForTeam(string userId);
        Task<List<TeamValidation>> GetAllValidations();

        Task ValidateSoloConfirmation(TeamValidationSubmitModel validateModel);
    }
}
