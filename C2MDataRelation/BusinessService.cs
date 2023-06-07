﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2MDataRelation
{
    class BusinessService
    {
        private string bus_svc_cd, svc_name, owner_flg, app_svc_id;
        private string rootSchema, finalSchema;
        private int version;

        public BusinessService()
        {
            this.bus_svc_cd = null;
            this.svc_name = null;
            this.version = 0;
            this.owner_flg = null;
            this.app_svc_id = null;
        }

        public BusinessService(OracleDataReader odr)
        {
            this.bus_svc_cd = odr.GetString(odr.GetOrdinal("BUS_SVC_CD"));
            this.svc_name = odr.GetString(odr.GetOrdinal("SVC_NAME"));
            this.version = odr.GetInt16(odr.GetOrdinal("VERSION"));
            this.owner_flg = odr.GetString(odr.GetOrdinal("OWNER_FLG"));
            this.app_svc_id = odr.GetString(odr.GetOrdinal("APP_SVC_ID"));
            this.rootSchema = "";
            this.finalSchema = "";
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
