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

using MarlonApi.DatabaseInteraction;

namespace MarlonApi.Controllers
{
    //[Route("api/[[todo]]")]
    /// <summary>
    /// Controller creation
    /// </summary>
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/student")]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        /// <summary>
        /// creates the first student object
        /// </summary>
        public TodoController(TodoContext context){

            _context = context;

            if(_context.TodoItems.Count()==0){
                _context.TodoItems.Add(new TodoStudent { Name = "Marlon Garcia",
                                                        Address = "10322 addison ave franklin park",
                                                        Email = "marlon@gmail.com",
                                                        PhoneNumber = "773-890-1234",
                                                        BSEducationSchool = "Depaul University" ,
                                                        BSEducationTitle = "Computer Science",
                                                        WorkExperienceCompanyNameOne = "FaceBook",
                                                        WorkExperienceTitleOne = "Developer",
                                                        ExtraCurricularActivitiesOne = "Programming" });
                _context.SaveChanges();
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
/// <param name="Address"></param>
/// <param name="Email"></param>
/// <param name="PhoneNumber"></param>
/// <param name="BSEducationSchool"></param>
/// <param name="BSEducationTitle"></param>
/// <param name="MSEducationSchool"></param>
/// <param name="MSEducationTitle"></param>
/// <param name="PHdEducationSchool"></param>
/// <param name="PHdEducationTitle"></param>
/// <param name="WorkExperienceCompanyNameOne"></param>
/// <param name="WorkExperienceTitleOne"></param>
/// <param name="WorkExperienceCompanyNameTwo"></param>
/// <param name="WorkExperienceTitleTwo"></param>
/// <param name="WorkExperienceCompanyNameThree"></param>
/// <param name="WorkExperienceTitleThree"></param>
/// <param name="ExtraCurricularActivitiesOne"></param>
/// <param name="ExtraCurricularActivitiesTwo"></param>
[HttpPost("{Name}")]
public IActionResult createStudentFromResume(string Name, string Address, string Email, string PhoneNumber, string BSEducationSchool, string BSEducationTitle, string MSEducationSchool, string MSEducationTitle, string PHdEducationSchool, string PHdEducationTitle, string WorkExperienceCompanyNameOne, string WorkExperienceTitleOne, string WorkExperienceCompanyNameTwo, string WorkExperienceTitleTwo, string WorkExperienceCompanyNameThree, string WorkExperienceTitleThree, string ExtraCurricularActivitiesOne, string ExtraCurricularActivitiesTwo)
{

    if(Name == null || Name.Equals("")){

        return BadRequest();
    }

    if(Address == null || Address.Equals("")){

        return BadRequest();
    }
    
    if(Email == null || Email.Equals("")){
  
        return BadRequest();
    }

     if(PhoneNumber == null || PhoneNumber.Equals("")){
  
        return BadRequest();
    }

    string name = Name;
    string address = Address;
    string email = Email;
    string phoneNumber = PhoneNumber;
    string bsEducationSchool = BSEducationSchool;
    string bsEducationTitle = BSEducationTitle;
    string msEducationSchool = MSEducationSchool;
    string msEducationTitle = MSEducationTitle;
    string phdEducationSchool = PHdEducationSchool;
    string phdEducationTitle = PHdEducationTitle;
    string workExperienceCompanyNameOne = WorkExperienceCompanyNameOne;
    string workExperienceTitleOne = WorkExperienceTitleOne;
    string workExperienceCompanyNameTwo = WorkExperienceCompanyNameTwo;
    string workExperienceTitleTwo = WorkExperienceTitleTwo;
    string workExperienceCompanyNameThree = WorkExperienceCompanyNameThree;
    string workExperienceTitleThree = WorkExperienceTitleThree;
    string extraCurricularActivitiesOne = ExtraCurricularActivitiesOne;
    string extraCurricularActivitiesTwo = ExtraCurricularActivitiesTwo;


    DatabaseInteraction.DatabaseInteraction dbObj = new MarlonApi.DatabaseInteraction.DatabaseInteraction();

      email = dbObj.GetUserByName("dick");

    var student =  new TodoStudent {
          Name = name,
          Address = address, 
          Email = email,
          PhoneNumber = phoneNumber, 
          BSEducationSchool = bsEducationSchool , 
          BSEducationTitle = bsEducationTitle,
          MSEducationSchool = msEducationSchool,
          MSEducationTitle = msEducationTitle,
          PHdEducationSchool = phdEducationSchool,
          PHdEducationTitle = phdEducationTitle,
          WorkExperienceCompanyNameOne = workExperienceCompanyNameOne,
          WorkExperienceTitleOne = workExperienceTitleOne, 
          WorkExperienceCompanyNameTwo = workExperienceCompanyNameTwo,
          WorkExperienceTitleTwo = workExperienceTitleTwo,
          WorkExperienceCompanyNameThree = workExperienceCompanyNameThree,
          WorkExperienceTitleThree = workExperienceTitleThree,
          ExtraCurricularActivitiesOne = extraCurricularActivitiesOne,
          ExtraCurricularActivitiesTwo = extraCurricularActivitiesTwo
          };
   
    _context.TodoItems.Add(student);
     _context.SaveChanges();

    return CreatedAtRoute("GetTodo", new { id = student.Id }, student);
}



        /// <summary>
        /// Gets All Students Information
        /// </summary>
        [HttpGet]
        public IEnumerable<TodoStudent> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        
        /// <summary>
        /// Gets a specific Student information.
        /// </summary>
        /// <param name="id"></param> 
        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if(item == null){
                
                return NotFound();
            }

            return new ObjectResult(item);
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

    todo.Name = item.Name;
    todo.Address = item.Address;
    todo.Email = item.Email;
    todo.PhoneNumber = item.PhoneNumber;
    todo.BSEducationSchool = item.BSEducationSchool;
    todo.BSEducationTitle = item.BSEducationTitle;
    todo.MSEducationSchool = item.MSEducationSchool;
    todo.MSEducationTitle = item.MSEducationTitle;
    todo.PHdEducationSchool = item.PHdEducationSchool;
    todo.PHdEducationTitle = item.PHdEducationTitle;
    todo.WorkExperienceCompanyNameOne = item.WorkExperienceCompanyNameOne;
    todo.WorkExperienceTitleOne = item.WorkExperienceTitleOne;
    todo.WorkExperienceCompanyNameTwo = item.WorkExperienceCompanyNameTwo;
    todo.WorkExperienceTitleTwo = item.WorkExperienceTitleTwo;
    todo.WorkExperienceCompanyNameThree = item.WorkExperienceCompanyNameThree;
    todo.WorkExperienceTitleThree = item.WorkExperienceTitleThree;
    todo.ExtraCurricularActivitiesOne = item.ExtraCurricularActivitiesOne;
    todo.ExtraCurricularActivitiesTwo = item.ExtraCurricularActivitiesTwo;

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

    _context.TodoItems.Add(new TodoStudent { Name = "hotel", Address = "10322 asdasdas", Email = "marlon@gmail.com", PhoneNumber = "773-890-1234", BSEducationSchool = "Depaul University" , BSEducationTitle = "Computer Science", WorkExperienceCompanyNameOne = "FaceBook", WorkExperienceTitleOne = "Developer", ExtraCurricularActivitiesOne = "Programming" });
     _context.SaveChanges();

    // process uploaded files
    // Don't rely on or trust the FileName property without validation.

    return Ok(new { count = files.Count, size, filePath});

    }

    }
}