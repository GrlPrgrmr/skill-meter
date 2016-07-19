using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMeter.Models
{
   public class UserTest
    {

        private int testId;

        public int TestId
        {
            get { return testId; }
            set { testId = value; }
        }

        private string testGiven;

        public string TestGiven
        {
            get { return testGiven; }
            set { testGiven = value; }
        }


        private string testDuration;

        public string TestDuration
        {
            get { return testDuration; }
            set { testDuration = value; }
        }
        
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string setName;

        public string SetName
        {
            get { return setName; }
            set { setName = value; }
        }
        
        
        private int setId;

        public int SetId
        {
            get { return setId; }
            set { setId = value; }
        }



        
        private string date;

        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        private int score;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        private string testName;

        public string TestName
        {
            get { return testName; }
            set { testName = value; }
        }

        private int attempted;

        public int Attempted
        {
            get { return attempted; }
            set { attempted = value; }
        }

        private int correctlyAnswered;

        public int CorrectlyAnswered
        {
            get { return correctlyAnswered; }
            set { correctlyAnswered = value; }
        }
        
        
    }
}
