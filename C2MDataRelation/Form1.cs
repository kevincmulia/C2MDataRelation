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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //QUERY BO SCHEMA
                richTextBox1.Clear();
                string query = "SELECT SCHEMA_DEFN FROM F1_SCHEMA WHERE SCHEMA_NAME='" + textBox1.Text + "'";
                OracleCommand orc = new OracleCommand(query, conn);
                using (OracleDataReader orr = orc.ExecuteReader())
                {
                    if (orr.HasRows)
                    {
                        while (orr.Read())
                        {
                            richTextBox1.Text = orr.GetString(0);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
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
    }
}
