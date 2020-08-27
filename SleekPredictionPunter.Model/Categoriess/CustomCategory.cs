using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SleekPredictionPunter.Model
{
    /// <summary>
    /// This table/model herebyy represents the sports or league category of the matches
    /// </summary>
    public class CustomCategory:BaseEntity
    {
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
