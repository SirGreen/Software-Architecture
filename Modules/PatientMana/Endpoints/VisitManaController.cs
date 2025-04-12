using Microsoft.AspNetCore.Mvc;
using BTL_SA.Modules.PatientMana.Domain.Models;
using BTL_SA.Modules.PatientMana.Facade;

namespace BTL_SA.Modules.PatientMana.Endpoints
{
    [ApiController]
    [Route("api/VisitMana")]
    public class VisitManaController(FacadeVisitInformationManagement visitInfoMana) : ControllerBase
    {
        private readonly FacadeVisitInformationManagement _visitInfoMana= visitInfoMana;

        [HttpPost("create")]
        public IActionResult CreateEmployee([FromBody] PatientVisitForm visit)
        {
            var result = _visitInfoMana.Create(visit);
            if (result != 0)
            {
                return Ok(new { message = "Patient visit created successfully.", visitId = result });
            }
            return BadRequest(new { message = "Failed to create visit." });
        }

        [HttpPost("edit")]
        public IActionResult EditEmployee([FromRoute] int id, [FromBody] PatientVisitForm visit)
        {
            Console.WriteLine("Editing employee with ID: " + id);
            var result = _visitInfoMana.Update(id ,visit);
            if (result != 0)
            {
                return Ok(new { message = "Patient visit edited successfully.", visitId = id });
            }
            return BadRequest(new { message = "Failed to edit visit." });
        }
        
        [HttpPost("delete/{id}")]
        public IActionResult DeleteEmployee([FromRoute] int id)
        {
            var result = _visitInfoMana.Delete(id);
            if (result == 1)
            {
                return Ok(new { message = "Patient visit deleted successfully.", employeeId = id });
            }
            return BadRequest(new { message = "Failed to delete visit." });
        }

        [HttpGet("findbyid/{id}")]
        public IActionResult findById([FromRoute] int id)
        {
            var visit = _visitInfoMana.findById(id);
            if (visit != null)
            {
                return Ok(new PatientVisitView(visit));
            }
            return NotFound(new { message = "visit not found." });
        }

        [HttpGet("findBypatientid/{patient_id}")]
        public IActionResult findByPatientId([FromRoute] int id)
        {
            var visits = _visitInfoMana.findByPatientId(id);
            if (visits.Count > 0)
            {
                var visitViewModels = visits.Select(e => new PatientVisitView(e)).ToList();
                return Ok(visitViewModels);
            }
            return NotFound(new { message = "visit not found." });
        }

        [HttpGet("getall")]
        public IActionResult findAll()
        {
            var visits = _visitInfoMana.findAll();
            if (visits.Count > 0)
            {
                var visitViewModels = visits.Select(e => new PatientVisitView(e)).ToList();
                return Ok(visitViewModels);
            }
            return NotFound(new { message = "No visits found." });
        }
    }
}