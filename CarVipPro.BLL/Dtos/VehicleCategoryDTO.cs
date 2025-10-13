using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarVipPro.BLL.Dtos
{
    public class VehicleCategoryDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên loại xe không được để trống")]
        [StringLength(100, ErrorMessage = "Tên loại xe tối đa 100 ký tự")]
        public string CategoryName { get; set; }

        public bool IsActive { get; set; }

        public int ElectricVehicleCount { get; set; }
    }
}
