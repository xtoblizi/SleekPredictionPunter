using SleekPredictionPunter.Model.PricingPlan;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Plans
{
    public interface IPricingPlanAppService
    {
        Task<IEnumerable<PlanPricingBenefitsModel>> GetAllBenefits();
        Task<PlanBenefitQuestionsModel> GetQuestionById(long id);
        Task<IEnumerable<PlanBenefitQuestionsModel>> GetAllQuestion();
        Task<bool> insertPanQuestions(PlanBenefitQuestionsModel model);
        Task<bool> InsertPricePlanBenefit(PlanPricingBenefitsModel model);
        Task<PricingPlanModel> InsertPricingPlan(PricingPlanModel model);
        Task<PlanPricingCreateDto> GetAllPlansForSubscriber();
        Task<IEnumerable<PricingPlanModel>> GetAllPlans();
    }
}