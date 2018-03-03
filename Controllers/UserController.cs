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

//using MarlonApi.DatabaseInteraction;


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

            if(_context.TodoItems.Count()==0){


            }

        }

/// <summary>
/// Creates a new student 
/// </summary>
/// <remarks>
/// Sample request:
///
///     POST /student
///     {
///        "name": "Student 1",
///        "address": "address 1",
///        "email": " student@gmail.com",
///        "phoneNumber": "334-123-6789",
///        "bsEducationSchool": "Depaul University",
///        "bsEducationTitle": "Computer Science",
///        "msEducationSchool": "DePaul Univercity",
///        "msEducationTitle": "Game Programming",
///        "workExperienceCompanyNameOne": "Google",
///        "workExperienceTitleOne": "Software Developer",
///        "workExperienceCompanyNameTwo": "Facebook",
///        "workExperienceTitleTwo": "Software Developer Intern",
///        "extraCurricularActivitiesOne": " Computer Science Society"
///     }
///
/// </remarks>
/// <param name="item"></param>
/// <returns>A newly-created TodoStudent</returns>
/// <response code="201">Returns the newly-created student</response>
/// <response code="400">If the item is null</response> 
[HttpPost]
[ProducesResponseType(typeof(TodoStudent), 201)]
[ProducesResponseType(typeof(TodoStudent), 400)]
public IActionResult Create([FromBody] TodoStudent item)
{
    if (item == null)
    {
        return BadRequest();
    }

    _context.TodoItems.Add(item);
    _context.SaveChanges();

    return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
}


    /// <summary>
    /// Creates a student by passing student information in the parameters.
    /// </summary>
    /// <param name="Name"></param> 
    /// <param name="PhoneNumber"></param>
    /// <param name="Email"></param>
    /// <param name="Password"></param>
    [HttpPost("{Name}")]
    public IActionResult CreateStudentFromResume(   string Name, 
                                                    string PhoneNumber, 
                                                    string Email, 
                                                    string Password)
    {
        string name = Name;
        string phoneNumber= PhoneNumber;
        string email = Email;
        string password = Password;
           DatabaseInteraction dbObj = new DatabaseInteraction();

        var student =  new TodoStudent {
              Name          = name,
              PhoneNumber   = phoneNumber, 
              Email         = email,
              Password      = password 
              };

                dbObj.CreateNewCandidate(student);
                dbObj.PrintCollection();

               // _context.TodoItems.Add(student);
                _context.TodoItems.Add(dbObj.GetUserByName(name));
                _context.SaveChanges();
            Console.WriteLine("saved context");
        //return CreatedAtRoute("GetTodo", new { id = student.Id }, student);
            return new ObjectResult(student);
        }


        /// <summary>
        /// Gets All Students Information
        /// </summary>
        [HttpGet]
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
        /// Updates a specific Student.
        /// </summary>
        /// <param name="id"></param> 
        /// <param name="item"></param>
        [HttpPut("{id}")]
public IActionResult Update(long id, [FromBody] TodoStudent item)
{
    if (item == null || item.Id != id)
    {
        return BadRequest();
    }

    var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
    if (todo == null)
    {
        return NotFound();
    }

    todo.Name           = item.Name;
    todo.PhoneNumber    = item.PhoneNumber;
    todo.Email          = item.Email;
    todo.Password       = item.Password;
    
    _context.TodoItems.Update(todo);
    _context.SaveChanges();
    return new NoContentResult();
}

/// <summary>
/// Deletes a specific Student.
/// </summary>
/// <param name="id"></param> 
[HttpDelete("{id}")]
public IActionResult Delete(long id)
{
    var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
    if (todo == null)
    {
        return NotFound();
    }

    _context.TodoItems.Remove(todo);
    _context.SaveChanges();
    return new NoContentResult();
}


[HttpPost("UploadFiles")]
public async Task<IActionResult> Post(List<IFormFile> files)
{
    long size = files.Sum(f => f.Length);

    // full path to file in temp location
    var filePath =  System.IO.Path.GetTempFileName();

    foreach (var formFile in files)
    {
        if (formFile.Length > 0)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
        }
    }

    // _context.TodoItems.Add(new TodoStudent { Name = "hotel", Address = "10322 asdasdas", Email = "marlon@gmail.com", PhoneNumber = "773-890-1234", BSEducationSchool = "Depaul University" , BSEducationTitle = "Computer Science", WorkExperienceCompanyNameOne = "FaceBook", WorkExperienceTitleOne = "Developer", ExtraCurricularActivitiesOne = "Programming" });
    //_context.SaveChanges();

    // process uploaded files
    // Don't rely on or trust the FileName property without validation.

    return Ok(new { count = files.Count, size, filePath});

    }

    }
}