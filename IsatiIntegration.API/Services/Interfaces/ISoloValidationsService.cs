using IsatiIntegration.API.Entities.SoloValidation;
using IsatiIntegration.API.Models.SoloValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Services.Interfaces
{
    public interface ISoloValidationsService
    {
        Task<List<SoloValidation>> GetValidationsForUser(string userId);
        Task<List<SoloValidation>> GetValidationsForTeam(string userId);
        Task<List<SoloValidation>> GetAllValidations();

        Task<byte[]> GetSoloValidationFile(string fileId);

        Task<string> SubmitValidation(string userId, SoloValidationSubmitModel submitModel);
        Task ValidateSoloConfirmation(string id, SoloValidationValidateModel validateModel);
        Task ValidationRejectConfirmation(string id);
    }
}
