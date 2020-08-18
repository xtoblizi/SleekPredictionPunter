using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SleekPredictionPunter.Model.PricingPlan
{
    public class PricingPlanModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long PlanId { get; set; }
        public string PlanName { get; set; }
        public PlanTypeEnum PlanType { get; set; }
        public string  Duration { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? DateTimeUpdated { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public enum PlanTypeEnum
    {
        Free = 1,
        Basic = 2,
        Premium = 3,
        Genius=4,
        Football_Expert_Advisor = 5,
        Platinum = 6
    }
}
