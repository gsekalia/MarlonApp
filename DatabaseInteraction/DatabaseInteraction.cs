﻿using System;
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

        //---------------------------BSON TO TODO OBJECT CONVERTERS----------------------------------------------------------------------
        private TodoStudent BsonToUser(BsonDocument doc)
        {
            String name = doc.GetValue("Name").ToString();
            String password = doc.GetValue("Password").ToString();
            String email = doc.GetValue("Email").ToString();
            String phoneNumber = doc.GetValue("PhoneNumber").ToString();

            Console.WriteLine(name);
            Console.WriteLine(password);
            Console.WriteLine(email);
            return new TodoStudent
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Email = email,
                Password = password
            };
        }


        private TodoJobPosting BsonToPosting(BsonDocument doc)
        {
            string Name = doc.GetValue("JobName").ToString();
            String descr = doc.GetValue("JobDescription").ToString();
            var k = doc.GetValue("Keywords").AsBsonArray.ToArray();

            int len = k.Length;
            string[] keywords = new string[len];
            for (int i = 0; i < len; i++)
            {
                keywords[i] = k[i].ToString();
               // Console.WriteLine(keywords[i]);
            }
            return new TodoJobPosting
            {
                JobName = Name,
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
           // MongoCursor<BsonDocument> cursor = collection.Find<BsonDocument>().//find().iterator();
            //var doc = collection.Find(new BsonDocument()).FirstOrDefault();

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
                            {"Password"     , "null" }
                        };
            }
            return BsonToUser(found);
         
            //String password = found.GetValue("Password").ToString();
            //String email = found.GetValue("Email").ToString();
            //String phoneNumber = found.GetValue("PhoneNumber").ToString();
            ////return new User(name, password, email);

            //Console.WriteLine(name);
            //Console.WriteLine(password);
            //Console.WriteLine(email);
            //return new TodoStudent{ Name = name,
            //                        PhoneNumber = phoneNumber,
            //                        Email = email,
            //                        Password = password
            //                        };       
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
                            {"Password"     , "null" }
                        };
            }
            return BsonToUser(found);

            //String password = found.GetValue("Password").ToString();
            //String name = found.GetValue("Name").ToString();
            //email = found.GetValue("Email").ToString();
            //String phoneNumber = found.GetValue("PhoneNumber").ToString();

            ////return new User(email, password, email);

            //Console.WriteLine(email);
            //Console.WriteLine(password);
            //Console.WriteLine(email);
            //return new TodoStudent
            //{
            //    Name        = name,
            //    PhoneNumber = phoneNumber,
            //    Email       = email,
            //    Password    = password
            //};
        }

        public TodoJobPosting GetPostingByName(String Name)
        {
            var collection = this.db.GetCollection<BsonDocument>("JobPosting");

            var search = new BsonDocument("JobName", Name);

            BsonDocument found;
            try
            {
                found = collection.Find(search).First();
            }
            catch (InvalidOperationException e)
            {
                found = new BsonDocument
                        {
                            {"JobName"      , "null" },
                            {"JobDescription"  , "null" },
                            {"Keywords"     , new BsonArray("") }
                        };
            }
            return BsonToPosting(found);
            //Name = found.GetValue("JobName").ToString();
            //String descr = found.GetValue("JobDescription").ToString();
            //var k = found.GetValue("Keywords").AsBsonArray.ToArray();

            //int len = k.Length;
            //string[] keywords = new string[len];
            //for(int i = 0; i < len; i++)
            //{
            //    keywords[i] = k[i].ToString();
            //    Console.WriteLine(keywords[i]);
            //}

            //Console.WriteLine(Name);
            //Console.WriteLine(descr);
            //return new TodoJobPosting
            //{
            //    JobName = Name,
            //    Description = descr,
            //    Keywords = keywords

            //};
        }

        public void CreateNewCandidate(String name, String password, String email)
        {
            var collection = db.GetCollection<BsonDocument>("candidate");

            var doc = new BsonDocument
            {
                {"Name", name },
                {"Password", password },
                {"Email", email }
            };
            collection.InsertOne(doc);
        }

        public void CreateNewJobPosting(TodoJobPosting posting)
        {
            if (posting.JobName == null)           posting.JobName = "none";
            if (posting.Description == null)    posting.Description = "none";
            if (posting.Keywords == null)       posting.Keywords = new string[] { };


            var coll = db.GetCollection<BsonDocument>("JobPosting");
            var doc = new BsonDocument
            {
                {"JobName", posting.JobName },
                {"JobDescription", posting.Description },
                {"Keywords", new BsonArray(posting.Keywords) }
            };
            coll.InsertOne(doc);
        }

        public void CreateNewCandidate(TodoStudent stu)
        {
                                                                                               
            if(stu.Name         == null) stu.Name        = "none";
            if(stu.PhoneNumber  == null) stu.PhoneNumber = "none";
            if(stu.Email        == null) stu.Email       = "none";
            if(stu.Password     == null) stu.Password    = "none";
           

            var collection = db.GetCollection<BsonDocument>("candidate");

            var doc = new BsonDocument
            {                                                                               
                {"Name"         , stu.Name       },
                {"PhoneNumber"  , stu.PhoneNumber},
                {"Email"        , stu.Email      },
                {"Password"     , stu.Password   }
             
            };
            collection.InsertOne(doc);
        }

        public bool AuthenticateUserLogin(String name, String password)
        {
            var collection = db.GetCollection<BsonDocument>("candidate");

            var search = new BsonDocument
            {
                {"name", name },
                {"password", password }
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

    }


}
