using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Entities.Form
{
    public class FormQA
    {
        /// <summary>
        /// The question asked
        /// </summary>
        /// <example>Un mot qui te décrirait ?</example>
        public string Question { get; set; }

        /// <summary>
        /// The question answered
        /// </summary>
        /// <example>Je suis une API REST !</example>
        public string Answer { get; set; }

        /// <summary>
        /// The number of points this answer give
        /// </summary>
        /// <example>4</example>
        public int Value { get; set; }
    }
}
