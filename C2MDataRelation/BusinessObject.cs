using Oracle.ManagedDataAccess.Client;
using System;
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
        private string rootSchema, finalSchema;
        private int version;
        OracleConnection conn;

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

        public BusinessObject(OracleDataReader odr, OracleConnection con)
        {
            this.conn = con;
            this.bus_obj_cd = odr.GetString(odr.GetOrdinal("BUS_OBJ_CD"));
            this.version = odr.GetInt16(odr.GetOrdinal("VERSION"));
            this.maint_obj_cd = odr.GetString(odr.GetOrdinal("MAINT_OBJ_CD"));
            this.app_svc_id = odr.GetString(odr.GetOrdinal("APP_SVC_ID"));
            this.life_cycle_bo_cd = odr.GetString(odr.GetOrdinal("LIFE_CYCLE_BO_CD"));
            this.owner_flg = odr.GetString(odr.GetOrdinal("OWNER_FLG"));
            this.parent_bo_cd = odr.GetString(odr.GetOrdinal("PARENT_BO_CD"));
            this.instance_ctrl_flg = odr.GetString(odr.GetOrdinal("INSTANCE_CTRL_FLG"));
            this.rootSchema = getSchema(odr.GetString(odr.GetOrdinal("BUS_OBJ_CD")));
            this.finalSchema = getFullSchema(odr.GetString(odr.GetOrdinal("BUS_OBJ_CD")));
        }

        private string getFullSchema(string schemaName)
        {
            return "<" + schemaName + ">\n" + getSchemas(schemaName) + "</" + schemaName + ">";
        }

        private string getSchema(string schemaName)
        {
            string query = "SELECT SCHEMA_DEFN FROM F1_SCHEMA WHERE SCHEMA_NAME='" + schemaName + "'";
            OracleCommand orc = new OracleCommand(query, conn);
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        return orr.GetString(0);
                    }
                }
                else
                {
                    return ("Schema might not exist!");
                }
            }
            return "error";
        }

        private string printCN(XmlNode parentNode)
        {
            string temp = "";
            XmlNodeList nl = parentNode.ChildNodes;
            for (int i = 0; i < nl.Count; i++)
            {
                XmlNode cN = parentNode.ChildNodes[i];
                if (cN.NodeType == XmlNodeType.Element)
                {
                    if (cN.Name == "includeBO" || cN.Name == "includeDA" || cN.Name == "includeBS")
                    {
                        temp += getSchemas(cN.Attributes["name"].Value);
                    }
                    else if (cN.HasChildNodes)
                    {
                        XmlNodeList xnl = cN.ChildNodes;
                        temp += "<" + cN.Name + ">\n";
                        temp += printCN(cN);
                        temp += "</" + cN.Name + ">\n";
                    }
                    else if (!cN.HasChildNodes)
                    {
                        if (cN.Name.ToLower().Contains("uihint"))
                        {

                        }
                        else
                        {
                            temp += "<" + cN.Name;
                            foreach (XmlAttribute att in cN.Attributes)
                            {
                                if (att.Name.ToLower().Contains("uihint"))
                                {

                                }
                                else
                                {
                                    temp += " " + att.Name + "=\"" + att.Value + "\"";
                                }
                            }
                            temp += "/>\n";
                        }
                        //temp += "<" + cN.Name + "/>\n";
                    }
                }
            }
            return temp;
        }

        private string getSchemas(string schemaName)
        {
            XmlNodeList cNode;
            string temp = "";
            string schema = @"" + getSchema(schemaName);
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(schema);
            XmlNode node = xd.DocumentElement;
            if (node.HasChildNodes)
            {
                cNode = node.ChildNodes;
                for (int i = 0; i < cNode.Count; i++)
                {
                    XmlNode xn = node.ChildNodes[i];
                    if (xn.NodeType == XmlNodeType.Element)
                    {
                        if (xn.Name == "includeBO" || xn.Name == "includeDA" || xn.Name == "includeBS")
                        {
                            temp += getSchemas(xn.Attributes["name"].Value);
                        }
                        else
                        {
                            if (xn.HasChildNodes)
                            {
                                temp += "<" + xn.Name + ">\n";
                                temp += printCN(xn);
                                temp += "</" + xn.Name + ">\n";
                            }
                            else
                            {
                                if (xn.Name.ToLower().Contains("uihint"))
                                {

                                }
                                else
                                {
                                    temp += "<" + xn.Name;
                                    foreach (XmlAttribute att in xn.Attributes)
                                    {
                                        if (att.Name.ToLower().Contains("uihint"))
                                        {

                                        }
                                        else
                                        {
                                            temp += " " + att.Name + "=\"" + att.Value + "\"";
                                        }
                                    }
                                    temp += "/>\n";
                                }
                            }
                        }
                    }
                    else if (xn.NodeType == XmlNodeType.EndElement)
                    {
                        temp += "</" + xn.Name + ">\n";
                    }
                }
            }
            return temp;
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

        public void setrootSchema(string rootSchema) { this.rootSchema = rootSchema; }
        public string getrootSchema() { return this.rootSchema; }

        public void setfinalSchema(string finalSchema) { this.finalSchema = finalSchema; }
        public string getfinalSchema() { return this.finalSchema; }

        public string ToString => "Business Object : " + bus_obj_cd + "\nVersion : " + version + "\nMaintanance Object Inherited : " + maint_obj_cd
            + "\nApplication Service : " + app_svc_id + "\nLife Cycle : " + life_cycle_bo_cd + "\nOwner Flag : " + owner_flg + "\nParent BO : " +
            parent_bo_cd + "\nInstance Control Flag : " + instance_ctrl_flg;
    }
}
