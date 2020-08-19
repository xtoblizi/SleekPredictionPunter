using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleekPredictionPunter.WebApp.Controllers
{
	public class BaseController : Controller
	{
		// place generalized code here.
		public BaseController()
		{
			ShowBreadCumBanner = false;
			ViewBag.ShowBreadCum = ShowBreadCumBanner;
			ViewBag.AddLinkScriptforPackage = false;
		}

		public bool ShowBreadCumBanner { get; set; }
		public string SwitchReturnUrl { get; set; }

		public string LoggedInUserName { get; set; }
		public string LoggedInUserRole { get; set; }

		public string LoggedInUserFullName { get; set; }

		public  void ShowBreadCumBannerSetter(bool showBreadCumBanner)
		{
			ViewBag.ShowBreadCum = showBreadCumBanner;
			ShowBreadCumBanner = showBreadCumBanner;
		}
		public  void AddLinkScriptforPackageSetter(bool addscript)
		{
			ViewBag.AddLinkScriptforPackage = addscript; ;
			
		}

	}
}