using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MDataRelation
{
    class CalcRule
    {
        String calcGroup;
        String name;
        int seq;
        String referredCalcGroup;
        Sq sq;
        String calcRuleType;
        String boDataArea;
        String dst_Id;
        String schema;
        public CalcRule()
        {
        }
        public CalcRule(OracleDataReader orr)
        {
            this.calcGroup = orr.GetString(orr.GetOrdinal("CALC_GRP_CD"));
            this.name = orr.GetString(orr.GetOrdinal("CALC_RULE_CD"));
            this.seq = orr.GetInt16(orr.GetOrdinal("CR_EXEC_SEQ"));
            this.referredCalcGroup = orr.GetString(orr.GetOrdinal("REF_CALC_GRP_CD"));
            this.CalcRuleType = orr.GetString(orr.GetOrdinal("BUS_OBJ_CD"));
            this.BoDataArea = "<" + this.name + ">\n" + orr.GetString(orr.GetOrdinal("BO_DATA_AREA")) + "</" + this.name + ">";
            this.dst_Id = orr.GetString(orr.GetOrdinal("DST_ID"));
            this.sq = new Sq(orr.GetString(orr.GetOrdinal("UOM_CD")), orr.GetString(orr.GetOrdinal("SQI_CD")), orr.GetString(orr.GetOrdinal("TOU_CD")));
            
        }
        public string CalcGroup { get => calcGroup; set => calcGroup = value; }
        public string Name { get => name; set => name = value; }
        public int Seq { get => seq; set => seq = value; }
        public string ReferredCalcGroup { get => referredCalcGroup; set => referredCalcGroup = value; }
        public string BoDataArea { get => boDataArea; set => boDataArea = value; }
        public string Dst_Id { get => dst_Id; set => dst_Id = value; }
        public string Schema { get => schema; set => schema = value; }
        public string CalcRuleType { get => calcRuleType; set => calcRuleType = value; }
        internal Sq Sq { get => sq; set => sq = value; }
    }
}
