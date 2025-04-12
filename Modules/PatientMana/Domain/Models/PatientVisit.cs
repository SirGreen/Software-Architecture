
namespace BTL_SA.Modules.PatientMana.Domain.Models
{
    public class PatientVisit
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime VisitDate { get; set; }
        public string? Notes { get; set; }
        public Patient? Patient { get; set; }
        public List<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();

    }

    public class PatientVisitView : Patient {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime VisitDate { get; set; }
        public string? Notes { get; set; }

        public PatientVisitView(PatientVisitView visit){
            Id = visit.Id;
            PatientId = visit.PatientId;
            VisitDate = visit.VisitDate;
            Notes = visit.Notes;
        }
        
        public PatientVisitView() { }

    }

    public class PatientVisitForm {
        public int PatientId { get; set; }
        public DateTime VisitDate { get; set; }
        public string Notes { get; set; }
    }
}
