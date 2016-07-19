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
   public class CreateAdminViewModel:ViewModelBase
    {
       private DataAccessADO objData;
       public CreateAdminViewModel()
       {
           if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
           {
               AdminUser = new User();
               objData = new DataAccessADO();
               this.LoggedInUser = "Welcome " + this.CurrentSession.UserName;
               this.AdminUser.PropertyChanged += AdminUser_PropertyChanged;
               

           }
       
       }


        void AdminUser_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AllPropertiesValid")
            {
                this.CanSubmit = this.AdminUser.AllPropertiesValid;
            }

        }


        private string loggedInUser;

        public string LoggedInUser 
        {
            get { return loggedInUser; }
            set { loggedInUser = value; OnPropertyChanged(()=>this.LoggedInUser);}
        }
        
        private bool canSubmit;

        public bool CanSubmit
        {
            get { return canSubmit; }
            set { canSubmit = value; OnPropertyChanged(()=>this.CanSubmit); }
        }
        
        private User adminUser;
        public User AdminUser 
        {
            get { return adminUser; }
            set{adminUser =value;OnPropertyChanged(()=>this.AdminUser);}
        }


        public ICommand SubmitAdminCommand
        {
            get { return new RelayCommand(param => SubmitAdminRecord()); }

        }

        public ICommand LogOutCommand
        {
            get { return new RelayCommand(param => PerformLogout()); }

        }

        private void PerformLogout()
        {

            this.CurrentSession.VMBInstance = "LoginViewModel";


        }
        private void SubmitAdminRecord()
        {
            this.adminUser.IsAdmin = 1;
            try
            {
               bool result = objData.createAdminProfile(adminUser);
               if (result == false)
               {
                   MessageBox.Show("UserName Already Exists!!!Please Choose Other Name");
                   refreshObjects();

                  
                   
               }
               else
               {
                   this.CurrentSession.NewlyCreatedAdmin = this.AdminUser.UserName;
                   MessageBox.Show("Admin Profile Created Successfully...");
                   refreshObjects();
                  
               }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Admin cannot be created!!! \n" + ex.ToString());
                refreshObjects();
            }

        }

        private void refreshObjects()
        {
            this.AdminUser.AllPropertiesValid = false;
            this.AdminUser.UserName = String.Empty;
            this.AdminUser.Password = String.Empty;
            this.AdminUser.IsAdmin = 0;
            this.AdminUser.QuestionSet = String.Empty;

            OnPropertyChanged(() => this.AdminUser);

        }





    }

    }

