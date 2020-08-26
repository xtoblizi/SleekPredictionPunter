using SleekPredictionPunter.Model.Matches;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.PredicationMatchMaps
{
	public class PredictionMatchMap: BaseEntity
	{
		public long MatchId { get; set; }

		public DateTime TimeOfMatch { get; set; }
		public long PredictionId { get; set; }

		public MatchStatusEnum	MatchStatus { get; set; }
	}
}
