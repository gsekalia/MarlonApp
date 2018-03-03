using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarlonApi.Models
{
    public class TodoJobPosting
    {
        public long Id { get; set; }
        [Required]
        public string JobName { get; set; }                             
        public string Description { get; set; }                          
        public string[] Keywords { get; set; }            
    }

    
}