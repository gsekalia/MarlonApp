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
        /// Creates a Job posting by passing information in the parameters.
        /// </summary>
        /// <param name="JobName"></param> 
        /// <param name="Description"></param>
        /// <param name="Keywords"></param>
        [HttpPost("{JobName}")]
        public IActionResult CreateJobPosting(string JobName, string Description, string[] Keywords)
        {
            string name = JobName;
            string descr = Description;
            string[] keywords = Keywords;// new string[] { "ect" };// Keywords;
            DatabaseInteraction dbObj = new DatabaseInteraction();

            var posting = new TodoJobPosting
            {
                JobName = name,
                Description = descr,
                Keywords = keywords // keywords

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
    }

}