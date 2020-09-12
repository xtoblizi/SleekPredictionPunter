using SleekPredictionPunter.AppService.Wallet;
using SleekPredictionPunter.Model;
using SleekPredictionPunter.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleekPredictionPunter.AppService.Agents
{
    public class AgentService: IAgentService
    {
        private readonly IBaseRepository<Agent> _repo;
        private readonly IWalletAppService _walletAppService;
        public AgentService(IBaseRepository<Agent> baseRepository, IWalletAppService walletAppService)
        {
            _repo = baseRepository;
            _walletAppService = walletAppService;
        }

        public async Task<long> CreateAgent(Agent agent)
        {
            try
            {
                var referrerCode = RandomString(8);
                if (!string.IsNullOrEmpty(agent.Email))
                {
                    agent.Username = agent.Email;
                }
                if (!string.IsNullOrEmpty(agent.TenantUniqueName))
                {
                    agent.TenantUniqueName = $"{agent.FirstName}-{agent.BrandNameOrNickName}";
                }
                if (!string.IsNullOrEmpty(referrerCode))
                {
                    var getAll = await _repo.GetAllQueryable();
                    //Here, check if generated referrerCode already exist on the db for another agent..
                    var checkForExistingReferrerCode = getAll.Where(options => options.RefererCode == referrerCode);
                    if (checkForExistingReferrerCode.Any())
                    {
                        referrerCode = $"{referrerCode}_{RandomString(2)}";
                    };
                }

                agent.EntityStatus = Model.Enums.EntityStatusEnum.NotActive;
                agent.DateCreated = DateTime.Now;
                agent.IsTenant = true;
                agent.RefererCode = referrerCode;

                return await _repo.Insert(agent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Update(Agent model, bool savechage = true)
        {
            await _repo.Update(model, savechage);
        }

        public async Task<Agent> GetAgentById(long id)
        {
            return await _repo.GetById(id);
        }

        public async Task<IEnumerable<Agent>> GetAgents(Func<Agent, bool> predicate = null, int startIndex = 0, int count = int.MaxValue)
        {
            return await _repo.GetAllQueryable(predicate, startIndex, count);
        }

        public async Task<Agent> GetAgentsPredicate(Func<Agent, bool> predicate)
        {
            return await _repo.GetFirstOrDefault(predicate);
        }

        public async Task RemoveAgentById(Agent owner, bool savechage = true)
        {
            await _repo.Delete(owner, savechage);
        }

        public async Task<long> GetMonthlySummaryForNewAgents()
        {
            var dateFrom = DateTime.Now;
            var firstDayOfTheMonth = new DateTime(dateFrom.Year, dateFrom.Month, 1);
            var dateTo = DateTime.Now;

            var getAllAgents = await _repo.GetAllQueryable();

            var filter = getAllAgents.Where(x => x.DateCreated >= firstDayOfTheMonth && x.DateCreated <= dateTo);
            return filter.LongCount();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
