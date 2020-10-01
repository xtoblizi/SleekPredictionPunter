using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SleekPredictionPunter.AppService.Plans;
using SleekPredictionPunter.AppService.Subscriptions;
using SleekPredictionPunter.AppService.UserManagement;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.IdentityModels;
using SleekPredictionPunter.Model.TransactionLogs;
using SleekPredictionPunter.Model.Wallets;

namespace SleekPredictionPunter.WebApp.Controllers
{
	
    public class UserManagementController : BaseController
    {
        private readonly IUserManagementAppService _userManagementAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISubscriptionAppService _subscriptionAppService;
        private readonly IPricingPlanAppService _planservices;
        public UserManagementController(IUserManagementAppService userManagementAppService
            ,IPricingPlanAppService pricingPlanAppService,
            ISubscriptionAppService subscriptionAppService)
        {
            _subscriptionAppService = subscriptionAppService;
            _planservices = pricingPlanAppService;
            _userManagementAppService = userManagementAppService;
        }
        public async Task<IActionResult> Index()
        {
			ViewBag.UserIndex = "active";
			
            var userEmail = await base.GetUserName();

            var dto = new PredicateForTransactionLog();

            Func<Subcription, bool> func = (x => x.SubscriberUsername == userEmail);

            Func<WalletModel, bool> predicateForWallet = (x=>x.UserEmailAddress == userEmail);
            Func<TransactionLogModel, bool> predicateForTransactionLog = (x => x.UserEmailAddress == userEmail);

            var getWallet = await _userManagementAppService.GetUserWalletDetails(predicateForWallet);
            var subcriptiondetails = await _subscriptionAppService.GetPredicateRecord(func);
            if(subcriptiondetails != null)
            {
                var plandetails = await _planservices.GetById(subcriptiondetails.PricingPlanId);
                dto.CurrentPlanExpirationDate = subcriptiondetails.ExpirationDateTime.ToString();
                
                if (plandetails != null)
                    dto.CurrentPlan = string.IsNullOrEmpty(plandetails.PlanName) ? "No Plan Active" : plandetails.PlanName;
            }

            var getAllLogs = await _userManagementAppService.UserLogs(predicateForTransactionLog);
            var getSubscriberDetails = await _userManagementAppService.GetSubscriberDetails(x => x.Email == userEmail);

            dto.WalletModel = getWallet;
            dto.TransactionLog = getAllLogs;
            dto.SubscriberModel = getSubscriberDetails;
            

            return View(dto);
        }

        public async Task<IActionResult> AllUserLogs()
        {
			ViewBag.AllUserLogs = "active";
			var SessionEmail = User.Identity.Name;
            Func<TransactionLogModel, bool> predicateForTransactionLog = (x => x.UserEmailAddress == SessionEmail);
            var getAllLogs = await _userManagementAppService.UserLogs(predicateForTransactionLog);
            var dto = new PredicateForTransactionLog
            {
                TransactionLog = getAllLogs,
            };
            return View(dto);
        }
    }
}
