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

        public DataArea(OracleDataReader odr, OracleConnection con)
        {
            this.conn = con;
            this.data_area_cd = odr.GetString(odr.GetOrdinal("DATA_AREA_CD"));
            this.version = odr.GetInt16(odr.GetOrdinal("VERSION"));
            this.owner_flg = odr.GetString(odr.GetOrdinal("OWNER_FLG"));
            this.f1_ext_data_area_cd = odr.GetString(odr.GetOrdinal("f1_ext_data_area_cd"));
            this.rootSchema = getSchema(odr.GetString(odr.GetOrdinal("DATA_AREA_CD")));
            this.finalSchema = getFullSchema(odr.GetString(odr.GetOrdinal("DATA_AREA_CD")));
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
