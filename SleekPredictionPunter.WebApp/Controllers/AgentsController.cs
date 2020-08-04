using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SleekPredictionPunter.AppService.Agents;
using SleekPredictionPunter.DataInfrastructure;
using SleekPredictionPunter.Model;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class AgentsController : Controller
    {
        private readonly PredictionDbContext _context;
        private readonly IAgentService _agentService;

        public AgentsController(PredictionDbContext context, IAgentService agentService)
        {
            _context = context;
            _agentService = agentService;
        }

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
        public async Task<IActionResult> Create(Agent agent)
        {
            if (ModelState.IsValid)
            {
                await _agentService.CreateAgent(agent);
                return RedirectToAction(nameof(Index));
            }
            return View(agent);
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
    }
}
