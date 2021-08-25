using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Models.SoloValidations
{
    public class SoloValidationValidateModel
    {
        /// <summary>
        /// Add some extra points to the user
        /// </summary>
        /// <example>10</example>
        public int ExtraPoints { get; set; }
    }
}
