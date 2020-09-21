using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class BookingCode : BaseEntity
	{
		public string BetCode { get; set; }
		public virtual long BetPlatformId { get; set; }
		public string Betplatform { get; set; }
		public virtual long PricingPlanId { get; set; }
		public string PricingPlan { get; set; }
	}
}
