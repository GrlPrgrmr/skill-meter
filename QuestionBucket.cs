using SkillMeter.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMeter.ViewModels
{
    class QuestionBucket
    {

        #region Properties

        public int Complexity { get; set; }

        public List<Question> Questions { get; set; }

        //public readonly List<string> BucketName = new List<string>(){ "Bucket1" ,"Bucket2","Bucket3","Bucket4"};



        //private List<QuestionBank> questionBucket1;
        //public List<QuestionBank> QuestionBucket1 { get { return questionBucket1; } set { questionBucket1 = value; } }

        //private List<QuestionBank> questionBucket2;
        //public List<QuestionBank> QuestionBucket2 { get { return questionBucket2; } set { questionBucket2 = value; } }

        //private List<QuestionBank> questionBucket3;
        //public List<QuestionBank> QuestionBucket3 { get { return questionBucket3; } set { questionBucket3 = value; } }

        //private List<QuestionBank> questionBucket4;
        //public List<QuestionBank> QuestionBucket4 { get { return questionBucket4; } set { questionBucket4 = value; } }

        #endregion

        //public void createQuesBuckets(ObservableCollection<QuestionBank> mainList)
        //{

        //    this.QuestionBucket1 = mainList.Where(row=>row.Complexity == 1).ToList();
        //    this.QuestionBucket2 = mainList.Where(row => row.Complexity == 2).ToList();
        //    this.QuestionBucket3 = mainList.Where(row=> row.Complexity==3).ToList();
        //    this.QuestionBucket4 = mainList.Where(row=>row.Complexity==4).ToList();

        //}
    }
}
