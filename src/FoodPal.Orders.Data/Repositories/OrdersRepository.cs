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
    public class OrdersRepository : IOrdersRepository
    {
        private readonly OrdersContext _ordersContext;

        public OrdersRepository(OrdersContext ordersContext)
        {
            _ordersContext = ordersContext;
        }

        public async Task<Order> CreateAsync(Order newOrder)
        {
            if (newOrder is null) throw new ArgumentNullException(nameof(newOrder));

            try
            {
                await _ordersContext.AddAsync(newOrder);
                await _ordersContext.SaveChangesAsync();

                return newOrder;
            }
            catch (Exception ex)
            {
                throw new Exception($"Order could not be saved. Reason:{ex.Message}");
            }
        }

        public async Task<Order> GetByIdAsync(int orderId)
        {
            try
            {
                return await _ordersContext.Orders.SingleOrDefaultAsync(x => x.Id == orderId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Order by id {orderId} could not be found. Reason:{ex.Message}");
            }
        }

        public async Task<(IEnumerable<Order> Orders, int AllOrdersCount)> GetByFiltersAsync(string customerId, OrderStatus? status, int page, int pageSize)
        {
            try
            {
                var orders = await _ordersContext.Orders
                    .Include(o => o.CustomerId)
                    .Include(o => o.Status)
                    .Where(o => o.CustomerId == customerId && o.Status == status)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .ToListAsync();

                return (orders, orders.Count());
            }
            catch (Exception ex)
            {
                throw new Exception($"Something went wrong. Reason:{ex.Message}");
            }
        }

        public async Task<OrderStatus?> GetStatusAsync(int orderId)
        {
            try
            {
                return (await _ordersContext.Orders.Include(o => o.Status).SingleOrDefaultAsync(o => o.Id == orderId)).Status;
            }
            catch (Exception ex)
            {
                throw new Exception($"Status for order with id: {orderId} could not be found. Reason:{ex.Message}");
            }
        }

        public async Task UpdateStatusAsync(Order orderEntity, OrderStatus newStatus)
        {
            try
            {
                var order = await _ordersContext.Orders.SingleOrDefaultAsync(o => o.Id == orderEntity.Id);
                if (order != null)
                {
                    order.Status = newStatus;
                    _ordersContext.Update(order);
                    await _ordersContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Order by id: {orderEntity.Id} could not be updated. Reason:{ex.Message}");
            }
        }
    }
}