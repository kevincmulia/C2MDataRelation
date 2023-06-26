using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Xml;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;
using System.Xml.Schema;

namespace C2MDataRelation
{
    public partial class Form1 : Form
    {
        //GLOBAL VAR
        private OracleConnection conn;
        Dictionary<String, UsageCalcRule> usageRulesLoaded;
        RateSchedule rateScheduleLoaded;
        List<BusinessObject> businessObjects = new List<BusinessObject>();
        List<BusinessService> businessServices = new List<BusinessService>();
        List<DataArea> dataAreas = new List<DataArea>();
        List<Algorithm> algorithms = new List<Algorithm>();

        public void connToDB()
        {
            try
            {
                string connStr = "Data Source=143.47.112.98:1521/C2MDB;User Id=CISADM;Password=CISADM;Pooling=true";
                conn = new OracleConnection(connStr);

                conn.Open();
                MessageBox.Show("Connection to database Ready!");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Algorithm");
        }
        private void changeSomething(string tag, string value)
        {

        }

        private void AddNode(XmlNode inXmlNode, TreeNode inTreeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList nodeList;
            int i = 0;
            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];
                    inTreeNode.Nodes.Add(new TreeNode(xNode.Name));
                    tNode = inTreeNode.Nodes[i];
                    AddNode(xNode, tNode);
                }
            }
            else
            {
                inTreeNode.Text = inXmlNode.Name.ToString();
            }
        }

        private string removeSchema(string xml)
        {
            string line, nxml = "";
            StringReader sr = new StringReader(xml);
            while (true)
            {
                line = sr.ReadLine();
                if (line == "<schema>" || line == "</schema>")
                {

                }
                else
                {
                    nxml += line + "\n";
                }
                if (line == null)
                {
                    break;
                }
            }
            return nxml;
        }

        private string findSchemaAffected(string schemaName)
        {
            string query = "SELECT schema_name from f1_schema where schema_defn like '%" + schemaName + "%'";
            //MessageBox.Show(query);
            OracleCommand orc = new OracleCommand(query, conn);
            string included = "";
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        included += orr.GetString(0) + "\n";
                    }
                }
            }
            //if arraylist.size = 0 then its not included in other schema
            return included;
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


        private List<UsageCalcRule> findUsageRuleReferences(string usageRuleName)
        {
            string query = "SElect * from D1_USG_RULE where referred_usg_grp_cd = '" + usageRuleName + "'";
            //MessageBox.Show(query);
            OracleCommand orc = new OracleCommand(query, conn);
            List<UsageCalcRule> result = new List<UsageCalcRule>();
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        result.Add(new UsageCalcRule(orr));
                    }
                }
            }
            //if list.size = 0 then its not included in other schema
            return result;
        }
        /**
        * return list of usage group that is affected the usage group and the groups that has a rule calling to the usage group
        * **/
        private List<String> findUsageGroupAffected(string usageGroup)
        {
            //select usg_grp_cd from D1_USG_RULE where usg_rule_cd ='KM-DSC';
            //select usg_grp_cd from D1_USG_RULE where referred_usg_grp_cd in (select usg_grp_cd from D1_USG_RULE where usg_rule_cd = 'KM-DSC');
            string query = "SElect distinct(usg_grp_cd) from D1_USG_RULE where usg_grp_cd = '" + usageGroup + "' or referred_usg_grp_cd = '" + usageGroup + "'";
            //MessageBox.Show(query);
            OracleCommand orc = new OracleCommand(query, conn);
            List<String> result = new List<String>();
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        result.Add(orr.GetString(orr.GetOrdinal("USG_GRP_CD")));
                    }
                }
            }
            //if arraylist.size = 0 then its not included in other schema
            return result;
        }
        /**
         * returnlist of usage rule given the usage group name
         * **/
        private List<UsageCalcRule> getUsageRuleFromUsageGroup(string usageGroup)
        {
            string query = "SElect * from D1_USG_RULE where usg_grp_cd = '" + usageGroup + "'";

            //MessageBox.Show(query);
            OracleCommand orc = new OracleCommand(query, conn);
            List<UsageCalcRule> result = new List<UsageCalcRule>();
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        UsageCalcRule ur = new UsageCalcRule(orr);
                        ur.Schema = getFullSchema(ur.UsageRuleType);
                        ur.Schema = combineSchemaAndBoDataArea(ur.Schema, ur.BoDataArea);
                        //ur.EligibilityCriteria = getEligibilityCriterias(ur.Name);
                        //ur.getSq();
                        result.Add(ur);
                    }
                }
            }
            return result;
        }
        /**
         * return list of eligibility criteria given the usage rule name
         * **/
        private List<EligibilityCriteria> getEligibilityCriterias(string usageRuleName)
        {
            string query = "select * from D1_USG_RULE_ELIG_CRIT where usg_rule_cd = '" + usageRuleName + "'";
            //MessageBox.Show(query);
            OracleCommand orc = new OracleCommand(query, conn);
            List<EligibilityCriteria> result = new List<EligibilityCriteria>();
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        result.Add(new EligibilityCriteria(orr));
                    }
                }
            }
            return result;
        }
        private RateSchedule getRateSchedule(string rateScheduleName)
        {
            string query = "SElect * from CI_RS where RS_CD = '" + rateScheduleName + "'";

            RateSchedule rs = new RateSchedule();
            OracleCommand orc = new OracleCommand(query, conn);
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        rs = new RateSchedule(orr);

                    }
                }
            }
            rs.PreProcessCalcGroupList = getCalcGroup(rateScheduleName, "C1_RS_PRE_PROC");
            rs.RateVersionCalcGroupList = getCalcGroup(rateScheduleName, "C1_RS_RV2");
            rs.PostProcessCalcGroupList = getCalcGroup(rateScheduleName, "C1_RS_POST_PROC");
            return rs;
        }
        private List<CalcRule> getCalcRule(String calcGroupName)
        {
            string query = "SElect * from C1_CALC_RULE where CALC_GRP_CD = '" + calcGroupName + "'";

            List<CalcRule> result = new List<CalcRule>();
            OracleCommand orc = new OracleCommand(query, conn);
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        CalcRule temp = new CalcRule(orr);
                        temp.Schema = getFullSchema(temp.CalcRuleType);
                        temp.Schema = combineSchemaAndBoDataArea(temp.Schema, temp.BoDataArea);

                        result.Add(temp);
                    }
                }
            }
            return result;

        }
        private List<CalcGroup> getCalcGroup(String rateScheduleName, String tableName)
        {
            string query = "SElect * from " + tableName + " where RS_CD = '" + rateScheduleName + "'";
            List<CalcGroup> result = new List<CalcGroup>();
            OracleCommand orc = new OracleCommand(query, conn);
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        CalcGroup temp;//only pre calc and post calc has seq while the rate version calc does not
                        if (tableName.Equals("C1_RS_RV2"))
                        {
                            temp = new CalcGroup(orr, false);
                        }
                        else
                        {
                            temp = new CalcGroup(orr, true);
                        }
                        temp.CalcRuleList = getCalcRule(temp.CalcGroupName);
                        result.Add(temp);
                    }
                }
            }
            return result;
        }
        private string getFullSchema(string schemaName)
        {
            return "<" + schemaName + ">\n" + getSchemas(schemaName) + "</" + schemaName + ">";
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

        private string getInclude(string schemaName)
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
                            temp += getInclude(xn.Attributes["name"].Value) + xn.Attributes["name"].Value + "\n";
                        }
                    }
                }
            }
            return temp;
        }

        //GET BUSINESS OBJECT SCHEMA
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

        public void fillTV(XmlDocument xmldoc)
        {

            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(new TreeNode(xmldoc.DocumentElement.Name));
            TreeNode node = new TreeNode();
            node = treeView1.Nodes[0];
            AddNode(xmldoc.DocumentElement, node);
        }

        public void printRTB(XmlDocument xmldoc)
        {
            richTextBox1.Clear();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.Encoding = Encoding.UTF8;
            xmlWriterSettings.NewLineOnAttributes = false;
            using (StringWriter sw = new StringWriter())
            {
                using (var xw = XmlWriter.Create(sw, xmlWriterSettings))
                {
                    xmldoc.Save(xw);
                }
                richTextBox1.Text = sw.ToString();
            }
        }
        public XmlDocument changeToXmldoc(String allSchema)
        {
            string xmlTV = removeSchema(allSchema);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xmlTV);
            return xmldoc;
        }
        public void displayTVandRTB(String allSchema)
        {
            XmlDocument xmldoc = changeToXmldoc(allSchema);
            fillTV(xmldoc);
            printRTB(xmldoc);


        }
        public String combineSchemaAndBoDataArea(String schema, String boDataArea)
        {
            String result = "";
            if (schema != null && boDataArea != null)
            {
                Dictionary<String, String> nodes = new Dictionary<string, string>();

                XmlDocument xd = new XmlDocument();
                xd.LoadXml(boDataArea);
                XmlNode node = xd.DocumentElement;

                XmlNode xn = node.ChildNodes[0];//initializing only

                XmlNodeList cNode = node.ChildNodes;
                for (int i = 0; i < cNode.Count; i++)
                {
                    xn = node.ChildNodes[i];
                    if (xn.NodeType == XmlNodeType.Element)
                    {
                        nodes.Add(xn.Name, xn.InnerXml);
                    }
                }
                xd = new XmlDocument();
                xd.LoadXml(schema);
                node = xd.DocumentElement;
                cNode = node.ChildNodes;
                for (int i = 0; i < cNode.Count; i++)
                {
                    xn = node.ChildNodes[i];
                    if (xn.NodeType == XmlNodeType.Element)
                    {
                        if (nodes.ContainsKey(xn.Name))
                        {
                            xn.InnerXml = nodes[xn.Name];
                        }
                    }
                }
                result = xd.OuterXml;
                //asdasd
            }
            return result;
        }
        public void getInfo(string name, string type)
        {
            string queries = "";

            if (type == "Business Object")
                queries = "SELECT * FROM F1_BUS_OBJ WHERE BUS_OBJ_CD='" + name + "'";
            else if (type == "Business Service")
                queries = "SELECT * FROM F1_BUS_SVC WHERE BUS_SVC_CD='" + name + "'";
            else if (type == "Data Area")
                queries = "SELECT * FROM F1_DATA_AREA WHERE DATA_AREA_CD='" + name + "'";
            else if (type == "Algorithm")
                queries = "SELECT FROM (CI_ALG alg JOIN C1_ALG_TYPE algt on alg.ALG_TYPE_CD=algt.ALG_TYPE_CD) WHERE alg.ALG_CD='" + name + "'";

            OracleCommand orc = new OracleCommand(queries, conn);
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        if (type == "Business Object")
                        {
                            businessObjects.Add(new BusinessObject(orr, findSchemaAffected(name), getInclude(name), getSchema(name), getFullSchema(name)));
                        }
                        else if (type == "Business Service")
                        {
                            businessServices.Add(new BusinessService(orr, findSchemaAffected(name), getInclude(name), getSchema(name), getFullSchema(name)));
                        }else if (type == "Data Area")
                        {
                            dataAreas.Add(new DataArea(orr, findSchemaAffected(name), getInclude(name), getSchema(name), getFullSchema(name)));
                        }else if (type == "Algorithm")
                        {
                            algorithms.Add(new Algorithm(orr));
                        }
                    }
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //TREEVIEW LABEL
            label3.Text = textBox1.Text;

            if (conn != null && conn.State == ConnectionState.Open)
            {
                if (textBox1.Text != "")
                {
                    if (comboBox1.Text.Equals("Business Object") || comboBox1.Text.Equals("Business Service") || comboBox1.Text.Equals("Data Area"))
                    {
                        try
                        {
                            if (comboBox1.Text.Equals("Business Object"))
                            {
                                bool newBO = true;
                                foreach (BusinessObject bo in businessObjects)
                                {
                                    if (bo.getBus_obj_cd() == textBox1.Text)
                                    {
                                        displayTVandRTB(bo.getfinalSchema());
                                        newBO = false;
                                    }
                                }
                                if (newBO)
                                {
                                    getInfo(textBox1.Text, comboBox1.Text);
                                    displayTVandRTB(businessObjects[businessObjects.Count - 1].getfinalSchema());
                                }
                            }
                            else if (comboBox1.Text.Equals("Business Service"))
                            {
                                bool newBS = true;
                                foreach (BusinessService bs in businessServices)
                                {
                                    if (bs.getBus_svc_cd() == textBox1.Text)
                                    {
                                        displayTVandRTB(bs.getfinalSchema());
                                        newBS = false;
                                    }
                                }
                                if (newBS)
                                {
                                    getInfo(textBox1.Text, "Business Service");
                                    displayTVandRTB(businessServices[businessServices.Count - 1].getfinalSchema());
                                }
                            }
                            else if (comboBox1.Text.Equals("Data Area"))
                            {
                                bool newDA = true;
                                foreach (DataArea da in dataAreas)
                                {
                                    if (da.getData_area_cd() == textBox1.Text)
                                    {
                                        displayTVandRTB(da.getfinalSchema());
                                        newDA = false;
                                    }
                                }
                                if (newDA)
                                {
                                    getInfo(textBox1.Text, "Data Area");
                                    displayTVandRTB(dataAreas[dataAreas.Count - 1].getfinalSchema());
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message);
                        }
                    }
                    else if (comboBox1.Text.Equals("Usage Calculation Group"))
                    { //Usage Calculation Group
                        richTextBox1.Clear();
                        string usageRuleGroupName = textBox1.Text;
                        UsageCalcGroup ug = new UsageCalcGroup(usageRuleGroupName);
                        ug.UsageRuleList = getUsageRuleFromUsageGroup(usageRuleGroupName);
                        usageRulesLoaded = new Dictionary<string, UsageCalcRule>();
                        treeView1.Nodes.Clear();
                        treeView1.Nodes.Add(new TreeNode(usageRuleGroupName));
                        foreach (UsageCalcRule ur in ug.UsageRuleList)//to be used in another place
                        {
                            usageRulesLoaded.Add(ur.Name, ur);
                            treeView1.Nodes[0].Nodes.Add(ur.Name);

                        }
                        richTextBox1.Text = ug.print();
                    }
                    else if (comboBox1.Text.Equals("Rate Schedule"))
                    {
                        richTextBox1.Clear();
                        treeView1.Nodes.Clear();
                        string rateScheduleName = textBox1.Text;
                        RateSchedule rs = getRateSchedule(rateScheduleName);
                        rateScheduleLoaded = rs;

                        treeView1.Nodes.Add(new TreeNode(rateScheduleName));
                        treeView1.Nodes[0].Nodes.Add("Pre-Processing Calculation Groups");
                        treeView1.Nodes[0].Nodes.Add("Rate Version Calculation Groups");
                        treeView1.Nodes[0].Nodes.Add("Post-Processing Calculation Groups");

                        if (rs.PreProcessCalcGroupList != null)
                        {
                            foreach (CalcGroup cg in rs.PreProcessCalcGroupList)
                            {
                                TreeNode preTN = new TreeNode(cg.CalcGroupName);
                                foreach (CalcRule cr in cg.CalcRuleList)
                                {
                                    preTN.Nodes.Add(cr.Name);
                                }
                                treeView1.Nodes[0].Nodes[0].Nodes.Add(preTN);
                            }
                        }
                        if (rs.RateVersionCalcGroupList != null)
                        {
                            foreach (CalcGroup cg in rs.RateVersionCalcGroupList)
                            {
                                TreeNode preTN = new TreeNode(cg.CalcGroupName);
                                foreach (CalcRule cr in cg.CalcRuleList)
                                {
                                    preTN.Nodes.Add(cr.Name);
                                }
                                treeView1.Nodes[0].Nodes[1].Nodes.Add(preTN);
                            }
                        }
                        if (rs.PostProcessCalcGroupList != null)
                        {
                            foreach (CalcGroup cg in rs.PostProcessCalcGroupList)
                            {
                                TreeNode preTN = new TreeNode(cg.CalcGroupName);
                                foreach (CalcRule cr in cg.CalcRuleList)
                                {
                                    preTN.Nodes.Add(cr.Name);
                                }
                                treeView1.Nodes[0].Nodes[2].Nodes.Add(preTN);
                            }
                        }

                        richTextBox1.Text = rs.print();//testing
                        //to do stuff cache


                    }else if (comboBox1.Text.Equals("Algorithm"))
                    {
                        bool newALG = true;
                        foreach (Algorithm alg in algorithms)
                        {
                            if (alg.getAlg_cd() == textBox1.Text)
                            {
                                richTextBox1.Clear();
                                richTextBox1.Text = alg.ToString();
                                newALG = false;
                            }
                        }
                        if (newALG)
                        {
                            getInfo(textBox1.Text, "Algorithm");
                            richTextBox1.Clear();
                            richTextBox1.Text = algorithms[algorithms.Count - 1].ToString();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please enter an objects name");
                }
            }
            else
            {
                MessageBox.Show("Connection to database is not initiated!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connToDB();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Close();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                if (textBox1.Text != "")
                {
                    if (comboBox1.Text.Equals("Business Object") || comboBox1.Text.Equals("Business Service") || comboBox1.Text.Equals("Data Area"))
                    {
                        try
                        {
                            //QUERY BO SCHEMA
                            richTextBox1.Clear();
                            if (comboBox1.Text.Equals("Business Object"))
                            {
                                foreach (BusinessObject bo in businessObjects)
                                {
                                    if (bo.getBus_obj_cd() == textBox1.Text)
                                    {
                                        richTextBox1.AppendText("Mentioned By:\n");
                                        richTextBox1.AppendText(bo.getIncluded());
                                        richTextBox1.AppendText("\nMentioning:\n");
                                        richTextBox1.AppendText(bo.getIncluding());
                                    }
                                }
                            }
                            else if (comboBox1.Text.Equals("Business Service"))
                            {
                                foreach (BusinessService bs in businessServices)
                                {
                                    if (bs.getBus_svc_cd() == textBox1.Text)
                                    {
                                        richTextBox1.AppendText("Mentioned By:\n");
                                        richTextBox1.AppendText(bs.getIncluded());
                                        richTextBox1.AppendText("\nMentioning:\n");
                                        richTextBox1.AppendText(bs.getIncluding());
                                    }
                                }
                            }
                            else if (comboBox1.Text.Equals("Data Area"))
                            {
                                foreach (DataArea da in dataAreas)
                                {
                                    if (da.getData_area_cd() == textBox1.Text)
                                    {
                                        richTextBox1.AppendText("Mentioned By:\n");
                                        richTextBox1.AppendText(da.getIncluded());
                                        richTextBox1.AppendText("\nMentioning:\n");
                                        richTextBox1.AppendText(da.getIncluding());
                                    }
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message);
                        }
                    }
                    else if (comboBox1.Text.Equals("Usage Calculation Group"))
                    {
                        string text = textBox1.Text;

                        UsageCalcRule usageRule;
                        String result = "";
                        if (usageRulesLoaded.ContainsKey(text))//calc rulle
                        {
                            usageRule = usageRulesLoaded[text];
                            foreach (KeyValuePair<string, UsageCalcRule> entry in usageRulesLoaded)
                            {
                                //entry.Value or entry.Key
                                if (!entry.Key.Equals(usageRule.Name) && entry.Value.sqListEquals(usageRule))
                                {
                                    result += entry.Value.Name + "\n";
                                }
                            }
                        }
                        else
                        {//calc group
                            List<String> usageGrpAffected = findUsageGroupAffected(text);
                            foreach (String entry in usageGrpAffected)
                            {
                                result += "\n" + entry;
                            }
                        }
                        richTextBox1.Clear();
                        richTextBox1.Text = result;
                    }
                }
                else
                {
                    MessageBox.Show("Please enter an objects name");
                }
            }
            else
            {
                MessageBox.Show("Connection to database is not initiated!");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //richTextBox1.Clear();
            //richTextBox1.Text = e.ToString();
        }
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            textBox1.Text = e.Node.Text;
            if (comboBox1.Text.Equals("Usage Calculation Group") && usageRulesLoaded != null)
            {
                richTextBox1.Clear();
                if (usageRulesLoaded.ContainsKey(e.Node.Text))
                {
                    XmlDocument xmldoc = changeToXmldoc(usageRulesLoaded[e.Node.Text].Schema);
                    printRTB(xmldoc);
                }
                else
                {
                    richTextBox1.Text = "schema not found";
                }

            }
            else if (comboBox1.Text.Equals("Rate Schedule") && rateScheduleLoaded != null)
            {
                TreeNode clicked = e.Node;
                if (clicked.Parent != null && clicked.Parent.Text.Equals("Pre-Processing Calculation Groups")) //calc groups
                {
                    foreach (CalcGroup cg in rateScheduleLoaded.PreProcessCalcGroupList)
                    {
                        if (cg.CalcGroupName.Equals(e.Node.Text))
                        {
                            richTextBox1.Text = cg.print();
                        }
                    }
                }
                else if (clicked.Parent != null && clicked.Parent.Text.Equals("Post-Processing Calculation Groups"))//calc groups
                {
                    foreach (CalcGroup cg in rateScheduleLoaded.PostProcessCalcGroupList)
                    {
                        if (cg.CalcGroupName.Equals(e.Node.Text))
                        {
                            richTextBox1.Text = cg.print();
                        }
                    }
                }
                else if (clicked.Parent != null && clicked.Parent.Text.Equals("Rate Version Calculation Groups"))//calc groups
                {
                    foreach (CalcGroup cg in rateScheduleLoaded.RateVersionCalcGroupList)
                    {
                        if (cg.CalcGroupName.Equals(e.Node.Text))
                        {
                            richTextBox1.Text = cg.print();
                        }
                    }
                }
                else if (clicked.Nodes.Count == 0)//calc rule
                {

                    if (e.Node.Parent.Parent.Text.Equals("Pre-Processing Calculation Groups"))
                    {
                        foreach (CalcGroup cg in rateScheduleLoaded.PreProcessCalcGroupList)
                        {
                            if (cg.CalcGroupName.Equals(e.Node.Parent))
                            {
                                foreach (CalcRule cr in cg.CalcRuleList)
                                {
                                    if (cr.Name.Equals(e.Node.Text))
                                    {
                                        richTextBox1.Text = cr.print();
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else if (e.Node.Parent.Parent.Text.Equals("Rate Version Calculation Groups"))
                    {
                        foreach (CalcGroup cg in rateScheduleLoaded.RateVersionCalcGroupList)
                        {
                            if (cg.CalcGroupName.Equals(e.Node.Parent.Text))
                            {
                                foreach (CalcRule cr in cg.CalcRuleList)
                                {
                                    if (cr.Name.Equals(e.Node.Text))
                                    {
                                        richTextBox1.Text = cr.print();
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else if (e.Node.Parent.Parent.Text.Equals("Post-Processing Calculation Groups"))
                    {
                        foreach (CalcGroup cg in rateScheduleLoaded.PostProcessCalcGroupList)
                        {
                            if (cg.CalcGroupName.Equals(e.Node.Parent))
                            {
                                foreach (CalcRule cr in cg.CalcRuleList)
                                {
                                    if (cr.Name.Equals(e.Node.Text))
                                    {
                                        richTextBox1.Text = cr.print();
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

        }
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
          
            
        }
    }
}
