using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Model.IdentityModels;
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
		public  bool IsAdmin()
		{
			if (User.Identity.IsAuthenticated)
			{
				if (User.IsInRole(RoleEnum.SuperAdmin.ToString()))
					return true;
				else
					return false;
			}

			return false;
			
		}

		public async Task<string> GetUserName()
		{
			string username = string.Empty;
			if (User.Identity.IsAuthenticated)
			{
				 username = User.Identity.Name;
			}
			return await Task.FromResult(username);
			
		}

	}
}