using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillMeter.ViewModels;

namespace SkillMeter.DataAccessLayer
{
   public class ExamSession
    {
       public ExamSession()
       {

          
       }

       private int examSessionId;

       public int ExamSessionId
       {
           get { return examSessionId; }
           set { examSessionId = value; }
       }
       
        private int testId;

        public int TestId
        {
            get { return testId; }
            set { testId = value; }
        }

        private int questionId;

        public int QuestionId
        {
            get { return questionId; }
            set { questionId = value; }
        }

        private int sequenceNo;

        public int SequenceNo
        {
            get { return sequenceNo; }
            set { sequenceNo = value; }
        }


        private string answer;

        public string Answer
        {
            get { return answer; }
            set 
            {
                answer = value;
            }
        }

        private string timeStampValue;

        public string TimeStampValue
        {
            get { return timeStampValue; }
            set { timeStampValue = value; }
        }

        private int appeared;

        public int Appeared
        {
            get { return appeared; }
            set { appeared = value; }
        }
        
        
        
    }
}
