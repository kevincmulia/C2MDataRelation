using Oracle.ManagedDataAccess.Client;
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
        int seq;
        String effdate;
        List<CalcRule>calcRuleList;

        public string CalcGroupName { get => calcGroupName; set => calcGroupName = value; }
        public int Seq { get => seq; set => seq = value; }
        internal List<CalcRule> CalcRuleList { get => calcRuleList; set => calcRuleList = value; }

        public CalcGroup()
        {
        }

        public CalcGroup(OracleDataReader orr,bool prepost)
        {
            this.calcGroupName = orr.GetString(orr.GetOrdinal("CALC_GRP_CD"));
            if (prepost)
            {//only pre calc and post calc has seq while the rate version calc does not
                this.seq = orr.GetInt16(orr.GetOrdinal("RS_PROC_SEQ"));
            }
            else {
                this.effdate = orr.GetString(orr.GetOrdinal("EFFDT"));
            }
        }

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
