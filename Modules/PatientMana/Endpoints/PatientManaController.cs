using Microsoft.AspNetCore.Mvc;
using BTL_SA.Modules.PatientMana.Domain.Models;
using BTL_SA.Modules.PatientMana.Facade;

namespace BTL_SA.Modules.PatientMana.Endpoints
{
    [ApiController]
    [Route("api/Patient")]
    public class PatientController(FacadePatientInformationManagement patinentInfoMana) : ControllerBase
    {
        private readonly FacadePatientInformationManagement _patientInfoMana = patinentInfoMana;

        [HttpPost("create")]
        public IActionResult CreatePatinent([FromBody] PatientForm patient)
        {
            var result = _patientInfoMana.Create(patient);
            if (result != 0)
            {
                return Ok(new { message = "Patient created successfully.", patientId = result });
            }
            return BadRequest(new { message = "Failed to create patient." });
        }

        [HttpPost("edit")]
        public IActionResult EditPatinent([FromBody] PatientForm patient)
        {
            Console.WriteLine("Editing Patient with ID: " + patient.Id);
            var result = _patientInfoMana.Update(patient.Id, patient);
            if (result != 0)
            {
                return Ok(new { message = "Patient edited successfully.", patientId = patient.Id });
            }
            return BadRequest(new { message = "Failed to edit patient." });
        }

        [HttpPost("delete/{id}")]
        public IActionResult DeletePatinent([FromRoute] int id)
        {
            var result = _patientInfoMana.Delete(id);
            if (result == 1)
            {
                return Ok(new { message = "Patinent deleted successfully.", patientId = id });
            }
            return BadRequest(new { message = "Failed to delete patient." });
        }

        [HttpGet("findbyid/{id}")]
        public IActionResult FindById([FromRoute] int id)
        {
            var patient = _patientInfoMana.FindById(id);
            if (patient != null)
            {
                return Ok(new PatientViewModel(patient));
            }
            return NotFound(new { message = "Patinent not found." });
        }

        [HttpGet("findbyemail/{id}")]
        public IActionResult FindByEmail([FromRoute] int id)
        {
            var patient = _patientInfoMana.FindByEmail(id);
            if (patient != null)
            {
                return Ok(new PatientViewModel(patient));
            }
            return NotFound(new { message = "Patinent not found." });
        }

        [HttpGet("getall")]
        public IActionResult GetAllPatient()
        {
            var patients = _patientInfoMana.FindAll();
            if (patients.Count > 0)
            {
                var PatientViewModels = patients.Select(e => new PatientViewModel(e)).ToList();
                return Ok(PatientViewModels);
            }
            return NotFound(new { message = "No Patient." });
        }

    }
}