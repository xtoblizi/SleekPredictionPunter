using SleekPredictionPunter.Model.Packages;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class Subcription : BaseEntity
	{
		public virtual ICollection<Package> Packages { get; set; }

		public virtual Subscriber Subscriber { get; set; }

		/// <summary>
		/// This should not be null at anypoint of creating a new subcription 
		/// </summary>
		public DateTime ExpirationDateTime { get; set; }
	}
}
