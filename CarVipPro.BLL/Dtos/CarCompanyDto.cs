using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarVipPro.BLL.Dtos
{
    public class CarCompanyDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên hãng xe không được để trống")]
        [StringLength(100, ErrorMessage = "Tên hãng xe tối đa 100 ký tự")]
        public string CatalogName { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả tối đa 500 ký tự")]
        public string? Description { get; set; }

        public bool IsActive { get; set; }

        // Thuộc tính hỗ trợ hiển thị thống kê
        public int ElectricVehicleCount { get; set; }
    }
}
