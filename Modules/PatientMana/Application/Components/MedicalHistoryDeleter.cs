using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Application.Interface;
using BTL_SA.Modules.PatientMana.Infrastructure;

namespace BTL_SA.Modules.PatientMana.Application.Components
{
  public class MedicalHistoryDeleter(IMedicalHistoryRepository medicalHistoryRepo) : IMedicalHistoryDeleter
  {
    private readonly IMedicalHistoryRepository _medicalHistoryRepo = medicalHistoryRepo;
    public int DeleteMedicalHistory(int id)
    {
      if (id < 0) return 0;
      return _medicalHistoryRepo.Delete(id);
    }
  }
}