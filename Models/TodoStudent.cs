using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarlonApi.Models
{
    public class TodoStudent
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }                             
        public string Address { get; set; }                          
        public string Email { get; set; }
        public string PhoneNumber { get; set; } = "none";               
        public string BSEducationSchool { get; set; }                
        public string BSEducationTitle { get; set; }                 
        public string MSEducationSchool { get; set; }                
        public string MSEducationTitle { get; set; }                 
        public string PHdEducationSchool { get; set; }               
        public string PHdEducationTitle { get; set; }                
        public string WorkExperienceCompanyNameOne { get; set; }     
        public string WorkExperienceTitleOne { get; set; }           
        public string WorkExperienceCompanyNameTwo { get; set; }     
        public string WorkExperienceTitleTwo { get; set; }           
        public string WorkExperienceCompanyNameThree { get; set; }   
        public string WorkExperienceTitleThree { get; set; }         
        public string ExtraCurricularActivitiesOne {get; set; }      
        public string ExtraCurricularActivitiesTwo {get; set; }        

    }

    
}