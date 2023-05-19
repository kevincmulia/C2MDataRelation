using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MDataRelation
{
    class EligibilityCriteria
    {
        String usageGroup;
        String usageRule;
        int sequence;
        String usageRuleType;
        String boDataArea;


        public EligibilityCriteria(string usageGroup, string usageRule, int sequence, string usageRuleType, string boDataArea)
        {
            this.usageGroup = usageGroup;
            this.usageRule = usageRule;
            this.sequence = sequence;
            this.usageRuleType = usageRuleType;
            this.boDataArea = boDataArea;
        }
        public EligibilityCriteria(OracleDataReader orr) {
            this.usageGroup = orr.GetString(orr.GetOrdinal("USG_GRP_CD"));
            this.usageRule = orr.GetString(orr.GetOrdinal("USG_RULE_CD"));
            this.sequence = orr.GetInt16(orr.GetOrdinal("EXE_SEQ"));
            this.usageRuleType = orr.GetString(orr.GetOrdinal("BUS_OBJ_CD"));
            this.boDataArea = orr.GetString(orr.GetOrdinal("BO_DATA_AREA"));
        }

        public int Sequence { get => sequence; set => sequence = value; }
        public string UsageRuleType { get => usageRuleType; set => usageRuleType = value; }
        public string BoDataArea { get => boDataArea; set => boDataArea = value; }
        public string UsageGroup { get => usageGroup; set => usageGroup = value; }
        public string UsageRule { get => usageRule; set => usageRule = value; }
    }
}
