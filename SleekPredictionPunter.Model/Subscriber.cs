using SleekPredictionPunter.Model.BaseModels;
using SleekPredictionPunter.Model.Packages;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class Subscriber : Person
	{
		public Subscriber()
		{
			IsTenant = false;
		}

		public string RefererCode { get; set; }

		// Navigation properties
		public virtual ICollection<Package> Packages { get; set; }

		public virtual Wallet Wallet { get; set; }
	}
}
