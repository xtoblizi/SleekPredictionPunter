using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SleekPredictionPunter.AppService;
using SleekPredictionPunter.AppService.Agents;
using SleekPredictionPunter.AppService.Wallet;
using SleekPredictionPunter.GeneralUtilsAndServices;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Model.IdentityModels;
using SleekPredictionPunter.Model.Wallets;

namespace SleekPredictionPunter.WebApp.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class RegisterModel : PageModel
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ILogger<RegisterModel> _logger;
		private readonly IEmailSender _emailSender;
		private readonly ISubscriberService _subscriberService;
		private readonly IAgentRefereeMapService _agentRefereeMapService;
		private readonly IAgentService _agentService;
		private readonly IWalletAppService _walletAppService;
		private readonly RoleManager<ApplicationRole> _roleManager;
		const string userRole = "userRole";
		public RegisterModel(
			RoleManager<ApplicationRole> roleManager,
			ISubscriberService susbscriberService,
			UserManager<ApplicationUser> userManager,
			IAgentService agentService,
			SignInManager<ApplicationUser> signInManager,
			ILogger<RegisterModel> logger,
			IEmailSender emailSender, 
			IAgentRefereeMapService agentRefereeMapService,
			IWalletAppService walletAppService)
		{
			_roleManager = roleManager;
            _userManager = userManager;
			_subscriberService = susbscriberService;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _agentService = agentService;
			_agentRefereeMapService = agentRefereeMapService;
			_walletAppService = walletAppService;
        }

		[BindProperty]
		public InputModel Input { get; set; }

		public string ReturnUrl { get; set; }

		public string RefLink { get; set; }

		[TempData]
		public string RegistrationStatusMessge { get; set; }

		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		public class InputModel
		{
			[Required]
			[EmailAddress]
			[Display(Name = "Email")]
			public string Email { get; set; }

			[Required]
			public string FirstName { get; set; }
			[Required]
			public string LastName { get; set; }

			public string Country { get; set; }
			public string State { get; set; }
			public string City { get; set; }
            public string PhoneNumber { get; set; }
            public int Gender { get; set; }
            public string Street { get; set; }

            [Required]
			[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
				MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
			public string ConfirmPassword { get; set; }

			[DataType(DataType.Date)]
			[Required]
			public DateTime DateOfBirth { get; set; }
			
			public string ReferrerCode { get; set; }
		}

        public async Task OnGetAsync(string returnUrl = null, int? userType = null, string refCode = null)
        {
            ViewData["userType"] =  userType == null ? "2" : userType.ToString() ;
			ViewData["makeRefCodeHidden"] = "false";

			ReturnUrl = returnUrl;
			if (!string.IsNullOrEmpty(refCode))
			{
				ViewData["makeRefCodeHidden"] = "true";
				ViewData["userType"] = "2";
				ViewData["refCode"] = refCode; 
			}

			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

		/// <summary>
		/// The register post method was extended to include the nullable role value from the route parameters
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <param name="role"></param>
		/// <returns></returns>
		public async Task<IActionResult> OnPostAsync(int registrationType, string returnUrl = null,
			 string userType=null)
		{
			try
			{
				int role = 0;
				var userTypeConvert = int.TryParse(userType, out role);
				//wallet model builder
				var walletModel = new WalletModel
				{
					UserEmailAddress = Input.Email,
					UserRole = RoleEnum.Subscriber,
					Amount = 0.0m,
				};
				if (userTypeConvert)
				{
					returnUrl = returnUrl ?? Url.Content("~/");
					var roleName = string.Empty;
					if (role == 0)
					{
						roleName = RoleEnum.Subscriber.ToString();
					}
					else
					{
						roleName = ((RoleEnum)role).ToString();
					}

					//ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
					if (registrationType == 1)
					{
						var user = new ApplicationUser
						{
							UserName = Input.Email,
							Email = Input.Email,
							FirstName = Input.FirstName,
							LastName = Input.LastName,
							City = Input.City,
							Country = Input.Country,
							DateofBirth = Input.DateOfBirth,
							State = Input.State,
						};

						var result = await _userManager.CreateAsync(user, Input.Password);
						if (result.Succeeded)
						{
							#region 
							if (result.Succeeded && !(await _userManager.IsInRoleAsync(user, roleName)))
							{
								// add user to role
								await _userManager.AddToRoleAsync(user, roleName);

								// create entity based on role
								if(role == (int)RoleEnum.Subscriber)
								{
									await _walletAppService.InsertNewAmount(walletModel);
									await CreateSubscriber(user, Input.ReferrerCode);
									ViewData["RegistrationStatusMessge"] = $"Welcome {user.FullName}, Your registration was succesuful, " +
										$"Guess what you can start making wins off our powerful predictions right way." +
										$"\n  Login now and start making that wins. ";
								}
								else 
								{ 
									var refCode = await CreateAgent(user);
									walletModel.UserRole = RoleEnum.Agent;
									await _walletAppService.InsertNewAmount(walletModel);
									var refLink = Url.Page("/Identity/Account/Register",pageHandler: null,
									values: new { area = "Identity", registrationType = "1", userType = "2", refCode = refCode },
									protocol: Request.Scheme);
									ViewData["RefLink"] = refLink;

									ViewData["RegistrationStatusMessge"] = $"Agent Registration Successful. \n Your RefererCode is {refCode}." +
										$" \n \n Preferably use your refererlink to start referring users to Predictive Power and make money ";
								}
							}
							else
							{
								ViewData["RegistrationStatusMessge"] = $"Registration was not successful at this time,please try again with your valid details. \n {result.Errors.FirstOrDefault().Description} .Thanks";
							}
							#endregion

							#region if registration confirmation is needed
							//var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
							//code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
							//var callbackUrl = Url.Page(
							//	"/Account/ConfirmEmail",
							//	pageHandler: null,
							//	values: new { area = "Identity", userId = user.Id, code = code },
							//	protocol: Request.Scheme);

							//await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
							//	$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

							#endregion

							if (_userManager.Options.SignIn.RequireConfirmedAccount)
							{
								return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
							}
							else
							{
								await _signInManager.SignInAsync(user, isPersistent: false);
								
							}
						}
						foreach (var error in result.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
					else if (registrationType == 2)
					{
						HttpContext.Session.SetString(userRole, userType);
						var redirectUrl = Url.Action("ThirdPartyLoginCallback", "ThirdPartyCallBack", new { returnUrl });
						redirectUrl = redirectUrl.Remove(0, 9);
						var prop = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
						return new ChallengeResult("Google", prop);
					}
				}
				else
				{
					RegistrationStatusMessge = "You can only register as a subscriber or an agent";
				}

				// If we got this far, something failed, redisplay form
				ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
				return Page();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

        private async Task<string> CreateAgent(ApplicationUser user)
        {
            var now = DateTime.Now;
            var refCode =  $"{user.FirstName}{now.Hour}{now.Minute}{now.Second}{now.Millisecond}";

            var agent = new Agent()
            {
                ActivatedStatus = EntityStatusEnum.Active,
                BrandNameOrNickName = user.FullName,
                City = user.City,
                Country = user.Country,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsTenant = true,
                Username = user.UserName,
                RefererCode = refCode
            };

            await _agentService.CreateAgent(agent);
            return refCode;
        }

		private async Task<bool> CreateSubscriber(ApplicationUser user, string refereerCode = null)
		{

			var subscriber = new Subscriber()
			{
				ActivatedStatus = EntityStatusEnum.Active,
				BrandNameOrNickName = user.FullName,
				City = user.City,
				Country = user.Country,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				IsTenant = true,
				Username = user.UserName,
				RefererCode = refereerCode
			};

			var insert = await _subscriberService.Insert(subscriber);
			if (insert > 0 && !string.IsNullOrEmpty(refereerCode)) 
			{
				var getAgentByReferralCode = await _agentService.GetAgentsPredicate(x=>x.RefererCode==Input.ReferrerCode);
				if(getAgentByReferralCode != null)
                {
					var subscriberToAgentMap = new AgentRefereeMap
					{
						RefereerCode = Input.ReferrerCode,
						AgentUsername = getAgentByReferralCode.Username,
						RefereeUsername = Input.Email,
					};
					await _agentRefereeMapService.Create(subscriberToAgentMap);
					return true;
				}
			}
			return true;
		}
		public async Task<IActionResult> ThirdPartySignUpCallback(string returnUrl = null)
        {
			returnUrl = returnUrl ?? Url.Content("~/");

			ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			var getRemoteInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (ModelState.IsValid && getRemoteInfo != null)
            {
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
				};
				var result = await _userManager.CreateAsync(externalUserModelBuilder, externalUserModelBuilder.PasswordHash);

                if (result.Succeeded)
                {
					// add user to correct role
					var roleName = string.Empty;

					roleName = RoleEnum.Subscriber.ToString();

					#region 
					if (result.Succeeded && !(await _userManager.IsInRoleAsync(externalUserModelBuilder, roleName)))
					{
						await _userManager.AddToRoleAsync(externalUserModelBuilder, roleName);
					}
					#endregion
					return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
				}
            }
			return Page();
        }

       
    }

    
}
