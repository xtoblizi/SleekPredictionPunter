using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SleekPredictionPunter.Model.PricingPlan
{
    public class PricingPlanModel : BaseEntity
    {
		public PricingPlanModel()
		{
			PlanType = PlanTypeEnum.Free;
		}
        public string PlanName { get; set; }
        public PlanTypeEnum PlanType { get; set; }
        public string  Duration { get; set; }
        public decimal Price { get; set; }
    }

    public enum PlanTypeEnum
    {
        Free = 1,
        Paid = 2,
    }
}
