using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Application.Interface;
using BTL_SA.Modules.PatientMana.Domain.Models;
using BTL_SA.Modules.PatientMana.Infrastructure;

namespace BTL_SA.Modules.PatientMana.Application.Components
{
  public class PatientVisitUpdater(IPatientVisitRepository patientVisitRepo) : IPatientVisitUpdater
  {
    private readonly IPatientVisitRepository _patientVisitRepo = patientVisitRepo;
    public int UpdatePatientVisit(int id, PatientVisitForm patientVisit)
    {
        if (id < 0 || patientVisit == null) return 0;
        return _patientVisitRepo.Update(id, patientVisit);
    }
  }
}