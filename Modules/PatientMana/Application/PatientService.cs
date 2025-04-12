using BTL_SA.Modules.PatientMana.Domain.Models;
using BTL_SA.Modules.PatientMana.Infrastructure;

namespace BTL_SA.Modules.PatientMana.Application;
public class PatientService(IPatientRepository patientRepository) : IPatientService
{
    private readonly IPatientRepository _patientRepository = patientRepository;


    public List<Patient> FindAll()
    {
        return _patientRepository.FindAll();
    }
    public Patient FindById(int id)
    {
        return _patientRepository.FindById(id);
    }
    public Patient FindByEmail(int id)
    {
        return _patientRepository.FindByEmail(id);
    }
    public int Create(PatientForm patient)
    {
        if (patient == null)
        {
            return 0;
        }
        var id = _patientRepository.Create(patient);
        return id;
    }
    public int Update(int id, PatientForm patient)
    {
        if (patient == null)
        {
            return 0;
        }
        id = _patientRepository.Update(id,patient);
        return id;
    }
    public int Delete(int id)    
    {
        return _patientRepository.Delete(id);
    }



}
