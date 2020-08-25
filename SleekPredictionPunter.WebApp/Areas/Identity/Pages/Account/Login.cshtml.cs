using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SleekPredictionPunter.Model.IdentityModels;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SleekPredictionPunter.WebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailSender _emailSender;

        //assigned variables..
        const string userEmail = "userEmail";
        const string userName = "userName";
        const string userRole = "userRole";
        const string userId = "userId";
        const string userPhoneNumber = "userPhoneNumber";

        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null,string loginType = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid && loginType=="1")
            {
				// This doesn't count login failures towards account lockout
				// To enable password failures to trigger account lockout, set lockoutOnFailure: true

				var user = await _userManager.FindByEmailAsync(Input.Email);
				if (user != null && await _userManager.CheckPasswordAsync(user, Input.Password))
				{
					#region SignIn using token
					var claims = new[]{
					new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
					new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())};

					var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("PredictivePowerSecurityTokens"));
					var token = new JwtSecurityToken(
						expires: DateTime.UtcNow.AddHours(24*2),
						claims: claims,
						signingCredentials: new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256));

					#endregion
					var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
					var userRolesList = await _userManager.GetRolesAsync(user);
					var userRoles = string.Join(",", userRolesList);

					if (result.Succeeded)
					{
						//Get user details by email then, pass user successful to httpcontextaccessor then, use on UI....
						//var getUser = await _userManager.FindByEmailAsync(Input.Email);
						HttpContext.Session.SetString(userEmail, user.Email);
						HttpContext.Session.SetString(userId, user.Id);
						HttpContext.Session.SetString(userName, user.UserName);
						HttpContext.Session.SetString("isAuthenticated", "true");
						HttpContext.Session.SetString("fullName", user.FullName);
						HttpContext.Session.SetString("roles", userRoles);
						HttpContext.Session.SetString("token", new JwtSecurityTokenHandler().WriteToken(token));

						_logger.LogInformation("User logged in.");
						return LocalRedirect(returnUrl);
					}
					else if (result.RequiresTwoFactor)
					{
						return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
					}
					else if (result.IsLockedOut)
					{
						_logger.LogWarning("User account locked out.");
						return RedirectToPage("./Lockout");
					}
					else
					{
						var notalloewd = result.IsNotAllowed == true ? "User trying to sign in is not allowed." : "";
						ErrorMessage = $"Sign was not succesful at the time. {notalloewd} ";
						ModelState.AddModelError(string.Empty, "Invalid login attempt.");
						return Page();
					}
				}

				ErrorMessage = "Invalid Username or password, could not find user, please try again with correct details";

			}

            else if (loginType == "2")
            {
                var redirectUrl = Url.Action("ThirdPartyLoginCallback", "ThirdPartyCallBack", new {returnUrl });
                redirectUrl = redirectUrl.Remove(0, 9);
                var prop = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
                return new ChallengeResult("Google", prop);
            }

            // If we got this far, something failed, redisplay form

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                Input.Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
            return Page();
        }
    }
}
