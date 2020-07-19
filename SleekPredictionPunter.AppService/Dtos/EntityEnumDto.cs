using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SleekPredictionPunter.AppService.Dtos
{
	public enum EntityStatusEnumDto
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

	public enum RoleEnumDto
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
