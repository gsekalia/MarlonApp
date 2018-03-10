using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        //---------------------------OBJECT TO BSON CONVERTERS----------------------------------------------------------------------
        private BsonDocument UserToBson(TodoStudent stu)
        {

            return new BsonDocument
            {
                {"Name"         , stu.Name       },
                {"PhoneNumber"  , stu.PhoneNumber},
                {"Email"        , stu.Email      },
                {"Password"     , stu.Password   },
                {"UserType"     , stu.UserType   },
                {"Resume"       , new BsonArray(stu.Resume)}

            };
            //return doc;

        }
        private BsonDocument PostingToBson(TodoJobPosting posting)
        {
            return new BsonDocument
            {
                {"JobTitle"         , posting.JobTitle                  },
                {"Company"          , posting.Company                   },
                {"Location"         , posting.Location                  },
                {"JobDescription"   , posting.Description               },
                {"Keywords"         , new BsonArray(posting.Keywords)   }
            };

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
            return new TodoJobPosting
            {
                JobTitle = Name,
                Company = comp,
                Location = loc,
                Description = descr,
                Keywords = keywords
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


        public void PrintCollection()
        {
            var collection = this.db.GetCollection<BsonDocument>("candidate");
            using (var cursor = collection.Find(new BsonDocument()).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                       Console.WriteLine(doc.GetValue("Name").ToString());
                    }
                }
            }
        }

        //-----------------------GET METHODS---------------------------------------------------------------

        public TodoStudent GetUserByName(String name)
        {
            var collection = this.db.GetCollection<BsonDocument>("candidate");

            var search = new BsonDocument("Name", name);
            BsonDocument found;
            try
            {
                found = collection.Find(search).First();
            }
            catch (InvalidOperationException e)
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
                            {"Keywords"         , new BsonArray("") }
                        };
            }
            return BsonToPosting(found); 
        }
        //-----------------------CREATE NEW ENTRY METHODS--------------------------------------------

        public void CreateNewJobPosting(TodoJobPosting posting)
        {
            posting = DefaultToNone(posting);

            var coll = db.GetCollection<BsonDocument>("JobPosting");
            //var doc = new BsonDocument
            //{
            //    {"JobTitle"         , posting.JobTitle                  },
            //    {"Company"          , posting.Company                   },
            //    {"Location"         , posting.Location                  },
            //    {"JobDescription"   , posting.Description               },
            //    {"Keywords"         , new BsonArray(posting.Keywords)   }
            //};
            var doc = PostingToBson(posting);
            coll.InsertOne(doc);
        }

        public void CreateNewCandidate(TodoStudent stu)
        {
            Console.WriteLine("Before DefaultToNone");
            stu = DefaultToNone(stu);
            Console.WriteLine("After DefaultToNone");
            var collection = db.GetCollection<BsonDocument>("candidate");

            //var doc = new BsonDocument
            //{
            //    {"Name"         , stu.Name       },
            //    {"PhoneNumber"  , stu.PhoneNumber},
            //    {"Email"        , stu.Email      },
            //    {"Password"     , stu.Password   },
            //    {"UserType"     , stu.UserType   },
            //    {"Resume"       , new BsonArray(stu.Resume)}

            //};
            //Console.WriteLine("Before usertoBson");
            var doc = UserToBson(stu);
            Console.WriteLine("After DefaultToNone");
            collection.InsertOne(doc);
            Console.WriteLine("After adding to collection");
        }

        //-------------------------------------------UPDATE METHODS-------------------------------------------------------------------

        public TodoStudent UpdateUserInfo(string email, TodoStudent newStu)
        {
            TodoStudent stu = GetUserByEmail(email);

            newStu = DefaultToExisting(stu, newStu);

            var collection = db.GetCollection<BsonDocument>("candidate");

            //var doc = new BsonDocument
            //{
            //    {"Name"         , newStu.Name       },
            //    {"PhoneNumber"  , newStu.PhoneNumber},
            //    {"Email"        , newStu.Email      },
            //    {"Password"     , newStu.Password   },
            //    {"Password"     , newStu.Password   }
            //    {"Resume"       , new BsonArray(newStu.Resume)   }
            //};

            var doc = UserToBson(newStu);
            var search = new BsonDocument("Email", email);

            BsonDocument found;
            found = collection.Find(search).First();
            collection.ReplaceOne(found, doc);
            return newStu;
        }

        public TodoJobPosting UpdatePostingInfo(string title, TodoJobPosting newPosting)
        {
            TodoJobPosting oldPosting = GetPostingByName(title);
            newPosting = DefaultToExisting(oldPosting, newPosting);

            var collection = db.GetCollection<BsonDocument>("candidate");

            var doc = new BsonDocument
            {
                {"JobTitle"         , newPosting.JobTitle                  },
                {"Company"          , newPosting.Company                   },
                {"Location"         , newPosting.Location                  },
                {"JobDescription"   , newPosting.Description               },
                {"Keywords"         , new BsonArray(newPosting.Keywords)   }
            };


            var search = new BsonDocument("JobTitle", title);

            BsonDocument found;
            found = collection.Find(search).First();
            collection.ReplaceOne(found, doc);
            return newPosting;
        }

        //-------------------------------------------AUTHENTICATION---------------------------------------------------------------------
        public bool AuthenticateUserLogin(String name, String password)
        {
            var collection = db.GetCollection<BsonDocument>("candidate");

            var search = new BsonDocument
            {
                {"name"     , name      },
                {"password" , password  }
            };

            BsonDocument found = null;
            try
            {
                   found = collection.Find(search).First();
            }
            catch( System.InvalidOperationException e)
            {
                
            }
            bool result = false;
            string debugMsg = "No User Found";
            if (found != null)
            {
                debugMsg = "Found User";
                result = true;
            }

            Console.WriteLine(debugMsg);
            return result;
        }




        //--------------------------------------DEFAULT POJO CONSTRUCTOR-----------------------------------------------------------
        private TodoStudent DefaultToNone(TodoStudent stu)
        {
            if (stu.Name         == null)    stu.Name         = "none" ;
            if (stu.PhoneNumber  == null)    stu.PhoneNumber  = "none" ;
            if (stu.Email        == null)    stu.Email        = "none" ;
            if (stu.Password     == null)    stu.Password     = "none" ;
            if (stu.UserType     == null)    stu.UserType     = "candidate";
            if (stu.Resume       == null)    stu.Resume       = new string[] { };

            return stu;
        }
        private TodoJobPosting DefaultToNone(TodoJobPosting posting)
        {
            if (posting.JobTitle    == null) posting.JobTitle       = "none";
            if (posting.Company     == null) posting.Company        = "none";
            if (posting.Location    == null) posting.Location       = "none";
            if (posting.Description == null) posting.Description    = "none";
            if (posting.Keywords    == null) posting.Keywords       = new string[] { };

            return posting;
        }

        private TodoStudent DefaultToExisting(TodoStudent oldStu, TodoStudent newStu)
        {
            if (newStu.Name         == null)    newStu.Name         = oldStu.Name         ;
            if (newStu.PhoneNumber  == null)    newStu.PhoneNumber  = oldStu.PhoneNumber  ;
            if (newStu.Email        == null)    newStu.Email        = oldStu.Email        ;
            if (newStu.Password     == null)    newStu.Password     = oldStu.Password     ;
            if (newStu.UserType     == null)    newStu.UserType     = oldStu.UserType     ;
            if (newStu.Resume       == null)    newStu.Resume       = oldStu.Resume       ;

            return newStu;
        }


        private TodoJobPosting DefaultToExisting(TodoJobPosting oldPosting, TodoJobPosting newPosting)
        {
            if (newPosting.JobTitle      == null) newPosting.JobTitle    = oldPosting.JobTitle   ;
            if (newPosting.Location      == null) newPosting.Location    = oldPosting.Location   ;
            if (newPosting.Company       == null) newPosting.Company     = oldPosting.Company    ;
            if (newPosting.Description   == null) newPosting.Description = oldPosting.Description; 
            if (newPosting.Keywords      == null) newPosting.Keywords    = oldPosting.Keywords   ;

            return newPosting;
        }
    }


}
