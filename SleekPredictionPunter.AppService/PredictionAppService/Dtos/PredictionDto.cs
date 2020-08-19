using Microsoft.AspNetCore.Http;
using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.AppService.PredictionAppService.Dtos
{
    public class PredictionDto
    {
		public string PredictorUserName { get; set; }
		public string ClubA { get; set; }


		public string ClubAOdd { get; set; }

		public string ClubB { get; set; }

		public string ClubBOdd { get; set; }

		public string PredictionValue { get; set; }

		public IFormFile FileA { get; set; }
		public IFormFile FileB { get; set; }
		public DateTime TimeofFixture { get; set; }
		//public string PredictionType { get; set; }
		/// <summary>
		/// These below are foreign key entities that can have multiple collections of predictions.
		/// This design creates a one to many relationship between the below entity and the named entity: Prediction.
		/// </summary>
		public Subscriber Subscriber { get; set; }
		public Predictor Predictor { get; set; }

		//public ICollection<Package> Packages { get; set; }
		public long PackgeId { get; set; }
	}
}
