namespace SleekPredictionPunter.Model.PricingPlan
{
	public class PricingPlanModel : BaseEntity
    {
		public PricingPlanModel()
		{
			PlanType = 
				Price > 0.000m ? 
				PlanTypeEnum.Paid 
				: PlanTypeEnum.Free;
		}

        public string PlanName { get; set; }
		public string Description { get; set; }
		public PlanTypeEnum PlanType { get; set; }
        public string Duration { get; set; }
		public RatingEnum Rating { get; set; }
		public int RateCount { get; set; }
		public decimal Price { get; set; }
        public decimal PlanCommission { get; set; }
    }

	public enum RatingEnum
	{
		Bad = 1,
		Fair = 2,
		Good = 3,
		VeryGood = 4,
		Excellent = 5
	}

    public enum PlanTypeEnum
    {
        Free = 1,
        Paid = 2,
    }
}
