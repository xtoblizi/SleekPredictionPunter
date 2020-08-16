﻿using System;
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
using SleekPredictionPunter.GeneralUtilsAndServices;
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
		const string userRole = "userRole";
		public RegisterModel(
			RoleManager<ApplicationRole> roleManager,
			ISubscriberService susbscriberService,
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			ILogger<RegisterModel> logger,
			IEmailSender emailSender)
		{
			_roleManager = roleManager;
			_userManager = userManager;
			_subscriberService = susbscriberService;
			_signInManager = signInManager;
			_logger = logger;
			_emailSender = emailSender;
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

		public async Task OnGetAsync(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

		}

		/// <summary>
		/// The register post method was extended to include the nullable role value from the route parameters
		/// </summary>
		/// <param name="returnUrl"></param>
		/// <param name="role"></param>
		/// <returns></returns>
		public async Task<IActionResult> OnPostAsync(int registrationType, string returnUrl = null, int? role = null, string provider = null, string userType=null)
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

				//ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
				if (/*ModelState.IsValid && */registrationType == 1)
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

					var subscriberModel = new SleekPredictionPunter.Model.Subscriber
					{
						State = Input.State,
						DateOfBirth = Input.DateOfBirth,
						FirstName = Input.FirstName,
						LastName = Input.LastName,
						Email = Input.Email,
						DateCreated = DateTime.Now,
						IsTenant = true,
						Street = Input.Street,
						City = Input.City,
						Country = Input.Country,
						PhoneNumber = Input.PhoneNumber,
						Gender=(GenderEnum)Input.Gender
						
					};

					var result = await _userManager.CreateAsync(user, Input.Password);
					if (result.Succeeded)
					{
						
						// add user to correct role
						#region 
						if (result.Succeeded && !(await _userManager.IsInRoleAsync(user, roleName)))
						{
							await _userManager.AddToRoleAsync(user, roleName);

							//add user to subscriber table..
							var subsciber = await _subscriberService.Insert(subscriberModel);
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
				else if (registrationType == 2)
				{
					HttpContext.Session.SetString(userRole,userType);
					var redirectUrl = Url.Action("ThirdPartyLoginCallback", "ThirdPartyCallBack", new { returnUrl });
					
					var prop = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
					return new ChallengeResult("Google", prop);
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

		public async Task<IActionResult> ThirdPartySignUpCallback(string returnUrl = null)
        {
			returnUrl = returnUrl ?? Url.Content("~/");

			ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			//if(remoteError != null)
   //         {
			//	ModelState.AddModelError(string.Empty, $"An error just occurred while signingin with google. \n See issues: {remoteError}");
			//	//write the exception out using a view bag
			//	//ViewBag.Error = remoteError;
			//	return Page();
   //         }

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
