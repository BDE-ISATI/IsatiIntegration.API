using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiIntegration.API.Models
{
    public class FileModel
    {
        public string Filename { get; set; }
        public byte[] Data { get; set; }
    }
}
