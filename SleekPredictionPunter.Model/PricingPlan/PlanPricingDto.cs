using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.PricingPlan
{
    public class PlanPricingDto
    {
        public PricingPlanModel PricingPlanModel { get; set; }
        public IEnumerable<PlanBenefitQuestionsModel> planBenefitQuestionsModels { get; set; }
    } 
    public class PlanWithBenefitsDto
    {
        public PricingPlanModel PricingPlanModel { get; set; }
        public IEnumerable<PlanPricingBenefitsModel> planPricingBenefitsModels { get; set; }
    }

    public class PlanPricingCreateDto
    {
        public IEnumerable<PricingPlanModel> PricingPlanModel { get; set; }
        public IEnumerable<PlanPricingBenefitsModel> planPricingBenefitsModels { get; set; }
    }

    public class PlanPricingDtoEdit
    {
        public PricingPlanModel PricingPlanModel { get; set; }
        public IEnumerable<PlanBenefitQuestionsModel> planBenefitQuestionsModelForNewAdds { get; set; }
        public IEnumerable<PlanPricingBenefitsModel> planPricingBenefitsModels { get; set; }
    }
}
