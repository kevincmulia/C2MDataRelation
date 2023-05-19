using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MDataRelation
{
    class usageGroup
    {
        String usageGroupName;
        List<UsageRule> usageRuleList;

        public usageGroup(string usageGroupName, List<UsageRule> usageRuleList)
        {
            this.usageGroupName = usageGroupName;
            this.usageRuleList = usageRuleList;
        }
        public usageGroup(string usageGroupName) {
            this.usageGroupName = usageGroupName;
            usageRuleList = new List<UsageRule>();
        }

        public string UsageGroupName { get => usageGroupName; set => usageGroupName = value; }
        internal List<UsageRule> UsageRuleList { get => usageRuleList; set => usageRuleList = value; }
        public void addUsageRule(UsageRule usageRule) {
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
                foreach (UsageRule usageRule in this.usageRuleList)
                {
                    result += usageRule.Name+"\n"; 
                }
            }
            return result;
        }
    }
}
