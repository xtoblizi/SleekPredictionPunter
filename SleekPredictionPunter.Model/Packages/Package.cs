using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.Packages
{
    public class Package: BaseEntity
    {
        public string PackageName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
