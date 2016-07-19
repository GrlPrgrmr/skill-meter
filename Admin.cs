using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SkillMeter.Models
{
   public class Admin
    {


        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value.ToLower(); }
        }

        private string timeStamp;

        public string TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }
        
        

    }

  
}
