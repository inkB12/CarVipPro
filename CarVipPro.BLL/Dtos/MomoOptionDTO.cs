using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarVipPro.BLL.Dtos
{
    public class MomoOptionDTO
    {
        public string? PartnerCode { get; set; }
        public string? AccessKey { get; set; }
        public string? SecretKey { get; set; }
        public string? RequestType { get; set; }
        public string? MomoApiUrl { get; set; }
        public string? RedirectUrl { get; set; }
        public string? IpnUrl { get; set; }
    }
}
