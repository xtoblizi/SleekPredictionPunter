using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PayStack.Net;
using SleekPredictionPunter.AppService.PaymentService;
using SleekPredictionPunter.AppService.Plans;
using SleekPredictionPunter.AppService.TransactionLog;
using SleekPredictionPunter.AppService.Wallet;
using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Model.IdentityModels;
using SleekPredictionPunter.Model.PricingPlan;
using SleekPredictionPunter.Model.TransactionLogs;
using SleekPredictionPunter.Model.Wallets;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPaymentAppService _paymentAppService;
        private readonly IPricingPlanAppService _pricingPlanAppService;
        private readonly IWalletAppService _walletAppService;
        private readonly ITransactionLogAppService _transactionLogAppService;
        public SubscriptionController(UserManager<ApplicationUser> userManager, IPaymentAppService paymentAppService,
            IPricingPlanAppService pricingPlanAppService, IWalletAppService walletAppService, ITransactionLogAppService transactionLogAppService)
        {
            _userManager = userManager;
            _paymentAppService = paymentAppService;
            _pricingPlanAppService = pricingPlanAppService;
            _walletAppService = walletAppService;
            _transactionLogAppService = transactionLogAppService;
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
                    var returnUrl = "/subscription/SubscribeToPlan?id="+id;
                    return Redirect("/Identity/Account/Login?returnUrl=" +returnUrl);
                }

                var userRole = HttpContext.Session.GetString("userRole");
                RoleEnum roleEnum = userRole == ((int)RoleEnum.Agent).ToString() ? RoleEnum.Agent
                                                : userRole == ((int)RoleEnum.Subscriber).ToString() ? RoleEnum.Subscriber
                                                : userRole == ((int)RoleEnum.Predictor).ToString() ? RoleEnum.Predictor
                                                : userRole == ((int)RoleEnum.SuperAdmin).ToString() ? RoleEnum.SuperAdmin
                                                : RoleEnum.Subscriber;

                if (roleEnum != RoleEnum.Subscriber)
                {
                    ViewBag.ProcessingMessage = $"Only a subscriber can subscribe to a pcakages. Your role is that of an {roleEnum.ToString()}";
                    return View();
                }
                //to appsettings.json
                string callbackUrl = "https://localhost:50012/subscription/PaymentCallBack";
                //check i user has money in his/her wallet. if first and foremost, any transaction records exist, check wallet else just redirect to payment platform.
                var getUserDetails = await _userManager.FindByEmailAsync(HttpContext.Session.GetString("userEmail"));
                //check if user is already subscribed to plan

                //get plan details here..
                var getPlanDetails = await _pricingPlanAppService.GetById(id);
                
                if (getUserDetails != null)
                {
                    var transLog = new TransactionLogModel
                    {
                        UserEmailAddress = getUserDetails.Email,
                        UserRole = roleEnum,
                        CurrentAmount = getPlanDetails.Price
                    };
                    var paymentservice = await _paymentAppService.PaystackPaymentOption(transLog, callbackUrl, getUserDetails);
                    if (paymentservice.Item1 != null)
                    {
                        var insert = await _transactionLogAppService.InsertNewLog(transLog);
                        if (insert != null)
                        {
                            return Redirect(paymentservice.Item1.Data.AuthorizationUrl);
                        } 
                    }
                }
                ViewBag.ProcessingMessage = "Subscription to this package was unsucessful. Please, retry.";
                return View();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IActionResult> PaymentCallBack(string reference)
        {
            try
            {
                var userRole = HttpContext.Session.GetString("userRole");
                RoleEnum roleEnum = userRole == "4" ? RoleEnum.Agent : userRole == "3" ? RoleEnum.Predictor : RoleEnum.Subscriber;
                string secretKey = "sk_test_ff43f0891562d8d81cfd17389654a0c11c157258";
                var transaction = new PayStackApi(secretKey);
                var confirmation = transaction.Transactions.Verify(reference);
                if(confirmation.Status == true)
                {
                    var getLogByRef = await _transactionLogAppService.GetPredicatedTransactionLog(x => x.ReferenceNumber == reference);
                    var walletModel = new WalletModel();

                    walletModel = new WalletModel
                    {
                        UserEmailAddress = HttpContext.Session.GetString("userEmail"),
                        UserRole = roleEnum,
                        Amount = confirmation.Data.Amount,
                        LastAmountTransacted = 0,//for now, please, refer back here. call the wallet service and pull out the info from there. get the last item in/from the list of recent transactions..
                        DateTimeLastTransacted = DateTime.Now,//do same heere.
                    };
                    var inserToWallet = await _walletAppService.InsertNewAmount(walletModel);

                    if(inserToWallet == true && getLogByRef != null)
                    {
                        var logModel = new TransactionLogModel
                        {
                            CurrentAmount = confirmation.Data.Amount,
                            TransactionStatus = TransactionstatusEnum.Success,
                            TransactionStatusName = TransactionstatusEnum.Success.ToString(),
                            DateUpdated = DateTime.Now,
                            LastAmountTransacted = walletModel.LastAmountTransacted,
                            DateTimeOfLastTransacted = walletModel.DateTimeLastTransacted
                        };
                         _transactionLogAppService.UpdateTransactionLog(logModel);
                        ViewBag.Message = "Successfully Subscribed";
                        return View();
                    }
                }
                var log = new TransactionLogModel
                {
                    CurrentAmount = confirmation.Data.Amount,
                    TransactionStatus = TransactionstatusEnum.Failed,
                    TransactionStatusName = TransactionstatusEnum.Failed.ToString(),
                    DateUpdated = DateTime.Now,
                    ErrorDescription = confirmation.Data.Message
                };
                _transactionLogAppService.UpdateTransactionLog(log);
                ViewBag.Message = "Could not process payment. Please,try again!";
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
