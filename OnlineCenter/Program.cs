using DataAccess;
using DataAccess.Repository;
using DataAccess.Reposetory.IReposetory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Models;
using Utilities;
using DataAccess.Reposetory;
using DataAccsess.Reposetory.IReposetory;
using Stripe;
using Microsoft.AspNetCore.Http.Features;

namespace OnlineCenter
{
    namespace OnlineCenter
    {
        public class Program
        {
            public static void Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddControllersWithViews();
                builder.Services.AddRazorPages();

                // Configure DbContext with SQL Server connection
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("Defaultconnection")));

                // Configure Identity with custom ApplicationUser and email confirmation requirement
                builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false; // Require confirmed email before login
                })
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

                // Register repositories as scoped services
                builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
                builder.Services.AddScoped<IStudentRepository, StudentRepository>();
                builder.Services.AddScoped<IAssistantRepository, AssistantRepository>();
                builder.Services.AddScoped<ISubjectRepsitory, SubjectRepository>();
                builder.Services.AddScoped<ILectureRepository, LectureRepository>();
                builder.Services.AddScoped<IGradeRepository, GradeRepository>();
                builder.Services.AddScoped<ICourseRepository, CourseRepository>();
                builder.Services.AddScoped<ICartRepository, CartRepository>();
                builder.Services.AddScoped<IBookRepository, BookRepository>();
                builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
                builder.Services.AddScoped<ITeacherCoursesRepository, TeacherCoursesRepository>();
                builder.Services.AddScoped<IGradeSubjectRepository, GradeSubjectRepository>();
                builder.Services.AddScoped<IAssestInReposetory,AssistantInRepository>();
                builder.Services.AddScoped<IStudentLectureRepository, StudentLectureRepository>();
                builder.Services.AddScoped<ILectureAnswerRepository, LectureAnswerRepository>();
                builder.Services.AddScoped<ITeacherBooksRepository, TeacherBooksRepository>();
                builder.Services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();




                builder.Services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
                builder.Services.AddScoped<IBookPaymentRepository, BookPaymentRepository>();


                builder.Services.AddScoped<IStripeService, StripeService>();

                // Add email sender service (used for confirmation emails)
                builder.Services.AddTransient<IEmailSender, EmailSender>();

                builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
                StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
                //builder.Services.Configure<FormOptions>(options =>
                //{
                //    options.MultipartBodyLengthLimit = 104857600; // 100 MB
                //});

                //builder.Services.Configure<IISServerOptions>(options =>
                //{
                //    options.MaxRequestBodySize = 524288000; // 500 MB
                //});


                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = 500 * 1024 * 1024; // 500 MB
                });

                // Add services and configuration
                builder.Services.Configure<FormOptions>(options =>
                {
                    options.MultipartBodyLengthLimit = 500 * 1024 * 1024; // 500 MB
                });

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();
                app.UseAuthorization();

                app.MapRazorPages();
                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                app.Run();
            }
        }
    }
}
    
