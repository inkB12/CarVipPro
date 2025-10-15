using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarVipPro.BLL.Dtos
{
    public class ElectricVehicleDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên model không được để trống")]
        [StringLength(100, ErrorMessage = "Model tối đa 100 ký tự")]
        public string Model { get; set; }

        [StringLength(50, ErrorMessage = "Phiên bản tối đa 50 ký tự")]
        public string? Version { get; set; }

        [Required(ErrorMessage = "Giá xe là bắt buộc")]
        [Range(1, 10000000000, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        [StringLength(1000, ErrorMessage = "Thông số kỹ thuật tối đa 1000 ký tự")]
        public string? Specification { get; set; }

        [Url(ErrorMessage = "Liên kết hình ảnh không hợp lệ")]
        public string? ImageUrl { get; set; }

        [StringLength(30, ErrorMessage = "Tên màu tối đa 30 ký tự")]
        public string? Color { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hãng xe")]
        public int CarCompanyId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại xe")]
        public int CategoryId { get; set; }

        public string? CarCompanyName { get; set; }
        public string? CategoryName { get; set; }

        public bool IsActive { get; set; }
    }
}
