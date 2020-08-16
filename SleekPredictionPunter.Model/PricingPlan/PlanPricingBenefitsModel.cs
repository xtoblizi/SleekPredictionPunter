using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SleekPredictionPunter.Model.PricingPlan
{
    public class PlanPricingBenefitsModel
    {
        [Key]
        public long BenefitId { get; set; }
        public long QuestionId { get; set; }
        public string Question { get; set; }
        public bool Answer { get; set; }
        public long PlanPricingId { get; set; }
        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeDeleted { get; set; } = DateTime.Now;
    }
}
