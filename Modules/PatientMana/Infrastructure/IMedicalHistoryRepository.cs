using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Domain.Models;

namespace BTL_SA.Modules.PatientMana.Infrastructure
{
    public interface IMedicalHistoryRepository
    {
        List<MedicalHistoryView> FindAll(); // []
        MedicalHistoryView? FindById(int id);
        List<MedicalHistoryView>? FindByPatientId(int id);
        int Create(MedicalHistoryForm medicalHistory);
        int Update(int id, MedicalHistoryForm medicalHistory);
        int Delete(int id);
    }
}