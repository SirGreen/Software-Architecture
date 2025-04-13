using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Application.Interface;
using BTL_SA.Modules.PatientMana.Domain.Models;
using BTL_SA.Modules.PatientMana.Infrastructure;

namespace BTL_SA.Modules.PatientMana.Application.Components
{
  public class MedicalHistoryRetriever(IMedicalHistoryRepository medicalHistoryRepo) : IMedicalHistoryRetriever
  {
    private readonly IMedicalHistoryRepository _medicalHistoryRepo = medicalHistoryRepo;
    public List<MedicalHistoryView> GetAllMedicalHistory()
    {
        return _medicalHistoryRepo.FindAll();
    }

    public MedicalHistoryView? GetMedicalHistoryById(int id)
    {
        if (id < 0) return null;
        return _medicalHistoryRepo.FindById(id);
    }
  }
}