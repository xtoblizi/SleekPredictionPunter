﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SleekPredictionPunter.AppService.Packages;
using SleekPredictionPunter.AppService.PredictionAppService;
using SleekPredictionPunter.AppService.PredictionAppService.Dtos;
using SleekPredictionPunter.DataInfrastructure;
using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp.Controllers
{
    //[Authorize]
    public class PredictionsController : Controller
    { 
        private readonly IPredictionService _predictionService;
        private readonly IPackageAppService _packageService;
        public PredictionsController(IPredictionService predictionService, IPackageAppService packageService)
        { 
            _predictionService = predictionService;
            _packageService = packageService;
        }

        // GET: Predictions
        public async Task<IActionResult> Index()
        {
            ViewBag.Predictions = "active";
            return View(await _predictionService.GetPredictions());
        }

        // GET: Predictions/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            ViewBag.Predictions = "active";
            if (id == null)
            {
                return NotFound();
            }

            var prediction = await _predictionService.GetPredictionByPredictor(id.Value);
            if (prediction == null)
            {
                return NotFound();
            }

            return View(prediction);
        }

        // GET: Predictions/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Predictions = "active";
            ViewBag.PackageId = new SelectList(await _packageService.GetPackages(), "Id", "PackageName");
            return View();
        }

        // POST: Predictions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]PredictionDto prediction)
        {
            ViewBag.Predictions = "active";
            System.Random random = new System.Random();
            int genNumberA = random.Next(1234567890);
            int genNumberB = random.Next(0987654321);

            if (prediction.FileA == null || prediction.FileA.Length == 0)
                return Content("file not selected");
            var pathA = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ClubLogo", Path.GetFileName(genNumberA + prediction.FileA.FileName));
            using(var stream = new FileStream(pathA, FileMode.Create))
			{ 
				await prediction.FileA.CopyToAsync(stream); 
				await stream.FlushAsync();
			}

            if (prediction.FileB == null || prediction.FileB.Length == 0)
                return Content("file not selected");
            var pathB = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ClubLogo", Path.GetFileName(genNumberB + prediction.FileB.FileName));
            using (var stream = new FileStream(pathB, FileMode.Create)) { 
				await prediction.FileB.CopyToAsync(stream); 
				await stream.FlushAsync(); 
			}

            var getpackage = await _packageService.GetPackageById(prediction.PackgeId);
            Prediction predictionModel = new Prediction()
            {
                ClubA = prediction.ClubA,
                ClubALogoPath = pathA,
                ClubB = prediction.ClubB,
                ClubBLogoPath = pathB,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                PredictionValue = prediction.PredictionValue,
                Predictor = prediction.Predictor,
                PredictorUserName = User.Identity.Name,
                Subscriber = prediction.Subscriber,
                TimeofFixture = prediction.TimeofFixture,
                Package=getpackage
            };

            if (ModelState.IsValid)
            {
                await _predictionService.InsertPrediction(predictionModel);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.PackageId = new SelectList(await _packageService.GetPackages(), "Id", "PackageName", prediction.PackgeId);
            return View(prediction);
        }

    
        // GET: Predictions/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            ViewBag.Predictions = "active";
            if (id == null)
            {
                return NotFound();
            }

            var prediction = await _predictionService.GetPredictionByPredictor(id.Value);
            if (prediction == null)
            {
                return NotFound();
            }

            return View(prediction);
        }

        // POST: Predictions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var prediction = await _predictionService.GetPredictionByPredictor(id);
            await _predictionService.RemovePredictionBy(prediction, true);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PredictionExists(long id)
        {
            var checkIfExist = await  _predictionService.GetPredictions();
            return checkIfExist.Any(e => e.Id == id);
        }
    }
}
