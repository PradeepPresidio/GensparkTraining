namespace FirstAPI.Contexts
{
    public class ClinicContext : DbContext
    {

        public ClinicContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointmnet> Appointmnets { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<DoctorSpeciality> DoctorSpecialities { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserId);

            modelBuilder.Entity<User>().HasOne(u => u.UserFollower)
                                        .WithMany(us => us.Followers)
                                        .HasForeignKey(u => u.FollwerId)
                                        .HasConstraintName("FK_Followers")
                                        .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Doctor>.HasKey(doc=>doc.Id);
            modelBuilder.Entity<Appointmnet>().HasKey(app => app.AppointmnetNumber).HasName("PK_AppointmentNumber");

            modelBuilder.Entity<Appointmnet>().HasOne(app => app.Patient)
                                              .WithMany(p => p.Appointmnets)
                                              .HasForeignKey(app => app.PatientId)
                                              .HasConstraintName("FK_Appoinment_Patient")
                                              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointmnet>().HasOne(app => app.Doctor)
                                              .WithMany(d => d.Appointmnets)
                                              .HasForeignKey(app => app.DoctorId)
                                              .HasConstraintName("FK_Appoinment_Doctor")
                                              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DoctorSpeciality>().HasKey(ds => ds.SerialNumber);

            modelBuilder.Entity<DoctorSpeciality>().HasOne(ds => ds.Doctor)
                                                   .WithMany(d => d.DoctorSpecialities)
                                                   .HasForeignKey(ds => ds.DoctorId)
                                                   .HasConstraintName("FK_Speciality_Doctor")
                                                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DoctorSpeciality>().HasOne(ds => ds.Speciality)
                                                   .WithMany(s => s.DoctorSpecialities)
                                                   .HasForeignKey(ds => ds.SpecialityId)
                                                   .HasConstraintName("FK_Speciality_Spec")
                                                   .OnDelete(DeleteBehavior.Restrict);

        }

    }
}