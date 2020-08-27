using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SleekPredictionPunter.Model.Matches
{
	public enum MatchStatusEnum
	{
		[Description("Upcoming")]
		Upcoming = 1,
		[Description("Playing")]
		Playing = 2,
		[Description("Played")]
		Played = 3,
		[Description("Postponed")]
		Postponed = 4,
		[Description("Cancelled")]
		Cancelled = 5
	}

}
