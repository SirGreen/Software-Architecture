using BTL_SA.Modules.PatientMana.Application;
using BTL_SA.Modules.PatientMana.Domain.Models;

namespace BTL_SA.Modules.PatientMana.Facade;

public class FacadeVisitInformationManagement(
    IPatientVisitService visitService
)
{
    private readonly IPatientVisitService _visitService = visitService;
    public List<PatientVisitView> findAll() {
        return _visitService.findAll();
    }

    public PatientVisitView findById(int id){
        return _visitService.findById(id);
    }

    public List<PatientVisitView> findByPatientId(int id){
        return _visitService.findByPatientId(id);
    }

    public int Create(PatientVisitForm visit){
        if (visit == null)
        {
            return 0;
        }
        var id = _visitService.Create(visit);
        return id;
    }

    public int Update(int id, PatientVisitForm visit) {
        if (visit == null)
        {
            return 0;
        }
        id = _visitService.Update(id, visit);
        return id;
    }

    public int Delete(int id) {
        return _visitService.Delete(id);
    }
}