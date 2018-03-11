using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

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
        public string[] Resume { get; set; }



        public TodoStudent DefaultToNone()
        {
            if (this.Name        == null)   this.Name        = "none";
            if (this.PhoneNumber == null)   this.PhoneNumber = "none";
            if (this.Email       == null)   this.Email       = "none";
            if (this.Password    == null)   this.Password    = "none";
            if (this.UserType    == null)   this.UserType    = "candidate";
            if (this.Resume      == null)   this.Resume      = new string[] { };

            return this;
        }
        public TodoStudent DefaultToExisting(TodoStudent oldStu)
        {
            if (this.Name == null)        this.Name           = oldStu.Name;
            if (this.PhoneNumber == null) this.PhoneNumber    = oldStu.PhoneNumber;
            if (this.Email == null)       this.Email          = oldStu.Email;
            if (this.Password == null)    this.Password       = oldStu.Password;
            if (this.UserType == null)    this.UserType       = oldStu.UserType;
            if (this.Resume == null)      this.Resume         = oldStu.Resume;

            return this;
        }

        public BsonDocument UserToBson()
        {
            return new BsonDocument
            {
                {"Name"         , this.Name       },
                {"PhoneNumber"  , this.PhoneNumber},
                {"Email"        , this.Email      },
                {"Password"     , this.Password   },
                {"UserType"     , this.UserType   },
                {"Resume"       , new BsonArray(this.Resume)}
            };
        }
    }

    
}