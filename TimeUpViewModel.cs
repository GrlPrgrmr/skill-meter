using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SkillMeter.DataAccessLayer;

namespace SkillMeter.ViewModels
{
    public class TimeUpViewModel:ViewModelBase
    {

        private DataAccessADO dataObj;

        public TimeUpViewModel()
        {
            dataObj = new DataAccessADO();
        
        }
        private ICommand exitCommand;

        public ICommand ExitCommand
        {
            get { return new RelayCommand(param => exitApplication()); }

        }

        private void exitApplication()
        {
            ResultNoNegative resultObj = new ResultNoNegative();

            if (dataObj.getUserTestsData().Where(r => r.UserName == this.CurrentSession.UserName && r.Attempted==0).Select(r => r.Attempted).FirstOrDefault() == 0)
            {
                resultObj.prepareResult(this.CurrentSession.TestId);
            }
           

            Application.Current.Shutdown();
        }
        
    }
}
