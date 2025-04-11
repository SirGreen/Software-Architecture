using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Domain.Models;

namespace BTL_SA.Modules.PatientMana.Infrastructure
{
    public interface IPatientVisitRepository
    {
        List<PatientVisitView> findAll();
        PatientVisitView findById(int id);
        List<PatientVisitView> findByPatientId(int id);
        int Create(PatientVisitForm patient);
        int Update(int id, PatientVisitForm patient);
        int Delete(int id);
    }
}