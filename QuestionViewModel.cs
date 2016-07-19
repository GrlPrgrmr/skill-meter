using SkillMeter.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using SkillMeter.Models;
using System.Data;
using System.Windows.Threading;

namespace SkillMeter.ViewModels
{
    class QuestionViewModel : ViewModelBase
    {

        #region fields Declaration
        DataAccessADO objdata;
        private List<Question> QuesList;
        private List<int> appearedQuestions;
        List<Option> OptionsList;
        readonly TimerViewModel _timer = new TimerViewModel();
        Question currentQuestion;
        static int seqNumberOnScreen;
        static Random random;
        ResultNoNegative objResult;
        UserTest userTestObj;
        private List<ExamSession> userData;
   
        #endregion


        public QuestionViewModel()
        {

          
            try
            {
                #region Initialization
               
                random = new Random();
                objdata = new DataAccessADO();
                Question = new Question();
                QuesList = new List<DataAccessLayer.Question>();
                currentQuestion = new Question();
                objResult = new ResultNoNegative();
                userTestObj = new UserTest();
                userData = new List<ExamSession>();
                appearedQuestions = new List<int>();


               this.CurrentSession.PropertyChanged+=CurrentSession_PropertyChanged;

               
                this.SeqOnScreen = 1;

                this.CandidateName ="Hi "+ this.CurrentSession.UserName.ToUpper();
                this.CurrentSession.SequenceNo = this.SeqOnScreen;


                userTestObj = objdata.getUserTestsData().Where(r=>r.UserName==this.CurrentSession.UserName && r.Attempted==0).FirstOrDefault();

                if (userTestObj == null)
                {
                    this.CurrentSession.AppTimeOut = true;
                    return;
                }

                this.CurrentSession.TestId = userTestObj.TestId;
                userData = objdata.getUserSessionData(this.CurrentSession.TestId);

                QuesList = objdata.getQuestions().Select(r => r).Where(r => r.QuesSetID == userTestObj.SetId).ToList();
                this.maxQuestionsPerExam = objdata.getAllQuestionSets().Where(r=>r.SetId==userTestObj.SetId).Select(r=>r.QuestionsPerTest).FirstOrDefault();

                //filtering questions for a second time user login
                if (userData.Count != 0)
                {
                    var filteredQuestions = QuesList.Where(r => !userData.Select(p => p.QuestionId).Contains(r.QueID)).ToList();
                    QuesList = filteredQuestions.ToList();
                   
                }
                OptionsList = objdata.getOptions();


                this.Buckets = (
                    from q in QuesList.AsEnumerable()
                    group q by q.Complexity into groupedData
                    select new QuestionBucket
                    {
                        Complexity = groupedData.Key,
                        Questions = groupedData.Select(g => g).ToList()
                    }).ToList();

                //this.maxQuestionsPerExam = this.Buckets.SelectMany(r=>r.Questions).ToList().Count;

                if (checkUserState())
                {
                #endregion

                    double initialComplexity = 0;
                    double complexityMin = this.Buckets.Min(c => c.Complexity);
                    double complexityMax = this.Buckets.Max(c=>c.Complexity);

                    if (complexityMin == complexityMax)
                    {
                        initialComplexity = complexityMin;
                    }
                    else
                    {
                         initialComplexity = Math.Ceiling((complexityMin + complexityMax) / 2);
                    }

                    this.Question = this.Buckets.Where(b => b.Complexity == initialComplexity).FirstOrDefault().
                                    Questions.Where(r=>QuesList.Select(p=>p.QueID).
                                    Contains(r.QueID)).FirstOrDefault();

                    this.appearedQuestions.Add(this.Question.QueID);
                    this.QuestionString = this.SeqOnScreen.ToString() + ". " + this.Question.QuestionString;
                    this.CurrentBucket = this.Question.Complexity;
                    this.QuestionOptions = OptionsList.Where(o => o.QuesId == this.Question.QueID).ToList();

                   
                    _timer.Start();
                }
                else
                {
                    //this.CurrentSession.VMBInstance = "TimeUpViewModel";
                    this.CurrentSession.AppTimeOut = true;
                   
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                this.CurrentSession.AppTimeOut = true;
            }
        }

        private void CurrentSession_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TimeStampValue")
            {

                this.TimerValue = this.CurrentSession.TimeStampValue;
            }
        }


        private bool checkUserState()
        {
            List<TimeSpan> timerData = new List<TimeSpan>();
            try
            {
                var userStateData = objdata.getUserSessionData(this.CurrentSession.TestId);

                int timerValueFromDB = Convert.ToInt32(objdata.getUserTestsData().Where(r => r.UserName == this.CurrentSession.UserName && r.Attempted == 0).Select(r => r.TestDuration).FirstOrDefault());
                var temp = userStateData.AsEnumerable().Where(r => r.TestId == this.CurrentSession.TestId).Select(row => row.TimeStampValue);


                timerData = temp.Select(r => TimeSpan.Parse(r)).ToList();

                if(timerData.Count==0)
                {
                  
                    _timer.Duration = new TimeSpan(0, timerValueFromDB, 0);
                    return true; //No data exists for test taker....New Test Taker
                }
                else if (timerData.Count >= this.maxQuestionsPerExam)
                {
                    return false;
                }

                else if (timerData.Min() > new TimeSpan(0,0,0)) //application stopped in between and test is continued
                {
                    var seq = userStateData.AsEnumerable().
                                Where(r => r.TestId== this.CurrentSession.TestId && TimeSpan.Parse(r.TimeStampValue) == timerData.Min())
                                ;
                    this.SeqOnScreen = Convert.ToInt32(seq.FirstOrDefault().SequenceNo)+1;
                    

                    this.appearedQuestions.AddRange(userStateData.AsEnumerable().Where(r=>r.TestId == this.CurrentSession.TestId).Select(row=>row.QuestionId));

                    _timer.Duration = seq.Select(r => TimeSpan.Parse(r.TimeStampValue)).FirstOrDefault();
                   
                    return true;
                }
                

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
              
                return false;
            }
            
        }


        #region properties

        private int maxQuestionsPerExam { get; set; }

        public List<QuestionBucket> Buckets { get; set; }

        private bool _isSelected;

        public bool isSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(() => this.isSelected); }
        }

        private int seqOnScreen;

        public int SeqOnScreen
        {
            get { return seqOnScreen; }
            set { seqOnScreen = value; OnPropertyChanged(()=>this.SeqOnScreen); }
        }
        

        private TimeSpan duration;
        /// <summary>
        /// Set or get the duration.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return duration;
            }
            set
            {
                if (duration != value)
                {
                    duration = value;

                }
            }
        }


        private string candidateName;

        public string CandidateName
        {
            get { return candidateName; }
            set { candidateName = value; OnPropertyChanged(()=>this.CandidateName); }
        }
        
        private string timerValue;

        public string TimerValue
        {
            get { return timerValue; }
            set { timerValue = _timer.TimerValue; OnPropertyChanged(() => this.TimerValue); }
        }


        private List<Option> opSelected;

        public List<Option> Opselected
        {
            get { return opSelected; }

            set { opSelected = value; OnPropertyChanged(() => this.Opselected); }
        }

        private List<Option> _QuestionOptions;
        public List<Option> QuestionOptions
        {
            get { return _QuestionOptions; }
            set { _QuestionOptions = value; OnPropertyChanged(() => this.QuestionOptions); }
        }



        private Question _question;
        public Question Question
        {
            get { return _question; }
            set { _question = value; OnPropertyChanged(() => this.Question); }
        }

        private string questionString;

        public string QuestionString
        {
            get { return questionString; }
            set { questionString = value; OnPropertyChanged(()=>this.QuestionString); }
        }
        

        private int currentBucket;

        public int CurrentBucket
        {
            get { return currentBucket; }
            set { currentBucket = value; }
        }

        #endregion


        #region Commands

        private ICommand submitCommand;

        public ICommand SubmitCommand
        {
            get
            {
                return new RelayCommand(param => submitQuestion());
            }

        }



        #endregion

      

      

        

        private void submitQuestion()
        {
            try
            {

                this.CurrentSession.SequenceNo = this.SeqOnScreen;

                bool res = ProcessAnswer();
               

                if (this.CurrentSession.Answer.Count == 0)
                {
                    MessageBox.Show("Please select a choice!!!");
                    return;
                }

                int nextBucket = 0;

                if (res)
                    nextBucket = ++this.Question.Complexity;
                else
                    nextBucket = --this.Question.Complexity;

                if (nextBucket > 5) nextBucket = 5;

                if (nextBucket < 1) nextBucket = 1;

                var tempListQues = this.Buckets.Where(b => b.Complexity == nextBucket).FirstOrDefault()
                              .Questions.Where(q=>!this.appearedQuestions.Contains(q.QueID)).ToList();

                this.CurrentBucket = nextBucket;

                
                if (tempListQues.Count==0 || this.appearedQuestions.Count==this.maxQuestionsPerExam)
                {
                    this.CurrentSession.VMBInstance = "TimeUpViewModel";
                }
                else
                {
                    this.Question = tempListQues.getRandomElement();
                    this.appearedQuestions.Add(this.Question.QueID);

                    ++this.SeqOnScreen;
                    this.CurrentSession.SequenceNo = this.SeqOnScreen;
                }

                this.QuestionString = this.SeqOnScreen.ToString() + ". " + this.Question.QuestionString;
                this.QuestionOptions = OptionsList.Where(o => o.QuesId == this.Question.QueID).ToList();

               
               this.CurrentSession.Answer.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            //var result = CheckResult();

        }

        

        private bool checkForduplicacy()
        {
            if(this.appearedQuestions.Contains(this.Question.QueID))
            {
            
                return false;
            }

            return true;
        }

        private bool ProcessAnswer()
        {
            List<Option> selectedOptions = new List<Option>();

            var test = QuestionOptions.Where(o => o.IsCorrect == 1).ToList().Count;

            this.CurrentSession.QuestionId = this.Question.QueID;

            foreach (var option in this.QuestionOptions)
            {
                if (option.IsSelected == 1)
                {
                    selectedOptions.Add(option);
                    this.CurrentSession.Answer.Add(option);
                    option.IsSelected = 0;
                }
            }

            if (this.CurrentSession.Answer.Count == 0)
            {
                return false;
            
            }

            this.CurrentSession.TimeStampValue = this.TimerValue;
            objdata.submitAnswersToDB(this.CurrentSession);


            if (this.Question.Questype == 1 && selectedOptions.Count > 1)
            {
                return false;
            }
            else if (this.Question.Questype == 1 && selectedOptions.Count == 1)
            {
                var result = QuestionOptions.Where(o => o.IsCorrect == 1).FirstOrDefault().Equals(selectedOptions.First());
            }
            else if (this.Question.Questype != 1 && (selectedOptions.Count == 1 || QuestionOptions.Where(o => o.IsCorrect == 1).ToList().Count != selectedOptions.Count))
            {
                return false;
            }
            else if (this.Question.Questype != 1 && selectedOptions.Count > 1)
            {
                foreach (var option in selectedOptions)
                {
                    if (option.IsCorrect != 1)
                    {
                        return false;
                    }

                }
            }
            return true;
        }


    }

    public class Option
    {
        public int? OptionId { get; set; }
        public int QuesId { get; set; }
        public string OptionDescription { get; set; }
        public int? IsSelected
        { get; set; }
        public int? IsCorrect { get; set; }
    }
}
