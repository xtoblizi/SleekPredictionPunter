using SleekPredictionPunter.AppService.BookingCodes.Dto;
using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.BookingCodes
{
	public interface IBookingCodeService : IDisposable
	{
		Task<long> Insert(BookingCode model, bool savechanges = true);
		Task Update(BookingCode model, bool savechanges = true);

		Task Delete(BookingCode model, bool savechanges = true);
		Task<IEnumerable<BookingCode>> GetAllQueryable(Func<BookingCode, bool> wherefunc = null, Func<BookingCode, DateTime> orderByFunc = null
			, int startIndex = 0, int count = int.MaxValue);

		Task<IEnumerable<BookingCodeDto>> GetAllQueryableDto(Func<BookingCode, bool> wherefunc = null, Func<BookingCode, DateTime> orderByFunc = null
			, int startIndex = 0, int count = int.MaxValue);

		Task<BookingCode> GetById(long id);
		Task<BookingCode> GetByName(string bookingcode);
	}
}
