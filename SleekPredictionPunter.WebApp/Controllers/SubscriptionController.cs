using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SleekPredictionPunter.AppService.PaymentService;
using SleekPredictionPunter.AppService.Plans;
using SleekPredictionPunter.AppService.Wallet;
using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Model.IdentityModels;
using SleekPredictionPunter.Model.PricingPlan;
using SleekPredictionPunter.Model.Wallet;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPaymentAppService _paymentAppService;
        private readonly IPricingPlanAppService _pricingPlanAppService;
        private readonly IWalletAppService _walletAppService;
        public SubscriptionController(UserManager<ApplicationUser> userManager, IPaymentAppService paymentAppService,
            IPricingPlanAppService pricingPlanAppService, IWalletAppService walletAppService)
        {
            _userManager = userManager;
            _paymentAppService = paymentAppService;
            _pricingPlanAppService = pricingPlanAppService;
            _walletAppService = walletAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SubscribeToPlan(long id)
        {
            try
            {
                var procesingMessage = string.Empty;
                /*Check if user session exist. if exist, continue else, redirect to login and return back to same page after login is successful.*/
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("userEmail")))
                {
                    var returnUrl = "subscription/SubscribeToPlan";
                    return Redirect("/Identity/Account/Login?returnUrl=" +returnUrl);
                }
                var userRole = HttpContext.Session.GetString("userRole");
                RoleEnum roleEnum = userRole == "4" ? RoleEnum.Agent : userRole == "3" ? RoleEnum.Predictor : RoleEnum.Subscriber;
                string callbackUrl = "/pricePlan/index";
                //check i user has money in his/her wallet. if first and foremost, any transaction records exist, check wallet else just redirect to payment platform.
                var getUserDetails = await _userManager.FindByEmailAsync(HttpContext.Session.GetString("userEmail"));

                var getPlanDetails = await _pricingPlanAppService.GetPlanById(id);
                if (getUserDetails != null)
                {
                    var walletModel = new WalletModel
                    {
                        UserEmailAddress = getUserDetails.Email,
                        UserRole = roleEnum,
                        Amount = getPlanDetails.Price
                    };
                    var paymentservice = await _paymentAppService.PaystackPaymentOption(walletModel, callbackUrl, getUserDetails);
                    if (paymentservice.Item1 != null)
                    {
                        var inserToWallet = await _walletAppService.InsertNewAmount(paymentservice.Item2);
                        if (inserToWallet == true)
                        {
                            //do mappings here for user <-> plan into subscription table.. the  redirect to payment platform..
                            return Redirect(paymentservice.Item1.Data.AuthorizationUrl);
                        }

                    }
                }
                ViewBag.ProcessingMessage = "Subscription to this package was unsucessful. Please, retry.";
                return View();
            }
            catch (Exception e)
            {
                //log to bla bla bla here lol. booooossssssss
                throw e;
            }
        }
    }
}
