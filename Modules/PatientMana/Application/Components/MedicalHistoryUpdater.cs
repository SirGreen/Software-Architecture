using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Application.Interface;
using BTL_SA.Modules.PatientMana.Domain.Models;
using BTL_SA.Modules.PatientMana.Infrastructure;

namespace BTL_SA.Modules.PatientMana.Application.Components
{
  public class MedicalHistoryUpdater(IMedicalHistoryRepository medicalHistoryRepo) : IMedicalHistoryUpdater
  {
    private readonly IMedicalHistoryRepository _medicalHistoryRepo = medicalHistoryRepo;
    public int UpdateMedicalHistory(int id, MedicalHistoryForm medicalHistoryForm)
    {
      if (id < 0 || medicalHistoryForm == null) return 0;
      return _medicalHistoryRepo.Update(id, medicalHistoryForm);
    }
  }
}