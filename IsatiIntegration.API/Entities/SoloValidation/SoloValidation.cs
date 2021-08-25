using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Entities.SoloValidation
{
    public class SoloValidation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// The challenge corresponding to the validation
        /// </summary>
        /// <example>5f1fe90a58c8ab093c4f772a</example>
        [BsonRepresentation(BsonType.ObjectId)]
        public string ChallengeId { get; set; }

        /// <summary>
        /// The user submitting the validation
        /// </summary>
        /// <example>5f1fe90a58c8ab093c4f772a</example>
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        /// <summary>
        /// The user's team 
        /// </summary>
        /// <example>5f1fe90a58c8ab093c4f772a</example>
        [BsonRepresentation(BsonType.ObjectId)]
        public string TeamId { get; set; }

        /// <summary>
        /// The validation status. Can be "Waiting", "Validated" or "Rejected"
        /// </summary>
        /// <example>Waiting</example>
        public string Status { get; set; }

        /// <summary>
        /// The list of files associated with this validation
        /// </summary>
        public List<string> FilesIds { get; set; }
    }
}
