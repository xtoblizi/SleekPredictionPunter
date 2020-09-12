using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SleekPredictionPunter.AppService.Clubs;
using SleekPredictionPunter.DataInfrastructure;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Enums;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class ClubsController : BaseController
    {
        private readonly IClubService _context;

        public ClubsController(IClubService context)
        {
            _context = context;
        }

        // GET: Clubs
        public async Task<IActionResult> Index()
        {
            ViewBag.Clubs = "active";
            var result = await _context.GetAllQueryable();
            return View(result);
        }

        // GET: Clubs/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            ViewBag.Clubs = "active";
            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.GetById(id.Value);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // GET: Clubs/Create
        public IActionResult Create()
        {
            ViewBag.Clubs = "active";
            return View();
        }

        // POST: Clubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ClubDto model)
        {
            ViewBag.Clubs = "active";
            System.Random random = new System.Random();
            int genNumber = random.Next(1234567890);

            if (model.ClubLogo == null || model.ClubLogo.Length == 0)
                return Content("file not selected");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/TeamLogo", Path.GetFileName(genNumber + model.ClubLogo.FileName));
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.ClubLogo.CopyToAsync(stream);
                await stream.FlushAsync();
            }

            Club club = new Club()
            {
                ClubLogRelativePath=path,
                ClubName=model.ClubName,
                DateCreated=DateTime.UtcNow,
                DateUpdated= DateTime.UtcNow,
                Description=model.Description,
                EntityStatus=EntityStatusEnum.Active
            };

            if (ModelState.IsValid)
            {
                await _context.Insert(club);
                return RedirectToAction(nameof(Index));
            }
            return View(club);
        }

        // GET: Clubs/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            ViewBag.Clubs = "active";
            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.GetById(id.Value);
            if (club == null)
            {
                return NotFound();
            }
            return View(club);
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ClubName,Description,ClubLogRelativePath,Id,DateCreated,EntityStatus,DateUpdated")] Club club)
        {
            if (id != club.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Update(club);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubExists(club.Id))
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
            return View(club);
        }

        // GET: Clubs/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            ViewBag.Clubs = "active";
            if (id == null)
            {
                return NotFound();
            }

            var club = await _context.GetById(id.Value);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var club = await _context.GetById(id);
            await _context.Delete(club);
            return RedirectToAction(nameof(Index));
        }

        private  bool ClubExists(long id)
        {
            var returnValue =  _context.GetAllQueryable();
            return returnValue.Result.Any(e => e.Id == id);
        }
    }
}
