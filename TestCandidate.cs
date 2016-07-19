using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SkillMeter.Models
{
    public class TestCandidate : INotifyPropertyChanged, IDataErrorInfo
    {
        private Dictionary<string, bool> propertiesToBeValidated = new Dictionary<string, bool>();

        public TestCandidate()
        {

            propertiesToBeValidated.Add("UserName", true);
            propertiesToBeValidated.Add("Password", true);
        }
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value.ToLower(); }
        }


        private string testGiven;

        public string TestGiven
        {
            get { return testGiven; }
            set { testGiven = value; }
        }
        
        private string pwd;

        public string Password
        {
            get { return pwd; }
            set { pwd = value; }
        }


        private string testDuration;

        public string TestDuration
        {
            get { return testDuration; }
            set { testDuration = value; }
        }
        

        private string questionSet;

        public string QuestionSet
        {
            get { return questionSet; }
            set { questionSet = value; }
        }

        private int isAdmin;

        public int IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; }
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
                    case "UserName":
                        {
                            validationResult = validateProperty(this.UserName, "UserName");

                            break;
                        }
                    case "Password":
                        {
                            validationResult = validatePassword(this.Password);
                            break;
                        }
                    default:
                        {
                            validationResult = String.Empty;
                            break;
                        }

                }

                propertiesToBeValidated[propertyName] = String.IsNullOrEmpty(validationResult) ? true : false;
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
        private string validatePassword(string p)
        {

            if (String.IsNullOrEmpty(p) || p.Length < 8)
            {
                return "Minimum 8 characters are required";
            }
            else if (p.Length > 15)
            {
                return "Maximum limit of characters is 15";
            }
            else
            {

                return String.Empty;
            }
        }

        private string validateProperty(string prop, string propName)
        {

            Regex reg = new Regex("^[a-zA-Z0-9_]");

            if (String.IsNullOrEmpty(prop))
            {
                return propName + " " + " needs to be Entered";
            }
            else if (prop.Length > 50)
            {
                return "Too many characters in the input[Max limit is 50]";
            }

            else if (prop.Length < 4)
            {
                return "Minimum 4 characters are required";
            }
            else if (!reg.IsMatch(prop))
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
