using FoodPal.Orders.Dtos;
using System.Threading.Tasks;

namespace FoodPal.Orders.Services.Contracts
{
	public interface IDeliveryDetailsService
	{
		Task<DeliveryDetailsDto> GetByOrderIdAsync(int orderId);
	}
}
