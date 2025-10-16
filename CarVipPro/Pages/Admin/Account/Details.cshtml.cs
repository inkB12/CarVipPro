using CarVipPro.BLL.Dtos;
using CarVipPro.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarVipPro.APrenstationLayer.Pages.Admin.Accounts
{
    public class DetailsModel : PageModel
    {
        private readonly IAccountService _service;
        public DetailsModel(IAccountService service) { _service = service; }

        public AccountDTO? Item { get; set; }

        public async Task OnGetAsync(int id)
        {
            Item = await _service.GetByIdAsync(id);
        }
    }
}
