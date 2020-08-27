using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SleekPredictionPunter.Model.Categoriess
{
	public class BetCategory : BaseEntity
	{
		[Required]
		public string BetCategoryName { get; set; }
		[Required]
		public string Description { get; set; }
		public string GetNameAndDescription { get { return $"{BetCategoryName} [{Description}]"; } }

		/// <summary>
		/// one to many relationship to the predictions entity.
		/// I.E one category entoity can have multiple prediction records
		/// </summary>
		public virtual  ICollection<Prediction> Predictions { get; set; }
	}
	public class CreateBetCategoryDto : BaseEntity
	{
		public BetCategory BetCategory1 { get; set; }
		public BetCategory BetCategory2 { get; set; }
		public BetCategory BetCategory3 { get; set; }
	}

}
