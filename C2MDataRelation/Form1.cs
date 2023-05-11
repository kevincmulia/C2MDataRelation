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

        private string getSchemas(string schemaName)
        {
            string temp = "";
            string schema = @"" + getSchema(schemaName);
            XmlReader reader = XmlReader.Create(new System.IO.StringReader(schema));
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "includeBO" || reader.Name == "includeDA" || reader.Name == "includeBS")
                        {
                            temp += "<";
                            while (reader.MoveToNextAttribute())
                            {
                                temp += reader.Value + ">\n";
                                temp += getSchemas(reader.Value) + "</" + reader.Value + ">\n";
                            }
                            break;
                        }
                        else
                        {
                            temp += "<" + reader.Name;
                            while (reader.MoveToNextAttribute())
                            {
                                temp += " " + reader.Name + "='" + reader.Value + "'";
                            }
                            temp += ">\n";
                            break;
                        }
                        return temp;
                    case XmlNodeType.Text:
                        temp += reader.Value;
                        break;
                    case XmlNodeType.EndElement:
                        temp += "</" + reader.Name + ">\n";
                        break;
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

        private void button1_Click(object sender, EventArgs e)
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
                            richTextBox1.Text = getSchemas(textBox1.Text);
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message);
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
    }
}
