using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chipper.Models
{
    public class Chip
    {
        public string Name { get; set; }
        public string Reference { get; set; }
        public string ImageReference { get; set; }
        public string Price { get; set; }
        public string Availability { get; set; }
    }
}