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
		public EntityStatusEnum Status { get; set; } = EntityStatusEnum.Active;
		public bool IsTenant { get; set; } = false;

		public DateTime DateCreated { get; set; } = DateTime.UtcNow;
	}
}
