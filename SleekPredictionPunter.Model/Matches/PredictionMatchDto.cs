using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.Matches
{
	/// <summary>
	/// Use this to map predications detailes relating to the match created
	/// </summary>
	public class PredictionMatchDto
	{
		public long PredicationMatchMapId { get; set; }

		public DateTime DateOfMap { get; set; }

		public DateTime DateTimeUpdated { get; set; }

		public long	 MatchId { get; set; }
		public IEnumerable<Prediction> Predictions { get; set; }
	}
}
