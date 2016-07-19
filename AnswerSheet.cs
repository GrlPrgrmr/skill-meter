using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMeter.Models
{
    public class AnswerSheet : INotifyPropertyChanged
    {



        private int queId;

        public int QueId
        {
            get { return queId; }
            set { queId = value; }
        }


        private string quesString;

        public string QuesString
        {
            get { return quesString; }
            set { quesString = value; }
        }


        private string userAnswer;

        public string UserAnswer
        {
            get { return userAnswer; }
            set { userAnswer = value; }
        }

        private string correctAnswer;

        public string CorrectAnswer
        {
            get { return correctAnswer; }
            set { correctAnswer = value; }
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
