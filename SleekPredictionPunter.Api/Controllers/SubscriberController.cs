using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SleekPredictionPunter.AppService;
using SleekPredictionPunter.AppService.Dtos;

namespace SleekPredictionPunter.Api.Controllers
{
    public class SubscriberController : Controller
    {
        private readonly ISubscriberService _subscriberService;
        private readonly ILogger<SubscriberController> _logger;

        public SubscriberController(ISubscriberService subscriberService, ILogger<SubscriberController> logger)
        {
            _subscriberService = subscriberService;
            _logger = logger;
        }

        [HttpGet]
        [Route("Subscribers")]
        public async Task<IActionResult> GetAllSubscribersAsynch(int? activationstatus = null, int? gender = null,
            DateTime? startDate = null, DateTime? endDate = null,
            int startIndex = 0, int count = int.MaxValue)
        {
            try
            {
                var resuult = await _subscriberService.GetAllQueryable(activationstatus,gender,startDate,endDate,startIndex, count);
                return Ok(resuult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
        }

        [HttpGet]
        [Route("Subscriber")]
        public async Task<IActionResult> GetRouteByIdAsync(long id)
        {
            try
            {
                var result = await _subscriberService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
        }

        [HttpGet]
        [Route("DefoultSubscriber")]
        public async Task<IActionResult> GetFirstOrDefoultSubscriber(SubscriberDto model)
        {
            try
            {
                var result = await _subscriberService.GetFirstOrDefault(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeleteSubscriber")]
        public async Task<IActionResult>DeleteSubAsynch(SubscriberDto phoneOwner, bool savechage = true)
        {
            try
            {
                await _subscriberService.Delete(phoneOwner, savechage);
                return Ok("Record Deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
        }

        [HttpPut]
        [Route("UpdateSubscriber")]
        public async Task<IActionResult> Update(SubscriberDto model, bool savechanges = true)
        {
            try
            {
                await _subscriberService.Update(model, savechanges);
                return Ok("Record Updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
        }


        [HttpPost]
        [Route("CreateSubscriber")]
        public async Task<IActionResult> Insert(SubscriberDto phoneOwner, bool savechage = true)
        {
            try
            {
               var result = await _subscriberService.Insert(phoneOwner, savechage);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
        }
    }
}