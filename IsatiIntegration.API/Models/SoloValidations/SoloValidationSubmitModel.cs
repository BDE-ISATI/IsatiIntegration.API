using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Models.SoloValidations
{
    public class SoloValidationSubmitModel
    {
        /// <summary>
        /// The challenge corresponding to the validation
        /// </summary>
        /// <example>5f1fe90a58c8ab093c4f772a</example>
        [BsonRepresentation(BsonType.ObjectId)]
        [Required]
        public string ChallengeId { get; set; }

        /// <summary>
        /// The list of files associated with this validation
        /// </summary>
        [Required]
        public List<byte[]> Files { get; set; }
    }
}
