using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Models.TeamValidations
{
    public class TeamValidationSubmitModel
    {
        /// <summary>
        /// The challenge to validate
        /// </summary>
        /// <example>45d5ae0ad9221e701ceeba5b</example>
        [BsonRepresentation(BsonType.ObjectId)]
        public string ChallengeId { get; set; }

        /// <summary>
        /// The team to give points
        /// </summary>
        /// <example>45d5ae0ad9221e701ceeba5b</example>
        [BsonRepresentation(BsonType.ObjectId)]
        public string TeamId { get; set; }

        /// <summary>
        /// Add some extra points to the team
        /// </summary>
        /// <example>10</example>
        public int ExtraPoints { get; set; }

        /// <summary>
        /// Specify a number of member
        /// </summary>
        /// <example>3</example>
        public int MembersCount { get; set; }
    }
}
