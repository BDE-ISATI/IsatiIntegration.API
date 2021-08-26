using IsatiIntegration.API.Entities.Form;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Models.Form
{
    public class FormSubmitModel
    {
        /// <summary>
        /// The first drawing
        /// </summary>
        [Required]
        public byte[] Drawing1 { get; set; }

        /// <summary>
        /// The second drawing
        /// </summary>
        [Required]
        public byte[] Drawing2 { get; set; }

        /// <summary>
        /// The third drawing
        /// </summary>
        [Required]
        public byte[] Drawing3 { get; set; }

        /// <summary>
        /// All the questions/answer of the form
        /// </summary>
        public List<FormQA> QAs { get; set; }
    }
}
