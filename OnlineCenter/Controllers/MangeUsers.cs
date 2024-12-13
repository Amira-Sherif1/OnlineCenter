using DataAccess;
using DataAccess.Reposetory.IReposetory;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Models;
using Models.ViewData;
using System.Data;
using Utilities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnlineCenter.Controllers
{
    [Authorize(Roles = "Admin , Teacher")]
    public class MangeUsers : Controller
    {

        public ApplicationDbContext dbcontext { get; set; }
        public ITeacherRepository teacherRepository { get; set; }
        public IAssistantRepository AssistantRepository { get; set; }
        public IStudentRepository StudentRepository { get; set; }


        public UserManager<IdentityUser> UserManager { get; set; }
        public SignInManager<IdentityUser> signInManager;
        public RoleManager<IdentityRole> roleManager;
        public IGradeRepository gradeRepository { get; set; }

        public MangeUsers(ApplicationDbContext dbcontext, IGradeRepository gradeRepository, ITeacherRepository teacherRepository, IAssistantRepository AssistantRepository, IStudentRepository StudentRepository, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> rolemanager)
        {
            UserManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager; 
            this.teacherRepository = teacherRepository;
            this.StudentRepository = StudentRepository;
            this.AssistantRepository = AssistantRepository;
            this.dbcontext = dbcontext;
            this.gradeRepository = gradeRepository;
        }

        [HttpGet]
        public IActionResult AddUser(int number)
        {
            ViewBag.Number=number;
            var grades = gradeRepository.GetAll().ToList();
            ViewBag.Grades = new SelectList(grades, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(ApplicationUserVm userVM , int number )
        {
            if (ModelState.IsValid)
            {
                //string filename="";
                //if (Image != null && Image.Length > 0)
                //{
                //    filename = Guid.NewGuid() + Path.GetExtension(Image.FileName);
                //    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", filename);

                //    using (var stream = System.IO.File.Create(filepath))
                //    {
                //        await Image.CopyToAsync(stream);
                //    }
                //}
                ApplicationUser application = new()
                {
                    UserName = userVM.Name,
                    Email = userVM.Email,
                    Address = userVM.Address,
                    PhoneNumber = userVM.Phone,
                    //Image = filename
                };

                await UserManager.SetUserNameAsync(application, application.Email);

                var result = await UserManager.CreateAsync(application, userVM.Password);
                if (result.Succeeded)
                {
                    if (number == 1)  // teahcer 
                    {
                        if (result.Succeeded)
                        {

                            Teacher teacher = new()
                            {
                                ApplicationUserId = application.Id,
                                Degree = userVM.Degree
                            }
                            ;

                            teacherRepository.Add(teacher);
                            teacherRepository.Save();
                            var roleResult = await UserManager.AddToRoleAsync(application, "Teacher");
                            return RedirectToAction("AllTeachers", "Teacher"); // will redirect to the page 

                        }
                        else
                        {
                            return RedirectToAction("AddUser", new { application  , number=1}); // will redirect to the page 

                        }

                    }
                    else if (number == 2) // student
                    {
                        if (result.Succeeded) 
                        {
                            var gradeId = Request.Form["GradeId"];
                            int.TryParse(gradeId, out int parsedGradeId);
                            Student student = new()
                            {
                                ApplicationUserId = application.Id,
                                GradeId = parsedGradeId
                            }
                            ;

                            StudentRepository.Add(student);
                            StudentRepository.Save();
                            var roleResult = await UserManager.AddToRoleAsync(application, "Student");

                            return RedirectToAction("AllStudents", "Student"); // will redirect to the page that has table of students 

                        }
                        else
                        {
                            return RedirectToAction("AddUser", new { application, number = 2 }); // will redirect to the page 


                        }

                    }
                    else if (number == 3) // assistant
                    {
                        if (result.Succeeded)
                        {

                            Assistant assistant = new()
                            {
                                ApplicationUserId = application.Id,
                                Degree = userVM.Degree
                            }
                            ;
                            AssistantRepository.Add(assistant);
                            AssistantRepository.Save();
                            var roleResult = await UserManager.AddToRoleAsync(application, "Assistant");

                            return RedirectToAction("AllAssistants", "Assistant"); // will redirect to the page that has table of assistants 
                        }
                        else
                        {
                            return RedirectToAction("AddUser", new { application, number = 3}); // will redirect to the page 


                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("Password", "Invalid password");

                }

            }
            return RedirectToAction (nameof(AddUser)); // will be changed
        }

        public IActionResult EditUser(int number, string Id)
        {
            object user = null; // Declare user variable outside the if block to make it accessible after the conditions

            // Check which number and fetch user accordingly
            if (number == 1)
            {
                user = teacherRepository.GetOne(expresion: e => e.ApplicationUserId == Id);
            }
            else if (number == 2)
            {
                user = StudentRepository.GetOne(expresion: e => e.ApplicationUserId == Id);
            }
            else if (number == 3)
            {
                user = AssistantRepository.GetOne(expresion: e => e.ApplicationUserId == Id);
            }

            
            // Pass the number to the view to keep track of which type of user is being edited
            ViewBag.Number = number;

            // Fetch grades to pass to the view
            var grades = gradeRepository.GetAll().ToList();
            ViewBag.Grades = new SelectList(grades, "Id", "Name");

            // Return the correct user object to the view
            return View(user);
        }

        //public async Task<IActionResult> EditUser(ApplicationUserVm userVM, int number)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        object user = null;
        //        //string filename="";
        //        //if (Image != null && Image.Length > 0)
        //        //{
        //        //    filename = Guid.NewGuid() + Path.GetExtension(Image.FileName);
        //        //    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", filename);

        //        //    using (var stream = System.IO.File.Create(filepath))
        //        //    {
        //        //        await Image.CopyToAsync(stream);
        //        //    }
        //        //}
        //        if (number == 1)
        //        {
        //            user = teacherRepository.GetOne(expresion: e => e.ApplicationUserId == Id);
        //        }
        //        else if (number == 2)
        //        {
        //            user = StudentRepository.GetOne(expresion: e => e.ApplicationUserId == Id);
        //        }
        //        else if (number == 3)
        //        {
        //            user = AssistantRepository.GetOne(expresion: e => e.ApplicationUserId == Id);
        //        }

        //        ApplicationUser application = new()
        //        {
        //            UserName = userVM.Name,
        //            Email = userVM.Email,
        //            Address = userVM.Address,
        //            PhoneNumber = userVM.Phone,
        //            //Image = filename
        //        };
        //        var result = await UserManager.CreateAsync(application, userVM.Password);
        //        if (result.Succeeded)
        //        {
        //            if (number == 1)  // teahcer 
        //            {
        //                await UserManager.AddToRoleAsync(application, Users.Teacher);

        //                Teacher teacher = new()
        //                {
        //                    ApplicationUserId = application.Id,
        //                    Degree = userVM.Degree
        //                }
        //                ;

        //                teacherRepository.Add(teacher);
        //                teacherRepository.Save();
        //                return RedirectToAction("AllTeachers", "Teacher"); // will redirect to the page that has table of tachers 
        //            }
        //            else if (number == 2) // student
        //            {
        //                await UserManager.AddToRoleAsync(application, Users.Student);
        //                var gradeId = Request.Form["GradeId"];
        //                int.TryParse(gradeId, out int parsedGradeId);
        //                Student student = new()
        //                {
        //                    ApplicationUserId = application.Id,
        //                    GradeId = parsedGradeId
        //                }
        //                ;

        //                StudentRepository.Add(student);
        //                StudentRepository.Save();
        //                return RedirectToAction("AllStudents", "Student"); // will redirect to the page that has table of students 

        //            }
        //            else if (number == 3) // assistant
        //            {
        //                await UserManager.AddToRoleAsync(application, Users.Assistant);

        //                Assistant assistant = new()
        //                {
        //                    ApplicationUserId = application.Id,
        //                    Degree = userVM.Degree
        //                }
        //                ;
        //                AssistantRepository.Add(assistant);
        //                AssistantRepository.Save();
        //                return RedirectToAction("AllAssistants", "Assistant"); // will redirect to the page that has table of assistants 

        //            }

        //        }
        //        else
        //        {
        //            ModelState.AddModelError("Password", "Invalid password");

        //        }

        //    }
        //    return RedirectToAction(nameof(AddUser)); // will be changed
        //}

        
        public IActionResult Delete(string ApplicationUserId, int number)
        {
            object user = null; // Declare user variable outside the if block to make it accessible after the conditions

            // Check which number and fetch user accordingly
            if (number == 1)
            {
                user = teacherRepository.GetOne(expresion: e => e.ApplicationUserId == ApplicationUserId);
                teacherRepository.Delete((Teacher)user);
                teacherRepository.Save();
                return RedirectToAction("AllTeachers", "Teacher"); // will redirect to the page that has table of tachers 

            }
            else if (number == 2)
            {
                user = StudentRepository.GetOne(expresion: e => e.ApplicationUserId == ApplicationUserId);
                StudentRepository.Delete((Student)user);
                StudentRepository.Save();
                return RedirectToAction("AllStudents", "Student"); // will redirect to the page that has table of tachers 

            }
            else if(number == 3) 
            {
                user = AssistantRepository.GetOne(expresion: e => e.ApplicationUserId == ApplicationUserId);
                AssistantRepository.Delete((Assistant)user);
                AssistantRepository.Save();
                return RedirectToAction("AllAssistants", "Assistant"); // will redirect to the page that has table of tachers 

            }
            return RedirectToAction("Error", "Home");
        }
    }
}
