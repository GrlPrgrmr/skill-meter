using SkillMeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillMeter.DataAccessLayer;

namespace SkillMeter.ViewModels
{
   public class ViewTestResultViewModel:ViewModelBase
    {
        DataAccessADO objData;
        
        public ViewTestResultViewModel()
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                objData = new DataAccessADO();
                List<QuestionSet> setsData = new List<QuestionSet>();
                this.CurrentSession.PropertyChanged += CurrentSession_PropertyChanged;
                setsData = objData.getQuestionSetData(this.CurrentSession.UserName);

                List<UserTest> userTestData = new List<UserTest>();
                userTestData = objData.getUserTestsData();

                List<QuestionSet> allSets = objData.getAllQuestionSets().ToList();

               

               var scoreResultForCurrentUser = objData.getUserTestsData().ToList();
                var joinData = (from test in scoreResultForCurrentUser
                               join set in allSets  on test.SetId equals set.SetId
                               select new UserTest
                               {
                                   SetName = set.SetName,
                                   Date= test.Date,
                                   Score =test.Score,
                                   TestName = test.TestName,
                                   TestId = test.TestId,
                                   UserName =test.UserName,
                                   Attempted =test.Attempted,
                                   CorrectlyAnswered=test.CorrectlyAnswered,
                                   TestGiven =test.TestGiven

                               }).ToList();

                this.ScoreData = joinData;



                int count = scoreResultForCurrentUser.Count;
            }
        
        }

        private void CurrentSession_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "NewlyCreatedCandidate" || e.PropertyName == "CandidateDeleted")
            {
                List<QuestionSet> allSets = objData.getAllQuestionSets().ToList();
                var scoreResultForCurrentUser = objData.getUserTestsData().ToList();
                var joinData = (from test in scoreResultForCurrentUser
                                join set in allSets on test.SetId equals set.SetId
                                select new UserTest
                                {
                                    SetName = set.SetName,
                                    Date = test.Date,
                                    Score = test.Score,
                                    TestName = test.TestName,
                                    TestId = test.TestId,
                                    UserName = test.UserName,
                                    Attempted = test.Attempted,
                                    CorrectlyAnswered = test.CorrectlyAnswered,
                                    TestGiven =test.TestGiven

                                }).ToList();

                ScoreData = joinData;

            }
          
        }


        private List<UserTest> scoreData;

        public List<UserTest> ScoreData
        {
            get { return scoreData; }
            set { scoreData = value; OnPropertyChanged(()=>this.ScoreData); }
        }
        

    }
}
