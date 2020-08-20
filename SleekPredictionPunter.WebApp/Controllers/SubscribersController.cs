using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SleekPredictionPunter.AppService;
using SleekPredictionPunter.DataInfrastructure;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class SubscribersController : BaseController
    {
        private readonly PredictionDbContext _context;
        private readonly ISubscriberService _subscriberService;

        public SubscribersController(PredictionDbContext context, ISubscriberService subscriberService)
        {
            _context = context;
            _subscriberService = subscriberService;
        }

        // GET: Subscribers
        public async Task<IActionResult> Index()
        {
            return View(await _subscriberService.GetAllQueryable());
        }

        // GET: Subscribers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriber = await _subscriberService.GetById(id.Value);
            if (subscriber == null)
            {
                return NotFound();
            }

            return View(subscriber);
        }

        // GET: Subscribers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subscribers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Subscriber subscriber)
        {
            subscriber.ActivatedStatus = EntityStatusEnum.Active;
            subscriber.DateCreated = DateTime.UtcNow;
            subscriber.DateUpdated = DateTime.UtcNow;
            subscriber.IsTenant = true;
            subscriber.Username = User.Identity.Name;

            if (ModelState.IsValid)
            {
                await _subscriberService.Insert(subscriber);
                return RedirectToAction("Index","Admin");
            }
            return View(subscriber);
        }

        // GET: Subscribers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriber = await _subscriberService.GetById(id.Value);
            if (subscriber == null)
            {
                return NotFound();
            }
            return View(subscriber);
        }

        // POST: Subscribers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Subscriber subscriber)
        {
            if (id != subscriber.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _subscriberService.Update(subscriber, true);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SubscriberExists(subscriber.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(subscriber);
        }

        // GET: Subscribers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriber = await _subscriberService.GetById(id.Value); 
            if (subscriber == null)
            {
                return NotFound();
            }

            return View(subscriber);
        }

        // POST: Subscribers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var subscriber = await _subscriberService.GetById(id);
            await _subscriberService.Delete(subscriber, true);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SubscriberExists(long id)
        {
            var checkIfExist = await _subscriberService.GetAllQueryable();
            return checkIfExist.Any(e => e.Id == id);
        }
    }
}
