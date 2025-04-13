using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Application.Interface;
using BTL_SA.Modules.PatientMana.Domain.Models;
using BTL_SA.Modules.PatientMana.Infrastructure;

namespace BTL_SA.Modules.PatientMana.Application.Components
{
  public class PatientVisitCreator(IPatientVisitRepository patientVisitRepo) : IPatientVisitCreator
  {
    private readonly IPatientVisitRepository _patientVisitRepo = patientVisitRepo;
    public int CreatePatientVisit(PatientVisitForm patientVisitForm)
    {
      if (patientVisitForm == null) {
        return 0;
      }
      return _patientVisitRepo.Create(patientVisitForm);
    }
  }
}