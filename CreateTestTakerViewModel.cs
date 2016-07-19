using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillMeter.DataAccessLayer;
using SkillMeter.ViewModels;
using SkillMeter.Models;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;

namespace SkillMeter.ViewModels
{
   public class CreateTestTakerViewModel:ViewModelBase
   {

      
       private DataAccessADO objData;
      


       public CreateTestTakerViewModel()
       {
           if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
           {
               CandidateEntry = new TestCandidate();
               

               objData = new DataAccessADO();
              
               
               this.CandidateEntry.PropertyChanged+=CandidateEntry_PropertyChanged;

               this.CurrentSession.PropertyChanged+=CurrentSession_PropertyChanged;

               QuestionSets = objData.getQuestionSetData(this.CurrentSession.UserName).Select(r => r.SetName).ToList();

               DurationOptions = new List<int>();
               DurationOptions.Add(5);
               DurationOptions.Add(10);
               DurationOptions.Add(15);
               DurationOptions.Add(30);
               DurationOptions.Add(60);
           }
       
       }

       private void CurrentSession_PropertyChanged(object sender, PropertyChangedEventArgs e)
       {
           if (e.PropertyName == "QuestionSetForDropDownTestTaker")
           {
               QuestionSets = objData.getQuestionSetData(this.CurrentSession.UserName).Select(r => r.SetName).ToList();
           }
       }

      

       private void CandidateEntry_PropertyChanged(object sender, PropertyChangedEventArgs e)
       {
           if (e.PropertyName == "AllPropertiesValid")
           {
               this.CanSubmit = this.CandidateEntry.AllPropertiesValid;
           }
           
       }

       #region properties

       private TestCandidate candidateEntry;
       public TestCandidate CandidateEntry
       {

           get { return candidateEntry; }
           set { candidateEntry = value; OnPropertyChanged(()=>this.CandidateEntry); }
       }


       private QuestionSet question;

       public QuestionSet Question
       {
           get { return question; }
           set { question = value; OnPropertyChanged(()=>this.Question); }
       }
       
     
       private bool canSubmit;
       public bool CanSubmit
       {
           get { return canSubmit; }
           set { canSubmit = value; OnPropertyChanged(()=>this.CanSubmit); }
       }
       
       private List<string> questionSets;

       public List<string> QuestionSets
       {
           get { return questionSets; }
           set { questionSets = value; OnPropertyChanged(()=>this.QuestionSets); }
       }

       private List<int> durationOptions;

       public List<int> DurationOptions
       {
           get { return durationOptions; }
           set { durationOptions = value; OnPropertyChanged(()=>this.DurationOptions); }
       }
       

       #endregion

       //#region Validation

       //public string Error
       //{
       //    get { return (candidateEntry as IDataErrorInfo).Error; }
       //}

       //public string this[string propertyName]
       //{
       //    get
       //    {
       //        string error = (candidateEntry as IDataErrorInfo)[Error];
       //        validProperties[propertyName] = String.IsNullOrEmpty(error) ? true : false;
       //        ValidateProperties();
       //        CommandManager.InvalidateRequerySuggested();
       //        return error;
       //    }
       //}

       //private void ValidateProperties()
       //{
       //    foreach (bool isValid in validProperties.Values)
       //    {
       //        if (!isValid)
       //        {
       //            this.AllPropertiesValid = false;
       //            return;
       //        }
       //    }
       //    this.AllPropertiesValid = true;
       //}

       //private bool allPropertiesValid;
       //public bool AllPropertiesValid
       //{
       //    get { return allPropertiesValid; }
       //    set
       //    {
       //        if (allPropertiesValid != value)
       //        {
       //            allPropertiesValid = value;
       //            base.OnPropertyChanged("AllPropertiesValid");
       //        }
       //    }
       //}
       //#endregion

       

       public ICommand CreateCandidateCommand
       {
           get { return new RelayCommand(param=> createTestCandidate()); }
           
       }

       private void createTestCandidate()
       {
           this.CandidateEntry.IsAdmin = 0;

           try
           {

              bool result = objData.createTestTakerProfile(this.CandidateEntry, this.CurrentSession.UserName);

              if (result == true)
              {
                  MessageBox.Show("Test Candidate " + this.CandidateEntry.UserName + " Profile has been created successfully.");
                  this.CurrentSession.NewlyCreatedCandidate = this.CandidateEntry.UserName;
                  refreshObjects();

                 
              }
              else
              {
                  MessageBox.Show("Test Taker profile could not created successfully...UserName already exists...");
                  refreshObjects();
              }
           }
           catch (Exception ex)
           {

               MessageBox.Show("Test Taker profile could not created successfully...please try after some time"+ex.ToString());
               refreshObjects();
           }
       }

       private void refreshObjects()
       {
           this.CandidateEntry.UserName = String.Empty;
           this.CandidateEntry.Password = String.Empty;
           
           this.CandidateEntry.AllPropertiesValid = false;
           OnPropertyChanged(() => this.CandidateEntry);
       }
       
   }
}
