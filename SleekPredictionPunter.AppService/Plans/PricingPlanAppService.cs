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
            catch (Exception)
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
            var insert = await _planBaseRepository.Inserts(model, true);
            if (insert!=null)
            {
                //var getById = await _planBaseRepository.GetById(insert.PlanId);
                return insert;
            }
            return null;
        }

        public async Task<IEnumerable<PricingPlanModel>> GetAllPlans()
        {
            var getAllPlans = await _planBaseRepository.GetAllQueryable();
            return getAllPlans;
        }

        public async Task<IEnumerable<PlanWithBenefitsDto>> GetAllPlansWithBenefits()
        {
            var getAllPlans = await _planBaseRepository.GetAllQueryable();

          
            var dtos = new List<PlanWithBenefitsDto>();

            foreach (var item in getAllPlans)
            {

                Func<PlanPricingBenefitsModel, bool> predicate = (x => x.PlanPricingId == item.Id);
                var getAllBenefit = await _benefitBaseRepository.GetAllQueryable(predicate);

                var dto = new PlanWithBenefitsDto
                { 
                    PricingPlanModel = item, 
                    planPricingBenefitsModels = getAllBenefit
                };
                dtos.Add(dto);
            }

            return dtos;
        }

        public async Task<PlanPricingCreateDto> GetAllPlansForSubscriber()
        {
            var getAllPlans = await _planBaseRepository.GetAllQueryable();
            var getAllBenefit = await _benefitBaseRepository.GetAllQueryable();
            var dto = new PlanPricingCreateDto();

            foreach (var item in getAllPlans)
            {
                var filter = getAllBenefit.Where(x => x.PlanPricingId == item.Id);
                var list = getAllBenefit.Union(filter).ToList();
                dto = new PlanPricingCreateDto
                {
                    PricingPlanModel = getAllPlans.Distinct(),
                    planPricingBenefitsModels = /*getAllBenefit*/ list
                };
            }
            return dto;
        }

        public async Task<PricingPlanModel> GetPlanById(long id)
        {
            var result = await _planBaseRepository.GetById(id);
            return result;
        }

        /// <summary>
        /// Create your func queryable filter and pass in here.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<PricingPlanModel> GetFirstOfDefault(Func<PricingPlanModel,bool> func = null)
        {
            return await _planBaseRepository.GetFirstOrDefault(func);
        }

		public async Task<PricingPlanModel> GetById(long packgeId)
		{
			return await _planBaseRepository.GetById(packgeId);
		}

		public async Task DeletePricingPlan(long packgeId)
		{
			var obj = await GetById(packgeId);
			if(obj != null)
			{
				// delete the obj
				await _planBaseRepository.Delete(obj);
			}
		}
		#endregion
	}
}
