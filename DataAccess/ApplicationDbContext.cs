//using DataAccess.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Models;
using Models.ViewData;

namespace DataAccess
{
    public class ApplicationDbContext :IdentityDbContext<IdentityUser>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Assistant> Assistants { get; set; }
        public DbSet<AssistIn> AssistIns { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<GradeSubject> GradeSubjects { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<LectureAnswer> LectureAnswers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentLecture> StudentLectures { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherBook> TeacherBooks { get; set; }
        public DbSet<TeacherCourse> TeacherCourses { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<ApplicationUserVm> applicationUserVms { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<BookPayment> Bookpayments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AssistIn>()
                .Property(ai => ai.AssistantId)
                .HasMaxLength(450); // Smaller length
            modelBuilder.Entity<AssistIn>()
                .Property(ai => ai.TeacherId)
                .HasMaxLength(450);
            modelBuilder.Entity<AssistIn>()
                .Property(ai => ai.CourseId)
                .HasMaxLength(50);

            // Composite key for AssistIn
            modelBuilder.Entity<AssistIn>()
                .HasKey(ai => new { ai.AssistantId, ai.TeacherId, ai.CourseId });

            modelBuilder.Entity<Cart>()
               .HasKey(c => new { c.StudentId, c.BookId });

            // Composite key for StudentLecture
            modelBuilder.Entity<StudentLecture>()
                .HasKey(sl => new { sl.StudentId, sl.LectureId });

            // Composite key for TeacherCourse
            modelBuilder.Entity<TeacherCourse>()
                .HasKey(tc => new { tc.TeacherId, tc.CourseId });

            // Composite key for GradeSubject
            modelBuilder.Entity<GradeSubject>()
                .HasKey(gs => new { gs.GradeId, gs.SubjectId });

            // Composite key for LectureAnswer
            modelBuilder.Entity<LectureAnswer>()
                .HasKey(la => new { la.StudentId, la.AnswerId, la.LectureId });

            // Composite key for Teacherbook
            modelBuilder.Entity<TeacherBook>()
                .HasKey(tb => new { tb.TeacherId, tb.BookId, tb.CourseId });

            //Configure any relationships or cascade delete behavior as needed
            modelBuilder.Entity<AssistIn>()
                .HasOne(ai => ai.Teacher)
                .WithMany(t => t.AssistIns)
                .HasForeignKey(ai => ai.TeacherId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading deletes

            modelBuilder.Entity<AssistIn>()
                .HasOne(ai => ai.Course)
                .WithMany(c => c.AssistIns)
                .HasForeignKey(ai => ai.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AssistIn>()
                .HasOne(ai => ai.Assistant)
                .WithMany(a => a.AssistIns)
                .HasForeignKey(ai => ai.AssistantId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Book)
                .WithMany(b => b.Carts)
                .HasForeignKey(c => c.BookId);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Student)
                .WithMany(s => s.Carts)
                .HasForeignKey(c => c.StudentId);

            modelBuilder.Entity<GradeSubject>()
                .HasOne(gs => gs.Grade)
                .WithMany(g => g.GradeSubjects)
                .HasForeignKey(gs => gs.GradeId);

            modelBuilder.Entity<GradeSubject>()
                .HasOne(gs => gs.Subject)
                .WithMany(s => s.GradeSubjects)
                .HasForeignKey(gs => gs.SubjectId);

            modelBuilder.Entity<LectureAnswer>()
                .HasOne(la => la.Student)  // One LectureAnswer has one Student
                .WithOne(s => s.LectureAnswer)  // One Student has one LectureAnswer
                .HasForeignKey<LectureAnswer>(la => la.StudentId);


            // One-to-one relationship between LectureAnswer and Answer
            modelBuilder.Entity<LectureAnswer>()
                .HasOne(la => la.Answer)  // One LectureAnswer has one Answer
                .WithOne(a => a.lectureAnswer)  // One Answer has one LectureAnswer
                .HasForeignKey<LectureAnswer>(la => la.AnswerId);

            // One-to-one relationship between LectureAnswer and Lecture
            modelBuilder.Entity<LectureAnswer>()
                .HasOne(la => la.Lecture)  // One LectureAnswer has one Lecture
                .WithOne(l => l.LectureAnswer)  // One Lecture has one LectureAnswer
                .HasForeignKey<LectureAnswer>(la => la.LectureId);


            modelBuilder.Entity<StudentLecture>()
                .HasOne(sl => sl.Student)
                .WithMany(s => s.StudentLectures)
                .HasForeignKey(sl => sl.StudentId);

            modelBuilder.Entity<StudentLecture>()
                .HasOne(sl => sl.Lecture)
                .WithMany(l => l.StudentLectures)
                .HasForeignKey(sl => sl.LectureId);


            modelBuilder.Entity<TeacherBook>()
                .HasOne(tb => tb.Teacher)
                .WithMany(t => t.Teacherbooks)
                .HasForeignKey(tb => tb.TeacherId);

            modelBuilder.Entity<TeacherBook>()
                .HasOne(tb => tb.Book)
                .WithOne(b => b.Teacherbook)
                .HasForeignKey<TeacherBook>(tb => tb.BookId);


            modelBuilder.Entity<TeacherBook>()
                .HasOne(tb => tb.Course)
                .WithMany(c => c.Teacherbooks)
                .HasForeignKey(tb => tb.CourseId);


            modelBuilder.Entity<TeacherCourse>()
                .HasOne(tc => tc.Teacher)
                .WithMany(t => t.TeacherCourses)
                .HasForeignKey(tc => tc.TeacherId);


            modelBuilder.Entity<TeacherCourse>()
                .HasOne(tc => tc.Course)
                .WithMany(c => c.TeacherCourses)
                .HasForeignKey(tc => tc.CourseId);
               

            modelBuilder.Entity<Admin>()
            .HasKey(c => new { c.ApplicationuserId });


            modelBuilder.Entity<Teacher>()
           .HasKey(c => new { c.ApplicationUserId });

            modelBuilder.Entity<BookPayment>()
          .HasKey(c => new {c.Id});


            modelBuilder.Entity<Student>()
           .HasKey(c => new { c.ApplicationUserId });

            modelBuilder.Entity<Assistant>()
           .HasKey(c => new { c.ApplicationUserId });

            modelBuilder.Entity<Grade>()
                .HasMany(e => e.Students)
                .WithOne(p => p.Grade);

            modelBuilder.Entity<Grade>()
            .HasMany(e => e.GradeSubjects)
            .WithOne(p => p.Grade);

            modelBuilder.Entity<Course>()
         .HasMany(e => e.TeacherCourses)
         .WithOne(p => p.Course);


            modelBuilder.Entity<Course>()
         .HasMany(e => e.Teacherbooks)
         .WithOne(p => p.Course);


            modelBuilder.Entity<Course>()
         .HasMany(e => e.AssistIns)
         .WithOne(p => p.Course);
             
            modelBuilder.Entity<Lecture>()
        .HasMany(e => e.StudentLectures)
        .WithOne(p => p.Lecture);



        }


    }
}
