using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MDataRelation
{
    class RateSchedule
    {
        String rateScheduleName;
        String serviceType;
        String frequency;
        String currency;



        //the key is the effective date
        List<CalcGroup> preProcessCalcGroupList;
        List<CalcGroup> rateVersionCalcGroupList;
        List<CalcGroup> postProcessCalcGroupList;

        public RateSchedule()
        {
        }
        public RateSchedule(OracleDataReader orr)
        {
            this.RateScheduleName = orr.GetString(orr.GetOrdinal("RS_CD"));
            this.ServiceType = orr.GetString(orr.GetOrdinal("SVC_TYPE_CD"));
            this.Frequency = orr.GetString(orr.GetOrdinal("FREQ_CD"));
            this.Currency = orr.GetString(orr.GetOrdinal("CURRENCY_CD"));
        }

        public string RateScheduleName { get => rateScheduleName; set => rateScheduleName = value; }
        public string ServiceType { get => serviceType; set => serviceType = value; }
        public string Frequency { get => frequency; set => frequency = value; }
        public string Currency { get => currency; set => currency = value; }
        internal List<CalcGroup> PreProcessCalcGroupList { get => preProcessCalcGroupList; set => preProcessCalcGroupList = value; }
        internal List<CalcGroup> RateVersionCalcGroupList { get => rateVersionCalcGroupList; set => rateVersionCalcGroupList = value; }
        internal List<CalcGroup> PostProcessCalcGroupList { get => postProcessCalcGroupList; set => postProcessCalcGroupList = value; }
        public string print()
        {
            String result = "";
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(this);
                result += String.Format("{0}={1}", name, value) + "\n\n\n";
            }
            return result;
        }
    }
}
