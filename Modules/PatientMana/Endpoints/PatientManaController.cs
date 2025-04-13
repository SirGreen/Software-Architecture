using Microsoft.AspNetCore.Mvc;
using BTL_SA.Modules.PatientMana.Domain.Models;
using BTL_SA.Modules.PatientMana.Facade;

namespace BTL_SA.Modules.PatientMana.Endpoints
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController(FacadePatientManagement patinentMana) : ControllerBase
    {
        private readonly FacadePatientManagement _patientMana = patinentMana;

        [HttpGet("PatientInfo")]
        public IActionResult GetAllPatient()
        {
            var patients = _patientMana.GetAllPatient();
            if (patients.Count > 0)
            {
                var PatientViewModels = patients.Select(e => new PatientViewModel(e)).ToList();
                return Ok(PatientViewModels);
            }
            return NotFound(new { message = "No Patient." });
        }

        [HttpGet("PatientInfo/{id:int}")]
        public IActionResult FindById([FromRoute] int id)
        {
            var patient = _patientMana.GetPatientById(id);
            if (patient != null)
            {
                return Ok(new PatientViewModel(patient));
            }
            return NotFound(new { message = "Patient not found." });
        }

        // [HttpGet("PatientInfo/FindByEmail")]
        // public IActionResult FindByEmail([FromBody] string email)
        // {
        //     var patient = _patientMana.GetPatientByEmail(email);
        //     if (patient != null)
        //     {
        //         return Ok(new PatientViewModel(patient));
        //     }
        //     return NotFound(new { message = "Patinent not found." });
        // }

        [HttpPost("PatientInfo")]
        public IActionResult CreatePatient([FromBody] PatientForm patient)
        {
            var result = _patientMana.CreatePatient(patient);
            if (result != 0)
            {
                return Ok(new { message = "Patient created successfully.", patientId = result });
            }
            return BadRequest(new { message = "Failed to create patient." });
        }

        [HttpPut("PatientInfo/{id}")]
        public IActionResult EditPatient([FromRoute]int id ,[FromBody] PatientForm patient)
        {
            var result = _patientMana.UpdatePatient(id, patient);
            if (result != 0)
            {
                return Ok(new { message = "Patient edited successfully.", patientId = id });
            }
            return BadRequest(new { message = "Failed to edit patient." });
        }

        [HttpDelete("PatientInfo/{id}")]
        public IActionResult DeletePatient([FromRoute] int id)
        {
            var result = _patientMana.DeletePatient(id);
            if (result == 1)
            {
                return Ok(new { message = "Patinent deleted successfully.", patientId = id });
            }
            return BadRequest(new { message = "Failed to delete patient." });
        }

        [HttpGet("PatientVisit")]
        public IActionResult GetPatientVisit() {
            var visits = _patientMana.GetAllVisit();
            if (visits != null) {
                return Ok(visits);
            }
            return NotFound(new { message = "No Patient Visit"});
        }

        [HttpGet("PatientVisit/{id}")]
        public IActionResult GetPatientVisitById([FromRoute] int id) {
            var visits = _patientMana.GetVisitById(id);
            if (visits != null) {
                return Ok(visits);
            }
            return NotFound(new { message = "No Patient Visit"});
        }

        [HttpPost("PatientVisit")]
        public IActionResult CreatePatientVisit([FromBody] PatientVisitForm patientVisitForm) {
            var result = _patientMana.CreatePatientVisit(patientVisitForm);
            if (result != 0)
            {
                return Ok(new { message = "Patient Visit created successfully.", patientVisitId = result });
            }
            return BadRequest(new { message = "Failed to create patient visit." });
        }

        [HttpPut("PatientVisit/{id}")]
        public IActionResult EditPatientVisit([FromRoute]int id ,[FromBody] PatientVisitForm patientVisitForm)
        {
            var result = _patientMana.UpdatePatientVisit(id, patientVisitForm);
            if (result != 0)
            {
                return Ok(new { message = "Patient Visit edited successfully.", patientVisitId = id });
            }
            return BadRequest(new { message = "Failed to edit patient visit." });
        }

        [HttpDelete("PatientVisit/{id}")]
        public IActionResult DeletePatientVisit([FromRoute] int id)
        {
            var result = _patientMana.DeletePatientVisit(id);
            if (result == 1)
            {
                return Ok(new { message = "Patient Visit deleted successfully.", patientVisitId = id });
            }
            return BadRequest(new { message = "Failed to delete patient visit." });
        }

        [HttpGet("MedicalHistory")]
        public IActionResult GetMedicalHistory() {
            var medicalHistory = _patientMana.GetAllMedicalHistory();
            if (medicalHistory.Count > 0) {
                return Ok(medicalHistory);
            }
            return NotFound(new { message = "No Medical History"});
        }

        [HttpGet("MedicalHistory/{id}")]
        public IActionResult GetMedicalHistoryById([FromRoute] int id) {
            var medicalHistory = _patientMana.GetMedicalHistoryById(id);
            if (medicalHistory != null) {
                return Ok(medicalHistory);
            }
            return NotFound(new { message = "No Medical History"});
        }

        [HttpPost("MedicalHistory")]
        public IActionResult CreateMedicalHistory([FromBody] MedicalHistoryForm medicalHistoryForm) {
            var result = _patientMana.CreateMedicalHistory(medicalHistoryForm);
            if (result != 0)
            {
                return Ok(new { message = "Medical History created successfully.", medicalHistoryId = result });
            }
            return BadRequest(new { message = "Failed to create Medical History." });
        }

        [HttpPut("MedicalHistory/{id}")]
        public IActionResult EditMedicalHistory([FromRoute]int id ,[FromBody] MedicalHistoryForm medicalHistoryForm)
        {
            var result = _patientMana.UpdateMedicalHistory(id, medicalHistoryForm);
            if (result != 0)
            {
                return Ok(new { message = "Medical History edited successfully.", medicalHistoryId = id });
            }
            return BadRequest(new { message = "Failed to edit Medical History." });
        }

        [HttpDelete("MedicalHistory/{id}")]
        public IActionResult DeleteMedicalHistory([FromRoute] int id)
        {
            var result = _patientMana.DeleteMedicalHistory(id);
            if (result == 1)
            {
                return Ok(new { message = "Medical History deleted successfully.", medicalHistoryId = id });
            }
            return BadRequest(new { message = "Failed to delete Medical History." });
        }
    }
}