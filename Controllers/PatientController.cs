using Microsoft.AspNetCore.Mvc;

namespace BTL_SA.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Visits()
        {
            return View();
        }

        public IActionResult MedicalHistory()
        {
            return View();
        }
    }
}