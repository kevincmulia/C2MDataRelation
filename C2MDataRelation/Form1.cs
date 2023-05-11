using Oracle.ManagedDataAccess.Client;
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

namespace C2MDataRelation
{
    public partial class Form1 : Form
    {
        //GLOBAL VAR
        OracleConnection conn;

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
        
        private ArrayList findSchemaAffected(string schemaName) { 
            string query = "SELECT schema_name from f1_schema where schema_defn like '%\""+schemaName+"\"%'";
            OracleCommand orc = new OracleCommand(query, conn);
            var arrList = new ArrayList();
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (conn != null && conn.State == ConnectionState.Open)
            {
                if (textBox1.Text != "")
                {
                    try
                    {
                        //bool
                        bool search = false;

                        //QUERY BO SCHEMA
                        richTextBox1.Clear();
                        string resultSC = @"" + getSchema(textBox1.Text);
                        //richTextBox1.Text = resultSC;

                        XmlReader reader = XmlReader.Create(new System.IO.StringReader(resultSC));
                        while (reader.Read())
                        {
                            switch(reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    if(reader.Name == "includeBO" || reader.Name == "includeDA" || reader.Name == "includeBS")
                                    {
                                        string includeBO = "";
                                        richTextBox1.Text += "<";
                                        while (reader.MoveToNextAttribute())
                                        {
                                            richTextBox1.Text += reader.Value + ">";
                                            includeBO = getSchema(reader.Value);
                                            StringReader strReader = new StringReader(includeBO);
                                            while (true)
                                            {
                                                string aLine = strReader.ReadLine();
                                                if (aLine != null)
                                                {
                                                    richTextBox1.Text += "  " + aLine + "\n";
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                            richTextBox1.Text += "</" + reader.Value + ">\n";
                                        }
                                    }
                                    else
                                    {
                                        richTextBox1.Text += "<" + reader.Name;
                                        while (reader.MoveToNextAttribute())
                                        {
                                            richTextBox1.Text += " " + reader.Name + "='" + reader.Value + "'";
                                        }
                                        richTextBox1.Text += ">\n";
                                    }
                                    break;
                                case XmlNodeType.Text:
                                    richTextBox1.Text += reader.Value;
                                    break;
                                case XmlNodeType.EndElement:
                                    richTextBox1.Text += "</" + reader.Name + ">\n";
                                    break;
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
                    MessageBox.Show("Enter Business Object's name!");
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
    }
}
