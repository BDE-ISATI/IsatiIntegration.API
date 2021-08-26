using IsatiIntegration.API.Entities.Form;
using IsatiIntegration.API.Models;
using IsatiIntegration.API.Models.Form;
using IsatiIntegration.API.Services.Interfaces;
using IsatiIntegration.API.Settings.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Services
{
    public class FormsService : IFormsService
    {
        private readonly IMongoCollection<Form> _forms;

        private readonly IFilesService _filesService;

        public FormsService(IMongoSettings mongoSettings, IFilesService filesService)
        {
            var mongoClient = new MongoClient(mongoSettings.ConnectionString);
            var database = mongoClient.GetDatabase(mongoSettings.DatabaseName);

            _forms = database.GetCollection<Form>(mongoSettings.FormsCollectionName);

            _filesService = filesService;
        }

        public async Task<Form> GetFormForUser(string userId)
        {
            var formCursor = await _forms.FindAsync(dbForm =>
                dbForm.UserId == userId
            );

            return await formCursor.FirstOrDefaultAsync();
        }

        public async Task<byte[]> GetDrawing(string id)
        {
            FileModel drawing = await _filesService.GetFileById(id);

            if (drawing == null) return null;

            return drawing.Data;
        }

        public async Task<string> SubmitForm(string userId, FormSubmitModel submitModel)
        {
            int totalScore = 0;

            foreach (var qa in submitModel.QAs)
            {
                totalScore += qa.Value;
            }

            var drawing1 = await _filesService.UploadFile($"{userId}_drawing1", submitModel.Drawing1, false);
            var drawing2 = await _filesService.UploadFile($"{userId}_drawing2", submitModel.Drawing2, false);
            var drawing3 = await _filesService.UploadFile($"{userId}_drawing3", submitModel.Drawing3, false);

            Form form = new()
            {
                TotalScore = totalScore,
                UserId = userId,
                Drawing1Id = drawing1,
                Drawing2Id = drawing2,
                Drawing3Id = drawing3,
                QAs = submitModel.QAs
            };

            await _forms.InsertOneAsync(form);

            return form.Id;
        }

    }
}
