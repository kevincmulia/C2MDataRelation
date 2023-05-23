﻿using Oracle.ManagedDataAccess.Client;
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
        OracleConnection conn;
        XmlDocument xmldoc;

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

        }
        private void changeSomething(string tag,string value) { 
            
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

        private ArrayList findSchemaAffected(string schemaName) { 
            string query = "SELECT schema_name from f1_schema where schema_defn like '%\""+schemaName+"\"%'";
            //MessageBox.Show(query);
            OracleCommand orc = new OracleCommand(query, conn);
            ArrayList arrList = new ArrayList();
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        arrList.Add(orr.GetString(0));
                    }
                }
            }
            //if arraylist.size = 0 then its not included in other schema
            return arrList;
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

        
        private List<UsageRule> findUsageRuleReferences(string usageRuleName) { 
            string query = "SElect * from D1_USG_RULE where referred_usg_grp_cd = '"+usageRuleName+"'";
            //MessageBox.Show(query);
            OracleCommand orc = new OracleCommand(query, conn);
            List<UsageRule> result = new List<UsageRule>();
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        result.Add(new UsageRule(orr));
                    }
                }
            }
            //if list.size = 0 then its not included in other schema
            return result;
        }
        /**
        * return list of usage group that is affected by the usage rule name whether its called by other usage rule and its own usage group
        * **/
        private List<usageGroup> findUsageGroupAffected(string usageRuleName) { 
            string query = "SElect distinct(usg_grp_cd) from D1_USG_RULE where usg_rule_cd = '"+usageRuleName+"' or referred_usg_grp_cd = '"+usageRuleName+"'";
            //MessageBox.Show(query);
            OracleCommand orc = new OracleCommand(query, conn);
            List<usageGroup> result = new List<usageGroup>();
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        result.Add(new usageGroup(orr.GetString(0)));
                    }
                }
            }
            //if arraylist.size = 0 then its not included in other schema
            return result;
        }
        /**
         * return usage rule given arraylist of usage rule name
         * **/
        private List<UsageRule> getUsageRule(List<String> usageRuleList)
        {
            string query = "SElect * from D1_USG_RULE where";
            foreach (string usageRuleName in usageRuleList) { 
                   query = query+" usg_rule_cd ='"+usageRuleName+"' or";
            }
            query = query.Substring(0,query.Length-3);
            //MessageBox.Show(query);
            OracleCommand orc = new OracleCommand(query, conn);
            List<UsageRule> result =new List<UsageRule>();
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        result.Add(new UsageRule(orr));
                    }
                }
            }
            return result;
        }
        //get usage rule basedo n the name
        private UsageRule getUsageRule(string usageRuleName)
        {
            string query = "SElect * from D1_USG_RULE where  usg_rule_cd ='" + usageRuleName + "'";
            
            //MessageBox.Show(query);
            OracleCommand orc = new OracleCommand(query, conn);
            List<UsageRule> result = new List<UsageRule>();
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                       return (new UsageRule(orr));
                    }
                }
            }
            return null;
        }

        /**
         * returnlist of usage rule given the usage group name
         * **/
        private List<UsageRule> getUsageRuleFromUsageGroup(string usageGroup)
        {
            string query = "SElect * from D1_USG_RULE where usg_grp_cd = '" + usageGroup + "'";
            
            MessageBox.Show(query);
            OracleCommand orc = new OracleCommand(query, conn);
            List<UsageRule> result = new List<UsageRule>();
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        result.Add(new UsageRule(orr));
                    }
                }
            }
            return result;
        }
        /**
         * return list of usage rules that has the same group as the given usage rule
         * **/
        private List<UsageRule> findUsageRulesInTheSameGroup(string usageRuleName) { 
            string query = "select * from D1_USG_RULE where usg_grp_cd in (select usg_grp_cd from d1_usg_rule where usg_rule_cd ='"+usageRuleName+"'";
            //MessageBox.Show(query);
            OracleCommand orc = new OracleCommand(query, conn);
            List<UsageRule> result = new List<UsageRule>();
            using (OracleDataReader orr = orc.ExecuteReader())
            {
                if (orr.HasRows)
                {
                    while (orr.Read())
                    {
                        result.Add(new UsageRule(orr));
                    }
                }
            }
            //if arraylist.size = 0 then its not included in other schema
            return result;
        }
        /**
         * return list of eligibility criteria given the usage rule name
         * **/
        private List<EligibilityCriteria> getEligibilityCriterias(string usageRuleName) { 
            string query = "select * from D1_USG_RULE_ELIG_CRIT where usg_rule_cd = '"+usageRuleName+"'";
            //MessageBox.Show(query);
            OracleCommand orc = new OracleCommand(query, conn);
            List<EligibilityCriteria> result =new List<EligibilityCriteria>();
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

        //GET BUSINESS OBJECT SCHEMA
        private string getSchema(string schemaName) {
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

        public void fillTV(string allSchema)
        {
            string xmlTV = removeSchema(allSchema);
            xmldoc = new XmlDocument();
            xmldoc.LoadXml(xmlTV);
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(new TreeNode(xmldoc.DocumentElement.Name));
            TreeNode node = new TreeNode();
            node = treeView1.Nodes[0];
            AddNode(xmldoc.DocumentElement, node);
        }

        public void printRTB()
        {
            richTextBox1.Clear();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.Encoding = Encoding.UTF8;
            xmlWriterSettings.NewLineOnAttributes = true;
            using (StringWriter sw = new StringWriter())
            {
                using (var xw = XmlWriter.Create(sw, xmlWriterSettings))
                {
                    xmldoc.Save(xw);
                }
                richTextBox1.Text = sw.ToString();
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
                            //GET RESULT OF ALL SCHEMA
                            string allSchema = "<" + textBox1.Text + ">\n" + getSchemas(textBox1.Text) + "</" + textBox1.Text + ">";
                            MessageBox.Show(allSchema);

                            //FILL TREEVIEW
                            fillTV(allSchema);

                            //OVERALL SCHEMA
                            printRTB();
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
                        usageGroup ug = new usageGroup(usageRuleGroupName);
                        ug.UsageRuleList = getUsageRuleFromUsageGroup(usageRuleGroupName);
                        richTextBox1.Text = ug.print();
                    } 
                    else if (comboBox1.Text.Equals("Usage Calculation Rule"))
                    { //Usage Calculation Rule
                        string usageRuleName = textBox1.Text;
                        UsageRule temp = getUsageRule(usageRuleName);
                        string usageRuleGroupName = temp.UsageGroup;
                        usageGroup ug = new usageGroup(usageRuleGroupName);

                        ug.UsageRuleList = getUsageRuleFromUsageGroup(usageRuleGroupName);
                        richTextBox1.Text = ug.print();
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
                    try
                    {
                        //QUERY BO SCHEMA
                        richTextBox1.Clear();
                        ArrayList arr = findSchemaAffected(textBox1.Text);
                        if (arr.Count == 0)
                        {
                            MessageBox.Show("Schema is not reference in other schema");
                        }
                        else
                        {
                            foreach (string schema in arr)
                            {
                                richTextBox1.AppendText(schema + "\n");
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
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
            richTextBox1.Clear();
            richTextBox1.Text = e.Node.Text;

        }
    }
}
