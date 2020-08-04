using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Contacts
{
	public interface IContactAppService
	{
		Task<long> Insert(Contact model, bool savechanges = true);
		Task Update(Contact model, bool savechanges = true);

		Task Delete(Contact model, bool savechanges = true);

		Task<Contact> GetFirstOrDefault(Contact model);

		Task<IEnumerable<Contact>> GetAllQueryable(string email = null,
			DateTime? startDate = null, DateTime? endDate = null,
			int startIndex = 0, int count = int.MaxValue);

		Task<Contact> GetById(long id);
	}
}
