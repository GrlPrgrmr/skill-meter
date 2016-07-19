using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SkillMeter.ViewModels;

namespace SkillMeter.Models
{
   public class QuestionSet:INotifyPropertyChanged, IDataErrorInfo
    {

       private Dictionary<string, bool> propertiesToBeValidated = new Dictionary<string, bool>();

       public QuestionSet()
       {
           propertiesToBeValidated.Add("SetName", true);
           propertiesToBeValidated.Add("Description", true);
           propertiesToBeValidated.Add("Category", true);
           propertiesToBeValidated.Add("Owner", true);
           propertiesToBeValidated.Add("FilePath", true);
       

       }


       ///// <summary>
       ///// Returns true if this object has no validation errors.
       ///// </summary>
       //public bool IsValid
       //{
       //    get
       //    {
       //        foreach (bool property in propertiesToBeValidated.Values)
       //        {

       //            if (GetValidationError(property) != null) // there is an error
       //                return false;
       //        }

       //        return true;
       //    }
       //}

       private object GetValidationError(string property)
       {
           throw new NotImplementedException();
       }

        private int setId;

        public int SetId
        {
            get { return setId; }
            set { setId = value; }
        }

        private int ownerId;

        public int OwnerId
        {
            get { return ownerId; }
            set { ownerId = value; }
        }
        

        private string setName;

        public string SetName
        {
            get { return setName; }
            set { setName = value; this.RaisePropertyChanged("SetName"); }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; this.RaisePropertyChanged("Description"); }
        }


        private string category;

        public string Category
        {
            get { return category; }
            set { category = value; this.RaisePropertyChanged("Category"); }
        }

        private string owner;

        public string Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        private int questionsPerTest;

        public int QuestionsPerTest
        {
            get { return questionsPerTest; }
            set { questionsPerTest = value; this.RaisePropertyChanged("QuestionsPerTest"); }
        }
        

        private string timeStamp;

        public string TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; this.RaisePropertyChanged("TimeStamp"); }
        }


        private string filePath;

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
        

        public string Error
        {
            get { return (this as IDataErrorInfo).Error; }
        }

        public string this[string propertyName]
        {
            
            get
            {
                

                string validationResult = null;
                switch (propertyName)
                {
                    case "SetName":
                        {
                            validationResult = validateProperty(this.SetName,"SetName");
                            propertiesToBeValidated[propertyName] = String.IsNullOrEmpty(validationResult) ? true : false;    
                            break;
                        }
                    case "Description":
                        {
                            validationResult = validateProperty(this.Description,"Description");
                            propertiesToBeValidated[propertyName] = String.IsNullOrEmpty(validationResult) ? true : false;    
                            break;
                        }
                    case "Category":
                        {
                            validationResult = validateProperty(this.Category,"Category");
                            propertiesToBeValidated[propertyName] = String.IsNullOrEmpty(validationResult) ? true : false;    
                            break;
                        }
                    case "Owner":
                        {
                            validationResult = validateProperty(this.Owner,"Owner");
                            propertiesToBeValidated[propertyName] = String.IsNullOrEmpty(validationResult) ? true : false;    
                            break;
                        }
                    case "FilePath":
                        {

                            validationResult = validateProperty(this.FilePath, "FilePath");
                            propertiesToBeValidated[propertyName] = String.IsNullOrEmpty(validationResult) ? true : false;    
                            break;
                        }
                    //case "QuestionPerTest":
                    //    {

                    //        validationResult = validateQuestionsPerTest(t);
                    //    }

                        
                
                }

                ValidateProperties();

                return validationResult;
                
                }
             

            
        }


        private void ValidateProperties()
        {
            foreach (bool isValid in propertiesToBeValidated.Values)
            {
                if (!isValid)
                {
                    this.AllPropertiesValid = false;
                    return;
                }
            }
            this.AllPropertiesValid = true;
        }

        private bool allPropertiesValid;
        public bool AllPropertiesValid
        {
            get { return allPropertiesValid; }
            set
            {
                if (allPropertiesValid != value)
                {
                    allPropertiesValid = value;
                    this.RaisePropertyChanged("AllPropertiesValid");
                }
            }
        }


        private string validateProperty(string inputProperty,string propName)
        {
            Regex reg = new Regex("^[a-zA-Z0-9_]");

            if (String.IsNullOrEmpty(inputProperty))
            {
                return propName+" " + " needs to be Entered";
            }
            else if (propName == "FilePath")
            {

                
                Regex fileRegex = new Regex(@"([^\s]+(\.(?i)(xls|xlsx))$)");
                if (!fileRegex.IsMatch(inputProperty))
                {
                    return "Invalid File Path";
                }
                else
                {
                    return String.Empty;
                }
            }
            else if (inputProperty.Length > 50 && propName != "FilePath")
            {
                return "Too many characters in the input";
            }

            else if (!reg.IsMatch(inputProperty))
            {

                return "Special Characters are not allowed";

            }
            else
            {
                return String.Empty;
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }    
    }
}
