using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace C2MDataRelation
{
    class DataArea
    {
        private string data_area_cd, owner_flg, f1_ext_data_area_cd;
        private string rootSchema, finalSchema;
        private int version;
        OracleConnection conn;

        public DataArea()
        {
            this.data_area_cd = null;
            this.version = 0;
            this.owner_flg = null;
            this.f1_ext_data_area_cd=null;
        }

        public DataArea(OracleDataReader odr, OracleConnection con, string root, string final)
        {
            this.data_area_cd = odr.GetString(odr.GetOrdinal("DATA_AREA_CD"));
            this.version = odr.GetInt16(odr.GetOrdinal("VERSION"));
            this.owner_flg = odr.GetString(odr.GetOrdinal("OWNER_FLG"));
            this.f1_ext_data_area_cd = odr.GetString(odr.GetOrdinal("f1_ext_data_area_cd"));
            this.rootSchema = root;
            this.finalSchema = final;
        }

        public void setData_area_cd(string data_area_cd) { this.data_area_cd = data_area_cd; }
        public string getData_area_cd() { return this.data_area_cd; }

        public void setVersion(int version) { this.version = version; }
        public int getVersion() { return this.version; }

        public void setOwner_flg(string owner_flg) { this.owner_flg = owner_flg; }
        public string getOwner_flg() { return this.owner_flg; }

        public void setF1_ext_data_area_cd(string f1_ext_data_area_cd) { this.f1_ext_data_area_cd = f1_ext_data_area_cd; }
        public string getF1_ext_data_area_cd() { return this.f1_ext_data_area_cd; }

        public void setrootSchema(string rootSchema) { this.rootSchema = rootSchema; }
        public string getrootSchema() { return this.rootSchema; }

        public void setfinalSchema(string finalSchema) { this.finalSchema = finalSchema; }
        public string getfinalSchema() { return this.finalSchema; }
    }
}
