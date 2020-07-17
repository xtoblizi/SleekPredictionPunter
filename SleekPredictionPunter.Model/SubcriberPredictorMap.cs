using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class SubcriberPredictorMap : BaseEntity
	{
		/// <summary>
		///  this is the username of the subscriber user
		/// </summary>
		public string SubscriberUsername { get; set; }

		/// <summary>
		/// this is the username of the predictor user
		/// </summary>
		public string PredictorUsername { get; set; }
	}

	public class AgentRefereeMap
	{
		public string AgentUsername { get; set; }

		public string RefereeUsername { get; set; }
	}
}
