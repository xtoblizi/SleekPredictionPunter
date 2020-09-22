using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.AppService.AdvertPlacements.Dto
{
	public class AdverPlacementDto
	{
		public long Id { get; set; }
		public string AdverTitle { get; set; }
		public string FullPathForRead { get; set; }
		public string AdvertCaption { get; set; }
		public string AdvertDescription { get; set; }
		public string AdLink { get; set; }

		public IFormFile AdImageFile { get; set; }

	}
}
