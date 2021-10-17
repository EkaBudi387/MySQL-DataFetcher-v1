using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace WindowsForms_NET_Framework4
{
    public partial class Form6 : Form
    {

        string path_access = "accessPath.txt";
        string path_log = "logPath.txt";

        string path_accessSQL = "accessPathSQL.txt";
        string path_logSQL = "logPathSQL.txt";


        public Form6()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Form2.IsMySQL)
            {
                if (textBox3.Text == "We202005%%")
                {

                    File.WriteAllText($"{Form2.personalMySQLpath}//{path_access}", GlobalCryptography.Encrypt(textBox1.Text));
                    File.WriteAllText($"{Form2.personalMySQLpath}//{path_log}", GlobalCryptography.Encrypt(textBox2.Text));

                    MessageBox.Show("Path Added", "Information");

                    this.Hide();
                }

                else
                {
                    MessageBox.Show("Wrong Password", "Information");
                }
            }
            else
            {
                if (textBox3.Text == "We202005%%")
                {

                    File.WriteAllText($"{Form2.personalMSSQLpath}//{path_accessSQL}", GlobalCryptography.Encrypt(textBox1.Text));
                    File.WriteAllText($"{Form2.personalMSSQLpath}//{path_logSQL}", GlobalCryptography.Encrypt(textBox2.Text));

                    MessageBox.Show("Path Added", "Information");

                    this.Hide();
                }

                else
                {
                    MessageBox.Show("Wrong Password", "Information");
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = Directory.GetCurrentDirectory();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = Directory.GetCurrentDirectory();

        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }
    }
}
