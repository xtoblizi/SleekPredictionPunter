using SleekPredictionPunter.Model.BaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class Agent  : Person
	{
		public string RefererCode { get; set; }

		public virtual Wallet Wallet { get; set; }
	}
}
