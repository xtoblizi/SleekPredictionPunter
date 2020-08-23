using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class GeneralSystemConfig
	{
		/// <summary>
		/// This is the id of the package to display results from
		/// </summary>
		public long PackagePlanIdToDisplayResultFor { get; set; }
		public long PackagePlanNameToDisplayResultFor { get; set; }
	}
}
