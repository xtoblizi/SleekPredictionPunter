using SleekPredictionPunter.Model.Packages;
using SleekPredictionPunter.Model.PricingPlan;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class Subcription : BaseEntity
	{
		public virtual long PricingPlanId  { get; set; }

		public virtual long SubscriberId { get; set; }

		/// <summary>
		/// This is as well the email of the subscriber as 
		/// user being that we set email as the username on registration
		/// </summary>
        public string SubscriberUsername { get; set; }

        /// <summary>
        /// This should not be null at anypoint of creating a new subcription 
        /// </summary>
        public DateTime ExpirationDateTime { get; set; }
		public int NumberOfMonths { get; set; }
	}
}
