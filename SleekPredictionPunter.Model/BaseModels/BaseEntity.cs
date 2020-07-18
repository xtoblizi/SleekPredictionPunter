using SleekPredictionPunter.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SleekPredictionPunter.Model
{
	public abstract class BaseEntity
	{
		public BaseEntity()
		{
			DateCreated = DateTime.UtcNow;
			EntityStatus = EntityStatusEnum.Active;
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public long Id { get; set; }

		public DateTime DateCreated { get; set; }

		public EntityStatusEnum EntityStatus { get; set; }

		public DateTime? DateUpdated { get; set; }
	}
}
