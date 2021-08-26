using IsatiIntegration.API.Entities.Form;
using IsatiIntegration.API.Models.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Services.Interfaces
{
    public interface IFormsService
    {
        Task<Form> GetFormForUser(string userId);

        Task<byte[]> GetDrawing(string id);

        Task<string> SubmitForm(string userId, FormSubmitModel submitModel);
    }
}
