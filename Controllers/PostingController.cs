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
    [Route("api/Posting")]
    public class PostingController : Controller
    {
        private readonly TodoContext _context;

        /// <summary>
        /// creates the first student object
        /// </summary>
        public PostingController(TodoContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Gets All Job Postings
        /// </summary>
        [HttpGet]
        public IEnumerable<TodoJobPosting> GetAll()
        {
            DatabaseInteraction dbObj = new DatabaseInteraction();
            return dbObj.GetAllPostings();
        }


        /// <summary>
        /// Creates a Job posting by passing information in the parameters.
        /// </summary>
        /// <param name="JobTitle"></param> 
        /// <param name="Company"></param>
        /// <param name="Location"></param>
        /// <param name="Description"></param>
        /// <param name="Keywords"></param>
        /// <param name="UserAndScore"></param>
        [HttpPost("{JobTitle}")]
        public IActionResult CreateJobPosting(string JobTitle,
                                              string Company,
                                              string Location,
                                              string Description,
                                              string[] Keywords,
                                              string[][] UserAndScore)
        {
            string name       = JobTitle;
            string comp       = Company;
            string loc        = Location;
            string descr      = Description;
            string[] keywords = Keywords;// new string[] { "ect" };// Keywords;
            string[][] userAndScore = UserAndScore;//= new string[UserAndScore.Length, 2];// = UserAndScore;// new string[] { "ect" };// Keywords;

            DatabaseInteraction dbObj = new DatabaseInteraction();

            var posting = new TodoJobPosting
            {
                JobTitle    = name,
                Company     = comp,
                Location    = loc,
                Description = descr,
                Keywords    = keywords, // keywords
                UserAndScore= userAndScore
            };
            dbObj.CreateNewJobPosting(posting);
            return new ObjectResult(posting);
        }


        /// <summary>
        /// Gets a Job Posting's information.
        /// </summary>
        /// <param name="Name"></param> 
        [HttpGet("{Name}", Name = "GetPosting")]
        public IActionResult GetPostingByName(string Name)
        {
            DatabaseInteraction dbObj = new DatabaseInteraction();
            TodoJobPosting posting = dbObj.GetPostingByName(Name);
            return new ObjectResult(posting);
        }


        /// <summary>
        /// Add a user to a given jobPosting.
        /// </summary>
        /// <param name="JobTitle"></param> 
        /// <param name="Email"></param>
        [HttpPost("{Email}")]
        public IActionResult CreateJobPosting(string JobTitle,
                                              string Email
                                                            )
        {
            DatabaseInteraction dbObj = new DatabaseInteraction();

            TodoJobPosting  posting  = dbObj.GetPostingByName(JobTitle);
            TodoStudent stu = dbObj.GetUserByEmail(Email);
            dbObj.SubmitResumeToJob(posting, stu );

            //var posting = new TodoJobPosting
            //{
            //    JobTitle = title,
            //    Company = comp,
            //    Location = loc,
            //    Description = descr,
            //    Keywords = keywords, // keywords
            //    UserAndScore = userAndScore
            //};
            dbObj.CreateNewJobPosting(posting);
            return new ObjectResult(posting);
        }

    }

}