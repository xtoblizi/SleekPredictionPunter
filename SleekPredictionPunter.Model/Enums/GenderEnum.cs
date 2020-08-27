using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SleekPredictionPunter.Model.Enums
{
	public enum GenderEnum : int
	{
		[Description("Male")]
		Male = 1 ,
		[Description("Female")]
		Female = 2,
		[Description("None")]
		None = 3
	}

	public enum PredictionResultEnum : int
	{
		[Description("Prediction Won")]
		PredictionWon,
		[Description("Prediction Lost")]
		PredictionLost,
		[Description("Match Postponed")]
		MatchPostponed,
		[Description("Matcg Pending")]
		MatchPending,
		[Description("Match Cancelled")]
		MatchCancelled
	}
}
