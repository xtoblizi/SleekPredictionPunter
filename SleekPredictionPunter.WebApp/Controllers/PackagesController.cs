using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SleekPredictionPunter.AppService.Packages;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Model.Packages;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class PackagesController : BaseController
    {
		private readonly IPackageAppService _packageAppService;
		public PackagesController(IPackageAppService packageAppService)
		{
			_packageAppService = packageAppService;
		}
        // GET: Packages
        public async Task<IActionResult> Index(int startIndex= 0, int count = int.MaxValue)
        {
            ViewBag.Package = "active";
            var result = await _packageAppService.GetPackages();

            return View(result);
        }
        
        public async Task<IActionResult> FrontEndIndex(int startIndex= 0, int count = int.MaxValue)
        {
            ViewBag.Package = "active";
            var result = await _packageAppService.GetPackages();
			var resultDto = new List<PackageDto>();

			base.AddLinkScriptforPackageSetter(true);

			resultDto.AddRange(result.OrderByDescending(x => x.DateCreated).Select(x => new PackageDto
			{
				DateCreated = x.DateCreated,
				DateUpdated = x.DateUpdated,
				Description = x.Description,
				EntityStatus = x.EntityStatus,
				Id = x.Id,
				PackageName = x.PackageName,
				Price = x.Price,
			}));

            return View(resultDto);
        }

        // GET: Packages/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.Package = "active";
            var result = await _packageAppService.GetPackageById(id);
            return View(result);
        }

        // GET: Packages/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Package = "active";
            return View();
        }

        // POST: Packages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Create(PackageDto package)
        {
            ViewBag.Package = "active";
            package.DateCreated = DateTime.UtcNow;
            package.DateUpdated = DateTime.UtcNow;
            package.EntityStatus = EntityStatusEnum.Active;
           
            try
            {
                // TODO: Add insert logic here

                await _packageAppService.Insert(package);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Packages/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Package = "active";
            return View();
        }

        // POST: Packages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PackageDto model)
        {
            ViewBag.Package = "active";
            try
            {
                // TODO: Add update logic here
                await _packageAppService.Update(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Packages/Delete/5
        public ActionResult Delete(int id)
        {
            ViewBag.Package = "active";
            return View();
        }

        // POST: Packages/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, PackageDto model)
        {
            try
            {
                // TODO: Add delete logic here
                await _packageAppService.Delete(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}