using SleekPredictionPunter.Model.PredictionBookings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.PredictionsBookings
{
	public interface IPredictionBookingService : IDisposable
	{
		Task<long> Insert(PredictionBooking model, bool savechanges = true);
		Task Update(PredictionBooking model, bool savechanges = true);
		Task Delete(PredictionBooking model, bool savechanges = true);
		Task<IEnumerable<PredictionBooking>> GetBookings<OrderByKey>(Func<PredictionBooking, bool> whereFunc = null,
			Func<PredictionBooking, OrderByKey> orderByDescFunc = null,
			int startIndex = 0, int count = int.MaxValue);
		Task<PredictionBooking> GetBookingById(long id);
		Task<PredictionBooking> GetDefault(Func<PredictionBooking, bool> predicate = null, bool includeSetAsHotPreviewCriteria = false);
	}
}
