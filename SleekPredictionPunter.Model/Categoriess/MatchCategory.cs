using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SleekPredictionPunter.Model
{
    public class MatchCategory:BaseEntity
    {
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
