using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using SleekPredictionPunter.AppService;
using SleekPredictionPunter.AppService.Agents;
using SleekPredictionPunter.AppService.Predictors;
using SleekPredictionPunter.AppService.ThirdPartyAppService;
using SleekPredictionPunter.AppService.Wallet;
using SleekPredictionPunter.DataInfrastructure;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Model.IdentityModels;
using SleekPredictionPunter.Model.Wallets;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class ThirdPartyCallBackController : BaseController
    {
        private readonly PredictionDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IThirdPartyUsersAppService _thirdPartyUsersAppService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAgentService _agentService;
        private readonly ISubscriberService _subscriberService;
        private readonly IPredictorService _predictorService;
        private readonly IWalletAppService _walletAppService;

        //assigned variables..
        const string userEmail = "userEmail";
        const string userName = "userName";
        const string userRole = "userRole";
        const string userId = "userId";
        const string userPhoneNumber = "userPhoneNumber";
        public ThirdPartyCallBackController(PredictionDbContext context, IThirdPartyUsersAppService thirdPartyUsersAppService,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IAgentService agentService, ISubscriberService subscriberService, IPredictorService predictorService, IWalletAppService walletAppService)
        {
            _context = context;
            _thirdPartyUsersAppService = thirdPartyUsersAppService;
            _userManager = userManager;
            _signInManager = signInManager;
            _subscriberService = subscriberService;
            _predictorService = predictorService;
            _agentService = agentService;
            _walletAppService = walletAppService;
        }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        // GET: Agents
        public async Task<IActionResult> ThirdPartyLoginCallback(string returnUrl = null)
        {
            //returnUrl = returnUrl ?? Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var processingMessage = string.Empty;
            var getRemoteInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (ModelState.IsValid && getRemoteInfo != null)
            {
                //Check if user already exist..
                var getAllThirdpartyUsers = await _thirdPartyUsersAppService.GetThirdPartyUserDetailsByEmail();
                var filter = getAllThirdpartyUsers.Where(x => x.EmailAddress == getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Email) && x.ProviderName == getRemoteInfo.ProviderDisplayName);

                var getUserByEmail = await _userManager.FindByEmailAsync(getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Email));
                var userRolesList = await _userManager.GetRolesAsync(getUserByEmail);
                var userRoles = string.Join(",", userRolesList);
                var authId = string.Empty;
                if (getUserByEmail != null)
                {
                    authId = getUserByEmail.Id;
                }

                if (filter.Any() && filter.Count() == 1)
                {
                    foreach (var getUser in filter)
                    {
                        HttpContext.Session.SetString(userEmail, getUser.EmailAddress);
                        HttpContext.Session.SetString(userId, getUser.Id.ToString());
                        HttpContext.Session.SetString(userName, getUser.Username);
                        HttpContext.Session.SetString(userRole,getUser.UserRole.ToString());
                        return LocalRedirect(returnUrl);
                    }
                }
                else if (filter.Any() && filter.Count() > 1)
                {
                    filter = getAllThirdpartyUsers.Where(x => x.EmailAddress == getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Email)
                                && x.ProviderName == getRemoteInfo.ProviderDisplayName && x.FirstName == getRemoteInfo.Principal.FindFirstValue(ClaimTypes.GivenName)
                                && x.LastName == getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Surname) && x.AuthId == int.Parse(authId));
                    if (filter.Any())
                    {
                        foreach (var getUser in filter)
                        {
                            HttpContext.Session.SetString(userEmail, getUser.EmailAddress);
                            HttpContext.Session.SetString(userId, getUser.Id.ToString());
                            HttpContext.Session.SetString(userName, getUser.Username);
                            HttpContext.Session.SetString(userRole, getUser.UserRole.ToString());
                            return LocalRedirect(returnUrl);
                        }
                    }
                }
                else
                {
                    #region Model builder
                 
                    var getPhoneNumber = string.Empty;
                    getPhoneNumber = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.MobilePhone) == null ? getRemoteInfo.Principal.FindFirstValue(ClaimTypes.HomePhone) : getRemoteInfo.Principal.FindFirstValue(ClaimTypes.OtherPhone);

                    #region Model Builder region
                    var externalUserModelBuilder = new ApplicationUser
                    {
                        UserName = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Email),
                        Email = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Email),
                        FirstName = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.GivenName),
                        LastName = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Surname),
                        City = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.StreetAddress),
                        Country = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Country),
                        DateofBirth = Convert.ToDateTime(getRemoteInfo.Principal.FindFirstValue(ClaimTypes.DateOfBirth)),
                        State = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.StateOrProvince),
                        PhoneNumber = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.MobilePhone),
                        IsTenant = true,
                    };


                    var modelBuilder = new ThirdPartyUsersModel
                    {
                        EmailAddress = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Email),
                        PhoneNumbers = getPhoneNumber,
                        FirstName = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Name),
                        LastName = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Surname),
                        Username = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Email),
                        ProviderName = getRemoteInfo.ProviderDisplayName,
                        DateCreated = DateTime.Now,
                        DateOfBirth = Convert.ToDateTime(getRemoteInfo.Principal.FindFirstValue(ClaimTypes.DateOfBirth)),
                        UserRole = RoleEnum.Subscriber,
                        UserRoleName = RoleEnum.Subscriber.ToString()
                    };

                    //wallet model builder
                    var walletModel = new WalletModel
                    {
                        UserEmailAddress = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Email),
                        UserRole = RoleEnum.Subscriber,
                        Amount = 0.0m,
                    };

                    // subscriber model builder
                    var subscriber = new Subscriber
                    {
                       FirstName = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Name),
                        LastName = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Surname),
                        Email = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Email),
                        State = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.StateOrProvince),
                        Street = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.StreetAddress),
                        DateCreated = DateTime.Now,
                        DateOfBirth = Convert.ToDateTime(getRemoteInfo.Principal.FindFirstValue(ClaimTypes.DateOfBirth)),
                        City = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Locality),
                        Country = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Country),
                        Gender = (GenderEnum)Convert.ToInt32(getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Gender)),
                        PhoneNumber = getPhoneNumber,
                        IsTenant = true,
                        Username = getRemoteInfo.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                    #endregion

                    var password = "1234567890";
                    var result = await _userManager.CreateAsync(externalUserModelBuilder, password);

                    #endregion

                    if (result.Succeeded)
                    {

                        if (string.IsNullOrEmpty(getPhoneNumber))
                        {
                            ViewBag.Errors = "Could not obtain mobile number from your google. Please, Update your Mobile number on your profile.";
                        }

                        if (result.Succeeded && !(await _userManager.IsInRoleAsync(externalUserModelBuilder, RoleEnum.Subscriber.ToString())))
                        {
                            await _userManager.AddToRoleAsync(externalUserModelBuilder, RoleEnum.Subscriber.ToString());

                            //inserting to db at this point..
                            var insert = await _thirdPartyUsersAppService.InsertNewThirdPartyUser(modelBuilder);

                            if (insert > 0)
                            {
                                var inserts = await _subscriberService.Insert(subscriber);
                                if (inserts > 0)
                                {
                                    var createUserWallet = await _walletAppService.InsertNewAmount(walletModel);
                                    if (createUserWallet == true)
                                    {
                                        HttpContext.Session.SetString(userEmail, modelBuilder.EmailAddress);
                                        HttpContext.Session.SetString(userId, modelBuilder.Id.ToString());
                                        HttpContext.Session.SetString(userName, modelBuilder.Username);
                                        return LocalRedirect(returnUrl);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return RedirectToAction("index","home");
        }
    }
}
