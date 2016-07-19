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
using System.Collections.ObjectModel;

namespace SkillMeter.ViewModels
{
    class AdminScreenViewModel:ViewModelBase
    {
        
        public AdminScreenViewModel()
        {
            this.LoggedInUser ="Welcome "+ this.CurrentSession.UserName;
           
        }

        private string loggedInUser;

        public string LoggedInUser
        {
            get { return loggedInUser; }
            set { loggedInUser = value; OnPropertyChanged(() => this.LoggedInUser); }
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
