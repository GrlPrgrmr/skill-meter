using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMeter.DataAccessLayer
{
    interface IScoreCalculator
    {
        void prepareResult(int UserId);
    }
}
