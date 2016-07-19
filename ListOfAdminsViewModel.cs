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
   public class ListOfAdminsViewModel:ViewModelBase
    {
       private DataAccessADO objData;
       public ListOfAdminsViewModel()
       {
           if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
           {
               objData = new DataAccessADO();
                AdminsList = objData.getAllAdmins().ToList();
                this.CurrentSession.PropertyChanged += CurrentSession_PropertyChanged;
           }
       
       }

       void CurrentSession_PropertyChanged(object sender, PropertyChangedEventArgs e)
       {
           if (e.PropertyName == "NewlyCreatedAdmin")
           {
               AdminsList = objData.getAllAdmins().ToList();
           }
       }

       private List<Admin> adminsList;

       public List<Admin> AdminsList
       {
           get { return adminsList; }
           set { adminsList = value; OnPropertyChanged(() => this.AdminsList); }
       }
        
    }

  

	
}
