using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HealthcareSystemDesign.Models
{
    public partial class healthcareContext : DbContext
    {
        public healthcareContext()
        {
        }

        public healthcareContext(DbContextOptions<healthcareContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointment { get; set; }
        public virtual DbSet<Billing> Billing { get; set; }
        public virtual DbSet<BillingType> BillingType { get; set; }
        public virtual DbSet<CardPayment> CardPayment { get; set; }
        public virtual DbSet<CashPayment> CashPayment { get; set; }
        public virtual DbSet<CheckPayment> CheckPayment { get; set; }
        public virtual DbSet<DailyReport> DailyReport { get; set; }
        public virtual DbSet<Doctor> Doctor { get; set; }
        public virtual DbSet<DoctorUnavailability> DoctorUnavailability { get; set; }
        public virtual DbSet<MedicalReport> MedicalReport { get; set; }
        public virtual DbSet<MedicalreportType> MedicalreportType { get; set; }
        public virtual DbSet<MonthlyReport> MonthlyReport { get; set; }
        public virtual DbSet<Nurse> Nurse { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<VisitRecord> VisitRecord { get; set; }

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => e.ApptId)
                    .HasName("PK__Appointm__E43EE996A79E0621");

                entity.Property(e => e.ApptId)
                    .HasColumnName("appt_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AppointmentReason)
                    .HasColumnName("appointment_reason")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ApptDate)
                    .HasColumnName("appt_date")
                    .HasColumnType("date");

                entity.Property(e => e.DoctorId).HasColumnName("doctor_id");

                entity.Property(e => e.PatientEmail)
                    .IsRequired()
                    .HasColumnName("patient_email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PatientId).HasColumnName("patient_id");

                entity.Property(e => e.VisitRecord).HasColumnName("visit_record");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Appointme__docto__30F848ED");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK__Appointme__patie__300424B4");
            });

            modelBuilder.Entity<Billing>(entity =>
            {
                entity.Property(e => e.BillingId)
                    .HasColumnName("billing_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.BillingAmount).HasColumnName("billing_amount");

                entity.Property(e => e.BillingDate)
                    .HasColumnName("billing_date")
                    .HasColumnType("date");

                entity.Property(e => e.Paid).HasColumnName("paid");

                entity.Property(e => e.PatientId).HasColumnName("patient_id");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Billing)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Billing__patient__2D27B809");
            });

            modelBuilder.Entity<BillingType>(entity =>
            {
                entity.ToTable("Billing_type");

                entity.Property(e => e.BillingtypeId)
                    .HasColumnName("billingtype_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.BillingId).HasColumnName("billing_id");

                entity.Property(e => e.BillingName)
                    .IsRequired()
                    .HasColumnName("billing_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Billing)
                    .WithMany(p => p.BillingType)
                    .HasForeignKey(d => d.BillingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Billing_t__billi__33D4B598");
            });

            modelBuilder.Entity<CardPayment>(entity =>
            {
                entity.HasKey(e => e.ReferenceNo)
                    .HasName("PK__Card_Pay__8E861397F266045F");

                entity.ToTable("Card_Payment");

                entity.Property(e => e.ReferenceNo)
                    .HasColumnName("reference_no")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.BillingId).HasColumnName("billing_id");

                entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount");

                entity.Property(e => e.PaymentDate)
                    .HasColumnName("payment_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.Billing)
                    .WithMany(p => p.CardPayment)
                    .HasForeignKey(d => d.BillingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Card_Paym__billi__46E78A0C");
            });

            modelBuilder.Entity<CashPayment>(entity =>
            {
                entity.HasKey(e => e.PaymentId)
                    .HasName("PK__Cash_Pay__ED1FC9EA9C60FDAE");

                entity.ToTable("Cash_Payment");

                entity.Property(e => e.PaymentId)
                    .HasColumnName("payment_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.BillingId).HasColumnName("billing_id");

                entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount");

                entity.Property(e => e.PaymentDate)
                    .HasColumnName("payment_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.Billing)
                    .WithMany(p => p.CashPayment)
                    .HasForeignKey(d => d.BillingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cash_Paym__billi__440B1D61");
            });

            modelBuilder.Entity<CheckPayment>(entity =>
            {
                entity.HasKey(e => e.CheckNo)
                    .HasName("PK__Check_Pa__C0EA50AF0D940E2F");

                entity.ToTable("Check_Payment");

                entity.Property(e => e.CheckNo)
                    .HasColumnName("check_no")
                    .ValueGeneratedNever();

                entity.Property(e => e.BillingId).HasColumnName("billing_id");

                entity.Property(e => e.PaymentAmount).HasColumnName("payment_amount");

                entity.Property(e => e.PaymentDate)
                    .HasColumnName("payment_date")
                    .HasColumnType("date");

                entity.HasOne(d => d.Billing)
                    .WithMany(p => p.CheckPayment)
                    .HasForeignKey(d => d.BillingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Check_Pay__billi__412EB0B6");
            });

            modelBuilder.Entity<DailyReport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("PK__daily_re__779B7C584332640F");

                entity.ToTable("daily_report");

                entity.Property(e => e.ReportId)
                    .HasColumnName("report_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DailyIncome).HasColumnName("daily_income");

                entity.Property(e => e.DoctorName)
                    .IsRequired()
                    .HasColumnName("doctor_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NoPatients).HasColumnName("no_patients");

                entity.Property(e => e.ReportDate)
                    .HasColumnName("report_date")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.Property(e => e.DoctorId)
                    .HasColumnName("doctor_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DoctorEmail)
                    .IsRequired()
                    .HasColumnName("doctor_email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DoctorName)
                    .IsRequired()
                    .HasColumnName("doctor_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Salary).HasColumnName("salary");
            });

            modelBuilder.Entity<DoctorUnavailability>(entity =>
            {
                entity.HasKey(e => e.Unavailability)
                    .HasName("PK__Doctor_u__1F48084F4F7F1A4F");

                entity.ToTable("Doctor_unavailability");

                entity.Property(e => e.Unavailability)
                    .HasColumnName("unavailability")
                    .HasColumnType("date");

                entity.Property(e => e.DoctorId).HasColumnName("doctor_id");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.DoctorUnavailability)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Doctor_un__docto__3E52440B");
            });

            modelBuilder.Entity<MedicalReport>(entity =>
            {
                entity.HasKey(e => e.MedicalreportName)
                    .HasName("PK__Medical___4756A6E81FA73CBD");

                entity.ToTable("Medical_Report");

                entity.Property(e => e.MedicalreportName)
                    .HasColumnName("medicalreport_name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MedicalreportId).HasColumnName("medicalreport_id");

                entity.Property(e => e.PatientId).HasColumnName("patient_id");

                entity.Property(e => e.ReportFile)
                    .IsRequired()
                    .HasColumnName("report_file")
                    .HasMaxLength(1);

                entity.HasOne(d => d.Medicalreport)
                    .WithMany(p => p.MedicalReport)
                    .HasForeignKey(d => d.MedicalreportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Medical_R__medic__49C3F6B7");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.MedicalReport)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Medical_R__patie__4AB81AF0");
            });

            modelBuilder.Entity<MedicalreportType>(entity =>
            {
                entity.HasKey(e => e.MedicalreportId)
                    .HasName("PK__medicalr__9B8597D501EC7947");

                entity.ToTable("medicalreport_type");

                entity.Property(e => e.MedicalreportId)
                    .HasColumnName("medicalreport_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ReportType)
                    .IsRequired()
                    .HasColumnName("report_type")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MonthlyReport>(entity =>
            {
                entity.HasKey(e => e.ReportId)
                    .HasName("PK__monthly___779B7C58824CEC36");

                entity.ToTable("monthly_report");

                entity.Property(e => e.ReportId)
                    .HasColumnName("report_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DoctorName)
                    .IsRequired()
                    .HasColumnName("doctor_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MonthlyIncome).HasColumnName("monthly_income");

                entity.Property(e => e.NoPatients).HasColumnName("no_patients");

                entity.Property(e => e.ReportMonth)
                    .HasColumnName("report_month")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Nurse>(entity =>
            {
                entity.Property(e => e.NurseId)
                    .HasColumnName("nurse_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DoctorId).HasColumnName("doctor_id");

                entity.Property(e => e.NurseEmail)
                    .IsRequired()
                    .HasColumnName("nurse_email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NurseName)
                    .IsRequired()
                    .HasColumnName("nurse_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Salary).HasColumnName("salary");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.Nurse)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Nurse__doctor_id__267ABA7A");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.PatientId)
                    .HasColumnName("patient_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Allergy)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Bloodpressure)
                    .HasColumnName("bloodpressure")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Height)
                    .HasColumnName("height")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Insurance)
                    .IsRequired()
                    .HasColumnName("insurance")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PatientEmail)
                    .HasColumnName("patient_email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PatientName)
                    .IsRequired()
                    .HasColumnName("patient_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Pulse).HasColumnName("pulse");

                entity.Property(e => e.Ssn)
                    .HasColumnName("ssn")
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.Weight).HasColumnName("weight");
            });

            modelBuilder.Entity<VisitRecord>(entity =>
            {
                entity.HasKey(e => e.PatientId)
                    .HasName("PK__Visit_Re__4D5CE4761E964A42");

                entity.ToTable("Visit_Record");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patient_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DoctorId).HasColumnName("doctor_id");

                entity.Property(e => e.Prescription)
                    .IsRequired()
                    .HasColumnName("prescription")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.VisitDate)
                    .HasColumnName("visit_date")
                    .HasColumnType("date");

                entity.Property(e => e.VisitReason)
                    .IsRequired()
                    .HasColumnName("visit_reason")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Visited).HasColumnName("visited");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.VisitRecord)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Visit_Rec__docto__3B75D760");

                entity.HasOne(d => d.Patient)
                    .WithOne(p => p.VisitRecord)
                    .HasForeignKey<VisitRecord>(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Visit_Rec__patie__3A81B327");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
