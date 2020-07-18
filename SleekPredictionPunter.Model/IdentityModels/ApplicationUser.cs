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
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName { get { return $"{FirstName} {LastName}"; } }
		public UserStatusEnum Status { get; set; } = UserStatusEnum.NotActivated;
		public bool IsTenant { get; set; } = false;

		public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	}
}
