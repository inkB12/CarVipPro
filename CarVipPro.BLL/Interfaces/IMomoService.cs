using CarVipPro.BLL.Dtos;

namespace CarVipPro.BLL.Interfaces
{
    public interface IMomoService
    {
        Task<MomoResponseDTO> CreatePaymentAsync(int orderId, decimal orderTotal);
    }
}
