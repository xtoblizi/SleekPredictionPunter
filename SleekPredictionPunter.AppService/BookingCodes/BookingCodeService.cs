using SleekPredictionPunter.AppService.BookingCodes.Dto;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Model.BettingPlatform;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.BookingCodes
{
	public class BookingCodeService : IBookingCodeService
	{
		private readonly IBaseRepository<BookingCode> _baseRepository;
		private readonly IBaseRepository<BetPlanform> _betplatform;
		public BookingCodeService(IBaseRepository<BookingCode> baseRepository, 
			IBaseRepository<BetPlanform> betplatformserivce)
		{
			_baseRepository = baseRepository;
			_betplatform = betplatformserivce;
		}

		public async Task<long> Insert(BookingCode model, bool savechanges = true)
		{
			return await _baseRepository.Insert(model);
		}
		public async Task Delete(BookingCode model, bool savechanges = true)
		{
			 await _baseRepository.Delete(model);
		}

		public async Task Update(BookingCode model, bool savechanges = true)
		{
			await _baseRepository.Update(model);
		}

		public async Task<IEnumerable<BookingCode>> GetAllQueryable(Func<BookingCode, bool> wherefunc = null,
			Func<BookingCode, DateTime> orderByFunc = null,
			int startIndex = 0, int count = int.MaxValue)
		{
			return await _baseRepository.GetAllQueryable(wherefunc, orderByFunc, startIndex, count);
		}

		public async Task<BookingCode> GetById(long id)
		{
			return await _baseRepository.GetById(id);
		}

		public async Task<BookingCode> GetByName(string bookingcode)
		{
			Func<BookingCode, bool> wherefunc = (x => x.BetCode == bookingcode);
			return await _baseRepository.GetFirstOrDefault(wherefunc);
		}

		public async Task<IEnumerable<BookingCodeDto>> GetAllQueryableDto(Func<BookingCode, bool> wherefunc = null,
			Func<BookingCode, DateTime> orderByFunc = null, int startIndex = 0, int count = int.MaxValue)
		{
			var result = await _baseRepository.GetAllQueryable(wherefunc, orderByFunc, startIndex, count);
			var dtolist = new List<BookingCodeDto>();

			dtolist.AddRange(result.Select(x => new BookingCodeDto
			{
				BetCode = x.BetCode,
				Betplatform = x.Betplatform,
				BetPlatformId = x.BetPlatformId,
				PricingPlan = x.PricingPlan,
				PricingPlanId = x.PricingPlanId,
				DateCreated = x.DateCreated,
				DateUpdated = x.DateUpdated,
				EntityStatus = x.EntityStatus,
				Id = x.Id,
				BetPlatformFilePath = GetBetPlanform(x.BetPlatformId)?.LogoPath
			}));

			return dtolist;
		}
		
		public async Task<IEnumerable<BookingCodeDto>> GetAllQueryableDto(List<Func<BookingCode, bool>> wherefuncs = null,
			Func<BookingCode, DateTime> orderByFunc = null, int startIndex = 0, int count = int.MaxValue)
		{

			List<BookingCode> result = new List<BookingCode>();
			int subcount = count;

			if (wherefuncs != null)
				subcount = count / wherefuncs.Count;

			foreach (var item in wherefuncs)
			{
				var tempResult = await this.GetAllQueryable(item, (c => c.DateCreated), startIndex, subcount);
				result.AddRange(tempResult.ToList());
			}
			
			var dtolist = new List<BookingCodeDto>();

			dtolist.AddRange(result.Select(x => new BookingCodeDto
			{
				BetCode = x.BetCode,
				Betplatform = x.Betplatform,
				BetPlatformId = x.BetPlatformId,
				PricingPlan = x.PricingPlan,
				PricingPlanId = x.PricingPlanId,
				DateCreated = x.DateCreated,
				DateUpdated = x.DateUpdated,
				EntityStatus = x.EntityStatus,
				Id = x.Id,
				BetPlatformFilePath = GetBetPlanform(x.BetPlatformId)?.LogoPath
			}));

			return dtolist;
		}

		public async Task<long> GetCount()
		{
			return await _baseRepository.GetCount();
		}
		private BetPlanform GetBetPlanform(long id)
		{
			return _betplatform.GetById(id).Result;
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		~BookingCodeService()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			GC.SuppressFinalize(this);
		}

	


		#endregion
	}
}
