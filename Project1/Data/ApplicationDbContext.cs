using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Project1.Models;
using Project1.Models.Entitys;

namespace Project1.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<Person>
    {
        public DbSet<Group>? Groups { get; set; }
        public DbSet<Institute>? Institutes { get; set; }
        public DbSet<Person>? Persons { get; set; }
        public DbSet<Student>? Students { get; set; }
        public DbSet<Subgroup>? Subgroups { get; set; }
        public DbSet<Teacher>? Teachers { get; set; }
        public DbSet<TeacherGroup>? TeacherGroups { get; set; }
        public DbSet<NumPara> NumParas { get; set; }
        public DbSet<TypePara> TypeParas { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Groups ↔ Institutes
            modelBuilder.Entity<Group>()
                .HasOne(g => g.Institute)
                .WithMany(i => i.Groups)
                .HasForeignKey(g => g.InstituteId);

            // Groups ↔ Subgroups
            modelBuilder.Entity<Subgroup>()
                .HasOne(sg => sg.Group)
                .WithMany(g => g.Subgroups)
                .HasForeignKey(sg => sg.GroupId);

            // Groups ↔ Students
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId);

            // Students ↔ Person
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Person)
                .WithOne(p => p.Student)
                .HasForeignKey<Student>(s => s.PersonId);

            // Teachers ↔ Person
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.Person)
                .WithOne(p => p.Teacher)
                .HasForeignKey<Teacher>(t => t.PersonId);

            // Teachers ↔ Groups через TeacherGroups
            modelBuilder.Entity<TeacherGroup>()
                .HasKey(tg => new { tg.TeacherId, tg.GroupId });

            modelBuilder.Entity<TeacherGroup>()
                .HasOne(tg => tg.Teacher)
                .WithMany(t => t.TeacherGroups)
                .HasForeignKey(tg => tg.TeacherId);

            modelBuilder.Entity<TeacherGroup>()
                .HasOne(tg => tg.Group)
                .WithMany(g => g.TeacherGroups)
                .HasForeignKey(tg => tg.GroupId);

            // NumPara - Schedule (1 to many)
            modelBuilder.Entity<NumPara>()
                .HasMany(n => n.Schedules)
                .WithOne(s => s.NumPara)
                .HasForeignKey(s => s.NumParaId);

            // TypePara - Schedule (1 to many)
            modelBuilder.Entity<TypePara>()
                .HasMany(t => t.Schedules)
                .WithOne(s => s.TypePara)
                .HasForeignKey(s => s.TypeParaId);

            // Subject - Mark (1 to many)
            modelBuilder.Entity<Subject>()
                .HasMany(s => s.Marks)
                .WithOne(m => m.Subject)
                .HasForeignKey(m => m.SubjectId);

            // Subject - Schedule (1 to many)
            modelBuilder.Entity<Subject>()
                .HasMany(s => s.Schedules)
                .WithOne(sch => sch.Subject)
                .HasForeignKey(sch => sch.SubjectId);


            // Subject - Marks (No Cascade Delete)
            modelBuilder.Entity<Mark>()
                .HasOne(m => m.Subject)
                .WithMany(s => s.Marks)
                .HasForeignKey(m => m.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Subject - Schedule (Cascade Delete)
            modelBuilder.Entity<Schedule>()
                .HasOne(sch => sch.Subject)
                .WithMany(sub => sub.Schedules)
                .HasForeignKey(sch => sch.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Налаштування зв'язку Subgroup ↔ Schedule
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Subgroup) // Schedule має одну Subgroup
                .WithMany(sg => sg.Schedules) // Subgroup має багато Schedule
                .HasForeignKey(s => s.SubgroupId) // Зовнішній ключ у Schedule
                .OnDelete(DeleteBehavior.Cascade); // Каскадне видалення

            // Teacher ↔ Subject (1 to many)
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Teacher) // Кожен Subject має одного Teacher
                .WithMany(t => t.Subjects) // Кожен Teacher може мати багато Subject
                .HasForeignKey(s => s.TeacherId); // Встановлюємо зовнішній ключ в Subject
        }

        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {

        }
    }
}