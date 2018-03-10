using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarlonApi.Models
{
    public class TodoStudent
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }                             
        public string PhoneNumber { get; set; }                          
        public string Email { get; set; }
        public string Password { get; set; }    
        public string UserType { get; set; }
            
    }

    
}