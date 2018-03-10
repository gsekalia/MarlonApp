using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarlonApi.Models
{
    public class TodoJobPosting
    {
        public long Id { get; set; }
        [Required]
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }                          
        public string[] Keywords { get; set; }            
    }

    
}