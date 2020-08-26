using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.Matches
{
	public enum MatchStatusEnum
	{
		Upcoming = 1,
		Playing = 2,
		Played = 3,
		Postponed = 4,
		Cancelled = 5
	}
	public enum UpcomingOrPastEnum
	{
		UpComing = 1,
		Past = 2
	}
}
