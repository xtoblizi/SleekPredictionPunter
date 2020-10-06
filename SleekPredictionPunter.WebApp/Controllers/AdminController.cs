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
using SleekPredictionPunter.Model;
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

     
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
               

                var subscriberCount = await _subscriberService.GetMonthlySummaryForPredictions();
                var agentCount = await _agentService.GetMonthlySummaryForNewAgents();
                var totalsubscriberCount = await _subscriberService.GetCount();
                var predictionCount = await _predictionService.GetMonthlySummaryForPredictions();
                var predictionCountTotal = await _predictionService.GetCount();

                var totalagentcount = await _agentService.GetCount();

                var dashboardViewModel = new DashboardViewModel
                {
                    NewSubscribers = subscriberCount,
                    NewAgents = agentCount,
                    NewPredictions = predictionCount,

                    AllAgentsCount = totalagentcount,
                    AllSubscriberCount = totalsubscriberCount,
                    AllPredictionCount= predictionCountTotal,
                    TotalRevenueOnSubscription = "Coming soon"
                };
                return View(dashboardViewModel);
            }
            catch (Exception)
            {
                return View($"Dear user, an error occurred. Please, try reloaded!!!");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAgents(int page = 1,int count = 20)
        {
            try
            {
                ViewBag.UsersAgents = "active";
                ViewBag.UsersandAgents = "active";
                var skip = count * (page - 1);
                var dashboardViewModel = new PaginationModel<Agent>
                {
                    PerPage = count,
                    CurrentPage = page,
                    TotalRecordCountOfTheTable = await _agentService.GetCount(),
                    TModel = await _agentService.GetAgents(startIndex:skip,count:count),
                };
                return View(dashboardViewModel);
            }
            catch (Exception)
            {
                return View($"Dear user, an error occurred. Please, try reloading!!!");
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllSubscribers(int page = 1,int count = 50)
        {
            try
            {
                ViewBag.UsersandAgents = "active";
                ViewBag.UsersSubscribers = "active";
                var skip = count * (page - 1);
                var dashboardViewModel = new PaginationModel<Subscriber>
                {
                    PerPage = count,
                    CurrentPage = page,
                    TotalRecordCountOfTheTable = await _subscriberService.GetCount(),
                    TModel = await _subscriberService.GetAllQueryable(startIndex: skip, count: count),
                };
                return View(dashboardViewModel);
            }
            catch (Exception)
            {
                return View($"Dear user, an error occurred. Please, try reloading!!!");
            }
        }
    }

}
