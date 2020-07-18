using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class Prediction : BaseEntity
	{
		public string ClubA { get; set; }

		public dynamic ClubALogoPath { get; set; }

		public string ClubB { get; set; }

		public string ClubBLogoPath { get; set; }

		public string PredictionValue { get; set; }

		public DateTime TimeofFixture { get; set; }
	}
}
