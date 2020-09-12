using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.BettingPlatform
{
	public class BetPlanform :BaseEntity
	{
		public string BetPlatformName { get; set; }

		public string Caption { get; set; }
		public string LogoPath { get; set; }
	}
}
