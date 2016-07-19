using SkillMeter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMeter.DataAccessLayer
{
    public class Question : ViewModelBase
    {

        private string _questionString;

        public string QuestionString
        {
            get { return _questionString; }
            set { _questionString = value; OnPropertyChanged(() => this.QuestionString); }
        }

        private int _queID;

        public int QueID
        {
            get { return _queID; }
            set { _queID = value; OnPropertyChanged(() => this.QueID); }
        }

        private int _complexity;

        public int Complexity
        {
            get { return _complexity; }
            set { _complexity = value; OnPropertyChanged(() => this.Complexity); }
        }



        private List<Option> _optionlist;

        public List<Option> OptionList
        {
            get { return _optionlist; }
            set { _optionlist = value; OnPropertyChanged(() => this.OptionList); }
        }


        private string _correctOption;

        public string CorrectOption
        {
            get { return _correctOption; }
            set { _correctOption = value; OnPropertyChanged(() => this.CorrectOption); }
        }

        private long? _quesSetID;

        public long? QuesSetID
        {
            get { return _quesSetID; }
            set { _quesSetID = value; OnPropertyChanged(() => this.QuesSetID); }
        }

        private int _quesType;

        public int Questype
        {
            get { return _quesType; }
            set { _quesType = value; OnPropertyChanged(() => this.Questype); }
        }

        private int _seqOnScreen;

        public int SeqOnScreen
        {
            get { return _seqOnScreen; }
            set { _seqOnScreen = value; OnPropertyChanged(()=>this.SeqOnScreen); }
        }

        private int appeared;

        public int Appeared
        {
            get { return appeared; }
            set { appeared = value; OnPropertyChanged(()=>this.Appeared); }
        }
        

    }
}
