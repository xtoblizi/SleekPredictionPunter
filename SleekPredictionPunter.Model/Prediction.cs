using SleekPredictionPunter.Model.Packages;
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

		public DateTime TimeofFixture { get; set; }

		/// <summary>
		/// These below are foreign key entities that can have multiple collections of predictions.
		/// This design creates a one to many relationship between the below entity and the named entity: Prediction.
		/// </summary>
		public virtual Predictor Predictor { get; set; }

		public virtual long PackageId { get; set; }
		public virtual Package Package { get; set; }

	}

	
}
