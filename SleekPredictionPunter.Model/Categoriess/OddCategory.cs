using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class OddCategory: BaseEntity
	{
		[Required]
		public string CategoryName { get; set; }
		[Required]
		public string OddValue { get; set; }
	}
}
