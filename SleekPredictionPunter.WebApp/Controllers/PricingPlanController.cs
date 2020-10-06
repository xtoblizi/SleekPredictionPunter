﻿using System;
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
            ViewBag.Message = "Successfully created Benefit Pointline";
            ViewBag.Questions = await _pricingPlanAppService.GetAllQuestion();
            return View(new PlanBenefitQuestionsModel { });
        } 
        
        [HttpGet]
        public async Task<IActionResult> DeleteQuestion(long Id)
        {
            var question = await _pricingPlanAppService.GetQuestionById(Id);
            await _pricingPlanAppService.DeleteDynamicMode(question);

            TempData["TempMessage"] = $"Question successfully deteted";
            return RedirectToAction(nameof(CreateNewQuestion));
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

        [HttpGet]
        public async Task<ActionResult> EditQuestionGet(long id)
        {
            var getPlanQuestionById = await _pricingPlanAppService.GetQuestionById(id);
            return PartialView(getPlanQuestionById);
        }

        [HttpPost]
        public async Task<IActionResult> EditQuestion(PlanBenefitQuestionsModel model)
        {
            _pricingPlanAppService.UpdatePanQuestions(model);
            TempData["TempMessage"] = $"Question successfully Updated";
            return RedirectToAction(nameof(CreateNewQuestion));
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
                    Price = model.PricingPlanModel.Price,
                    PlanCommission = model.PricingPlanModel.PlanCommission
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

        [HttpGet]
        public async Task<IActionResult> EditPlan(long id)
        {
            var getPlanById = await _pricingPlanAppService.GetPlanById(id);
            Func<PlanPricingBenefitsModel, bool> predicate = (x => x.PlanPricingId == id);
            var getBenefitsByPlanId = await _pricingPlanAppService.GetAllBenefitsByPredicate(predicate);
            var getAllQquestions = await _pricingPlanAppService.GetAllQuestion();
            #region filter existing benefits
            //var questionsId = getAllQquestions.Where 
            IEnumerable<PlanBenefitQuestionsModel> filterAlreadyChosenQuestions = null;
            foreach (var item in getBenefitsByPlanId)
            {
                filterAlreadyChosenQuestions = getAllQquestions.Where(x => x.QuestionId != item.QuestionId);
            }
            #endregion

            var editPlanDto = new PlanPricingDtoEdit
            {
                PricingPlanModel = getPlanById,
                planPricingBenefitsModels = getBenefitsByPlanId,
                planBenefitQuestionsModelForNewAdds = filterAlreadyChosenQuestions
            };
            return View(editPlanDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditPlan(PlanPricingDto model, IEnumerable<PlanPricingBenefitsModel> getOldeBenefits,
                                                  int[] answers, long[] questionId)
        {
            try
            {
                var result = false;
                var planModelBuilder = new PricingPlanModel
                {
                    PlanName = model.PricingPlanModel.PlanName,
                    PlanType = model.PricingPlanModel.PlanType,
                    Duration = model.PricingPlanModel.Duration,
                    Price = model.PricingPlanModel.Price,
                    PlanCommission = model.PricingPlanModel.PlanCommission,
                    Id = model.PricingPlanModel.Id,
                    DateUpdated = DateTime.Now
                };
                var benefits = new List<BenefitOutlines>();

                Func<PricingPlanModel, bool> predicate = (x => x.PlanName == planModelBuilder.PlanName);

                var checkExistingplanProperties = await _pricingPlanAppService.GetFirstOfDefault(predicate);
                if (checkExistingplanProperties != null)
                {
                   await _pricingPlanAppService.UpdatePricingPlan(planModelBuilder);
                    //if (insertPlan != null && insertPlan.Id > 0)
                    //{
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
                                PlanPricingId = planModelBuilder.Id
                            };

                           await _pricingPlanAppService.PricePlanBenefit_InsertUpdateOrIgnoreIfExist(insertToPlanBenefits);
                        }

                        var getAllQquestions = await _pricingPlanAppService.GetAllQuestion();
                        var dtoModel = new PlanPricingDto
                        {
                            planBenefitQuestionsModels = getAllQquestions,
                            PricingPlanModel = null,
                        };

                    return RedirectToAction("ListofPlans", "Pricingplan");
                }
                ViewBag.Errors = "";
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeletePricingPlan(long Id)
        {
             await _pricingPlanAppService.DeletePricingPlan(Id);

            TempData["TempMessage"] = $"Pricing-Plan successfully deteted";
            return RedirectToAction(nameof(ListofPlans));
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> DeleteBenefit(long id)
        {
          await _pricingPlanAppService.DeleteBenefit(id);
            return RedirectToAction("listofPlans", "pricingplan");
        }
    }
  
    public class BenefitOutlines
    {
        public long QuestionId { get; set; }
        public int AnswerId { get; set; }
    }
}
