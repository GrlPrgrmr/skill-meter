using SkillMeter.DataAccessLayer;
using SkillMeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SkillMeter.ViewModels
{
    class AssignTestViewModel:ViewModelBase
    {
        private DataAccessADO objData;

        public AssignTestViewModel()
        {

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                CandidateEntry = new TestCandidate();
                objData = new DataAccessADO();
                this.CurrentSession.PropertyChanged += CurrentSession_PropertyChanged;

                UserNames = objData.getAllUsers().Where(r => r.IsAdmin == 0).Select(r => r.UserName).ToList();
                QuesSets = objData.getQuestionSetData(this.CurrentSession.UserName).Select(r => r.SetName).ToList();
                DurationOptions = new List<int>();
                DurationOptions.Add(5);
                DurationOptions.Add(10);
                DurationOptions.Add(15);
                DurationOptions.Add(30);
                DurationOptions.Add(60);
                this.CanSubmit = true;
                
            }
        
        }

        private void CurrentSession_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "QuestionSetForDropDownTestTaker")
            {
                QuesSets = objData.getQuestionSetData(this.CurrentSession.UserName).Select(r => r.SetName).ToList();
            }
            if (e.PropertyName == "NewlyCreatedCandidate")
            {
                UserNames = objData.getAllUsers().Where(r => r.IsAdmin == 0).Select(r => r.UserName).ToList();
            }
        }

        private bool canSubmit;

        public bool CanSubmit
        {
            get { return canSubmit; }
            set { canSubmit = value; OnPropertyChanged(() => this.CanSubmit); }
        }


        private TestCandidate candidateEntry;
        public TestCandidate CandidateEntry
        {

            get { return candidateEntry; }
            set { candidateEntry = value; OnPropertyChanged(() => this.CandidateEntry); }
        }

        private List<string> userNames;

        public List<string> UserNames
        {
            get { return userNames; }
            set { userNames = value; OnPropertyChanged(()=>this.UserNames); }
        }


        private List<string> quesSets;

        public List<string> QuesSets
        {
            get { return quesSets; }
            set
            {
                quesSets = value;
                OnPropertyChanged(()=>this.QuesSets); 
            }
        }


        private List<int> durationOptions;

        public List<int> DurationOptions
        {
            get { return durationOptions; }
            set { durationOptions = value; OnPropertyChanged(() => this.DurationOptions); }
        }
        public ICommand AssignTestCommand
        {
            get { return new RelayCommand(param=>assignTest()); }
        }

        private void assignTest()
        {

            try
            {
                if (String.IsNullOrEmpty(this.CandidateEntry.UserName ) ||  String.IsNullOrEmpty(this.CandidateEntry.QuestionSet )|| String.IsNullOrEmpty(this.CandidateEntry.TestDuration.ToString()))
                {

                    MessageBox.Show("Please Select All the Choices From Drop Down");
                    return;
                }
                var previousTests = objData.getUserTestsData().Where(r => r.UserName == this.CandidateEntry.UserName).ToList();

                foreach (var test in previousTests)
                {

                    if (test.TestGiven == "No")
                    {
                        MessageBox.Show("Please Let Candidate Finish Previously Assigned Test!!");
                        this.CandidateEntry.UserName = String.Empty;
                        this.CandidateEntry.QuestionSet = String.Empty;
                        this.CandidateEntry.TestDuration = String.Empty;
                        return;
                    }
                   
                }

                 
                   var result = objData.assignNewTestToCandidate(this.CandidateEntry,this.CurrentSession.UserName);

                   if (result == false)
                   {
                       MessageBox.Show("Problem Assigning Test...Please Try After Some Time!!!");
                       this.CandidateEntry.UserName = String.Empty;
                       this.CandidateEntry.QuestionSet = String.Empty;
                       this.CandidateEntry.TestDuration = String.Empty;
                       return;
                   }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem Assigning Test...Please Try After Some Time!!!");
                this.CandidateEntry.UserName = String.Empty;
                this.CandidateEntry.QuestionSet = String.Empty;
                this.CandidateEntry.TestDuration = String.Empty;
                return;
            
            
            }
           
        }
        
    }
}
