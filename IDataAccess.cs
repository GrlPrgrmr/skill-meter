using SkillMeter.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMeter.DataAccessLayer
{
    interface IDataAccess
    {
        /// <summary>
        /// This function will return the data table corresponding to the questionbank table in database.
        /// </summary>
        /// <returns></returns>
        List<Question> getQuestions();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<Option> getOptions();

        List<ExamSession> getUserSessionData(int userId);


    }
}
