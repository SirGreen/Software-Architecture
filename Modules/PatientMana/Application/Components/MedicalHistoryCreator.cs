using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Application.Interface;
using BTL_SA.Modules.PatientMana.Domain.Models;
using BTL_SA.Modules.PatientMana.Infrastructure;

namespace BTL_SA.Modules.PatientMana.Application.Components
{
  public class MedicalHistoryCreator(IMedicalHistoryRepository medicalHistoryRepo) : IMedicalHistoryCreator
  {
    private readonly IMedicalHistoryRepository _medicalHistoryRepo = medicalHistoryRepo;
    public int CreateMedicalHistory(MedicalHistoryForm medicalHistoryForm)
    {
      if (medicalHistoryForm == null) {
        return 0;
      }
      return _medicalHistoryRepo.Create(medicalHistoryForm);
    }
  }
}