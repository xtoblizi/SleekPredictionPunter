using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class Package : BaseEntity
	{
		public long PackageId { get; set; }
		public string PackageName { get; set; }
		public decimal Price { get; set; }
		public string Description { get; set; }
	}
}
