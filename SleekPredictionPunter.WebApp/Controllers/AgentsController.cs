using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SleekPredictionPunter.AppService;
using SleekPredictionPunter.AppService.Agents;
using SleekPredictionPunter.AppService.Subscriptions;
using SleekPredictionPunter.AppService.Wallet;
using SleekPredictionPunter.DataInfrastructure;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Model.IdentityModels;
using SleekPredictionPunter.Model.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class AgentsController : Controller
    {
        private readonly PredictionDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAgentService _agentService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISubscriberService _subscriberService;
        private readonly ISubscriptionAppService _subscriptionAppService;
        private readonly IWalletAppService _walletAppService;
        private readonly IAgentRefereeMapService _agentRefereeMapService;
        public AgentsController(PredictionDbContext context, IAgentService agentService,
            UserManager<ApplicationUser> userManager, ISubscriptionAppService subscriptionAppService,
            SignInManager<ApplicationUser> signInManager, ISubscriberService subscriberService,
            IWalletAppService walletAppService, IAgentRefereeMapService agentRefereeMapService)
        {
            _context = context;
            _agentService = agentService;
            _userManager = userManager;
            _signInManager = signInManager;
            _subscriberService = subscriberService;
            _subscriptionAppService = subscriptionAppService;
            _walletAppService = walletAppService;
            _agentRefereeMapService = agentRefereeMapService;
        }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        // GET: Agents
        public async Task<IActionResult> Index()
        {
            return View(await _agentService.GetAgents());
        }

        // GET: Agents/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agent = await _agentService.GetAgentById(id.Value);
            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }

        // GET: Agents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Agents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Agent agent,string password, string confirmPassword)
        {
            try
            {
                var processingMessage = string.Empty;
                if (string.IsNullOrEmpty(password))
                {
                    processingMessage = "Password cannot be empty";
                    ViewBag.EmptyPassword = processingMessage;
                }
                else if (string.IsNullOrEmpty(confirmPassword))
                {
                    //processingMessage = "Confirm Password cannot be empty";
                    ViewBag.ProcessingMessage = "Confirm Password cannot be empty";
                }
                else if (confirmPassword != password)
                {
                    processingMessage = "Confirm Password is not same as password";
                    ViewBag.PasswordMismMatch = "Confirm Password is not same as password"; ;
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Add agent details to users table. Upon successful creation, Move unto creating the agent details forthis user.
                        #region Register agent details to users table
                        var user = new ApplicationUser
                        {
                            UserName = agent.Email,
                            Email = agent.Email,
                            FirstName = agent.FirstName,
                            LastName = agent.LastName,
                            City = agent.City,
                            Country = agent.Country,
                            DateofBirth = agent.DateOfBirth,
                            State = agent.State
                        };

                        var insertToUser = await _userManager.CreateAsync(user, password);
                        #endregion
                        if (insertToUser != null)
                        {
                            var role = RoleEnum.Agent;
                           
                            //add to roles table
                            if (insertToUser.Succeeded && !(await _userManager.IsInRoleAsync(user, role.ToString())))
                            {
                                await _userManager.AddToRoleAsync(user,role.ToString());
                            }
                            var insert = await _agentService.CreateAgent(agent);
                            if (insert > 0)
                                ViewBag.ProcessingMessage = "User Successfully registered.";
                            return RedirectToAction(nameof(Index));
                        }
                        else
                            return View(agent);
                    }
                }
                return View(agent);
            }
            catch (Exception ex)
            {
                ViewBag.ProcessingMessage = "Ooops! An error just occurred. \n Please, try again.";
                return View(agent);
            }
        }



        // GET: Agents/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agent = await _agentService.GetAgentById(id.Value);
            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }

        // POST: Agents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var agent = await _agentService.GetAgentById(id);
            await _agentService.RemoveAgentById(agent, true); 
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("Agent/MyDashboard")]
        public async Task<IActionResult> AgentDashboard()
        {
            try
            {
                var dateFrom = DateTime.Now;
                var firstDayOfTheMonth = new DateTime(dateFrom.Year, dateFrom.Month, 1);
                var dateTo = DateTime.Now;

                var user = User.Identity.Name;
                Func<Agent, bool> predicateForAgent = (x => x.Username == user);
                var getAgentInfoByUsername = await _agentService.GetAgentsPredicate(predicateForAgent);

                Func<Subscriber, bool> getAllSubscribersByRefcode = (sub =>
                (string.IsNullOrEmpty(getAgentInfoByUsername.RefererCode) || sub.RefererCode == getAgentInfoByUsername.RefererCode) &&
                (sub.DateCreated >= firstDayOfTheMonth && sub.DateCreated <= dateTo));

                var getAllSubcriberByRefCode = await _subscriberService.GetAllSubscribersByAgentRefcode(getAllSubscribersByRefcode);
                List<Subcription> subscriberSubscription = new List<Subcription>();

                foreach (var item in getAllSubcriberByRefCode)
                {
                    Func<Subcription, bool> getAllSubscriberSubscriptions = (plan => plan.SubscriberUsername == item.Username);
                    var subscription = await _subscriptionAppService.GetPredicateRecord(getAllSubscriberSubscriptions);
                    if (subscription != null)
                    {
                        subscriberSubscription.Add(subscription);
                    }
                }
                Func<Subscriber, bool> getAllSubscribers = (s => s.RefererCode == getAgentInfoByUsername.RefererCode);
                var subscriberCount = await _subscriberService.GetAllSubscribersByAgentRefcode(getAllSubscribers);

                Func<WalletModel, bool> agentWalletPredicate = (w => w.UserEmailAddress == getAgentInfoByUsername.Username);
                var agentWallet = await _walletAppService.GetAllWalletDetailsForUser(agentWalletPredicate);
                var getLastItemInTheList = agentWallet.Where(agentWalletPredicate).LastOrDefault();

                var getAgentRevenue = await _agentRefereeMapService.CalculateAgentRevenueByRefereerCode(getAgentInfoByUsername.RefererCode);

                var agentDashboardDto = new AgentDashboardDto
                {
                    SubcribrrCount = subscriberCount.LongCount(),
                    AgentEarnings = getAgentRevenue,
                    AllSubscriber = getAllSubcriberByRefCode,
                    Subscription = subscriberSubscription,
                    AgentWalletBalance = getLastItemInTheList.Amount
                };
                return View(agentDashboardDto);
            }
            catch (Exception)
            {
                var agentDashboardDto = new AgentDashboardDto
                {
                    SubcribrrCount = 0,
                    AgentEarnings = 0,
                    AllSubscriber = null,
                    Subscription = null,
                    AgentWalletBalance = 0,
                    ProcessingMessage = "An error occurred. Please, check your internet connection then, refresh your browser."
                };
                return View(agentDashboardDto);
            }

        }

        private async Task<bool> AgentExists(long id)
        {
            var checkIfExist = await _agentService.GetAgents();
            return checkIfExist.Any(e => e.Id == id);
        }
    }
}
