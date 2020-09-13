using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.PredictionBookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.AppService.PredictionsBookings
{
	public class PredictionBookingDto : PredictionBooking
	{
		public virtual List<Prediction> PredictionsList { get; set; }

		public virtual List<PlatformBookingCode> PlatformBookingCodesList { get; set; }

	}
}
