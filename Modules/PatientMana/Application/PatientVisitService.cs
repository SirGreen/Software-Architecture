using BTL_SA.Modules.PatientMana.Domain.Models;
using BTL_SA.Modules.PatientMana.Infrastructure;

namespace BTL_SA.Modules.PatientMana.Application;
public class PatientVisitService(IPatientVisitRepository patientVisitRepository) : IPatientVisitService
{
    private readonly IPatientVisitRepository _patientVisitRepository = patientVisitRepository;

    public List<PatientVisitView> findAll() {
        return _patientVisitRepository.findAll();
    }

    public PatientVisitView findById(int id){
        return _patientVisitRepository.findById(id);
    }

    public List<PatientVisitView> findByPatientId(int id){
        return _patientVisitRepository.findByPatientId(id);
    }

    public int Create(PatientVisitForm visit){
        if (visit == null)
        {
            return 0;
        }
        var id = _patientVisitRepository.Create(visit);
        return id;
    }

    public int Update(int id, PatientVisitForm visit) {
        if (visit == null)
        {
            return 0;
        }
        id = _patientVisitRepository.Update(id, visit);
        return id;
    }

    public int Delete(int id) {
        return _patientVisitRepository.Delete(id);
    }
}