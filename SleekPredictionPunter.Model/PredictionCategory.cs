using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class PredictionCategory : BaseEntity
	{
		public string CategoryName { get; set; }

		public string CreatorUserName { get; set; }
	}
}
