using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.ElectricVehicle
{
    public class DeleteModel : PageModel
    {
        private readonly IElectricVehicleService _service;

        public DeleteModel(IElectricVehicleService service)
        {
            _service = service;
        }

        [BindProperty]
        public ElectricVehicleDTO Vehicle { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Vehicle = await _service.GetById(id);
            if (Vehicle == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _service.Delete(Vehicle.Id);
            return RedirectToPage("Index");
        }
    }
}
