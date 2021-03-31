using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms_NET_Framework4
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            if (Form1.useSQLEditor)
                richTextBox1.Text = Form1.dbSQLEditor;
            else
                richTextBox1.Text = Form1.dbSQL;
        }

        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1.dbSQLEditor = richTextBox1.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.dbSQL = richTextBox1.Text;
        }
    }
}
