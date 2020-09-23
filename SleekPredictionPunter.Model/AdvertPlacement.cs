using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class AdvertPlacement : BaseEntity
	{
		public string AdTitle { get; set; }

		public string AdCaption { get; set; }

		public AdvertSection AdvertSection { get; set; }

		public string AdImageRelativePath { get; set; }

		public string AdDescription { get; set; }
		public string AdLink { get; set; }
		public string ButtonText { get; set; }

	}

	public enum AdvertSection : int
	{
		[Description("Above Footer Left Section")]
		FooterLeft = 1,

		[Description("Above Footer Right Section")]
		FooterRight = 2
	}
}
