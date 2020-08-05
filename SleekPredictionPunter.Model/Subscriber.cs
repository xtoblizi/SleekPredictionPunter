using SleekPredictionPunter.Model.BaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class Subscriber : Person
	{
		private bool istenant = false;
		public Subscriber()
		{
			IsTenant = false;
		}

		public string RefererCode { get; set; }
	}
}
