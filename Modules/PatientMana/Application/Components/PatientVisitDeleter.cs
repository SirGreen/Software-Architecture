using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Application.Interface;
using BTL_SA.Modules.PatientMana.Infrastructure;

namespace BTL_SA.Modules.PatientMana.Application.Components
{
  public class PatientVisitDeleter(IPatientVisitRepository patientVisitRepo) : IPatientVisitDeleter
  {
    private readonly IPatientVisitRepository _patientVisitRepo = patientVisitRepo;
    public int DeletePatientVisit(int id)
    {
      if (id < 0) return 0;
      return _patientVisitRepo.Delete(id);
    }
  }
}