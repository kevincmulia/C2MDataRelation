using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MDataRelation
{
    class UsageCalcGroup
    {
        String usageGroupName;
        List<UsageCalcRule> usageRuleList;

        public UsageCalcGroup(string usageGroupName, List<UsageCalcRule> usageRuleList)
        {
            this.usageGroupName = usageGroupName;
            this.usageRuleList = usageRuleList;
        }
        public UsageCalcGroup(string usageGroupName) {
            this.usageGroupName = usageGroupName;
            usageRuleList = new List<UsageCalcRule>();
        }

        public string UsageGroupName { get => usageGroupName; set => usageGroupName = value; }
        internal List<UsageCalcRule> UsageRuleList { get => usageRuleList; set => usageRuleList = value; }
        public void addUsageRule(UsageCalcRule usageRule) {
            this.usageRuleList.Add(usageRule);
        }
        public string print() {
            string result ="";
            result = result + " name = " + usageGroupName + "\n";
            result = result + "usage rule list: \n";
            if (this.usageRuleList.Count == 0)
            {
                result = result + "there are no usage rule list";
            }
            else {
                foreach (UsageCalcRule usageRule in this.usageRuleList)
                {
                    result += usageRule.Name+"\n"; 
                }
            }
            return result;
        }
    }
}
