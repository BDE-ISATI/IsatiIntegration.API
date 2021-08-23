using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Entities
{
    public class SoloChallenge
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// The challenge title
        /// </summary>
        /// <example>Barbecuite</example>
        public string Title { get; set; }
        /// <summary>
        /// The challenge description
        /// </summary>
        /// <example>Participer au barbecue de rentrée</example>
        public string Description { get; set; }

        /// <summary>
        /// The number of point the challenge will give
        /// </summary>
        /// <example>5</example>
        public int Value { get; set; }
        /// <summary>
        /// The number of time the challenge has to be done 
        /// </summary>
        /// <example>1</example>
        public int NumberOfRepetitions { get; set; }

        /// <summary>
        /// The challenge starting date 
        /// </summary>
        /// <example>2021-09-03T14:40:04.1351158+01:00</example>
        public DateTime StartingDate { get; set; }
        /// <summary>
        /// The challenge ending date 
        /// </summary>
        /// <example>2021-09-07T14:40:04.1351158+01:00</example>
        public DateTime EndingDate { get; set; }

    }
}
