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
    static class FileReader
    {
        private static int CheckWordAgainstKeywords(string word, string[] keywords)
        {
            int point = 0;
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

        public static int ReadandAssignVal(string[] resStr, string[] keywords)
        {
            int score = 0;
           // string word = "";
            int resLen = resStr.Length;
            for (int i = 0; i < resLen; i++ )
            {
                //char currChar = resStr[i];
                //if (!currChar.Equals(" ") && !currChar.Equals(".") && !currChar.Equals(",") && !currChar.Equals("/"))
                //    word += resStr[i];
                //else
                //{
                //    score += CheckWordAgainstKeywords(word, keywords);                
                //}
                    score += CheckWordAgainstKeywords(resStr[i], keywords);                


            }

            return score;
        }

    }


}
