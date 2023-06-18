using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace C2MDataRelation
{
    class BusinessObject
    {
        private string bus_obj_cd, maint_obj_cd, app_svc_id, life_cycle_bo_cd, owner_flg, parent_bo_cd, instance_ctrl_flg;
        private string rootSchema, finalSchema, including, included;
        private int version;
        

        public BusinessObject()
        {
            this.bus_obj_cd = null;
            this.version = 0;
            this.maint_obj_cd = null;
            this.app_svc_id = null;
            this.life_cycle_bo_cd = null;
            this.owner_flg = null;
            this.parent_bo_cd = null;
            this.instance_ctrl_flg = null;
            this.rootSchema = null;
            this.finalSchema = null;
        }

        public BusinessObject(OracleDataReader odr, OracleConnection con, string included, string including, string root, string final)
        {
            this.bus_obj_cd = odr.GetString(odr.GetOrdinal("BUS_OBJ_CD")).Trim();
            this.version = odr.GetInt16(odr.GetOrdinal("VERSION"));
            this.maint_obj_cd = odr.GetString(odr.GetOrdinal("MAINT_OBJ_CD")).Trim();
            this.app_svc_id = odr.GetString(odr.GetOrdinal("APP_SVC_ID")).Trim();
            this.life_cycle_bo_cd = odr.GetString(odr.GetOrdinal("LIFE_CYCLE_BO_CD")).Trim();
            this.owner_flg = odr.GetString(odr.GetOrdinal("OWNER_FLG")).Trim();
            this.parent_bo_cd = odr.GetString(odr.GetOrdinal("PARENT_BO_CD")).Trim();
            this.instance_ctrl_flg = odr.GetString(odr.GetOrdinal("INSTANCE_CTRL_FLG")).Trim();
            this.included = included;
            this.including = including;
            this.rootSchema = root;
            this.finalSchema = final;
        }

        public void setBus_obj_cd(string bus_obj_cd) { this.bus_obj_cd = bus_obj_cd; }
        public string getBus_obj_cd() { return bus_obj_cd; }

        public void setVersion(int version) { this.version = version; }
        public int getVersion() { return this.version; }

        public void setMaint_obj_cd(string maint_obj_cd) { this.maint_obj_cd = maint_obj_cd; }
        public string getMaint_obj_cd() { return this.maint_obj_cd; }

        public void setApp_svc_id(string app_svc_id) { this.app_svc_id = app_svc_id; }
        public string getApp_svc_id() { return this.app_svc_id; }

        public void setLife_cycle_bo_cd(string life_cycle_bo_cd) { this.life_cycle_bo_cd = life_cycle_bo_cd; }
        public string getLife_cycle_bo_cd() { return this.life_cycle_bo_cd; }

        public void setOwner_flg(string owner_flg) { this.owner_flg = owner_flg; }
        public string getOwner_flg() { return this.owner_flg; }

        public void setParent_bo_cd(string parent_bo_cd) { this.parent_bo_cd = parent_bo_cd; }
        public string getParent_bo_cd() { return this.parent_bo_cd; }

        public void setInstance_ctrl_flg(string instance_ctrl_flg) { this.instance_ctrl_flg = instance_ctrl_flg; }
        public string getInstance_ctrl_flg() { return this.instance_ctrl_flg; }

        public void setrootSchema(string rootSchema) { this.rootSchema = rootSchema; }
        public string getrootSchema() { return this.rootSchema; }

        public void setfinalSchema(string finalSchema) { this.finalSchema = finalSchema; }
        public string getfinalSchema() { return this.finalSchema; }

        public void setIncluded(string included)
        {
            this.included = included;
        }

        public string getIncluded()
        {
            return included;
        }

        public void setIncluding(string including)
        {
            this.including = including;
        }

        public string getIncluding()
        {
            return including;
        }

        public string ToString => "Business Object : " + bus_obj_cd + "\nVersion : " + version + "\nMaintanance Object Inherited : " + maint_obj_cd
            + "\nApplication Service : " + app_svc_id + "\nLife Cycle : " + life_cycle_bo_cd + "\nOwner Flag : " + owner_flg + "\nParent BO : " +
            parent_bo_cd + "\nInstance Control Flag : " + instance_ctrl_flg;
    }
}
