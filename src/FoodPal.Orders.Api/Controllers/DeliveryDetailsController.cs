using FoodPal.Orders.Dtos;
using FoodPal.Orders.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FoodPal.Orders.Api.Controllers
{
	/// <summary>
	/// Providers API methods for handling order item requests.
	/// </summary>
	public class DeliveryDetailsController : ApiBaseController
	{
		private readonly IDeliveryDetailsService _deliveryDetailsService;

		/// <summary>
		/// Constructor for Orders controller.
		/// </summary>
		public DeliveryDetailsController(IDeliveryDetailsService deliveryDetailsService)
		{
			_deliveryDetailsService = deliveryDetailsService;
		}


		/// <summary>
		/// Returns the specified order, if exists.
		/// </summary>
		/// <param name="orderId">The order identifier.</param>
		/// <returns>An object containing the order details.</returns>
		[HttpGet]
		[Route("{orderId}")]
		[ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesErrorResponseType(typeof(ErrorInfoDto))]
		public async Task<ActionResult<string>> GetDeliveryDetailsByOrderId(int orderId)
		{
			var orderResult = await _deliveryDetailsService.GetByOrderIdAsync(orderId);
			return Ok(orderResult);
		}

	}
}
