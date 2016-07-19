using SkillMeter.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SkillMeter.ViewModels
{
   public class Session : INotifyPropertyChanged
    {
        private Session()
        {
            this.Answer = new List<Option>();
        }

        private static Session sessionObj;
        public static Session SessionObj
        {
            get
            {

                if (sessionObj == null)
                {
                    sessionObj = new Session();

                }
                return sessionObj;

            }
        }

        private string vMBinstance;
        public string VMBInstance
        {

            get { return vMBinstance; }
            set { vMBinstance = value; OnPropertyChanged("VMBInstance"); }

        }

        private int userId;

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
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

        private List<Option> answer;

        public List<Option> Answer
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
            set { timeStampValue = value; OnPropertyChanged("TimeStampValue"); }
        }


        private int score;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        private bool appTimeOut;

        public bool AppTimeOut
        {
            get { return appTimeOut; }
            set { appTimeOut = value; OnPropertyChanged("AppTimeOut"); }
        }

        private string candidateDeleted;

        public string CandidateDeleted
        {
            get { return candidateDeleted; }
            set { candidateDeleted = value; OnPropertyChanged("CandidateDeleted"); }
        }
        
        private string questionSetForDropDownTestTaker;

        public string QuestionSetForDropDownTestTaker
        {
            get { return questionSetForDropDownTestTaker; }
            set { questionSetForDropDownTestTaker = value; OnPropertyChanged("QuestionSetForDropDownTestTaker"); }
        }

        private string newlyCreatedAdmin;

        public string NewlyCreatedAdmin
        {
            get { return newlyCreatedAdmin; }
            set { newlyCreatedAdmin = value; OnPropertyChanged("NewlyCreatedAdmin"); }
        }


        private string newlyCreatedCandidate;
        public string NewlyCreatedCandidate
        {

            get { return newlyCreatedCandidate; }
            set { newlyCreatedCandidate = value; OnPropertyChanged("NewlyCreatedCandidate"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected internal void OnPropertyChanged(string propName)
        {
            PropertyChangedEventHandler handle = PropertyChanged;
            if (handle != null)
            {
                handle(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
