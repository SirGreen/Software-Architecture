using Microsoft.AspNetCore.Mvc;
using BTL_SA.Modules.StaffMana.Facade;
using BTL_SA.Modules.StaffMana.Domain.Model;

namespace BTL_SA.Controllers
{
    public class StaffController : Controller
    {
        private readonly FacadeStaffInformationManagement _staffInfoMana;

        public StaffController(FacadeStaffInformationManagement staffInfoMana)
        {
            _staffInfoMana = staffInfoMana;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}