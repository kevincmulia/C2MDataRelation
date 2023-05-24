using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MDataRelation
{
    class UsageRule
    {
        String usageGroup;
        String name;
        int sequence;
        String referredUsageGroup;
        String usageRuleType;
        String boDataArea;
        System.Collections.Generic.List<EligibilityCriteria> eligibilityCriteria;
        String schema;

        public UsageRule(string usageGroup, string name, int sequence, string usageRuleType, string boDataArea, string referredUsageGroup, List<EligibilityCriteria> eligibilityCriteria)
        {
            this.usageGroup = usageGroup;
            this.Name = name;
            this.Sequence = sequence;
            this.ReferredUsageGroup = referredUsageGroup;
            this.UsageRuleType = usageRuleType;
            this.BoDataArea = boDataArea;
            this.EligibilityCriteria = eligibilityCriteria;
        }
        public UsageRule(OracleDataReader orr)
        {
            this.usageGroup = orr.GetString(orr.GetOrdinal("USG_GRP_CD"));
            this.name = orr.GetString(orr.GetOrdinal("USG_RULE_CD"));
            this.Sequence = orr.GetInt16(orr.GetOrdinal("EXE_SEQ"));
            this.ReferredUsageGroup = orr.GetString(orr.GetOrdinal("REFERRED_USG_GRP_CD"));
            this.UsageRuleType = orr.GetString(orr.GetOrdinal("BUS_OBJ_CD"));
            this.BoDataArea = orr.GetString(orr.GetOrdinal("BO_DATA_AREA"));
            this.EligibilityCriteria = new List<EligibilityCriteria>();
            
        }

        public string Name { get => name; set => name = value; }
        public int Sequence { get => sequence; set => sequence = value; }
        public string UsageRuleType { get => usageRuleType; set => usageRuleType = value; }
        public string BoDataArea { get => boDataArea; set => boDataArea = value; }
        public string ReferredUsageGroup { get => referredUsageGroup; set => referredUsageGroup = value; }
        internal List<EligibilityCriteria> EligibilityCriteria { get => eligibilityCriteria; set => eligibilityCriteria = value; }
        public string UsageGroup { get => usageGroup; set => usageGroup = value; }
        public string Schema { get => schema; set => schema = value; }

        public void addEligibilityCriteria(EligibilityCriteria eligibilityCriteria) {
            this.eligibilityCriteria.Add(eligibilityCriteria);
        }
    }
}
