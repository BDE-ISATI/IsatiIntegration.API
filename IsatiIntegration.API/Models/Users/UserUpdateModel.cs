using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Models.Users
{
    public class UserUpdateModel
    {

        /// <summary>
        /// A new user profile picture
        /// </summary>
        public byte[] ProfilePicture { get; set; }

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
        [Required]
        public string FirstName { get; set; }
        /// <summary>
        /// The user's last name
        /// </summary>
        /// <example>DENIS</example>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// The user's email
        /// </summary>
        /// <example>admin@feldrise.com</example>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// The new user password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The user's role. The roles are : Admin, Captain, Player
        /// </summary>
        /// <example>Adoptant</example>
        [Required]
        public string Role { get; set; }

        /// <summary>
        /// The user score
        /// </summary>
        [Required]
        public int Score { get; set; }

    }
}
