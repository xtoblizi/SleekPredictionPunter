using SleekPredictionPunter.Model.PricingPlan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Plans
{
    public interface IPricingPlanAppService
    {
        Task DeleteBenefit(long id);
        Task<IEnumerable<PlanPricingBenefitsModel>> GetAllBenefits();
        Task<PlanBenefitQuestionsModel> GetQuestionById(long id);
        Task<IEnumerable<PlanBenefitQuestionsModel>> GetAllQuestion();
        Task<bool> insertPanQuestions(PlanBenefitQuestionsModel model);
        Task<PlanBenefitQuestionsModel> GetFirstOfDefaultQuestionProperties(Func<PlanBenefitQuestionsModel, bool> func = null);
        Task<bool> InsertPricePlanBenefit(PlanPricingBenefitsModel model);
        Task<PricingPlanModel> InsertPricingPlan(PricingPlanModel model);
        Task<IEnumerable<PlanWithBenefitsDto>> GetAllPlansWithBenefits();
        Task<PlanPricingCreateDto> GetAllPlansForSubscriber();
        Task<IEnumerable<PricingPlanModel>> GetAllPlans();
        Task<PricingPlanModel> GetFirstOfDefault(Func<PricingPlanModel, bool> func = null);
		Task<PricingPlanModel> GetById(long packgeId);
		Task DeletePricingPlan(long packgeId);
        Task<IEnumerable<PlanPricingBenefitsModel>> GetAllBenefitsByPredicate(Func<PlanPricingBenefitsModel, bool> predicate);
        Task UpdatePricePlanBenefit(PlanPricingBenefitsModel model);
        Task<PricingPlanModel> GetPlanById(long id);
        Task DeleteDynamicMode(dynamic model);
        Task PricePlanBenefit_InsertUpdateOrIgnoreIfExist(PlanPricingBenefitsModel model);
        Task UpdatePanQuestions(PlanBenefitQuestionsModel model);
        Task UpdatePricingPlan(PricingPlanModel model);
        
    }
    
}