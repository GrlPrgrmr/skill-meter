using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SkillMeter.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        ObservableCollection<object> _children;
        public ObservableCollection<object> Children { get { return _children; } }

        public MainWindowViewModel()
        {

            try
            {

                _children = new ObservableCollection<object>();
                _children.Add(new AdminScreenViewModel());
                this.CurrentSession.PropertyChanged += this.CurrentSession_PropertyChanged;
                this.CurrentSession.VMBInstance = "LoginViewModel";
              
              

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CurrentSession_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VMBInstance")
            {
                switch (this.CurrentSession.VMBInstance)
                {

                    case "QuestionViewModel":
                        {
                            this.SelectedVM = new QuestionViewModel();

                            if (this.CurrentSession.AppTimeOut == true)
                            {
                                this.SelectedVM = new TimeUpViewModel();
                            }
                            break;
                        }
                    case "TimeUpViewModel":
                        {
                            this.SelectedVM = new TimeUpViewModel();
                            break;
                        }
                    case "LoginViewModel":
                        {
                            this.SelectedVM = new LoginViewModel();
                            break;
                        }
                    case "AdminScreenViewModel":
                        {
                            this.SelectedVM = new AdminScreenViewModel();
                            break;
                        }
                    case "ViewTestResultViewModel":
                        {
                            this.SelectedVM = new ViewTestResultViewModel();
                            break;
                        }
                    case "SuperAdminViewModel":
                        {
                            this.SelectedVM = new SuperAdminViewModel();
                            break;
                        }

                }


            }
        }


        private ViewModelBase selectedVM;

        public ViewModelBase SelectedVM
        {
            get { return selectedVM; }
            set { selectedVM = value; OnPropertyChanged(() => this.SelectedVM); }
        }

        

        
    }
}
