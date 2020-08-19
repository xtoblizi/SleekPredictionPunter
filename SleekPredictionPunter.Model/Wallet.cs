using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class Wallet: BaseEntity
	{
		public string UserName { get; set; }

		public decimal Balance { get; set; }

	}
}
