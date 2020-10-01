using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PayStack.Net;
using SleekPredictionPunter.AppService;
using SleekPredictionPunter.AppService.Agents;
using SleekPredictionPunter.AppService.PaymentService;
using SleekPredictionPunter.AppService.Plans;
using SleekPredictionPunter.AppService.Subscriptions;
using SleekPredictionPunter.AppService.TransactionLog;
using SleekPredictionPunter.AppService.Wallet;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Model.IdentityModels;
using SleekPredictionPunter.Model.TransactionLogs;
using SleekPredictionPunter.Model.Wallets;
using System;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class SubscriptionController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPaymentAppService _paymentAppService;
        private readonly IPricingPlanAppService _pricingPlanAppService;
        private readonly IWalletAppService _walletAppService;
        private readonly ITransactionLogAppService _transactionLogAppService;
        private readonly ISubscriptionAppService _subscriptionAppService;
        private readonly ISubscriberService _subscriberService;
        private readonly IAgentService _agentService;
        private readonly IAgentRefereeMapService _agentRefereeMapService;
        private string AgentCommission = "commission";
        public SubscriptionController(UserManager<ApplicationUser> userManager, IPaymentAppService paymentAppService,
            IPricingPlanAppService pricingPlanAppService, IWalletAppService walletAppService,
            ITransactionLogAppService transactionLogAppService, 
            ISubscriptionAppService subscriptionAppService,ISubscriberService subscriberService,
            IAgentService agentService, IAgentRefereeMapService agentRefereeMapService)
        {
            _userManager = userManager;
            _paymentAppService = paymentAppService;
            _pricingPlanAppService = pricingPlanAppService;
            _walletAppService = walletAppService;
            _transactionLogAppService = transactionLogAppService;
            _subscriptionAppService = subscriptionAppService;
            _subscriberService = subscriberService;
            _agentService = agentService;
            _agentRefereeMapService = agentRefereeMapService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[Authorize(Roles = nameof(RoleEnum.Subscriber))]
        public async Task<IActionResult> SubscribeToPlan(long id)
        {
            try
            {
                var procesingMessage = string.Empty;
                if (!User.IsInRole(RoleEnum.Subscriber.ToString()))
                {
                    TempData["ProcessingMessage"] = $"Only a subscriber can subscribe to a pcakages.";
                }
             
                //to appsettings.json
                var actionLink = $"subscription/paymentcallback";
                var callbackUrl = $"{Request.Scheme}://{Request.Host}/{actionLink}";
               
				//check i user has money in his/her wallet. if first and foremost, any transaction records exist, check wallet else just redirect to payment platform.
				var getUserDetails = await _userManager.FindByEmailAsync(User.Identity.Name);
                
				if(getUserDetails == null)
				{
                    TempData["ProcessingMessage"] = "Your request cannot be completed, please relogin and try again. Your credential could not be validated";
					return View();
				}
                //get plan details here..
                var getPlanDetails = await _pricingPlanAppService.GetById(id);
                 HttpContext.Session.SetString(AgentCommission,Convert.ToString(getPlanDetails.PlanCommission));
              
                //check if user is already subscribed to plan
                var email = User.Identity.Name;
                Func<Subcription, bool> predicate = ((x => x.SubscriberUsername == email
                && x.PricingPlanId == getPlanDetails.Id && x.ExpirationDateTime > DateTime.Now));

                var getSubscribeddetails = await _subscriptionAppService.GetPredicateRecord(predicate);
                Func<WalletModel, bool> predicatedWallet = (x => x.UserEmailAddress == email);
                var checkUserBalance = await _walletAppService.GetAllWalletD(predicatedWallet);
                
                if (getSubscribeddetails == null )
                {
                    var transLog = new TransactionLogModel
                    {
                        UserEmailAddress = getUserDetails.Email,
                        UserRole = RoleEnum.Subscriber,
                        CurrentAmount = getPlanDetails.Price,
                        PlanId = getPlanDetails.Id
                    };
                    var paymentservice = await _paymentAppService.PaystackPaymentOption(transLog, callbackUrl, getUserDetails);
                    if (paymentservice.Item1 != null)
                    {
                        var insert = await _transactionLogAppService.InsertNewLog(transLog);
                        if (insert != null)
                        {
                            return Redirect(paymentservice.Item1.Data.AuthorizationUrl);

                            //var checkTotal = getPlanDetails.Price;
                            //if(checkUserBalance.Amount > 0 && checkUserBalance.Amount < checkTotal)
                            //{
                            //    //debit user from here and move on...
                            //    ViewBag.Message = "This transaction was successful..";
                            //    return RedirectToAction("index","home");
                            //}
                            //else
                            //{
                            //    //Todo: subscriber/plan map then insert into db
                            //    return Redirect(paymentservice.Item1.Data.AuthorizationUrl);
                            //}
                        } 
                    }
				}
				else
				{
					TempData["ProcessingMessage"] = $"Your Subscription for this package is still active. it expires on  {getSubscribeddetails?.ExpirationDateTime}";
                    return Redirect("/Pricingplan/index/");
                }

                TempData["ProcessingMessage"] = "Subscription to this package was unsucessful. Please, retry.";
				return Redirect("/Pricingplan/index/");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IActionResult> PaymentCallBack(string reference = null, string trxref = null)
        {
            try
            {

                var email = User.Identity.Name;
                var userRole = RoleEnum.Subscriber.ToString();
                RoleEnum roleEnum =  RoleEnum.Subscriber;
                string secretKey = "sk_live_ec2ce7fcefeefb71cb02c017e0a81a9e658cbcbc";
                var transaction = new PayStackApi(secretKey);
                var confirmation = transaction.Transactions.Verify(reference);
                if(confirmation.Status == true)
                {
                    Func<WalletModel, bool> predicateForsubSubscriber = (x => x.UserEmailAddress == email);
                    var getWalletDetailsForThisSubscriber = await _walletAppService.GetAllWalletD(predicate: predicateForsubSubscriber);
                    
                    Func<Subscriber, bool> predicate = (x => x.Email == email);
                    var getSubscriberDetails = await _subscriberService.GetFirstOrDefault(predicate);

                    var getLogByRef = await _transactionLogAppService.GetPredicatedTransactionLog(x => x.ReferenceNumber == reference);
                    var walletModel = new WalletModel 
                    {                 
                        Id = getWalletDetailsForThisSubscriber.Id,
                        UserEmailAddress = email,
                        UserRole = roleEnum,
                        Amount = confirmation.Data.Amount,
                        LastAmountTransacted = 0,//for now, please, refer back here. call the wallet service and pull out the info from there. get the last item in/from the list of recent transactions..
                        DateTimeLastTransacted = DateTime.Now,//do same heere.
                    };
                     await _walletAppService.UpdateWalletDetails(walletModel);

                    if(getLogByRef != null && getSubscriberDetails!=null)
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

                       await _transactionLogAppService.UpdateTransactionLog(logModel);

                       var subscriptionModel = new Subcription
                        {
                            PricingPlanId = getLogByRef.PlanId,
                            SubscriberId = getSubscriberDetails.Id,
                            NumberOfMonths= 1,
                            SubscriberUsername =email
                        };

                        subscriptionModel.ExpirationDateTime = DateTime.Now.AddMonths(subscriptionModel.NumberOfMonths);
                        var insertIntoSubscription = await _subscriptionAppService.CreateSubscription(subscriptionModel);

                        #region map details n bonus for agents
                        //get subscriber details by email from subscriber table
                        //var subscriberDetails = await _subscriberService.GetFirstOrDefault(new Subscriber { Email = email });
                        if (getSubscriberDetails != null && !string.IsNullOrEmpty(getSubscriberDetails.RefererCode))
                        {
                            //query agent table for this user with refcode
                            var getAgentByRefCode = await _agentService.GetAgentsPredicate(x => x.RefererCode == getSubscriberDetails.RefererCode);
                            //Get agent wallet to retrieve current balance
                            Func<WalletModel, bool> predicates = (x => x.UserEmailAddress == getAgentByRefCode.Email);
                            var getWalletDetailsForThisAgent = await _walletAppService.GetAllWalletD(predicate:predicates);
                            if (getAgentByRefCode != null)
                            {
                                if (getWalletDetailsForThisAgent != null)
                                {
                                    //process the  agent with some bucks here as bonus
                                    var commission = Convert.ToDecimal(HttpContext.Session.GetString("commission"));
                                   
                                    DateTime? lastTransactionDate = new DateTime();
                                    decimal? lastAmountTransacted = 0.0m;

                                    //insert to walletdb and insert to transactionLog
                                    lastTransactionDate = getWalletDetailsForThisAgent.DateTimeLastTransacted;
                                    lastAmountTransacted = getWalletDetailsForThisAgent.LastAmountTransacted;
                                    
                                    var agentMainWalletModel = new WalletModel
                                    {
                                        Id= getWalletDetailsForThisAgent.Id,
                                        UserEmailAddress = getAgentByRefCode.Email,
                                        UserRole = RoleEnum.Agent,
                                        Amount = confirmation.Data.Amount,
                                        LastAmountTransacted = lastAmountTransacted,
                                        DateTimeLastTransacted = DateTime.Now,//do same heere.
                                    };

                                    var agentLogModel = new TransactionLogModel
                                    {
                                        CurrentAmount = confirmation.Data.Amount,
                                        TransactionStatus = TransactionstatusEnum.Success,
                                        TransactionStatusName = TransactionstatusEnum.Success.ToString(),
                                        DateUpdated = DateTime.Now,
                                        LastAmountTransacted = lastAmountTransacted,
                                        DateTimeOfLastTransacted = lastTransactionDate,
                                        UserEmailAddress = getAgentByRefCode.Email,
                                        UserRole = RoleEnum.Agent
                                    };


                                    //log new general wallet price for agent
                                   await _walletAppService.UpdateWalletDetails(agentMainWalletModel);
                                    //create new log for this agent
                                    await _transactionLogAppService.UpdateTransactionLog(agentLogModel);

                                    //notifify the agent of newly added bonuses by sending email..
                                }
                                else
                                {
                                    //getAllCash and sum
                                    //process the  agent with some bucks here as bonus
                                    var bonus = confirmation.Data.Amount % 100;

                                    var agentMainWalletModel = new WalletModel
                                    {
                                        UserEmailAddress = getAgentByRefCode.Email,
                                        UserRole = RoleEnum.Agent,
                                        Amount = confirmation.Data.Amount,
                                    };

                                    var agentLogModel = new TransactionLogModel
                                    {
                                        CurrentAmount = confirmation.Data.Amount,
                                        TransactionStatus = TransactionstatusEnum.Success,
                                        TransactionStatusName = TransactionstatusEnum.Success.ToString(),
                                        DateUpdated = DateTime.Now,
                                        UserEmailAddress = getAgentByRefCode.Email,
                                        UserRole = RoleEnum.Agent
                                    };

                                    //log new general wallet price for agent
                                    await _walletAppService.InsertNewAmount(agentMainWalletModel);
                                    //create new log for this agent
                                    await _transactionLogAppService.UpdateTransactionLog(agentLogModel);

                                    //notifify the agent of newly added bonuses by sending email..
                                }
                            }
                        }
                        #endregion
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
               await  _transactionLogAppService.UpdateTransactionLog(log);
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
