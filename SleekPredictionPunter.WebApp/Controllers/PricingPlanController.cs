using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SleekPredictionPunter.AppService.Plans;
using SleekPredictionPunter.Model.PricingPlan;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class PricingPlanController : Controller
    {
        private readonly IPricingPlanAppService _pricingPlanAppService;

        public PricingPlanController(IPricingPlanAppService pricingPlanAppService)
        {
            _pricingPlanAppService = pricingPlanAppService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var getForSubscriber = await _pricingPlanAppService.GetAllPlansForSubscriber();
            return View(getForSubscriber);
        }

        [HttpGet]
        public async Task<IActionResult> AllPlans()
        {
            var getAllPlans = await _pricingPlanAppService.GetAllPlans();
            return View();
        }

        #region question region
        [HttpGet]
        public async Task<IActionResult> CreateNewQuestion()
        {
            return View(new PlanBenefitQuestionsModel { });
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewQuestion(PlanBenefitQuestionsModel model)
        {
            try
            {
                var insert = await _pricingPlanAppService.insertPanQuestions(model);
                if (insert == true)
                {
                    ViewBag.Message = "Record successfully inserted..";
                    return View(new PlanBenefitQuestionsModel { });
                }
                {
                    ViewBag.Messsage = "Could create Pricing Plan question. Please, retry!";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Pricing Plan region
        [HttpGet]
        public async Task<IActionResult> CreateNewPlan()
        {
            try
            {
                var getAllQquestions = await _pricingPlanAppService.GetAllQuestion();
                var model = new PlanPricingDto
                {
                    planBenefitQuestionsModels = getAllQquestions,
                    planPricingBenefitsModels = null,
                    PricingPlanModel = null,
                    BenefitsModels = null
                };
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewPlan(PlanPricingDto model, List<int> answer = null, List<long> questionId = null)
        {

            try
            {
                var answers = 0;
                var result = false;
                var getQuestionById = new PlanBenefitQuestionsModel();
                var planModelBuilder = new PricingPlanModel
                {
                    DateTimeCreated = DateTime.UtcNow,
                    PlanName = model.PricingPlanModel.PlanName,
                    PlanType = model.PricingPlanModel.PlanType,
                    IsActive = true,
                    Duration = model.PricingPlanModel.Duration,
                    Price = model.PricingPlanModel.Price
                };
                var insertPlan = await _pricingPlanAppService.InsertPricingPlan(planModelBuilder);
                if(insertPlan!=null && insertPlan.PlanId > 0)
                {
                    foreach (var item in questionId)
                    {
                        getQuestionById = await _pricingPlanAppService.GetQuestionById(item);
                    }
                    foreach (var ans in answer)
                    {
                        answers = ans;
                    }

                    var insertToPlanBenefits = new PlanPricingBenefitsModel
                    {
                        Answer = Convert.ToBoolean(answers),
                        Question = getQuestionById.Question,
                        QuestionId = getQuestionById.QuestionId,
                        DateTimeCreated = DateTime.UtcNow,
                        IsActive = true,
                        PlanPricingId = insertPlan.PlanId,

                    };
                    result = await _pricingPlanAppService.InsertPricePlanBenefit(insertToPlanBenefits);

                    if (result == true)
                    {
                        return RedirectToAction();
                    }

                    //means. an error occurred if it reaches this point..
                    var getAllQquestions = await _pricingPlanAppService.GetAllQuestion();
                    var dtoModel = new PlanPricingDto
                    {
                        planBenefitQuestionsModels = getAllQquestions,
                        planPricingBenefitsModels = null,
                        PricingPlanModel = null,
                        BenefitsModels = null
                    };
                    return View(dtoModel);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
