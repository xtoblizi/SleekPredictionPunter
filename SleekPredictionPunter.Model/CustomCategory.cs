using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
    public class CustomCategory:BaseEntity
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool IsFree { get; set; }
    }
}
