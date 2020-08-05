using SleekPredictionPunter.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.BaseModels
{
	public abstract class Person : BaseEntity
	{
		public Person()
		{
			ActivatedStatus = EntityStatusEnum.NotActive;
			IsTenant = false;

			TenantUniqueName = IsTenant ? Username : string.Empty;
			Gender = GenderEnum.Male;
		}

		/// <summary>
		/// should always be unique
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// If the identity is a tenant based identity then the username become the tenant
		/// </summary>
		public string TenantUniqueName { get; private set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }

		public string Email { get; set; }

		public string PhoneNumber { get; set; }

		public GenderEnum Gender { get; set; }

		public string BrandNameOrNickName { get; set; }
		public string FullName { get { return $"{FirstName} {LastName}"; } }
		public EntityStatusEnum ActivatedStatus { get; set; } 
		public virtual bool IsTenant { get; set; }

		#region Address Details 
		public string Country { get; set; }
		public string State { get; set; }
		public string City { get; set; }
		public string Street { get; set; }

		public string FullAddress { get { return $"{Street}, {City}, {State}, {Country}."; } }
		#endregion

	}
}
