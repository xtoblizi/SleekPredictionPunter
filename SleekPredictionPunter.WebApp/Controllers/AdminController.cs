using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleekPredictionPunter.AppService;
using SleekPredictionPunter.AppService.Agents;
using SleekPredictionPunter.AppService.PredictionAppService;
using SleekPredictionPunter.AppService.SubscriberPredictorMap;
using SleekPredictionPunter.WebApp.Models;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IPredictionService _predictionService;
        private readonly IAgentService _agentService;
        private readonly ISubscriberService _subscriberService;

        public AdminController(IPredictionService predictionService, IAgentService agentService, ISubscriberService subscriberService)
        {
            _predictionService = predictionService;
            _agentService = agentService;
            _subscriberService = subscriberService;
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.AdminIndex = "active";
                var subscriberCount = await _subscriberService.GetMonthlySummaryForPredictions();
                var agentCount = await _agentService.GetMonthlySummaryForNewAgents();
                var predictionCount = await _predictionService.GetMonthlySummaryForPredictions();

                var dashboardViewModel = new DashboardViewModel
                {
                    NewSubscribers = subscriberCount,
                    NewAgents = agentCount,
                    NewPredictions = predictionCount,
                    TotalRevenueOnSubscription = "Coming soon"
                };
                return View(dashboardViewModel);
            }
            catch (Exception)
            {
                return View($"Dear user, an error occurred. Please, retry!!");
            }
        }
    }
}
