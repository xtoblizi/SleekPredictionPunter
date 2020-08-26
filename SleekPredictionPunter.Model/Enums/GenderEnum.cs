using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.Enums
{
	public enum GenderEnum : int
	{
		Male = 1 ,

		Female = 2,

		None = 3
	}

	public enum PredictionResultEnum : int
	{
		PredictionWon,
		PredictionLost,
		MatchPostponed,
		MatchPending,
		MatchCancelled
	}
}
