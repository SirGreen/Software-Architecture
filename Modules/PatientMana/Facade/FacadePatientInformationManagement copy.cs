using BTL_SA.Modules.PatientMana.Application;
using BTL_SA.Modules.PatientMana.Domain.Models;

namespace BTL_SA.Modules.PatientMana.Facade;

public class FacadePatientInformationManagement(
    IPatientService patientService
)
{
    private readonly IPatientService _patientService = patientService;
    public List<Patient> FindAll()
    {
        return _patientService.FindAll();
    }
    public Patient FindById(int id)
    {
        return _patientService.FindById(id);
    }
    public Patient FindByEmail(int id)
    {
        return _patientService.FindByEmail(id);
    }
    public int Create(PatientForm patient)
    {
        if (patient == null)
        {
            return 0;
        }
        var id = _patientService.Create(patient);
        return id;
    }
    public int Update(int id, PatientForm patient)
    {
        if (patient == null)
        {
            return 0;
        }
        id = _patientService.Update(id,patient);
        return id;
    }
    public int Delete(int id)    
    {
        return _patientService.Delete(id);
    }
}