using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using BTL_SA.Modules.StaffMana.Domain.Model;
using Microsoft.Identity.Client;

namespace BTL_SA.Modules.PatientMana.Domain.Models
{
    public class Patient : Person
    {
        public int Id { get; set; }
        public string? HealthInsuranceId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<PatientVisit> PatientVisit { get; set; }
    }

    public class PatientViewModel : Person {
        public int Id { get; set; }
        public string? HealthInsuranceId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public new string Gender { get; set; }

        public PatientViewModel(Patient patient)
        {
            Id = patient.Id;
            HealthInsuranceId = patient.HealthInsuranceId;
            CreatedDate = patient.CreatedDate;
            ModifiedDate = patient.ModifiedDate;
            Name = patient.Name;
            Gender = patient.Gender ? "Male" : "Female";
            PhoneNumber = patient.PhoneNumber;
            Address = patient.Address;
            DateOfBirth = patient.DateOfBirth;
            Email = patient.Email;
        }
    }

    public class PatientForm : Person {
        public int Id { get; set; }  // Thêm property Id
        public string? HealthInsuranceId { get; set; }

        [System.Text.Json.Serialization.JsonConstructor]
        public PatientForm(int id, string name, bool gender, string phoneNumber, string address, DateTime dateOfBirth, string email, string healthInsuranceId)
        {
            Id = id;
            Name = name;
            Gender = gender;
            PhoneNumber = phoneNumber;
            Address = address;
            DateOfBirth = dateOfBirth;
            Email = email;
            HealthInsuranceId = healthInsuranceId;
        }

        public PatientForm(Patient patient)
        {
            Id = patient.Id;
            Name = patient.Name;
            Gender = patient.Gender;
            PhoneNumber = patient.PhoneNumber;
            Address = patient.Address;
            DateOfBirth = patient.DateOfBirth;
            Email = patient.Email;
            HealthInsuranceId = patient.HealthInsuranceId ?? string.Empty;
        }
    }
}