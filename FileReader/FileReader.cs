using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

using MarlonApi.Models;


namespace MarlonApi.FileReader
{
    class FileReader
    {

        //private String HOST = "mongodb://207.229.181.23:27017";
        //private String DATABASENAME = "csc394";
        //private MongoClient client;
        //private IMongoDatabase db;

        //static DatabaseInteraction instance;
        public FileReader()
        {
            ////HOST = 
            //this.client = new MongoClient(HOST);
            //this.db = client.GetDatabase(DATABASENAME);
            //Console.WriteLine("Connected to Database");
        }

        private int CheckWordAgainstKeywords(string word, string[] keywords)
        {
            int point = 0;
           // int len = keywords.Length;
            int keywordsLen = keywords.Length; ;
            int i = 0;
            bool isMatched = false;
            while (i < keywordsLen && !isMatched)
            {
                if (keywords[i].Equals(word))
                {
                    isMatched = true;
                    point = 1;
                }
                i++;
            }
            return point;
            

        }
        public int ReadandAssignVal(string resStr, string[] keywords)
        {
            int score = 0;
            string word = "";
            int resLen = resStr.Length;
            for (int i = 0; i < resLen; i++ )
            {
                char currChar = resStr[i];
                if (!currChar.Equals(" "))
                    word += resStr[i];
                else
                {
                    score += CheckWordAgainstKeywords(word, keywords);
                    //string[] keywords = new string[9];
                    //keywords[0] = "derp";  
                    //int keywordsLen = keywords.Length;
                    //for(int j = 0; j < keywordsLen; j++)
                    //{
                    //    if(keywords[j].Equals(word))
                    //    {
                    //        score++;
                    //    }
                    //}                  
                }

            }
           
            return score;
        }

    }


}
