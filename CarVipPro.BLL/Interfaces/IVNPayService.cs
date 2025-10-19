using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarVipPro.BLL.Interfaces
{
    public interface IVNPayService
    {
        String createVNPayUrl(int orderId, decimal amount);
    }
}
