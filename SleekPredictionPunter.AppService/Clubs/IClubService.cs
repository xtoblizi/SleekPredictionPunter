using SleekPredictionPunter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Clubs
{
    public interface IClubService
    {
		Task<long> Insert(Club model, bool savechanges = true);
		Task Update(Club model, bool savechanges = true);

		Task Delete(Club model, bool savechanges = true);

		//Task<Club> GetFirstOrDefault(Club model);

		Task<IEnumerable<Club>> GetAllQueryable();

		Task<Club> GetById(long id);
		Task<Club> GetByName(string name);
	}
}
