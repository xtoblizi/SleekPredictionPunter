using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class Contact: BaseEntity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName { get { return $"{FirstName} {LastName}"; } set { } }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }

		public string Subject { get; set; }
		public string ContactReason { get; set; }
	}
}
