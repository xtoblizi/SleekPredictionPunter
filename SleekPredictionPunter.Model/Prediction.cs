using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Model.Matches;
using SleekPredictionPunter.Model.PricingPlan;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SleekPredictionPunter.Model
{
	public class Prediction : BaseEntity
	{
		/// <summary>
		/// The owner of the prediction
		/// </summary>
		public string PredictorUserName { get; set; }
		public string ClubA { get; set; }
		public string ClubAOdd { get; set; }

		
		public string ClubALogoPath { get; set; }

		public string ClubB { get; set; }
		public string ClubBOdd { get; set; }

		public string ClubBLogoPath { get; set; }

		public string PredictionValue { get; set; }
		/// <summary>
		/// Set the time from the match as the Fixture Time of the prediction
		/// </summary>
		public DateTime TimeofFixture { get; set; }

		#region Result Based Properties
		public string ClubAScore { get; set; }
		public string ClubBScore { get; set; }
		public PredictionResultEnum PredictionResult { get; set; }
		public string ResultValueOdd { get; set; }
		#endregion

		#region Match Based Details
		/// <summary>
		/// These below are foreign key entities that can have one to one relationship
		/// This design creates a one to one relationship between the below entity and the named entity: Prediction.
		/// </summary>
		public virtual long MatchId { get; set; } // this relates to an upcoming match that 
												  //was created before a prediction was created out of it.
		public virtual string MatchCategory { get; set; }
		public virtual long MatchCategoryId { get; set; }
		/// <summary>
		/// Note the custome categoryid is now the sports category id 
		/// and it is same as the custom(i.e sportcategoryid) of the match table
		/// </summary>
		public virtual long CustomCategoryId { get; set; }
		public virtual string CustomCategory { get; set; }

		#endregion end of match based details

		#region prediction specific navigation detailS(Relaition based properties)
		public virtual long BetCategoryId { get; set; }
		public string BeCategory { get; set; }

		public virtual Predictor Predictor { get; set; }
		public virtual long  PredictorId { get; set; }

		public virtual PredictionCategory PredictionCategory { get; set; }
		public virtual string PredictionCategoryName { get; set; }
		public virtual long PredictionCategoryId { get; set; }
		public virtual long PricingPlanId { get; set; }
		public string  PricingPlanName { get; set; }
		public bool IsCorrectScore { get { return !string.IsNullOrEmpty(ClubAScore) && !string.IsNullOrEmpty(ClubBScore) ? true : false; } }
		#endregion

	}

	public enum MatchResultCategories
	{
		HomeWin = 1,
		AwayWin = 2,
		Draw = 3,
		NotSet = 4
	}

	
}
