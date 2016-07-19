using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillMeter.Models;
using SkillMeter.DataAccessLayer;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;


namespace SkillMeter.ViewModels
{
   public class LoginViewModel:ViewModelBase
    {
       DataAccessADO dataObj;
       User userObj;
       UserTest userTestObj;

        public LoginViewModel()
        {
            Submit = false;
            dataObj = new DataAccessADO();
            userObj = new User();
            userTestObj = new UserTest();
            
        }



        #region Properties
        private string userName;

        public string UserName
        {
            get
            {
                if (userName == String.Empty || userName == null || _password == String.Empty || _password == null)
                {
                    Submit = false;

                }
                return userName;
            }
            set
            {
                userName = value.ToLower();
                if (value != String.Empty || value != null)
                {
                    Submit = true;

                }
                OnPropertyChanged(() => this.UserName);
            }
        }

        private bool submit;

        public bool Submit
        {
            get { return submit; }
            set { submit = value; OnPropertyChanged(() => this.Submit); }
        }

        private string _password;

        public string Password
        {
            get
            {
                if (_password == String.Empty || _password == null || userName == String.Empty || userName == null)
                {
                    Submit = false;

                }
                return _password;
            }

            set
            {
                _password = value;
                if (value != String.Empty || value != null)
                {
                    Submit = true;

                }
                OnPropertyChanged(() => this.Password);
            }
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {

            get
            {
                try
                {

                    return new RelayCommand(param => Login());
                }
                catch (Exception ed)
                {
                    MessageBox.Show("Invalid Login Credentials");
                    return null;
                }
            }

        }


        public bool Login()
        {
            try
            {

                    this.CurrentSession.UserName =this.UserName;

                userObj = dataObj.getUserData(this.UserName);

                if (userObj == null || userObj.Password!= this.Password)
                {
                    MessageBox.Show("Invalid Login Details");
                    userObj = new User() { UserName=String.Empty,Password=String.Empty};
                    return false;
                }
                else
                {
                   if(userObj.UserName=="superadmin")
                    {

                        this.CurrentSession.VMBInstance = "SuperAdminViewModel";
                    
                    }
                    else if (userObj.IsAdmin == 1)
                    {

                        this.CurrentSession.VMBInstance = "AdminScreenViewModel";
                    }
                    else
                    {
                        this.CurrentSession.VMBInstance = "QuestionViewModel";
                    }
                    return true;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid Login Credentials");

                return false;
            }

            finally
            {
                //this.Session = null;
            }
        }
        #endregion

    }
}
