﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using MongoDB.Bson;
using MongoDB.Driver;

using MarlonApi.Models;

namespace MarlonApi.DatabaseInteraction
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
            return this.db.GetCollection<BsonDocument>(name);         }

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
                        Console.WriteLine(doc.GetValue("Name"));
                    }
                }
            }
        }


        public String GetUserByName(String name)
        {
            var collection = this.db.GetCollection<BsonDocument>("candidate");

            var search = new BsonDocument("Name", name);
            var found = collection.Find(search).First();

         
            String password = found.GetValue("Password").ToString();
            String email = found.GetValue("Email").ToString();
            //return new User(name, password, email);

            Console.WriteLine(name);
            Console.WriteLine(password);
            Console.WriteLine(email);
            return email;
        }

        public void CreateNewCandidate(String name, String password, String email)
        {
            var collection = db.GetCollection<BsonDocument>("candidate");

            var doc = new BsonDocument
            {
                {"Name", name },
                //{"Password", password },
                {"Email", email }
            };
            collection.InsertOne(doc);
        }
        public void CreateNewCandidate(TodoStudent stu)
        {
                                                                                               
            if(stu.Name                           == null) stu.Name                             = "none";
            if(stu.Address                        == null) stu.Address                          = "none";
            if(stu.Email                          == null) stu.Email                            = "none";
            if(stu.PhoneNumber                    == null) stu.PhoneNumber                      = "none";
            if(stu.BSEducationSchool              == null) stu.BSEducationSchool                = "none";
            if(stu.BSEducationTitle               == null) stu.BSEducationTitle                 = "none";
            if(stu.MSEducationSchool              == null) stu.MSEducationSchool                = "none";
            if(stu.MSEducationTitle               == null) stu.MSEducationTitle                 = "none";
            if(stu.PHdEducationSchool             == null) stu.PHdEducationSchool               = "none";
            if(stu.PHdEducationTitle              == null) stu.PHdEducationTitle                = "none";
            if(stu.WorkExperienceCompanyNameOne   == null) stu.WorkExperienceCompanyNameOne     = "none";
            if(stu.WorkExperienceTitleOne         == null) stu.WorkExperienceTitleOne           = "none";
            if(stu.WorkExperienceCompanyNameTwo   == null) stu.WorkExperienceCompanyNameTwo     = "none";
            if(stu.WorkExperienceTitleTwo         == null) stu.WorkExperienceTitleTwo           = "none";
            if(stu.WorkExperienceCompanyNameThree == null) stu.WorkExperienceCompanyNameThree   = "none";
            if(stu.WorkExperienceTitleThree       == null) stu.WorkExperienceTitleThree         = "none";
            if(stu.ExtraCurricularActivitiesOne   == null) stu.ExtraCurricularActivitiesOne     = "none";
            if(stu.ExtraCurricularActivitiesTwo   == null) stu.ExtraCurricularActivitiesTwo     = "none";

            var collection = db.GetCollection<BsonDocument>("candidate");

            var doc = new BsonDocument
            {                                                                               
                {"Name"                         , stu.Name                            },
                {"Address"                      , stu.Address                         },
                {"Email"                        , stu.Email                           },
                {"PhoneNumber"                  , stu.PhoneNumber                     },
                {"BSEducationSchool"            , stu.BSEducationSchool               },
                {"BSEducationTitle"             , stu.BSEducationTitle                },
                {"MSEducationSchool"            , stu.MSEducationSchool               },
                {"MSEducationTitle"             , stu.MSEducationTitle                },
                {"PHdEducationSchool"           , stu.PHdEducationSchool              },
                {"PHdEducationTitle"            , stu.PHdEducationTitle               },
                {"WorkExperienceCompanyNameOne" , stu.WorkExperienceCompanyNameOne    },
                {"WorkExperienceTitleOne"       , stu.WorkExperienceTitleOne          },
                {"WorkExperienceCompanyNameTwo" , stu.WorkExperienceCompanyNameTwo    },
                {"WorkExperienceTitleTwo"       , stu.WorkExperienceTitleTwo          },
                {"WorkExperienceCompanyNameThree",stu.WorkExperienceCompanyNameThree  },
                {"WorkExperienceTitleThree"     , stu.WorkExperienceTitleThree        },
                {"ExtraCurricularActivitiesOne" , stu.ExtraCurricularActivitiesOne    },
                { "ExtraCurricularActivitiesTwo", stu.ExtraCurricularActivitiesTwo    }
            };
            collection.InsertOne(doc);
        }

        public bool AuthenticateUserLogin(String name, String password)
        {
            var collection = db.GetCollection<BsonDocument>("candidate");

           // var search = new BsonDocument("name", name);

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