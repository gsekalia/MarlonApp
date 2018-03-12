using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarlonApi.FileReader;
using MongoDB.Bson;
using MongoDB.Driver;
//using System.InvalidOperationException;
using MarlonApi.Models;

namespace MarlonApp.DatabaseInteraction
{
    class DatabaseInteraction
    {

        private String HOST = "mongodb://207.229.181.23:27017";
        private String DATABASENAME = "csc394";
        private MongoClient client;
        private IMongoDatabase db;

        //static DatabaseInteraction instance;
        public DatabaseInteraction()
        {
            //HOST = 
            this.client = new MongoClient(HOST);
            this.db = client.GetDatabase(DATABASENAME);
            Console.WriteLine("Connected to Database");
        }

        //   public static DatabaseInteraction Instance() 
        //   {
        // if (DatabaseInteraction.instance==null)
        // {
        //  DatabaseInteraction.instance = new DatabaseInteraction();
        //       }

        // return DatabaseInteraction.instance;
        //}



        public IMongoCollection<BsonDocument> getCollection(String name)
        {
            return this.db.GetCollection<BsonDocument>(name);
        }


        //---------------------------BSON TO TODO OBJECT CONVERTERS----------------------------------------------------------------------
        private TodoStudent BsonToUser(BsonDocument doc)
        {
            Console.WriteLine("Inside BsonToUser");
            String name         = doc.GetValue("Name").ToString();
            String password     = doc.GetValue("Password").ToString();
            String email        = doc.GetValue("Email").ToString();
            String phoneNumber  = doc.GetValue("PhoneNumber").ToString();
            String userType     = doc.GetValue("UserType").ToString();        
            var r               = doc.GetValue("Resume").AsBsonArray.ToArray();

            int len = r.Length;
            string[] res = new string[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = r[i].ToString();
            }


            Console.WriteLine(name);
            Console.WriteLine(password);
            Console.WriteLine(email);
            return new TodoStudent
            {
                Name        = name,
                PhoneNumber = phoneNumber,
                Email       = email,
                Password    = password,
                UserType    = userType,
                Resume      = res
            };
        }

        private TodoJobPosting BsonToPosting(BsonDocument doc)
        {
            string Name     = doc.GetValue("JobTitle").ToString();
            String comp     = doc.GetValue("Company").ToString();
            string loc      = doc.GetValue("Location").ToString();
            String descr    = doc.GetValue("JobDescription").ToString();
            var k           = doc.GetValue("Keywords").AsBsonArray.ToArray();
            int len = k.Length;
            string[] keywords = new string[len];
            for (int i = 0; i < len; i++)
            {
                keywords[i] = k[i].ToString();
            }
            var s = doc.GetValue("UserAndScore").AsBsonArray.ToArray();
            len = s.Length;
            string[][] userAndScore = new string[len][];

            for (int i = 0; i < len; i++)
            {
                userAndScore[i] = new string[2];

                var currComb = s[i].AsBsonArray.ToArray();
                Console.WriteLine(currComb[0]);
                userAndScore[i][0] = currComb[0].ToString();

                try
                {
                    userAndScore[i][1] = currComb[1].ToString();
                }
                catch (IndexOutOfRangeException e)
                {
                    userAndScore[i][1] = "0";
                }
            }

            return new TodoJobPosting
            {
                JobTitle = Name,
                Company = comp,
                Location = loc,
                Description = descr,
                Keywords = keywords,
                UserAndScore = userAndScore
            };
        }

        //----------------------------GET ALL FROM DATABASE---------------------------------------------------------------------
        public List<TodoJobPosting> GetAllPostings()
        {
            var coll = this.db.GetCollection<BsonDocument>("JobPosting");
            List<TodoJobPosting> postingList = new List<TodoJobPosting>();
            using (var cursor = coll.Find(new BsonDocument()).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        postingList.Add(BsonToPosting(doc));                   
                    }
                }
            }
            return postingList;
        }

        public List<TodoStudent> GetAllUsers()
        {
            var coll = this.db.GetCollection<BsonDocument>("candidate");
            List<TodoStudent> postingList = new List<TodoStudent>();
            using (var cursor = coll.Find(new BsonDocument()).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        postingList.Add(BsonToUser(doc));
                    }
                }
            }
            return postingList;
        }

        //-----------------------GET METHODS---------------------------------------------------------------
        public TodoStudent GetUserByEmail(String email)
        {
            var collection = this.db.GetCollection<BsonDocument>("candidate");

            var search = new BsonDocument("Email", email);

            BsonDocument found;
            try
            {
                found = collection.Find(search).First();
            }
            catch(InvalidOperationException e)
            {
                found = new BsonDocument
                        {
                            {"Name"         , "null" },
                            {"PhoneNumber"  , "null" },
                            {"Email"        , "null" },
                            {"Password"     , "null" },
                            {"UserType"     , "null" },
                            {"Resume"       , new BsonArray("")   }
                        };               
            }
            return BsonToUser(found);
        }

        public TodoJobPosting GetPostingByName(String Name)
        {
            var collection = this.db.GetCollection<BsonDocument>("JobPosting");

            var search = new BsonDocument("JobTitle", Name);

            BsonDocument found;
            try
            {
                found = collection.Find(search).First();
            }
            catch (InvalidOperationException e)
            {
                found = new BsonDocument
                        {
                            {"JobTitle"         , "null"            },
                            {"Company"          , "null"            },
                            {"Location"         , "null"            },
                            {"JobDescription"   , "null"            },
                            {"Keywords"         , new BsonArray("") },
                            {"UserAndScore"     , new BsonArray()   },

                        };
            }
            return BsonToPosting(found); 
        }
        //-----------------------CREATE NEW ENTRY METHODS--------------------------------------------

        public void CreateNewJobPosting(TodoJobPosting posting)
        {

            posting = posting.DefaultToNone();
            var coll = db.GetCollection<BsonDocument>("JobPosting");
            var doc = posting.PostingToBson();
            coll.InsertOne(doc);
        }

        public void CreateNewCandidate(TodoStudent stu)
        {
            stu.DefaultToNone();
            var collection = db.GetCollection<BsonDocument>("candidate");
            //var doc = UserToBson(stu);
            var doc = stu.UserToBson();
            collection.InsertOne(doc);
        }

        //-------------------------------------------UPDATE METHODS-------------------------------------------------------------------

        public TodoStudent UpdateUserInfo(string email, TodoStudent newStu)
        {
            TodoStudent stu = GetUserByEmail(email);
        
            newStu = newStu.DefaultToExisting(stu);
            var collection = db.GetCollection<BsonDocument>("JobPosting");
            var doc = newStu.UserToBson();
            var search = new BsonDocument("Email", email);

            BsonDocument found;
            found = collection.Find(search).First();
            collection.ReplaceOne(found, doc);
            return newStu;
        }

        public TodoJobPosting SubmitResumeToJob(TodoJobPosting posting, TodoStudent stu)
        {
            TodoJobPosting newPosting = posting.DefaultToNone();
            newPosting = newPosting.DefaultToExisting(posting);

            var oldUserScore = posting.UserAndScore;
            int oldLen = posting.UserAndScore.Length;
            string[][] newUserScores = new string[oldLen + 1][];

            //transfer over existing submitals and appending the newest one
            for (int i = 0; i < oldLen; i++)
            {
                newUserScores[i] = new string[2];

                string[] currComb = oldUserScore[i];
                Console.WriteLine(currComb[0]);
                newUserScores[i][0] = currComb[0].ToString();
                try
                {
                    newUserScores[i][1] = currComb[1].ToString();
                }
                catch (IndexOutOfRangeException e)
                {
                    newUserScores[i][1] = "0";
                }
            }
            newUserScores[oldLen] = new string[2];
            newUserScores[oldLen][0] = stu.Email;
            int score = FileReader.ReadandAssignVal(stu.Resume, posting.Keywords);
            newUserScores[oldLen][1] = score.ToString();
            //newUserScores[oldLen][1] = "0";

            newPosting.UserAndScore = newUserScores;

            var collection = db.GetCollection<BsonDocument>("JobPosting");     
            var newDoc = newPosting.PostingToBson();
            var search = posting.PostingToBson();

            //BsonDocument found;
          //  found = collection.Find(search).First();
            collection.ReplaceOne(search, newDoc);
            return newPosting;
        }

    }


}
