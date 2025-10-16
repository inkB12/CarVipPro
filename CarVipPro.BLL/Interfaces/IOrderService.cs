using CarVipPro.BLL.Dtos;
using CarVipPro.DAL.Entities;

namespace CarVipPro.BLL.Interfaces
{
    public interface IOrderService
    {
        Task<(bool ok, string message, Order? data)> CreatePaidOrderAsync(int customerId,int staffAccountId,string paymentMethod,IEnumerable<OrderItemDto> items);
        Task<(bool ok, string message, Order? data)> UpdateOrderStatusAsync(string orderStatus, int orderId);
    }
}
