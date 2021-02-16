using FoodPal.Orders.Data.Contracts;
using FoodPal.Orders.Enums;
using FoodPal.Orders.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodPal.Orders.Data.Repositories
{
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly OrdersContext _ordersContext;

        public OrderItemsRepository(OrdersContext ordersContext)
        {
            _ordersContext = ordersContext;
        }

        public async Task<OrderItem> GetOrderItemAsync(int orderId, int orderItemId)
        {
            try
            {
                return await _ordersContext.OrderItems
                    .Include(o => o.Order).Where(o => o.Id == orderId)
                    .SingleOrDefaultAsync(o => o.Id == orderItemId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Order Item by orderId: {orderId} and orderItemId: {orderItemId} could not be found. Reason:{ex.Message}");
            }
        }

        public async Task<List<OrderItem>> GetItemsAsync(int orderId)
        {
            try
            {
                return await _ordersContext.OrderItems
                    .Include(o => o.Order).Where(o => o.Id == orderId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Order Items by orderId: {orderId} could not be found. Reason:{ex.Message}");
            }
        }

        public async Task UpdateStatusAsync(OrderItem orderItemEntity, OrderItemStatus newStatus)
        {
            try
            {
                var orderItem = await _ordersContext.OrderItems.SingleOrDefaultAsync(o => o.Id == orderItemEntity.Id);
                if (orderItem != null)
                {
                    orderItem.Status = newStatus;
                    _ordersContext.Update(orderItem);
                    await _ordersContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Order Item by id: {orderItemEntity.Id} could not be updated. Reason:{ex.Message}");
            }
        }
    }
}