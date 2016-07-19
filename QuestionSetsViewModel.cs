using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillMeter.DataAccessLayer;
using SkillMeter.Models;
using System.Collections.ObjectModel;

namespace SkillMeter.ViewModels
{
    class QuestionSetsViewModel
        :ViewModelBase
    {

        DataAccessADO dataObj;
       public QuestionSetsViewModel()
        {

            dataObj = new DataAccessADO();

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {

                Sets = new ObservableCollection<QuestionSet>( dataObj.getAllQuestionSets().Where(r=>r.Owner==this.CurrentSession.UserName).ToList());
                SetNames = Sets.Select(r => r.SetName).ToList();
                this.CurrentSession.PropertyChanged += CurrentSession_PropertyChanged;

            }
        
        }

       private void CurrentSession_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
       {
           if (e.PropertyName == "QuestionSetForDropDownTestTaker")
           {
               Sets =new ObservableCollection<QuestionSet>( dataObj.getAllQuestionSets().Where(r => r.Owner == this.CurrentSession.UserName).ToList());
               SetNames = Sets.Select(r => r.SetName).ToList();

           }
       }

       private List<string> setNames;

       public List<string> SetNames
       {
           get { return setNames; }
           set { setNames = value; OnPropertyChanged(()=>this.SetNames); }
       }
       

       private ObservableCollection<QuestionSet> sets;

       public ObservableCollection<QuestionSet> Sets
       {
           get { return sets;  }
           set { sets = value; OnPropertyChanged(() => this.Sets); }
       }
        
    }
}
