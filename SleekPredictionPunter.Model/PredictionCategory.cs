﻿using SleekPredictionPunter.Model.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class PredictionCategory : BaseEntity
	{
		public string CategoryName { get; set; }
		public string CategoryDescription { get; set; }

		public string CreatorUserName { get; set; }

		public ApplicationUser ApplicationUser { get; set; }

	}
}
