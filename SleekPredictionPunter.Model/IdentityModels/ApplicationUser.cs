using Microsoft.AspNetCore.Identity;
using SleekPredictionPunter.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SleekPredictionPunter.Model.IdentityModels
{
	public class ApplicationUser: IdentityUser 
	{
		public ApplicationUser()
		{
			DateCreated = DateTime.UtcNow;
			Status = EntityStatusEnum.Active;
		}
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName { get { return $"{FirstName} {LastName}"; } }
		public EntityStatusEnum Status { get; set; }
		public bool IsTenant { get; set; } = false;
		public string Country { get; set; }
		public string State { get; set; }
		public string City { get; set; }
		public DateTime DateCreated { get; set; }
		//public DateTime DateofBirth { get; set; }

	}
}
