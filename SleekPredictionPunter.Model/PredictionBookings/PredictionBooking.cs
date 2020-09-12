using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.PredictionBookings
{
	public class PredictionBooking : BaseEntity
	{
		public string BookingCode { get; set; }
		public decimal Odd { get; set; }
		public string BonusCode { get; set; }
		public string PredictedBy { get; set; }
		public DateTime LeastMatchstattime { get; set; }

		// nav properties
		public virtual long BettingPlatformId { get; set; }
		public string BettingPlatform { get; set; }

		public virtual long PricingPlanId { get; set; }
		public string PricingPlan { get; set; }
	}
}
