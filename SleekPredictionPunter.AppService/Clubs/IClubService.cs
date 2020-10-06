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

		Task<IEnumerable<Club>> GetAllQueryable(Func<Club,bool> wherefunc=null,Func<Club,DateTime> orderByFunc =null
			,int startIndex = 0,int count = int.MaxValue);

		Task<Club> GetById(long id);
		Task<Club> GetByName(string name);
		Task<long> GetCount();
	}
}
