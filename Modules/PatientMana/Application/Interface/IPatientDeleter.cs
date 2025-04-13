using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BTL_SA.Modules.PatientMana.Application.Interface
{
    public interface IPatientDeleter
    {
        int DeletePatient(int id);
    }
}