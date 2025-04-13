using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerOperation(Summary = "Retrieve all patients.")]
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
        [SwaggerOperation(Summary = "Retrieve a patient by ID.")]
        public IActionResult FindById([FromRoute] int id)
        {
            var patient = _patientMana.GetPatientById(id);
            if (patient != null)
            {
                return Ok(new PatientViewModel(patient));
            }
            return NotFound(new { message = "Patient not found." });
        }

        [HttpPost("PatientInfo")]
        [SwaggerOperation(Summary = "Create a new patient.")]
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
        [SwaggerOperation(Summary = "Edit an existing patient by ID.")]
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
        [SwaggerOperation(Summary = "Delete a patient by ID.")]
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
        [SwaggerOperation(Summary = "Retrieve all patient visits.")]
        public IActionResult GetPatientVisit() {
            var visits = _patientMana.GetAllVisit();
            if (visits != null) {
                return Ok(visits);
            }
            return NotFound(new { message = "No Patient Visit"});
        }

        [HttpGet("PatientVisit/{id}")]
        [SwaggerOperation(Summary = "Retrieve a patient visit by ID.")]
        public IActionResult GetPatientVisitById([FromRoute] int id) {
            var visits = _patientMana.GetVisitById(id);
            if (visits != null) {
                return Ok(visits);
            }
            return NotFound(new { message = "No Patient Visit"});
        }

        [HttpPost("PatientVisit")]
        [SwaggerOperation(Summary = "Create a new patient visit.")]
        public IActionResult CreatePatientVisit([FromBody] PatientVisitForm patientVisitForm) {
            var result = _patientMana.CreatePatientVisit(patientVisitForm);
            if (result != 0)
            {
                return Ok(new { message = "Patient Visit created successfully.", patientVisitId = result });
            }
            return BadRequest(new { message = "Failed to create patient visit." });
        }

        [HttpPut("PatientVisit/{id}")]
        [SwaggerOperation(Summary = "Edit an existing patient visit by ID.")]
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
        [SwaggerOperation(Summary = "Delete a patient visit by ID.")]
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
        [SwaggerOperation(Summary = "Retrieve all medical histories.")]
        public IActionResult GetMedicalHistory() {
            var medicalHistory = _patientMana.GetAllMedicalHistory();
            if (medicalHistory.Count > 0) {
                return Ok(medicalHistory);
            }
            return NotFound(new { message = "No Medical History"});
        }

        [HttpGet("MedicalHistory/{id}")]
        [SwaggerOperation(Summary = "Retrieve a medical history by ID.")]
        public IActionResult GetMedicalHistoryById([FromRoute] int id) {
            var medicalHistory = _patientMana.GetMedicalHistoryById(id);
            if (medicalHistory != null) {
                return Ok(medicalHistory);
            }
            return NotFound(new { message = "No Medical History"});
        }

        [HttpPost("MedicalHistory")]
        [SwaggerOperation(Summary = "Create a new medical history.")]
        public IActionResult CreateMedicalHistory([FromBody] MedicalHistoryForm medicalHistoryForm) {
            var result = _patientMana.CreateMedicalHistory(medicalHistoryForm);
            if (result != 0)
            {
                return Ok(new { message = "Medical History created successfully.", medicalHistoryId = result });
            }
            return BadRequest(new { message = "Failed to create Medical History." });
        }

        [HttpPut("MedicalHistory/{id}")]
        [SwaggerOperation(Summary = "Edit an existing medical history by ID.")]
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
        [SwaggerOperation(Summary = "Delete a medical history by ID.")]
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