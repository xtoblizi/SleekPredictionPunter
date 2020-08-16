using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SleekPredictionPunter.Model.PricingPlan
{
    public class PlanBenefitQuestionsModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long QuestionId { get; set; }
        public string Question { get; set; }
        public DateTime DateTimeCreated { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
        public DateTime? DateTimeDeleted { get; set; }
    }
}
