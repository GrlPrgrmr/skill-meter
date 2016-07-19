using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillMeter.DataAccessLayer;
using SkillMeter.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace SkillMeter.ViewModels
{
   public class AnswerSheetViewModel:ViewModelBase
    {
       private DataAccessADO dataObj;
       
       public AnswerSheetViewModel()
       {

           if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
           {

             
               dataObj = new DataAccessADO();

               this.CurrentSession.PropertyChanged += CurrentSession_PropertyChanged;
               //extracting candidate names from users table to feed combo box in UI
               CandidateNames =new ObservableCollection<string>( dataObj.getUserTestsData().Where(r => r.TestGiven == "Yes").Select(r => r.UserName).ToList());
               AllCandidateNames =new ObservableCollection<string>( dataObj.getAllUsers().Where(r => r.IsAdmin == 0).Select(r=>r.UserName).ToList());
           }

       }

       void CurrentSession_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
       {
           if (e.PropertyName == "NewlyCreatedCandidate")
           {
               AllCandidateNames = new ObservableCollection<string>(dataObj.getAllUsers().Where(r => r.IsAdmin == 0).Select(r => r.UserName).ToList());
              
           }
           
       }




       private string candidateName;

       public string CandidateName
       {
           get { return candidateName; }
           set { candidateName = value; this.OnPropertyChanged("CandidateName"); }
       }
        
      
       private string deleteCandidateName;

       public string DeleteCandidateName
       {
           get { return deleteCandidateName; }
           set { deleteCandidateName = value; this.OnPropertyChanged("DeleteCandidateName"); }
       }


       private ObservableCollection<String> candidateNames;

       public ObservableCollection<String> CandidateNames
       {
           get { return candidateNames; }
           set { candidateNames = value; OnPropertyChanged(()=>this.CandidateNames); }
       }

       private ObservableCollection<String> allcandidateNames;

       public ObservableCollection<String> AllCandidateNames
       {
           get { return allcandidateNames; }
           set { allcandidateNames = value; OnPropertyChanged(() => this.AllCandidateNames); }
       }


       private List<AnswerSheet> userAnswerSheet;

       public List<AnswerSheet> UserAnswerSheet
       {
           get { return userAnswerSheet; }
           set { userAnswerSheet = value; OnPropertyChanged(() => this.UserAnswerSheet); }
       }


       private ICommand submitCandidateNameCommand;

       public ICommand SubmitCandidateNameCommand
       {
           get { return new RelayCommand(param=>SubmitCandidateName()); }
          
       }


       private ICommand deleteCandidateCommand;

       public ICommand DeleteCandidateCommand
       {
           get { return new RelayCommand(param=>DeleteCandidate()); }
           
       }

     

       #region command helpers

       private void SubmitCandidateName()
       {
           var testName = dataObj.getUserTestData(this.CandidateName).TestName;
           var testId = dataObj.getUserTestData(this.CandidateName).TestId;

           var examSessionData = dataObj.getUserSessionData(testId).ToList();

           var quesList = dataObj.getQuestions().Where(r => examSessionData.Select(p=>p.QuestionId).Contains(r.QueID)).ToList();

          var userAnswers = examSessionData.SelectMany(r => r.Answer.Split(',')).ToList<string>();

           var optionStrings = dataObj.getOptions().Where(r => userAnswers.Contains(r.OptionId.ToString())).Select(r=>r).ToList();

           var QuesAnswerUser = (from o in optionStrings
                      group o by o.QuesId into userChoicesPerQues
                      select new 
                      {
                          QuesId= userChoicesPerQues.Key,
                          AnswerFromUser = String.Join( " , ", userChoicesPerQues.Select(g=>g.OptionDescription).ToList())

                      }).ToList();

           var correctOptionsFromDb = dataObj.getOptions().Where(r => quesList.Select(q => q.QueID).Contains(r.QuesId) && r.IsCorrect==1).ToList();

           var QuesAnswerDB = (from o in correctOptionsFromDb
                               group o by o.QuesId into correctAnswersQuestion
                               select new 
                               {
                                   QuesId = correctAnswersQuestion.Key,
                                   AnswerFromDb =String.Join( " , ", correctAnswersQuestion.Select(r=>r.OptionDescription).ToList())
                               
                               }).ToList();

           UserAnswerSheet = (from q in quesList
                             join o in QuesAnswerUser on q.QueID equals o.QuesId
                             join c in QuesAnswerDB on q.QueID equals c.QuesId
                             select new AnswerSheet 
                             {
                                QueId = q.QueID,
                                QuesString = q.QuestionString,
                                UserAnswer = o.AnswerFromUser,
                                CorrectAnswer = c.AnswerFromDb
                             }).ToList();
       }


       private void DeleteCandidate()
       {
           try
           {
               if (this.DeleteCandidateName == null)
               {
                   return;
               }
               dataObj.deleteCandidateFromDatabase(this.DeleteCandidateName);
               this.AllCandidateNames.Remove(this.DeleteCandidateName);

               if (CandidateNames.Contains(this.DeleteCandidateName))
               {
                   this.CandidateNames.Remove(this.DeleteCandidateName);
               }
               this.CurrentSession.CandidateDeleted = this.DeleteCandidateName;
           }
           catch (Exception ex)
           {
               MessageBox.Show("Problem with Candidate Deletion!!!Please try after sometime");
           }
       }
       
       #endregion


    }


	

}
