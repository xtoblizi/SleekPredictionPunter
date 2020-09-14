using SleekPredictionPunter.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.PredictionBookings
{
	public class PredictionBooking : BaseEntity
	{
		/// <summary>
		/// This is stringified list of bookingcode for the varied betplatforms
		/// </summary>
		public string BookingCodes { get; set; }
		/// <summary>
		/// This is a list of corresponding booking platformids for the booking names
		/// </summary>
		public string BookingPlatformIds { get; set; }

		/// <summary>
		/// This is a stringified list<PlatformBookingCode></PlatformBookingCode> serialized into a jobject and then stringified fied. 
		/// It should be deserialized on data retrieval and or usage
		/// </summary>
		public string BookingCodeWithRelationToPlatform { get; set; }

		/// <summary>
		/// This is an Icollection of predictions for the booking
		/// This collection would be stringified on insertion and deserialized on retrieval
		/// </summary>
		public string Predictions { get; set; }
		public decimal Odd { get; set; }
		public string BonusCode { get; set; }
		public string PredictedBy { get; set; }
		public DateTime LeastMatchstattime { get; set; }

		// nav properties
		public virtual long PricingPlanId { get; set; }
		public string PricingPlan { get; set; }
		public bool DisplayonHome { get; set; }
		public PredictionResultEnum PredictionResult { get; set; }


	}
}
