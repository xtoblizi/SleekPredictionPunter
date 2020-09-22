using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class AdvertPlacement : BaseEntity
	{
		public string AdTitle { get; set; }

		public string AdCaption { get; set; }

		public string AdImageRelativePath { get; set; }

		public string AdDescription { get; set; }
		public string AdLink { get; set; }

	}
}
