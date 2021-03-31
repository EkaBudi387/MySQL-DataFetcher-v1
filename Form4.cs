using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsForms_NET_Framework4
{
    public partial class Form4 : Form
    {

        public Form4()
        {
            InitializeComponent();
        }

        private void buttonFormClosed(object sender, FormClosedEventArgs e)
        {

            Form1.setConditionTable.Rows[0][Form1.field] = textBox1.Text;
            Form1.setConditionTable.Rows[1][Form1.field] = textBox2.Text;

            Form1.dbCondition = null;

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = Form1.setConditionTable.Rows[0][Form1.field].ToString();
                textBox2.Text = Form1.setConditionTable.Rows[1][Form1.field].ToString();

            }
            catch
            {
                textBox1.Text = null;
                textBox2.Text = null;
            }
        }
    }
}
