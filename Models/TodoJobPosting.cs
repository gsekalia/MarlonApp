using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

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
        public string[][] UserAndScore { get; set; }

        public TodoJobPosting DefaultToNone()
        {
            if (this.JobTitle     == null) this.JobTitle     = "none";
            if (this.Company      == null) this.Company      = "none";
            if (this.Location     == null) this.Location     = "none";
            if (this.Description  == null) this.Description  = "none";
            if (this.Keywords     == null) this.Keywords     = new string[] { };
            if (this.UserAndScore == null) this.UserAndScore = new string[][] { };

            return this;
        }

        public TodoJobPosting DefaultToExisting(TodoJobPosting oldPosting)
        {
            if (this.JobTitle      == null) this.JobTitle    = oldPosting.JobTitle   ;
            if (this.Location      == null) this.Location    = oldPosting.Location   ;
            if (this.Company       == null) this.Company     = oldPosting.Company    ;
            if (this.Description   == null) this.Description = oldPosting.Description; 
            if (this.Keywords      == null) this.Keywords    = oldPosting.Keywords   ;
            if (this.UserAndScore  == null) this.UserAndScore = new string[][] { };

            return this;
        }

        public BsonDocument PostingToBson()
        {
            //string[][] usScr = new string[UserAndScore.Length][];

            //for(int i = 0; i < UserAndScore.Length; i++)
            //{
            //    usScr[i] = new string[2];


            //}

            return new BsonDocument
            {
                {"JobTitle"         , this.JobTitle                  },
                {"Company"          , this.Company                   },
                {"Location"         , this.Location                  },
                {"JobDescription"   , this.Description               },
                {"Keywords"         , new BsonArray(this.Keywords)   },
                {"UserAndScore"     , new BsonArray(this.UserAndScore)},

            };
        }

    }

    
}