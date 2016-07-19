using SkillMeter.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillMeter.ViewModels;
using System.Windows;

namespace SkillMeter.DataAccessLayer
{
   public  class ResultNoNegative:IScoreCalculator
    {
        public  void prepareResult(int testId)
        {
            try
            {
                List<ExamSession> userReport = new List<ExamSession>();

                List<Option> options = new List<Option>();
                List<Question> Questions = new List<Question>();
                DataAccessADO objData = new DataAccessADO();

                userReport = objData.getUserSessionData(testId);
                Questions = objData.getQuestions();
                int score = 0;

                var totalAttempted = userReport.Count();
                int correctlyAnswered = 0;


                options = objData.getOptions().Select(row => row).Where(r => r.IsCorrect == 1).ToList();
                var countOptions = options.Count();

                foreach (var entry in userReport)
                {
                    var choices = entry.Answer.Split(',').Select(i => int.Parse(i)).ToList();

                    var temp = options.Select(row => row).Where(r => r.QuesId == entry.QuestionId).ToList();

                    if (temp.Count() == choices.Count())
                    {
                        try
                        {
                            var test = temp.Select(c => choices.Contains(c.OptionId ?? 0)).ToList().All(x => x.Equals(true));

                            if (test)
                            {
                                var currentQuestion = Questions.Where(r=>r.QueID == entry.QuestionId).SingleOrDefault();

                                score = score + currentQuestion.Complexity;
                                correctlyAnswered++;
                            }

                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }

                   

                }


                objData.sendResultToDB(score, testId, totalAttempted, correctlyAnswered);

                var setName=  objData.getUserTestsData().Where(r=>r.TestId==testId).Select(r=>r.TestName).FirstOrDefault().ToString();
                int quesAttempted = objData.getUserTestsData().Where(r=>r.TestId==testId).Select(r=>r.Attempted).FirstOrDefault();
                int maxQuestions = objData.getAllQuestionSets().Where(r => r.SetName == setName).Select(r => r.QuestionsPerTest).FirstOrDefault();

                

                if (maxQuestions == quesAttempted )
                {
                    objData.updateFieldInDb("Yes", "TestGiven", "UserTest", "TestId =" + testId.ToString());
                }
                else if (quesAttempted == 0 && maxQuestions == quesAttempted)
                {

                    objData.updateFieldInDb("Yes", "TestGiven", "UserTest", "TestId =" + testId.ToString());
                }
                else
                {
                    objData.updateFieldInDb("No", "TestGiven", "UserTest", "TestId =" + testId.ToString());
                
                }
              
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

      
    }
}
