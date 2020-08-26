using System;
using System.Collections.Generic;
using System.Text;

namespace SleekPredictionPunter.Model.Matches
{
	public class MatchDto
	{
		public MatchDto(DateTime timeOfMatch)
		{
			if (timeOfMatch < DateTime.Now)
				throw new ArgumentNullException("You cannot create a match that has already past");
			else
				MatchStatus = MatchStatusEnum.Upcoming;
			
		}
		public MatchDto()
		{

		}
		public virtual long ClubAId { get; set; }
		public Club	ClubA { get; set; }

		public virtual long ClubBId { get; set; }
		public Club ClubB { get; set; }

		public string MatchCategory { get; set; }
		public string SportCategory { get; set; }

		public bool IsSetAsHotPreview { get; set; }

		public DateTime TimeofMatch { get; set; }
		public MatchStatusEnum MatchStatus { get; set; }
	}

	public class Match : BaseEntity
	{
		public Match(DateTime timeOfMatch)
		{
			if (timeOfMatch < DateTime.Now)
				throw new ArgumentNullException("You cannot create a match that has already past");
			else
			{
				MatchStatus = MatchStatusEnum.Upcoming;
			}
		}
		public Match()
		{

		}

		public string ClubA { get; set; }

		public virtual long ClubAId { get; set; }
		public string ClubALogoPath { get; set; }

		public string ClubB { get; set; }
		public virtual long ClubBId { get; set; }
		public string ClubBLogoPath { get; set; }

		/// <summary>
		/// Categories relating to this match
		/// </summary>
		public string MatchCategory { get; set; }
		public long MatchCategoryId { get; set; }
		public string SportCategory { get; set; }
		public long	SportCategoryId { get; set; }
		
		public DateTime TimeofMatch { get; set; }
		
		public MatchStatusEnum MatchStatus { get; set; }

		/// <summary>
		/// Note : Use this field(IsSetAsHotPreview) and the DateUpdated properties to 
		/// know the last match set to be hottest for preview
		/// </summary>
		public bool IsSetAsHotPreview { get; set; }

		/// <summary>
		/// This is relationship of all predictions that can/ is created from this match.
		/// Note at the time a map is used to create the realationship between the two tables.
		/// </summary>
		public virtual ICollection<Prediction> Predictions { get; set; }
		
	}
}
