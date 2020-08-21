using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleekPredictionPunter.AppService.Plans;
using SleekPredictionPunter.Model.PricingPlan;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class PricingPlanController : BaseController
    {
        private readonly IPricingPlanAppService _pricingPlanAppService;

        public PricingPlanController(IPricingPlanAppService pricingPlanAppService)
        {
            _pricingPlanAppService = pricingPlanAppService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var getForSubscriber = await _pricingPlanAppService.GetAllPlansWithBenefits();
            return View(getForSubscriber);
        }

        [HttpGet]
        public async Task<IActionResult> AllPlans()
        {
            ViewBag.PlanSetup = "active";
            var getAllPlans = await _pricingPlanAppService.GetAllPlans();
            return View();
        }

        #region question region
        [HttpGet]
        public async Task<IActionResult> CreateNewQuestion()
        {
            ViewBag.PlanSetup = "active";
            ViewBag.Questions = await _pricingPlanAppService.GetAllQuestion();
            return View(new PlanBenefitQuestionsModel { });
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewQuestion(PlanBenefitQuestionsModel model)
        {
            ViewBag.PlanSetup = "active";
            ViewBag.Questions = await _pricingPlanAppService.GetAllQuestion();
            try
            {
                var insert = await _pricingPlanAppService.insertPanQuestions(model);
                if (insert == true)
                {
                    ViewBag.Message = "Record successfully inserted..";
                    // return View(new PlanBenefitQuestionsModel { });
                    return RedirectToAction("CreateNewQuestion");
                }
                {
                    ViewBag.Messsage = "Could create Pricing Plan question. Please, retry!";
                    return View(new PlanBenefitQuestionsModel { });
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
        public async Task<IActionResult> ListofPlans()
        {
			ViewBag.PlanSetup = "active";
			var result = await _pricingPlanAppService.GetAllPlans();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> CreateNewPlan()
        {
            ViewBag.PlanSetup = "active";
            try
            {
                var getAllQquestions = await _pricingPlanAppService.GetAllQuestion();
                var model = new PlanPricingDto
                {
                    planBenefitQuestionsModels = getAllQquestions,
                    PricingPlanModel = null,
                };

                var benefitmodel = new List<BenefitOutlines>();

                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewPlan(PlanPricingDto model, int[] answers , long[] questionId)
        {
            ViewBag.PlanSetup = "active";
            try
            {
                var result = false;
                var planModelBuilder = new PricingPlanModel
                {
                    PlanName = model.PricingPlanModel.PlanName,
                    PlanType = model.PricingPlanModel.PlanType,
                    Duration = model.PricingPlanModel.Duration,
                    Price = model.PricingPlanModel.Price
                };
                var benefits = new List<BenefitOutlines>();

                Func<PricingPlanModel, bool> predicate = (x => x.PlanName == planModelBuilder.PlanName);

                var checkExistingplanProperties = await _pricingPlanAppService.GetFirstOfDefault(predicate);
                if (checkExistingplanProperties ==null)
                {
                    var insertPlan = await _pricingPlanAppService.InsertPricingPlan(planModelBuilder);
                    if (insertPlan != null && insertPlan.Id > 0)
                    {
                        for (int i = 0; i < questionId.Length; i++)
                        {
                            var questionindex = questionId[i];
                            var getQuestionById = await _pricingPlanAppService.GetQuestionById(questionindex);
                            var insertToPlanBenefits = new PlanPricingBenefitsModel
                            {
                                Answer = Convert.ToBoolean(answers[i]),
                                Question = getQuestionById.Question,
                                QuestionId = getQuestionById.QuestionId,
                                DateTimeCreated = DateTime.UtcNow,
                                IsActive = true,
                                PlanPricingId = insertPlan.Id
                            };
                            
                            result = await _pricingPlanAppService.InsertPricePlanBenefit(insertToPlanBenefits);
                        }

                        if (result == true)
                        {
                            return RedirectToAction("ListofPlans", "Pricingplan");
                        }

                        //means. an error occurred if it reaches this point..
                        var getAllQquestions = await _pricingPlanAppService.GetAllQuestion();
                        var dtoModel = new PlanPricingDto
                        {
                            planBenefitQuestionsModels = getAllQquestions,
                            PricingPlanModel = null,
                        };
                        return View(dtoModel);
                    }
                }
                ViewBag.Errors = "";
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }

    public class BenefitOutlines
    {
        public long QuestionId { get; set; }
        public int AnswerId { get; set; }
    }
}
