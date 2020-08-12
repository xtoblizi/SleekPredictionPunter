﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SleekPredictionPunter.AppService.Agents;
using SleekPredictionPunter.DataInfrastructure;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Model.IdentityModels;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class AgentsController : Controller
    {
        private readonly PredictionDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAgentService _agentService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AgentsController(PredictionDbContext context, IAgentService agentService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _agentService = agentService;
            _userManager = userManager;
            _signInManager = signInManager;
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

        private async Task<bool> AgentExists(long id)
        {
            var checkIfExist = await _agentService.GetAgents();
            return checkIfExist.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ThirdPartySignUpCallback(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

           // ReturnUrl = returnUrl;
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
                var password = "1234567890";
                var result = await _userManager.CreateAsync(externalUserModelBuilder,password);

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
                    return RedirectToPage("index");
                }
            }
            return View();
        }
    }
}
