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
        }

        public string UsageGroupName { get => usageGroupName; set => usageGroupName = value; }
        internal List<UsageRule> UsageRuleList { get => usageRuleList; set => usageRuleList = value; }
    }
}
