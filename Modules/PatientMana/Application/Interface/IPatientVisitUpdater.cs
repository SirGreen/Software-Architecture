using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTL_SA.Modules.PatientMana.Domain.Models;

namespace BTL_SA.Modules.PatientMana.Application.Interface
{
    public interface IPatientVisitUpdater
    {
        int UpdatePatientVisit(int id, PatientVisitForm patientVisit);
    }
}