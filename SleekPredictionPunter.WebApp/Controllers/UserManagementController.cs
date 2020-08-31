using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SleekPredictionPunter.AppService.UserManagement;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.IdentityModels;
using SleekPredictionPunter.Model.TransactionLogs;
using SleekPredictionPunter.Model.Wallets;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly IUserManagementAppService _userManagementAppService;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserManagementController(IUserManagementAppService userManagementAppService)
        {
            _userManagementAppService = userManagementAppService;
        }
        public async Task<IActionResult> Index()
        {
            var SessionEmail = HttpContext.Session.GetString("userEmail");
            Func<WalletModel, bool> predicateForWallet = (x=>x.UserEmailAddress == SessionEmail);
            Func<TransactionLogModel, bool> predicateForTransactionLog = (x => x.UserEmailAddress == SessionEmail);
            var getWallet = await _userManagementAppService.GetUserWalletDetails(predicateForWallet);
            var getAllLogs = await _userManagementAppService.UserLogs(predicateForTransactionLog);
            var getSubscriberDetails = await _userManagementAppService.GetSubscriberDetails(x => x.Email == SessionEmail);
            var dto = new PredicateForTransactionLog
            {
                WalletModel = getWallet,
                TransactionLog  = getAllLogs,
                SubscriberModel = getSubscriberDetails
            };
            return View(dto);
        }

        [Route("userManagement/logs")]
        public async Task<IActionResult> AllUserLogs()
        {
            var SessionEmail = HttpContext.Session.GetString("userEmail");
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
