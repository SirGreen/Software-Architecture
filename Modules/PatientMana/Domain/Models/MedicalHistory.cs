using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.StaffMana.Domain.Model;

namespace BTL_SA.Modules.PatientMana.Domain.Models
{
    public class MedicalHistory
    {
        public int Id { get; set; }
        public int? PatientVisitId { get; set; }
        public int? DoctorId { get; set; }
        public string? Department { get; set; }
        public string? ReasonForVisit { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public string? PrescribedMedication { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public PatientVisit? PatientVisit { get; set; }
        public Employee? Doctor { get; set; }
    }

    public class MedicalHistoryView {
        public int Id { get; set; }
        public int? PatientVisitId { get; set; }
        public int? DoctorId { get; set; }
        public string? Department { get; set; }
        public string? ReasonForVisit { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public string? PrescribedMedication { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class MedicalHistoryForm {
        public int? PatientVisitId { get; set; }
        public int? DoctorId { get; set; }
        public string? Department { get; set; }
        public string? ReasonForVisit { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public string? PrescribedMedication { get; set; }
    }
}