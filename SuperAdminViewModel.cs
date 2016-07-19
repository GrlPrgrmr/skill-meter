using SkillMeter.DataAccessLayer;
using SkillMeter.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SkillMeter.ViewModels
{
    public class SuperAdminViewModel : ViewModelBase
    {

        
      

        public SuperAdminViewModel()
        {
            this.LoggedInUser = "Welcome " + this.CurrentSession.UserName;
        }

        private string loggedInUser;

        public string LoggedInUser
        {
            get { return loggedInUser; }
            set { loggedInUser = value.ToLower(); OnPropertyChanged(() => this.LoggedInUser); }
        }



        public ICommand LogOutCommand
        {
            get { return new RelayCommand(param => PerformLogout()); }

        }

        private void PerformLogout()
        {

            this.CurrentSession.VMBInstance = "LoginViewModel";


        }

    }
}
