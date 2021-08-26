using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Entities.Form
{
    public class Form
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// The user who filled the form
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        /// <summary>
        /// The full score of the form
        /// </summary>
        /// <example>34</example>
        public int TotalScore { get; set; }

        /// <summary>
        /// File id of the first drawing
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Drawing1Id { get; set; }

        /// <summary>
        /// File id of the second drawing
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Drawing2Id { get; set; }

        /// <summary>
        /// File id of the third drawing
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Drawing3Id { get; set; }

        /// <summary>
        /// All the questions/answer of the form
        /// </summary>
        public List<FormQA> QAs { get; set; }
    }
}
