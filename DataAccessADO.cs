using SkillMeter.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows;
using SkillMeter.Models;
using System.IO;

namespace SkillMeter.DataAccessLayer
{
    public class DataAccessADO : IDataAccess
    {


        public List<Question> getQuestions()
        {
            try
            {

                DataTable dtQuestions = SQLiteHelper.PerformSelect("Select * from Questions");


                //object x = dtQuestions.AsEnumerable().First()["QueID"];

                List<Question> questions = dtQuestions.AsEnumerable().Select
                                               (row =>
                                                   new Question
                                                   {
                                                       QueID = Convert.ToInt32(row.Field<long>("QueID")),
                                                       Complexity = Convert.ToInt32(row.Field<long>("Complexity")),
                                                       QuestionString = row.Field<string>("Question"),
                                                       Questype = Convert.ToInt32(row.Field<long>("OptionType")),
                                                       QuesSetID = Convert.ToInt32(row.Field<long?>("SetID"))

                                                   }).ToList();
               // ObservableCollection<QuestionBank> quesList = new ObservableCollection<QuestionBank>(questions);


                return questions;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return null;
            }
        }


        public List<Option> getOptions()
        {

            try
            {
                DataTable dtOptions = SQLiteHelper.PerformSelect("Select * from Options");
                List<Option> OptionsList = dtOptions.AsEnumerable().Select(row =>
                                                                               new Option
                                                                               {
                                                                                   OptionId = Convert.ToInt32(row.Field<long?>("OptionID")),
                                                                                   QuesId = Convert.ToInt32(row.Field<long?>("QueId")),
                                                                                   OptionDescription = row.Field<string>("OptionString"),
                                                                                   IsCorrect = Convert.ToInt32(row.Field<long?>("IsCorrect"))

                                                                               }).ToList();
                return OptionsList;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        public List<ExamSession> getUserSessionData(int TestId)
        {

            try
            {

                List<ExamSession> userReport = new List<ExamSession>();
                DataTable userData = SQLiteHelper.PerformSelect("Select * from ExamSessionData");

                userReport = userData.AsEnumerable().Select(r => new ExamSession
                {
                    ExamSessionId = Convert.ToInt32(r.Field<long>("ExamSessionId")),
                    TestId = Convert.ToInt32(r.Field<long>("TestId")),
                    QuestionId = Convert.ToInt32(r.Field<long>("QuestionId")),
                    SequenceNo = Convert.ToInt32(r.Field<long>("SequenceNo")),
                    Answer = r.Field<string>("Answer"),
                    TimeStampValue = r.Field<string>("TimeStamp")
                    

                }).Where(r => r.TestId == TestId).ToList();


                return userReport;
            }
            catch(Exception ex)
            {
                ex.ToString();
                return null;
            }
        
        }

        public void submitAnswersToDB(Session Answer)
        {

            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("TestId", Answer.TestId);
                dict.Add("QuestionId", Answer.QuestionId);
                dict.Add("SequenceNo", Answer.SequenceNo);
                string answer ="";
                foreach (var option in Answer.Answer.Select((value, i) => new { i, value }))
                {
                    answer = answer +option.value.OptionId.ToString();
                    answer = answer + ",";
                }

                if (answer.Last().Equals(','))
                {
                    answer = answer.TrimEnd(',');
                }
                dict.Add("Answer",answer);
                dict.Add("TimeStamp", Answer.TimeStampValue);
                SQLiteHelper.Insert("ExamSessionData", dict);
            }
            catch (Exception EX)
            {

                MessageBox.Show(EX.ToString());
            }

        }

        public void updateFieldInDb(string colValue,string columnName,string tableName,string condition)
        {
            try
            {
                Dictionary<String, String> Data = new Dictionary<String, String>();
                Data.Add(columnName,colValue);

                 
                SQLiteHelper.Update(tableName,Data,condition);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        public void sendResultToDB(int result,int testId,int totalAttempted,int correctlyAnswered)
        {

            try
            {
                Dictionary<String, String> scoreData = new Dictionary<String, String>();
                scoreData.Add("Score", result.ToString());
                scoreData.Add("TotalQuesAttempted", totalAttempted.ToString());
                scoreData.Add("CorrectlyAnswered", correctlyAnswered.ToString());

                string condition = "TestId = " + testId.ToString();
                SQLiteHelper.Update("UserTest", scoreData, condition);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public List<Admin> getAllAdmins()
        {
            try
            {
                List<Admin> obj = new List<Admin>();
                DataTable table = SQLiteHelper.PerformSelect("Select * from Users");
                var data = table.AsEnumerable().Where(row=>Convert.ToInt32( row.Field<long?>("IsAdmin"))==1)
                    .Select(row => new Admin
                    {
                        
                        UserName = row.Field<string>("UserName"),
                        TimeStamp = row.Field<string>("TimeStamp")
                    }).ToList();

                obj = data.ToList();
                return obj;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }

        }

        public List<User> getAllUsers()
        {
            try
            {
                List<User> obj = new List<User>();
                DataTable table = SQLiteHelper.PerformSelect("Select * from Users");
                var data = table.AsEnumerable()
                    .Select(row => new User
                    {
                        UserId = Convert.ToInt32(row.Field<long?>("UserId")),
                        UserName = row.Field<string>("UserName"),
                        Password = row.Field<string>("Password"),
                        IsAdmin = Convert.ToInt32(row.Field<long?>("IsAdmin"))
                    }).ToList();

                obj = data.ToList();
                return obj;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }


        }

       

        public User getUserData(string userName)
        {

            try
            {
                User obj = new User();
                DataTable table = SQLiteHelper.PerformSelect("Select * from Users");
                var data = table.AsEnumerable()
                    .Select(row => new User
                    {
                        UserId = Convert.ToInt32(row.Field<long?>("UserId")),
                        UserName = row.Field<string>("UserName"),
                        Password = row.Field<string>("Password"),
                        IsAdmin = Convert.ToInt32(row.Field<long?>("IsAdmin"))
                    }).ToList();

                obj = data.Single(u => u.UserName == userName);
                return obj;
            }
            catch (Exception ex)
            {
                
                return null;
            }


           

        
        }

        public UserTest getUserTestData(string userName)
        {
            UserTest obj = new UserTest();

            try
           {
          
            DataTable table = SQLiteHelper.PerformSelect("Select * from UserTest");
            var data = table.AsEnumerable()
                .Select(row => new UserTest
                {
                    UserName = row.Field<string>("UserName"),
                    SetId = Convert.ToInt32(row.Field<long>("SetID")),
                    TestId = Convert.ToInt32(row.Field<long>("TestId")),
                    Score = Convert.ToInt32(row.Field<long?>("Score")),
                    TestName =row.Field<string>("TestName"),
                    TestGiven=row.Field<string>("TestGiven"),
                    TestDuration =row.Field<string>("TestDuration")
                }).ToList();

            obj = data.SingleOrDefault(u => u.UserName == userName);

           }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            return obj;
 
        
        }

        public List<UserTest> getUserTestsData()
        { 
        
           List< UserTest> obj = new List<UserTest>();
           try
           {
               DataTable table = SQLiteHelper.PerformSelect("Select * from UserTest");
               var data = table.AsEnumerable()
                   .Select(row => new UserTest
                   {
                       UserName = row.Field<string>("UserName"),
                       SetId = Convert.ToInt32(row.Field<long>("SetID")),
                       TestId = Convert.ToInt32(row.Field<long>("TestId")),
                       Score = Convert.ToInt32(row.Field<long?>("Score")),
                       TestName = row.Field<string>("TestName"),
                       Date = row.Field<string>("Date"),
                       Attempted = Convert.ToInt32(row.Field<string>("TotalQuesAttempted")),
                       CorrectlyAnswered = Convert.ToInt32(row.Field<string>("CorrectlyAnswered")),
                       TestGiven = row.Field<string>("TestGiven"),
                       TestDuration = row.Field<string>("TestDuration")

                   }).ToList();

               obj = data.Select(u => u).ToList();
           }
           catch (Exception ex)
           {

               MessageBox.Show(ex.ToString());
           }
            return obj;

        
        }

        internal List<QuestionSet> getAllQuestionSets()
        {
            try
            {
                List<QuestionSet> obj = new List<QuestionSet>();
                DataTable tb = SQLiteHelper.PerformSelect("Select * from QuestionSets");

                var dataQuestioSet = tb.AsEnumerable().
                                     Select(row => new QuestionSet
                                     {

                                         SetId = Convert.ToInt32(row.Field<long>("SetID")),
                                         SetName = row.Field<string>("Name"),
                                         Description = row.Field<string>("Description"),
                                         Owner = row.Field<string>("Owner"),
                                         OwnerId = Convert.ToInt32(row.Field<long>("OwnerId")),
                                         Category = row.Field<string>("Category"),
                                         QuestionsPerTest = Convert.ToInt32(row.Field<long>("QuestionsPerTest")),
                                         TimeStamp = row.Field<string>("TimeStamp"),
                                       

                                     }).ToList();
                obj = dataQuestioSet.ToList();

                return obj;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        
        }

        internal List<QuestionSet> getQuestionSetData(string ownerName)
        {

            try
            {
                List<QuestionSet> obj = new List<QuestionSet>();
                DataTable tb = SQLiteHelper.PerformSelect("Select * from QuestionSets");

                var dataQuestioSet = tb.AsEnumerable().
                                     Select(row => new QuestionSet
                                     {

                                         SetId = Convert.ToInt32(row.Field<long>("SetID")),
                                         SetName = row.Field<string>("Name"),
                                         Description = row.Field<string>("Description"),
                                         Owner = row.Field<string>("Owner"),
                                         OwnerId = Convert.ToInt32(row.Field<long>("OwnerId")),
                                         Category = row.Field<string>("Category"),
                                         QuestionsPerTest = Convert.ToInt32(row.Field<long>("QuestionsPerTest")),
                                         TimeStamp = row.Field<string>("TimeStamp"),
                                       

                                     }).ToList();
                obj = dataQuestioSet.Where(qs => qs.Owner == ownerName).ToList();

                return obj;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        public QuestionSet getQuestionSetData(string ownerId,string testName)
        {
            try
            {
                QuestionSet obj = new QuestionSet();
                DataTable tb = SQLiteHelper.PerformSelect("Select * from QuestionSets");

                var dataQuestioSet = tb.AsEnumerable().
                                     Select(row => new QuestionSet
                                     {

                                         SetId = Convert.ToInt32(row.Field<long>("SetID")),
                                         SetName = row.Field<string>("Name"),
                                         Description = row.Field<string>("Description"),
                                         Owner = row.Field<string>("Owner"),
                                         OwnerId=Convert.ToInt32(row.Field<long>("OwnerId")),
                                         Category = row.Field<string>("Category"),
                                         QuestionsPerTest = Convert.ToInt32(row.Field<long>("QuestionsPerTest")),
                                         TimeStamp = row.Field<string>("TimeStamp")

                                     }).ToList();
                obj = dataQuestioSet.SingleOrDefault(qs => qs.Owner == ownerId && qs.SetName == testName);

                return obj;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }


        }

        public bool storeQuestionsToDB(int setId,string filePath,string testQuestions)
        {
            
            try
            {
                DataTable dt = SQLiteHelper.importCSV(filePath);
                if (dt == null)
                {
                    return false;
                }
                Dictionary<string, object> questions = new Dictionary<string, object>();
               

               
                DataRow row = dt.Rows[0];
                dt.Rows.Remove(row);

                dt = dt.Rows.Cast<DataRow>().Where(r => !r.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();
                var groups = dt.AsEnumerable();
                var buckets = (
                               from q in groups
                               group q by q[1] into groupedData //creating groups as per complexity given in the file
                               select new
                               {
                                   Complexity = groupedData.Key,
                                   Questions = groupedData.Select(g => g).ToList()
                               }).ToList();

                var questionsPerBucket = buckets.AsEnumerable().Select(r => r.Questions.Count).ToList();



                foreach (var questionsInBucket in questionsPerBucket)
                {

                    if (questionsInBucket < Convert.ToInt32( buckets.Count))
                    {

                        return false;
                    }
                }


                
                foreach (DataRow dr in dt.Rows)
                {
                    questions.Add("Complexity", dr[1]);
                    questions.Add("Question", dr[0].ToString().Replace("'","''"));
                    questions.Add("OptionType", dr[6]);
                    questions.Add("SetID", setId);

                    SQLiteHelper.Insert("Questions", questions);

                    questions.Clear();
                }



                bool resultOption = storeOptionsToDB(dt, setId);
                if (resultOption)
                {
                    return true;

                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Questions did not store correctly!!!");
                return false;
            }

        }

        internal bool deleteRowFromTable(string tableName, string condition)
        {

            try
            {
                SQLiteHelper.Delete(tableName,condition,null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }

        internal bool storeOptionsToDB(DataTable rawData,int setID)
        {
            List<Question> quesList = new List<Question>();
            Dictionary<string, object> Options = new Dictionary<string, object>();
            List<string> optionStrings = new List<string>();

            int[] isCorrectList = new int[]{ 0,0,0,0};
            try
            {
                quesList = getQuestions().Where(r => r.QuesSetID == setID).ToList();


                var quesId = 0;
                int index = 0;
                
                
                foreach (DataRow dr in rawData.Rows)
                {
                    isCorrectList =new int[]{0,0,0,0};

                    optionStrings.Add(dr[2].ToString() == String.Empty ? "0" : dr[2].ToString()); 
                    optionStrings.Add(dr[3].ToString() == String.Empty ? "0" : dr[3].ToString()); 
                    optionStrings.Add(dr[4].ToString() == String.Empty ? "0" : dr[4].ToString()); 
                    optionStrings.Add(dr[5].ToString()==String.Empty?"0" : dr[5].ToString());


                    if (dr[0].ToString() == quesList.Select(r => r.QuestionString).ToList()[index])
                    {
                        quesId = quesList.Select(r => r.QueID).ToList()[index];
                        index++;
                    }
                    
                    var correctOption = dr[7].ToString().Split(',');

                    foreach (var item in correctOption)
                    {
                        switch (Convert.ToInt32(item))
                        {
                            case 1:
                                {
                                    isCorrectList[0] = 1;
                                    break;
                                }
                            case 2:
                                {
                                    isCorrectList[1] = 1;
                                    break;
                                }
                            case 3:
                                {
                                    isCorrectList[2] = 1;
                                    break;
                                }
                            case 4:
                                {
                                    isCorrectList[3] = 1;
                                    break;
                                }
                        
                        }
                        
                    }

                    var loopVar = 0;
                    foreach (var opString in optionStrings)
                    {
                        if (opString != "0")
                        {
                            Options.Add("QueId", quesId);
                            Options.Add("OptionString", opString.Replace("'","''"));
                            Options.Add("IsCorrect", isCorrectList[loopVar]);

                            SQLiteHelper.Insert("Options", Options);
                            Options.Clear();
                            loopVar++;


                        }


                    }

                    optionStrings.Clear();
                    

                }

                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()+"Options did not store correctly in the database for given Excel!!!");
                return false;
            }

        }


        internal bool submitSetToDB(QuestionSet setObj)
        {

            Dictionary<string, object> setData = new Dictionary<string, object>();


            try
            {
                var allSets = getQuestionSetData(setObj.Owner);

                var setNameUsed = allSets.Where(r => r.SetName == setObj.SetName).FirstOrDefault();

                if (setNameUsed != null)
                {
                    return false;
                }

                setData.Add("Name", setObj.SetName);
                setData.Add("Description", setObj.Description);
                setData.Add("Category", setObj.Category);
                setData.Add("Owner", setObj.Owner);
                setData.Add("TimeStamp", DateTime.Now.ToString());
                setData.Add("OwnerId", setObj.OwnerId);
                setData.Add("QuestionsPerTest",setObj.QuestionsPerTest);

                SQLiteHelper.Insert("QuestionSets", setData);
                return true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        internal bool assignNewTestToCandidate(TestCandidate user,string owner)
        {

            Dictionary<string, object> userTestTableData = new Dictionary<string, object>();
            try
            {

                var userProfile = getAllUsers().Where(r=>r.UserName==user.UserName).FirstOrDefault();

                UserTest testRecord = new UserTest();
                testRecord.UserName = user.UserName;
                testRecord.SetId = getQuestionSetData(owner).Where(r => r.SetName == user.QuestionSet).FirstOrDefault().SetId;
                testRecord.TestName = user.QuestionSet;

                userTestTableData.Add("UserName", testRecord.UserName);
                userTestTableData.Add("Score", 0);
                userTestTableData.Add("SetID", testRecord.SetId);
                userTestTableData.Add("Date", DateTime.Now.ToString());
                userTestTableData.Add("TestName", testRecord.TestName);
                userTestTableData.Add("TestGiven", "No");
                userTestTableData.Add("TestDuration", user.TestDuration);

                SQLiteHelper.Insert("UserTest", userTestTableData);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        internal bool createTestTakerProfile(TestCandidate user,string owner)
        {
            Dictionary<string, object> userTableData = new Dictionary<string, object>();
            Dictionary<string, object> userTestTableData = new Dictionary<string, object>();

            try
            {
                if (user.QuestionSet==null || user.QuestionSet==String.Empty)
                {
                    MessageBox.Show("Please Create a Question Set before creating a TestTaker!!!");
                    return false;
                }

                var allUsers = getAllUsers();
                var ownerId = allUsers.Where(r => r.UserName == owner).FirstOrDefault().UserId;
                bool result = CheckforUserExistence(user);

                if (result == true)
                {
                    return false;
                }

                userTableData.Add("UserName", user.UserName);
                userTableData.Add("Password", user.Password);
                userTableData.Add("IsAdmin", user.IsAdmin);
                userTableData.Add("TimeStamp", DateTime.Now.ToString());
               

                SQLiteHelper.Insert("Users", userTableData);

                allUsers = getAllUsers();//refresh the data to include recently created user
                User testTaker = new User();
                testTaker = allUsers.Where(r => r.IsAdmin == 0 && r.UserName == user.UserName).FirstOrDefault();

                UserTest testRecord = new UserTest();
                testRecord.UserName = testTaker.UserName;
                testRecord.SetId = getQuestionSetData(owner).Where(r => r.SetName == user.QuestionSet).FirstOrDefault().SetId;//sets under a user have to be uniquely named
                testRecord.TestName = user.QuestionSet;


                userTestTableData.Add("UserName", testRecord.UserName);
                userTestTableData.Add("Score", 0);
                userTestTableData.Add("SetID", testRecord.SetId);
                userTestTableData.Add("Date", DateTime.Now.ToString());
                userTestTableData.Add("TestName", testRecord.TestName);
                userTestTableData.Add("TestGiven","No");
                userTestTableData.Add("TestDuration", user.TestDuration);

                SQLiteHelper.Insert("UserTest", userTestTableData);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Test Taker profile could not be created!!!!Please try after sometime /n"+ex.ToString());
                return false;
            }

        }

        private bool CheckforUserExistence(TestCandidate user)
        {

            var allUsers = getAllUsers();
            var userExists = allUsers.Where(r => r.UserName == user.UserName).FirstOrDefault();

            if (userExists != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool CheckforUserExistence(User user)
        {

            var allUsers = getAllUsers();
            var userExists = allUsers.Where(r => r.UserName == user.UserName).FirstOrDefault();

            if (userExists != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        internal bool createAdminProfile(User admin)
        {
            try
            {
                Dictionary<string, object> userTableData = new Dictionary<string, object>();
                bool userExists = CheckforUserExistence(admin);

                if (userExists == true)
                {
                    return false;
                }
                userTableData.Add("UserName", admin.UserName);
                userTableData.Add("Password", admin.Password);
                userTableData.Add("IsAdmin", admin.IsAdmin);
                userTableData.Add("TimeStamp", DateTime.Now.ToString());

                SQLiteHelper.Insert("Users", userTableData);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Admin profile could not be created!!!!Please try after sometime /n" + ex.ToString());
                return false;
               
            }
        }

        private static string _dbConnectionString { get { return "Data Source=" + System.IO.Directory.GetCurrentDirectory() + "\\Eval ;Pooling=True;Max Pool Size=100;Default Timeout=100;Connect Timeout=100"; } }



        internal void deleteCandidateFromDatabase(string p)
        {
            
            try
            {
                var testId = getUserTestData(p).TestId;
                using (SQLiteTransaction tran = SQLiteHelper.ConObj.BeginTransaction())
                {

                    SQLiteHelper.Delete("UserTest", "UserName='" + p + "'", tran);
                    SQLiteHelper.Delete("ExamSessionData", "TestId=" + testId.ToString(), tran);
                    SQLiteHelper.Delete("Users", "UserName='" + p+"'", tran);

                    tran.Commit();
                    tran.Dispose();
                }

                MessageBox.Show(p+" Profile Deleted Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Candidate Profile Could Not Be Deleted!!!!Please Try After Sometime /n"+ex.ToString());
            }
        }
    }
}
