using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.AppService.Dtos
{
    public class PersonDto:BaseEntityDto
    {
		public PersonDto()
		{
			ActivatedStatus = EntityStatusEnumDto.NotActive;
			IsTenant = false;

			TenantUniqueName = IsTenant ? Username : string.Empty;
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

		public GenderEnumDto Gender { get; set; }

		public string BrandNameOrNickName { get; set; }
		public string FullName { get { return $"{FirstName} {LastName}"; } }
		public EntityStatusEnumDto ActivatedStatus { get; set; }
		public virtual bool IsTenant { get; set; }

	}
}

