using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace C2MDataRelation
{
    class Algorithm
    {
        private string alg_cd, alg_type_cd, descr50, descrlong;
        
        public Algorithm() { 
            this.alg_cd = null;
            this.alg_type_cd = null;
            this.descr50 = null;
            this.descrlong = null;
        }

        public Algorithm(OracleDataReader odr)
        {
            this.alg_cd = odr.GetString(odr.GetOrdinal("ALG_CD")).Trim();
            this.alg_type_cd = odr.GetString(odr.GetOrdinal("ALG_TYPE_CD")).Trim();
            this.descr50 = odr.GetString(odr.GetOrdinal("DESCR50")).Trim();
            this.descrlong = odr.GetString(odr.GetOrdinal("DESCRLONG")).Trim();
        }

        public string getAlg_cd()
        {
            return this.alg_cd;
        }

        public string ToString()
        {
            return "Algorithm CD : " + alg_cd + "\n" + "Algoritm Type : " + alg_type_cd + "\n" + "Description 1 : " + descr50 + "\n" + 
                    "Description 2 : " + descrlong + "\n";
        }
    }
}
