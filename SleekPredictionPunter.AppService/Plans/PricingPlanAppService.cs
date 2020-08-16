using Microsoft.AspNetCore.Mvc.TagHelpers;
using SleekPredictionPunter.Model.PricingPlan;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Plans
{
    public class PricingPlanAppService : IPricingPlanAppService
    {
        private readonly IBaseRepository<PlanBenefitQuestionsModel> _baseRepository;
        private readonly IBaseRepository<PlanPricingBenefitsModel> _benefitBaseRepository;
        private readonly IBaseRepository<PricingPlanModel> _planBaseRepository;
        public PricingPlanAppService(IBaseRepository<PlanBenefitQuestionsModel> baseRepository, 
            IBaseRepository<PlanPricingBenefitsModel> benefitBaseRepository,
            IBaseRepository<PricingPlanModel> planBaseRepository)
        {
            _baseRepository = baseRepository;
            _benefitBaseRepository = benefitBaseRepository;
            _planBaseRepository = planBaseRepository;
        }

        #region Question region
        public async Task<bool> insertPanQuestions(PlanBenefitQuestionsModel model)
        {
            try
            {
                var result = await _baseRepository.Insert(model);
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PlanBenefitQuestionsModel> GetQuestionById(long id)
        {
            var result = await _baseRepository.GetById(id);
            return result;
        }

        public async Task<IEnumerable<PlanBenefitQuestionsModel>> GetAllQuestion()
        {
            var getAllQuestion = await _baseRepository.GetAllQueryable();
            var records = getAllQuestion.Where(x => x.IsActive == true);
            return records;
        }
        #endregion

        #region Benefit region
        public async Task<bool> InsertPricePlanBenefit(PlanPricingBenefitsModel model)
        {
            try
            {
                if (model == null) throw new ArgumentNullException("There's a problem with your model. Failed to insert to question table");

                    var insert = await _benefitBaseRepository.Insert(model);
                    return insert > 0 ? true : false;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<PlanPricingBenefitsModel>> GetAllBenefits()
        {
            var getAllRecords = await _benefitBaseRepository.GetAllQueryable();
            var result = getAllRecords.Where(x => x.IsActive == true);
            return result;
        }

        private async Task DeleteBenefit(PlanPricingBenefitsModel model, bool deleted = true)
        {
            await _benefitBaseRepository.Delete(model, deleted);
        }
        #endregion

        #region Pricing Plan region
        public async Task<PricingPlanModel> InsertPricingPlan(PricingPlanModel model)
        {
            var insert = await _planBaseRepository.Insert(model, true);
            if (insert > 0)
            {
                var getById = await _planBaseRepository.GetById(insert);
                return getById;
            }
            return null;
        }

        public async Task<IEnumerable<PricingPlanModel>> GetAllPlans()
        {
            var getAllPlans = await _planBaseRepository.GetAllQueryable();
            return getAllPlans;
        }

        public async Task<PlanPricingCreateDto> GetAllPlansForSubscriber()
        {
            var getAllPlans = await _planBaseRepository.GetAllQueryable();
            var getAllBenefit = await _benefitBaseRepository.GetAllQueryable();
            var dto = new PlanPricingCreateDto();

            foreach (var item in getAllPlans)
            {
                var filter = getAllBenefit.Where(x=>x.PlanPricingId == item.PlanId);

                dto = new PlanPricingCreateDto
                {
                    PricingPlanModel = getAllPlans,
                    planPricingBenefitsModels = getAllBenefit
                };
            }
            return dto;
        }
        #endregion
    }
}
