using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.AppService.PredictionsBookings
{
	public  class CreatePredictionBookingDto
	{
		public Prediction Prediction { get; set; }

		public static List<PlatformBookingCode> PlatformBookingCodes { get; set; }

		public static string OddValue { get; set; }

	}
}
