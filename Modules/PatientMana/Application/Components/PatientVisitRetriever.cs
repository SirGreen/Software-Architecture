using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Application.Interface;
using BTL_SA.Modules.PatientMana.Domain.Models;
using BTL_SA.Modules.PatientMana.Infrastructure;

namespace BTL_SA.Modules.PatientMana.Application.Components
{
  public class PatientVisitRetriever(IPatientVisitRepository patientRepo) : IPatientVisitRetriever
  {
    private readonly IPatientVisitRepository _patientRepo = patientRepo;
    public List<PatientVisitView>? GetAllVisit()
    {
      return _patientRepo.FindAll();
    }

    public PatientVisitView? GetVisitById(int id)
    {
      if (id < 0) return null;
      return _patientRepo.FindById(id);
    }

    public List<PatientVisitView>? GetVisitByPatientId(int id)
    {
      if (id < 0) return null;
      return _patientRepo.FindByPatientId(id);
    }
  }
}