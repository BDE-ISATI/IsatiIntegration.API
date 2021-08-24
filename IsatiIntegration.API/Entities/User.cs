using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// The user's profile picture
        /// </summary>
        /// <example>45d5ae0ad9221e701ceeba5b</example>
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProfilePictureId { get; set; }

        /// <summary>
        /// The user's team
        /// </summary>
        /// <example>45d5ae0ad9221e701ceeba5b</example>
        [BsonRepresentation(BsonType.ObjectId)]
        public string TeamId { get; set; }

        /// <summary>
        /// The user's first name
        /// </summary>
        /// <example>Victor</example>
        public string FirstName { get; set; }
        /// <summary>
        /// The user's last name
        /// </summary>
        /// <example>DENIS</example>
        public string LastName { get; set; }

        /// <summary>
        /// The user's email
        /// </summary>
        /// <example>admin@feldrise.com</example>
        public string Email { get; set; }

        // Authentication related
        [JsonIgnore]
        public string PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }

        /// <summary>
        /// The user's role. The roles are : Admin, Captain, Player
        /// </summary>
        /// <example>Adoptant</example>
        public string Role { get; set; }

        /// <summary>
        /// The user's connection token
        /// </summary>
        [BsonIgnore]
        public string Token { get; set; }
    }
}
