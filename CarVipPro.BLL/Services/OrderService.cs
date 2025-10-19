using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using CarVipPro.DAL.Entities;
using CarVipPro.DAL.Interfaces;

namespace CarVipPro.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orders;
        private readonly ICustomerRepository _customers;
        private readonly IAccountRepository _accounts;
        private readonly IElectricVehicleRepository _evs;

        public OrderService(
            IOrderRepository orders,
            ICustomerRepository customers,
            IAccountRepository accounts,
            IElectricVehicleRepository evs)
        {
            _orders = orders;
            _customers = customers;
            _accounts = accounts;
            _evs = evs;
        }

        public async Task<(bool ok, string message, Order? data)> CreatePaidOrderAsync(
            int customerId,
            int staffAccountId,
            string paymentMethod,
            IEnumerable<OrderItemDto> items)
        {
            if (items is null || !items.Any())
                return (false, "No items", null);

            var method = (paymentMethod ?? "CASH").ToUpperInvariant();
            if (method is not ("CASH" or "MOMO" or "INSTALLMENT" or "VNPAY"))
                return (false, "Invalid payment method", null);

            var staff = await _accounts.GetByIdAsync(staffAccountId);
            if (staff is null || !staff.IsActive || (staff.Role != "Staff" && staff.Role != "Manager"))
                return (false, "Staff not found or inactive", null);

            var customer = await _customers.GetByIdAsync(customerId);
            if (customer is null) return (false, "Customer not found", null);

            var evIds = items.Select(i => i.ElectricVehicleId).Distinct().ToList();
            var evMap = await _evs.GetActiveByIdsAsync(evIds);

            foreach (var i in items)
            {
                if (!evMap.ContainsKey(i.ElectricVehicleId))
                    return (false, $"Vehicle {i.ElectricVehicleId} not found or inactive", null);
                if (i.Quantity <= 0)
                    return (false, "Quantity must be > 0", null);
            }

            // Tạo order
            var order = new Order
            {
                CustomerId = customerId,
                AccountId = staffAccountId,
                DateTime = DateTime.UtcNow,
                Total = 0m,
                PaymentMethod = method,
                Status = "COMPLETED"   // thanh toán luôn
            };
            order = await _orders.CreateAsync(order);

            // Tạo chi tiết + tính total
            var details = new List<OrderDetail>();
            decimal total = 0m;
            foreach (var i in items)
            {
                var ev = evMap[i.ElectricVehicleId];
                var lineTotal = i.TotalPrice ?? (ev.Price * i.Quantity);

                details.Add(new OrderDetail
                {
                    OrderId = order.Id,
                    ElectricVehicleId = i.ElectricVehicleId,
                    Quantity = i.Quantity,
                    TotalPrice = lineTotal
                });
                total += lineTotal;
            }

            await _orders.AddDetailsAsync(details);

            order.Total = total;
            await _orders.UpdateAsync(order);

            return (true, "Order created & paid", order);
        }

        public async Task<(bool ok, string message, Order? data)> UpdateOrderStatusAsync(string orderStatus, int orderId)
        {
            var order = await _orders.GetByIdAsync(orderId);

            if (order == null)
            {
                return (false, "No order found", null);
            }

            order.Status = orderStatus;

            await _orders.UpdateAsync(order);
            return (true, "Update Order Successfully", null);
        }

        public async Task<List<OrderListItemDto>> GetOrdersAsync(string? q, string? status)
        {
            var list = await _orders.SearchAsync(q, status);
            return list.Select(o => new OrderListItemDto
            {
                Id = o.Id,
                CustomerName = o.Customer.FullName,
                CustomerEmail = o.Customer.Email,
                DateTime = o.DateTime,
                Total = o.Total,
                PaymentMethod = o.PaymentMethod,
                Status = o.Status
            }).ToList();
        }


        public async Task<OrderDto?> GetOrderAsync(int id)
        {
            var o = await _orders.GetWithDetailsAsync(id);
            if (o == null) return null;


            return new OrderDto
            {
                Id = o.Id,
                CustomerName = o.Customer.FullName,
                CustomerEmail = o.Customer.Email,
                CustomerPhone = o.Customer.Phone,
                DateTime = o.DateTime,
                PaymentMethod = o.PaymentMethod,
                Status = o.Status,
                Total = o.Total,
                Details = o.OrderDetails.Select(od => new OrderDetailItemDto
                {
                    VehicleModel = od.ElectricVehicle.Model,
                    CompanyName = od.ElectricVehicle.CarCompany.CatalogName,
                    CategoryName = od.ElectricVehicle.Category.CategoryName,
                    Color = od.ElectricVehicle.Color,
                    Quantity = od.Quantity,
                    TotalPrice = od.TotalPrice,
                    UnitPriceEstimated = od.Quantity > 0 ? Math.Round(od.TotalPrice / od.Quantity, 0) : 0
                }).ToList()
            };
        }

    }
}
