using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SleekPredictionPunter.AppService.AdvertPlacements;
using SleekPredictionPunter.AppService.AdvertPlacements.Dto;
using SleekPredictionPunter.DataInfrastructure;
using SleekPredictionPunter.GeneralUtilsAndServices;
using SleekPredictionPunter.Model;

namespace SleekPredictionPunter.WebApp.Controllers
{
    public class AdvertPlacementsController : Controller
    {
        private readonly IAdvertPlacementService _context;
        private readonly IFileHelper _fileHelper;

        public AdvertPlacementsController(IAdvertPlacementService context,IFileHelper fileHelper)
        {
            _context = context;
            _fileHelper = fileHelper;
        }

        // GET: AdvertPlacements
        public async Task<IActionResult> Index(int startIndex = 0, int count = 100)
        {
            ViewBag.AdvertPlacements = "active";
            var result = await _context.GetAdvertPlacements(null, (x => x.DateCreated), startIndex, count);
            return View(result);
        }

        // GET: AdvertPlacements/Details/5
        public async Task<IActionResult> Details(long id)
        {
            ViewBag.AdvertPlacements = "active";
            var advertPlacement = await _context.GetById(id);
            if (advertPlacement == null)
            {
                return NotFound();
            }

            return View(advertPlacement);
        }

        // GET: AdvertPlacements/Create
        public IActionResult Create()
        {
            ViewBag.AdvertPlacements = "active";
            var rolesEnumList = EnumHelper.GetEnumResults<AdvertSection>();
            ViewBag.AdSectionId = new SelectList(rolesEnumList, "Id", "Name");
            return View();
        }

        // POST: AdvertPlacements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]AdverPlacementDto advertPlacement)
        {
            if (ModelState.IsValid)
            {
                var wwwwrootDirectory = "Advert";
                var createFile = await _fileHelper.SaveFileToCustomWebRootPath(wwwwrootDirectory, advertPlacement.AdImageFile);

               
                if (createFile.FileStatueEnum == FileResultEnum.FileCreationSuccess)
                {
                    Func<AdvertPlacement, bool> predicate = (x => x.AdvertSection == (AdvertSection)advertPlacement.AdvertSection);
                    var getplacebySection = await _context.GetDefault(predicate);
                    if (getplacebySection != null)
                        await _context.Delete(getplacebySection);

                    var model = new AdvertPlacement()
                    {
                        AdTitle = advertPlacement.AdverTitle,
                        AdCaption = advertPlacement.AdvertCaption,
                        AdLink = advertPlacement.AdLink,
                        AdImageRelativePath = createFile.CreatedFileRelativePath,
                        AdDescription = advertPlacement.AdvertDescription,
                        AdvertSection = (AdvertSection)advertPlacement.AdvertSection,
                        ButtonText = advertPlacement.ButtonText
                    };

                    await _context.Create(model);
                }
                
                return RedirectToAction(nameof(Index));
            }

            var rolesEnumList = EnumHelper.GetEnumResults<AdvertSection>();
            ViewBag.AdSectionId = new SelectList(rolesEnumList, "Id", "Name");

            ViewBag.AdvertPlacements = "active";
            return View(advertPlacement);
        }

        // GET: AdvertPlacements/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var advertPlacement = await _context.GetById(id);
            if (advertPlacement == null)
            {
                return NotFound();
            }
            var advertDto = new AdverPlacementDto
            {
                FullPathForRead = await FileReaderHelper.ReadAsync(advertPlacement.AdImageRelativePath),
                AdLink = advertPlacement.AdLink,
                AdvertCaption = advertPlacement.AdCaption,
                AdverTitle = advertPlacement.AdTitle

            };

            ViewBag.AdvertPlacements = "active";
            return View(advertDto);
        }

        // POST: AdvertPlacements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdverPlacementDto advertPlacement)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var wwwwrootDirectory = "Advert";
                    var createFile = await _fileHelper.SaveFileToCustomWebRootPath(wwwwrootDirectory, advertPlacement.AdImageFile);

                    if (createFile.FileStatueEnum == FileResultEnum.FileCreationSuccess)
                    {
                        var model = await _context.GetById(advertPlacement.Id);

                        model.AdTitle = advertPlacement.AdverTitle;
                        model.AdCaption = advertPlacement.AdvertCaption;
                        model.AdLink = advertPlacement.AdLink;
                        model.AdDescription = advertPlacement.AdvertDescription;
                        model.AdImageRelativePath = createFile.CreatedFileRelativePath;
                        model.ButtonText = advertPlacement.ButtonText;

                        await _context.Update(model);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AdvertPlacements = "active";
            return View(advertPlacement);
        }

        // GET: AdvertPlacements/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var advertPlacement = await _context.GetById(id);
            if (advertPlacement == null)
            {
                return NotFound();
            }
            ViewBag.AdvertPlacements = "active";
            return View(advertPlacement);
        }

        // POST: AdvertPlacements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var model = await _context.GetById(id);

            await _context.Delete(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
