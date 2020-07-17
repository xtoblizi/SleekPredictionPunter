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
}
