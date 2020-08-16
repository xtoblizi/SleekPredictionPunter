using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SleekPredictionPunter.AppService;
using SleekPredictionPunter.AppService.Agents;
using SleekPredictionPunter.GeneralUtilsAndServices;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Model.IdentityModels;

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
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IAgentService _agentService;


        public RegisterModel(
            RoleManager<ApplicationRole> roleManager,
            ISubscriberService susbscriberService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IAgentService agentService)
        {
			_roleManager = roleManager;
            _userManager = userManager;
			_subscriberService = susbscriberService;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _agentService = agentService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

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
            public string ReferrerCode { get; set; }

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
		}

        public async Task OnGetAsync(string returnUrl = null, int? userType = null)
        {
            ViewData["userType"] = userType.ToString();
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

		/// <summary>
		/// The register post method was extended to include the nullable role value from the route parameters
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <param name="role"></param>
		/// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null,int? role = null)
        {
			try
			{
				returnUrl = returnUrl ?? Url.Content("~/");
				var roleName = string.Empty;
				if (role == null)
				{
					roleName = RoleEnum.Subscriber.ToString();
				}
				else
				{
					roleName = ((RoleEnum)role.Value).ToString();
				}

				ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
				if (ModelState.IsValid)
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
						State = Input.State
					};
					var result = await _userManager.CreateAsync(user, Input.Password);
					if (result.Succeeded)
					{
						// add user to correct role
						#region 
						if (result.Succeeded && !(await _userManager.IsInRoleAsync(user, roleName)))
						{
							await _userManager.AddToRoleAsync(user, roleName);
						}
						#endregion

						_logger.LogInformation("User created a new account with password.");

						var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
						code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
						var callbackUrl = Url.Page(
							"/Account/ConfirmEmail",
							pageHandler: null,
							values: new { area = "Identity", userId = user.Id, code = code },
							protocol: Request.Scheme);

						await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
							$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

						if (_userManager.Options.SignIn.RequireConfirmedAccount)
						{
							return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
						}
						else
						{
							await _signInManager.SignInAsync(user, isPersistent: false);
							return LocalRedirect(returnUrl);
						}
					}
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}

				// If we got this far, something failed, redisplay form
				return Page();
			}
			catch (Exception ex)
			{
				return Page();
				throw ex;
			}
        }

        private async Task<string> CreateAgent(ApplicationUser user)
        {
            var now = DateTime.Now;
            var refCode = Guid.NewGuid().ToString().Substring(0, 4);
            refCode += $"{now.Year}{now.Month}{now.Day}{now.Millisecond}";

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

        private async Task<bool> CreateSubscriber(ApplicationUser user,string refereerCode = null)
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

            await _subscriberService.Insert(subscriber);
            return true;
        }
    }

    
}
