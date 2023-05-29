using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MDataRelation
{
    internal class BusinessService
    {
        private string bus_svc_cd, svc_name, owner_flg, app_svc_id;
        private int version;

        public BusinessService()
        {
            this.bus_svc_cd = null;
            this.svc_name = null;
            this.version = 0;
            this.owner_flg = null;
            this.app_svc_id = null;
        }

        public BusinessService(string bus_svc_cd, string svc_name, int version, string owner_flg, string app_svc_id)
        {
            this.bus_svc_cd = bus_svc_cd;
            this.svc_name = svc_name;
            this.version = version;
            this.owner_flg = owner_flg;
            this.app_svc_id = app_svc_id;
        }

        public void setBus_svc_cd(string bus_svc_cd) { this.bus_svc_cd = bus_svc_cd; }
        public string getBus_svc_cd() { return this.bus_svc_cd; }

        public void setSvc_name(string svc_name) { this.svc_name = svc_name; }
        public string getSvc_name() { return this.svc_name; }

        public void setVersion(int version) { this.version = version; }
        public int getVersion() { return this.version; }

        public void setOwner_flg(string owner_flg) { this.owner_flg = owner_flg; }
        public string getOwner_flg() { return this.owner_flg; }

        public void setapp_svc_id(string app_svc_id) { this.app_svc_id = app_svc_id; }
        public string getapp_svc_id() { return this.app_svc_id; }
    }
}
