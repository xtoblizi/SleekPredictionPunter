using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PayStack.Net;
using SleekPredictionPunter.AppService;
using SleekPredictionPunter.AppService.PaymentService;
using SleekPredictionPunter.AppService.Plans;
using SleekPredictionPunter.AppService.Subscriptions;
using SleekPredictionPunter.AppService.TransactionLog;
using SleekPredictionPunter.AppService.Wallet;
using SleekPredictionPunter.Model;
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
        private readonly ISubscriptionAppService _subscriptionAppService;
        private readonly ISubscriberService _subscriberService;
        public SubscriptionController(UserManager<ApplicationUser> userManager, IPaymentAppService paymentAppService,
            IPricingPlanAppService pricingPlanAppService, IWalletAppService walletAppService,
            ITransactionLogAppService transactionLogAppService, 
            ISubscriptionAppService subscriptionAppService,ISubscriberService subscriberService)
        {
            _userManager = userManager;
            _paymentAppService = paymentAppService;
            _pricingPlanAppService = pricingPlanAppService;
            _walletAppService = walletAppService;
            _transactionLogAppService = transactionLogAppService;
            _subscriptionAppService = subscriptionAppService;
            _subscriberService = subscriberService;
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
               // string callbackUrl = "https://localhost:50012/subscription/PaymentCallBack";
				var callbackUrl = Url.Page("/subscription/paymentCallBack", pageHandler: null,
									values: null,
									protocol: Request.Scheme);
				//check i user has money in his/her wallet. if first and foremost, any transaction records exist, check wallet else just redirect to payment platform.
				var getUserDetails = await _userManager.FindByEmailAsync(HttpContext.Session.GetString("userEmail"));
                
				if(getUserDetails == null)
				{
					ViewBag.ProcessingMessage = "Your request cannot be completed, please relogin and try again. Your credential could not be validated";
					return View();
				}
                //get plan details here..
                var getPlanDetails = await _pricingPlanAppService.GetById(id);

                //check if user is already subscribed to plan
                var email = HttpContext.Session.GetString("userEmail");
                Func<Subcription, bool> predicate = ((x => x.SubscriberUsername == email
                && x.PricingPlanId == getPlanDetails.Id && x.DateCreated < DateTime.Now.AddMonths(-1)));

                var getSubscribeddetails = await _subscriptionAppService.GetPredicateRecord(predicate);

                if (getSubscribeddetails == null || getSubscribeddetails.ExpirationDateTime < DateTime.Now)
                {
                    var transLog = new TransactionLogModel
                    {
                        UserEmailAddress = getUserDetails.Email,
                        UserRole = roleEnum,
                        CurrentAmount = getPlanDetails.Price,
                        PlanId = getPlanDetails.Id
                    };
                    var paymentservice = await _paymentAppService.PaystackPaymentOption(transLog, callbackUrl, getUserDetails);
                    if (paymentservice.Item1 != null)
                    {
                        var insert = await _transactionLogAppService.InsertNewLog(transLog);
                        if (insert != null)
                        {
                            //Todo: subscriber/plan map then insert into db
                            return Redirect(paymentservice.Item1.Data.AuthorizationUrl);
                        } 
                    }
				}
				else
				{
					ViewBag.ProcessingMessage = $"Your Subscription for this package is still active. it expires on  {getSubscribeddetails?.ExpirationDateTime}";
				}

                ViewBag.ProcessingMessage = "Subscription to this package was unsucessful. Please, retry.";
				return Redirect("/Pricingplan/Account/");
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

                var email = HttpContext.Session.GetString("userEmail");
                var userRole = HttpContext.Session.GetString("userRole");
                RoleEnum roleEnum = userRole == "4" ? RoleEnum.Agent : userRole == "3" ? RoleEnum.Predictor : RoleEnum.Subscriber;
                string secretKey = "sk_test_ff43f0891562d8d81cfd17389654a0c11c157258";
                var transaction = new PayStackApi(secretKey);
                var confirmation = transaction.Transactions.Verify(reference);
                if(confirmation.Status == true)
                {
                    var getSubscriberDetails = await _subscriberService.GetFirstOrDefault(new Subscriber { Email = email });
                    var getLogByRef = await _transactionLogAppService.GetPredicatedTransactionLog(x => x.ReferenceNumber == reference);
                    var walletModel = new WalletModel();
                    walletModel = new WalletModel
                    {
                        UserEmailAddress = email,
                        UserRole = roleEnum,
                        Amount = confirmation.Data.Amount,
                        LastAmountTransacted = 0,//for now, please, refer back here. call the wallet service and pull out the info from there. get the last item in/from the list of recent transactions..
                        DateTimeLastTransacted = DateTime.Now,//do same heere.
                    };
                    var inserToWallet = await _walletAppService.InsertNewAmount(walletModel);

                    if(inserToWallet == true && getLogByRef != null && getSubscriberDetails!=null)
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

                       var subscriptionModel = new Subcription
                        {
                            PricingPlanId = getLogByRef.PlanId,
                            SubscriberId = getSubscriberDetails.Id,
                            NumberOfMonths= 1,
                            SubscriberUsername =email ,
                        };

                        subscriptionModel.ExpirationDateTime = DateTime.Now.AddMonths(-subscriptionModel.NumberOfMonths);
                        var insertIntoSubscription = await _subscriptionAppService.CreateSubscription(subscriptionModel);

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
