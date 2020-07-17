using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SleekPredictionPunter.Model.Enums
{
	public enum UserStatusEnum
	{
		[Description("Activated")]
		Activated = 1,
		[Description("Not Activated")]
		NotActivated = 2
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
