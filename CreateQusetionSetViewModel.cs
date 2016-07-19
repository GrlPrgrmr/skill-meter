using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillMeter.Models;
using SkillMeter.DataAccessLayer;
using System.Windows.Input;
using System.ComponentModel;
using Microsoft.Win32;
using System.Windows;

namespace SkillMeter.ViewModels
{
    class CreateQusetionSetViewModel :ViewModelBase
    {
       
        
        private DataAccessADO objData;

        public CreateQusetionSetViewModel()
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                try
                {
                   
                    QuestionSet = new QuestionSet();
                    objData = new DataAccessADO();

                    this.QuestionSet.PropertyChanged+=QuestionSet_PropertyChanged;  
                    Category = new List<string>();
                    OptionsInNumberOfQues = new List<int>();
                    var allSets = objData.getAllQuestionSets();


                    this.Category.Add("Automation");
                    this.Category.Add("Finance");
                    this.Category.Add("Training");
                    this.Category.Add("HR");

                    //foreach (var set in allSets)
                    //{

                    //    this.Category.Add(set.Category.ToString());

                    //}

                    //if (this.Category.All(s => String.IsNullOrWhiteSpace(s)))
                    //{
                    //    this.Category.Add("Automation");
                    //    this.Category.Add("Finance");
                    //    this.Category.Add("Training");
                    //    this.Category.Add("HR");
                    
                    //}
                    this.QuestionSet.Owner = this.CurrentSession.UserName;
                    this.CurrentSession.UserId = objData.getAllUsers().Where(r => r.UserName == this.CurrentSession.UserName).SingleOrDefault().UserId;
                    
                    this.OptionsInNumberOfQues.Add(5);
                    this.OptionsInNumberOfQues.Add(10);
                    this.OptionsInNumberOfQues.Add(15);
                    this.OptionsInNumberOfQues.Add(20);
                    this.OptionsInNumberOfQues.Add(25);
                    this.OptionsInNumberOfQues.Add(30);
                  
                    
                   
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString() + ex.InnerException.ToString());
                }
            }
        }

        private void QuestionSet_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AllPropertiesValid")
            {
                this.CanSubmit = this.QuestionSet.AllPropertiesValid;

            }
            else if (e.PropertyName == "FilePath")
            {
                this.FilePath = this.QuestionSet.FilePath;
            }

        }


        #region properties

        private QuestionSet questionSet;
        public QuestionSet QuestionSet 
        {
            get { return questionSet; }
            set { questionSet = value; OnPropertyChanged(() => this.QuestionSet); } 
        }



        private string filePath;

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; OnPropertyChanged(()=>this.FilePath); }
        }
        

        private bool canSubmit;

        public bool CanSubmit
        {
            get { return canSubmit; }
            set { canSubmit = value; OnPropertyChanged(() => this.CanSubmit); }
        }
        
        private List<string> category;
        public List<string> Category
        {
            get { return category; }
            set { category = value; OnPropertyChanged(() => this.Category); }
        }

        


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


        #endregion

        #region Commands
        private ICommand openFileCommand;

        public ICommand OpenFileCommand
        {
            get
            {
                return new RelayCommand(param => SelectFile());
            }

        }


        private List<int> optionsInNumberOfQues;

        public List<int> OptionsInNumberOfQues
        {
            get { return optionsInNumberOfQues; }
            set { optionsInNumberOfQues = value; OnPropertyChanged(()=>this.OptionsInNumberOfQues); }
        }
        

        public ICommand SubmitSetCommand
        {
            get { return new RelayCommand(param => SubmitSetData()); }

        }




        #endregion

        #region command helpers

        private void SubmitSetData()
        {
            try
            {
                this.QuestionSet.OwnerId = this.CurrentSession.UserId;


                bool resultSubmission = objData.submitSetToDB(this.QuestionSet);

                if (resultSubmission == false)
                {

                    MessageBox.Show("Test Name Already Exists!!! Please Choose a Different Name.");
                    return;
                }

                this
                    .QuestionSet.SetId = objData.getQuestionSetData(this.CurrentSession.UserName, this.QuestionSet.SetName).SetId;
               


                bool result = objData.storeQuestionsToDB(this.QuestionSet.SetId, this.QuestionSet.FilePath, this.QuestionSet.QuestionsPerTest.ToString());


                if (result)
                {
                    MessageBox.Show("Data loaded successfully...");
                    this.CurrentSession.QuestionSetForDropDownTestTaker = this.QuestionSet.SetName;
                    refreshViewObject();
                    
                }
                else
                {
                    MessageBox.Show("File Data Is Inconsistent with the Requirements!!!Please check the data and template format");
                    objData.deleteRowFromTable("QuestionSets","SetID="+this.QuestionSet.SetId);
                    refreshViewObject();
                    return;
                }

               
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "There is a problem with data loading...");
                refreshViewObject();
            }
        }

        private void refreshViewObject()
        {
            this.QuestionSet.AllPropertiesValid = false;
            this.QuestionSet.Category = String.Empty;
            this.QuestionSet.Description = String.Empty;
            this.QuestionSet.SetName = String.Empty;
            this.QuestionSet.FilePath = String.Empty;
            //this.QuestionSet.QuestionsPerTest = 5;
            this.QuestionSet.Owner = this.CurrentSession.UserName;
            OnPropertyChanged(() => this.QuestionSet);
        }

        
        private void SelectFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files(*.xls)|*.xlsx;*.xls;*.xlsm";

            ofd.FileOk += new System.ComponentModel.CancelEventHandler(ofd_FileOk);
            var dialodresult = ofd.ShowDialog();
        }

        private void ofd_FileOk(object sender, CancelEventArgs e)
        {
            OpenFileDialog fileDialog = sender as OpenFileDialog;
            string selectedFile = fileDialog.FileName;
            if (string.IsNullOrEmpty(selectedFile) || selectedFile.Contains(".lnk"))
            {
                MessageBox.Show("Please select a valid Excel File");
                e.Cancel = true;
            }
            else
            {
                this.QuestionSet.FilePath = selectedFile;
                this.QuestionSet.RaisePropertyChanged("FilePath");
            }
            return;
        }

        #endregion
        //#region validation
        //public string Error
        //{
        //    get { return (questionSet as IDataErrorInfo).Error; }
        //}

        //public string this[string propertyName]
        //{

        //    get
        //    {
        //        string error = (questionSet as IDataErrorInfo)[propertyName];
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
        //#endregion
    }
}
