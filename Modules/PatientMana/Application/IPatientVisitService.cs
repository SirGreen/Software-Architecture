using BTL_SA.Modules.PatientMana.Domain.Models;

namespace BTL_SA.Modules.PatientMana.Application;

public interface IPatientVisitService
{
    List<PatientVisitView> findAll();
    PatientVisitView findById(int id);
    List<PatientVisitView> findByPatientId(int id);
    int Create(PatientVisitForm visit);
    int Update(int id, PatientVisitForm visit);
    int Delete(int id);
}