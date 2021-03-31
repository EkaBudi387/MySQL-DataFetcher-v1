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
    public partial class Form3 : Form
    {

        public Form3()
        {
            InitializeComponent();
        }

        private void buttonFormClosed(object sender, FormClosedEventArgs e)
        {

            Form1.setConditionTable.Rows[0][Form1.field] = richTextBox1.Text;

            Form1.dbCondition = null;

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                richTextBox1.Text = Form1.setConditionTable.Rows[0][Form1.field].ToString();
            }
            catch
            {
                richTextBox1.Text = null;
            }
        }
    }
}
