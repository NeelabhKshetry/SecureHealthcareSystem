using System;
using System.Collections.Generic;

namespace HealthcareSystemDesign.Models
{
    public partial class Patient
    {
        public Patient()
        {
            Appointment = new HashSet<Appointment>();
            Billing = new HashSet<Billing>();
            MedicalReport = new HashSet<MedicalReport>();
        }

        public string PatientName { get; set; }
        public string PatientEmail { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Ssn { get; set; }
        public int? Weight { get; set; }
        public string Height { get; set; }
        public string Insurance { get; set; }
        public string Bloodpressure { get; set; }
        public int? Pulse { get; set; }
        public int PatientId { get; set; }
        public string Allergy { get; set; }

        public virtual VisitRecord VisitRecord { get; set; }
        public virtual ICollection<Appointment> Appointment { get; set; }
        public virtual ICollection<Billing> Billing { get; set; }
        public virtual ICollection<MedicalReport> MedicalReport { get; set; }
    }
}
