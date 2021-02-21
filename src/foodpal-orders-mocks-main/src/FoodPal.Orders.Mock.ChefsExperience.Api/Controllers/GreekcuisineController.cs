using FoodPal.Orders.Mock.ChefsExperience.Api.BackgroundProcessing;
using FoodPal.Orders.Mock.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodPal.Orders.Mock.ChefsExperience.Api.Controllers
{
    public class GreekcuisineController : Controller
    {
		private readonly IBackgroundTaskQueue _taskQueue;

		public GreekcuisineController(IBackgroundTaskQueue taskQueue)
		{
			_taskQueue = taskQueue;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status202Accepted)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesErrorResponseType(typeof(ErrorInfoDto))]
		public ActionResult<string> CreateGreekcuisineOrder(HttpOrderRequestDto newOrderDto)
		{
			QueueGreekcuisineOrderProcessingAsync(newOrderDto);
			return Accepted();
		}

		#region Private Methods

		private void QueueGreekcuisineOrderProcessingAsync(HttpOrderRequestDto newOrderDto)
		{
			foreach (var orderItem in newOrderDto.OrderItems)
			{
				_taskQueue.QueueBackgroundWorkItem(async token =>
				{
					var rnd = new Random();
					var httpProxy = new HttpProxy();

					// order is being processed...
					var secondsRequiredForProcessing = rnd.Next(5, 15); // between 5 and 15 seconds
					await Task.Delay(TimeSpan.FromSeconds(secondsRequiredForProcessing), token);

					await httpProxy.PatchAsync<string>(orderItem.CallbackEndpoint);
				});
			}
		}

		#endregion
	}
}
