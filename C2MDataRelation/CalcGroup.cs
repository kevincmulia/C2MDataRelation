using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MDataRelation
{
    class CalcGroup
    {
        String calcGroupName;
        List<CalcRule>calcRuleList;

        public string CalcGroupName { get => calcGroupName; set => calcGroupName = value; }
        internal List<CalcRule> CalcRuleList { get => calcRuleList; set => calcRuleList = value; }
        public void addCalcRule(CalcRule calcRule)
        {
            this.calcRuleList.Add(calcRule);
        }
        public string print()
        {
            string result = "";
            result = result + " name = " + calcGroupName + "\n";
            result = result + "usage rule list: \n";
            if (this.calcRuleList.Count == 0)
            {
                result = result + "there are no usage rule list";
            }
            else
            {
                foreach (CalcRule calcRule in this.calcRuleList)
                {
                    result += calcRule.Name + "\n";
                }
            }
            return result;
        }
    }
}
