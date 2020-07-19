using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SleekPredictionPunter.AppService.Dtos
{
    public class BaseEntityDto
    {
		public BaseEntityDto()
		{
			DateCreated = DateTime.UtcNow;
			EntityStatus = EntityStatusEnumDto.Active;
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public long Id { get; set; }

		public DateTime DateCreated { get; set; }

		public EntityStatusEnumDto EntityStatus { get; set; }

		public DateTime? DateUpdated { get; set; }
	}
}

