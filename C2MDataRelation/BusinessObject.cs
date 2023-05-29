using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace C2MDataRelation
{
    internal class BusinessObject
    {
        private string bus_obj_cd, maint_obj_cd, app_svc_id, life_cycle_bo_cd, owner_flg, parent_bo_cd, instance_ctrl_flg;
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
        }

        public BusinessObject(string bus_obj_cd, string maint_obj_cd, string app_svc_id, string life_cycle_bo_cd, string owner_flg, string parent_bo_cd, string instance_ctrl_flg, int version)
        {
            this.bus_obj_cd = bus_obj_cd;
            this.maint_obj_cd = maint_obj_cd;
            this.app_svc_id = app_svc_id;
            this.life_cycle_bo_cd = life_cycle_bo_cd;
            this.owner_flg = owner_flg;
            this.parent_bo_cd = parent_bo_cd;
            this.instance_ctrl_flg = instance_ctrl_flg;
            this.version = version;
        }

        public void setBus_obj_cd(string bus_obj_cd) { this.bus_obj_cd = bus_obj_cd; }
        public string getBus_obj_cd() { return this.bus_obj_cd; }

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

        public string ToString => "Business Object : " + bus_obj_cd + "\nVersion : " + version + "\nMaintanance Object Inherited : " + maint_obj_cd
            + "\nApplication Service : " + app_svc_id + "\nLife Cycle : " + life_cycle_bo_cd + "\nOwner Flag : " + owner_flg + "\nParent BO : " +
            parent_bo_cd + "\nInstance Control Flag : " + instance_ctrl_flg;
    }
}
