using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MarlonApi.Models;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Cors;

using MarlonApp.DatabaseInteraction;


namespace MarlonApi.Controllers
{
    //[Route("api/[[todo]]")]
    /// <summary>
    /// Controller creation
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/student")]
    public class UserController : Controller
    {
        private readonly TodoContext _context;

        /// <summary>
        /// creates the first student object
        /// </summary>
        public UserController(TodoContext context){

            _context = context;

            //if(_context.TodoItems.Count()==0){
            //}

        }

        /// <summary>
        /// Creates a student by passing student information in the parameters.
        /// </summary>
        /// <param name="Name"></param> 
        /// <param name="PhoneNumber"></param>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <param name="UserType"></param>
        /// <param name="Resume"></param>
        [HttpPost("{Name}")]
    public IActionResult CreateNewStudent(   string Name, 
                                             string PhoneNumber, 
                                             string Email, 
                                             string Password,
                                             string UserType,
                                             string[] Resume)
    {
            //Console.WriteLine("Before building student");
            string name         = Name;
            string phoneNumber  = PhoneNumber;
            string email        = Email;
            string password     = Password;
            string userType     = UserType;
            string[] res        = Resume; // new string[] { "ect" };// Keywords; 
            Console.WriteLine("Before building student");
            var student =  new TodoStudent
            {
                  Name          = name,
                  PhoneNumber   = phoneNumber, 
                  Email         = email,
                  Password      = password, 
                  UserType      = userType,
                  Resume        = res
            };
            DatabaseInteraction dbObj = new DatabaseInteraction();
            Console.WriteLine("Before CreatNewCandidate");
            dbObj.CreateNewCandidate(student);
            return new ObjectResult(student);
        }


        /// <summary>
        /// Gets All Students Information
        /// </summary>
        [HttpGet]
       // [HttpGet(Name = "GetAll")]
        public IEnumerable<TodoStudent> GetAll()
        {
            DatabaseInteraction dbObj = new DatabaseInteraction();
            return dbObj.GetAllUsers();
        }

        /// <summary>
        /// Gets a specific Student information.
        /// </summary>
        /// <param name="Email"></param> 
        [HttpGet("{Email}", Name = "GetTodo")]
       public IActionResult GetByEmail(string Email)
        {
            DatabaseInteraction dbObj = new DatabaseInteraction();

            TodoStudent user = dbObj.GetUserByEmail(Email);

            return new ObjectResult(user);
        }


        /// <summary>
        /// Updates a specific user.
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="newName"></param> 
        /// <param name="newPhoneNumber"></param>
        /// <param name="newEmail"></param>
        /// <param name="newPassword"></param>
        /// <param name="newUserType"></param>
        /// <param name="newResume"></param>
        [HttpPut("{Email}", Name = "UpdateUser")]
       // [HttpPost("UpdateUser")]
        public IActionResult Update(string Email, 
                                    string newName, 
                                    string newPhoneNumber, 
                                    string newEmail, 
                                    string newPassword, 
                                    string newUserType,
                                    string[] newResume)
        {
            //Get the user via email and then update
            DatabaseInteraction dbObj = new DatabaseInteraction();
            var student = new TodoStudent
            {
                Name        = newName,
                PhoneNumber = newPhoneNumber,
                Email       = newEmail,
                Password    = newPassword,
                UserType    = newUserType,
                Resume      = newResume
            };

            dbObj.UpdateUserInfo(Email, student);

            return new NoContentResult();
        }


        ///// <summary>
        ///// Assigns a resume to a specific user.
        ///// </summary>
        ///// <param name="Email"></param>
        ///// <param name="Resume"></param> 
        //[HttpPut("{Email}")]
        //public IActionResult AddResume(string Email,
        //                            string[] Resume)
        //{
        //    string[] res = Resume;
        //    //Get the user via email and then update
        //    DatabaseInteraction dbObj = new DatabaseInteraction();
        //    TodoStudent stu = dbObj.GetUserByEmail(Email);
        //    var student = new TodoStudent
        //    {
        //        Name        = stu.Name,       
        //        PhoneNumber = stu.PhoneNumber,
        //        Email       = stu.Email,      
        //        Password    = stu.Password,   
        //        UserType    = stu.UserType,
        //        Resume      = res
        //    };

        //    dbObj.UpdateUserInfo(Email, student);

        //    return new NoContentResult();
        //}

//        /// <summary>
//        /// Deletes a specific Student.
//        /// </summary>
//        /// <param name="id"></param> 
//        [HttpDelete("{id}")]
//public IActionResult Delete(long id)
//{
//    var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
//    if (todo == null)
//    {
//        return NotFound();
//    }

//    _context.TodoItems.Remove(todo);
//    _context.SaveChanges();
//    return new NoContentResult();
//}


//[HttpPost("UploadFiles")]
//public async Task<IActionResult> Post(List<IFormFile> files)
//{
//    long size = files.Sum(f => f.Length);

//    // full path to file in temp location
//    var filePath =  System.IO.Path.GetTempFileName();

//    foreach (var formFile in files)
//    {
//        if (formFile.Length > 0)
//        {
//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await formFile.CopyToAsync(stream);
//            }
//        }
//    }

//    // _context.TodoItems.Add(new TodoStudent { Name = "hotel", Address = "10322 asdasdas", Email = "marlon@gmail.com", PhoneNumber = "773-890-1234", BSEducationSchool = "Depaul University" , BSEducationTitle = "Computer Science", WorkExperienceCompanyNameOne = "FaceBook", WorkExperienceTitleOne = "Developer", ExtraCurricularActivitiesOne = "Programming" });
//    //_context.SaveChanges();

//    // process uploaded files
//    // Don't rely on or trust the FileName property without validation.

//    return Ok(new { count = files.Count, size, filePath});

//    }

    }
}