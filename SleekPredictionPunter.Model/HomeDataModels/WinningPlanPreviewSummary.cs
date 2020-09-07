using SleekPredictionPunter.Model.Enums;
using SleekPredictionPunter.Model.PricingPlan;
using System;
using System.Collections.Generic;

namespace SleekPredictionPunter.Model.HomeDataModels
{
	public class WinningPlanPreviewSummary : BaseEntity
	{
		public string PlanName { get; set; }
		public string Paragraph1Description { get; set; }
		public string Paragraph2Description { get; set; }
		public string Paragraph3Description { get; set; }
		public int RatingValue { get; set; }
		public string AdvertActionMessage { get; set; }
		public  virtual int PricingPlanId { get; set; }
		public bool SetforHomePreview { get; set; }
	}

	public class ResultPlanViewModel
	{
		public WinningPlanPreviewSummary WinningPlanPreview { get; set; }

		public List<LastestPredictionDateValues> LatestPredictionDates { get; set; }

		public PricingPlanModel PricingPlan { get; set; }

		public Prediction TodayPrediction { get; set; }
	}

	public class LastestPredictionDateValues
	{
		public PredictionResultEnum predictionResultEnum  { get; set; }

		public DateTime DateOfPrediction { get; set; }
	}

	public class RatingValue
	{
		public int Id { get; set; }
		public string RateName { get; set; }
	}
}
