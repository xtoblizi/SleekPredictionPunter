using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class OddCategory: BaseEntity
	{
		public string CategoryName { get; set; }
		public string OddValue { get; set; }
	}
}
