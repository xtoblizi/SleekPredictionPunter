using System.ComponentModel;

namespace SleekPredictionPunter.Model.Enums
{
	public enum EntityStatusEnum
	{
		[Description("Activated")]
		Active = 1,
		[Description("NotActivated")]
		NotActive = 2,
		[Description("Disabled")]
		Disabled = 3,
		[Description("Deleted")]
		Deleted = 4
	}

	public enum RoleEnum
	{
		[Description("SuperAdmin")]
		SuperAdmin = 1,
		[Description("Subscriber")]
		Subscriber = 2,
		[Description("Predictor")]
		Predictor = 3,
		[Description("Agent")]
		Agent = 4

	}

	public enum CategoriesType : int
	{
		[Description("BetCategory")]
		BetCategory = 1,
		[Description("SportCategory")]
		SportCategory = 2,
		[Description("MatchCategory")]
		MatchCategory = 3,
		[Description("OddCategory")]
		OddCategory = 4
	}
}
