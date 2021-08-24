using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Entities
{
    public class Team
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// The team captain
        /// </summary>
        /// <example>5f1fe90a58c8ab093c4f772a</example>
        [BsonRepresentation(BsonType.ObjectId)]
        public string CaptainId { get; set; }

        /// <summary>
        /// The team name
        /// </summary>
        /// <example>Isatirebouchon</example>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The team color
        /// </summary>
        /// <example>#f70c35</example>
        [Required]
        public string TeamHEXColor { get; set; }
    }
}
