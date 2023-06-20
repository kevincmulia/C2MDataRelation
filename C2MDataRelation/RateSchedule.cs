using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MDataRelation
{
    class RateSchedule
    {
        String rateScheduleName;
        //the key is the effective date
        Dictionary<String, CalcRule> preProcessCalcGroupList;
        Dictionary<String, CalcRule> rateVersionCalcGroupList;
        Dictionary<String, CalcRule> postProcessCalcGroupList;

        public RateSchedule()
        {
        }


    }
}
