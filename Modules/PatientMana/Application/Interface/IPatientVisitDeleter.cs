using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_SA.Modules.PatientMana.Application.Interface
{
    public interface IPatientVisitDeleter
    {
        int DeletePatientVisit(int id);
    }
}