using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.BaseModels
{
	public class AddressDetails : BaseEntity
	{
		#region Address Details

		/// <summary>
		/// This is the username of the user that owns this address
		/// </summary>
		public string UserName { get; set; }

		public string Country { get; set; }
		public string State { get; set; }
		public string City { get; set; }
		public string Street { get; set; }

		public string FullAddress { get { return $"{Street}, {City}, {State}, {Country}."; } }
		#endregion
	}
}
