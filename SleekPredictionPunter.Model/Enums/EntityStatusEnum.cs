using System.ComponentModel;

namespace SleekPredictionPunter.Model.Enums
{
	public enum EntityStatusEnum
	{
		[Description("Activated")]
		Active = 1,
		[Description("Not Activated")]
		NotActive = 2,
		[Description("Disabled")]
		Disabled = 3,
		[Description("Deleted")]
		Deleted = 4
	}

	public enum RoleEnum
	{
		[Description("SuperAdmin")]
		SystemAdmin = 1,
		[Description("Subscriber")]
		Subscriber = 2,
		[Description("Predictor")]
		Predictor = 3,
		[Description("Agent")]
		Agent = 4

	}
}
