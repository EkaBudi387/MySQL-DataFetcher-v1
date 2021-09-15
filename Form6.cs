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

        string path_access = Path.Combine(Directory.GetCurrentDirectory(), "accessPath.txt");
        string path_log = Path.Combine(Directory.GetCurrentDirectory(), "logPath.txt");

        string path_accessSQL = Path.Combine(Directory.GetCurrentDirectory(), "accessPathSQL.txt");
        string path_logSQL = Path.Combine(Directory.GetCurrentDirectory(), "logPathSQL.txt");


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
                    try
                    {
                        File.WriteAllText(path_access, GlobalCryptography.Decrypt(File.ReadAllText(path_access)));
                        File.WriteAllText(path_log, GlobalCryptography.Decrypt(File.ReadAllText(path_log)));
                    }
                    catch
                    {
                        MessageBox.Show("No path stored");
                    }

                    if (textBox1.Text != "")
                        File.AppendAllText(path_access, textBox1.Text + "\n");

                    if (textBox2.Text != "")
                        File.AppendAllText(path_log, textBox2.Text + "\n");

                    File.WriteAllText(path_access, GlobalCryptography.Encrypt(File.ReadAllText(path_access)));
                    File.WriteAllText(path_log, GlobalCryptography.Encrypt(File.ReadAllText(path_log)));

                    MessageBox.Show("Path Added", "Information");
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
                    try
                    {
                        File.WriteAllText(path_accessSQL, GlobalCryptography.Decrypt(File.ReadAllText(path_accessSQL)));
                        File.WriteAllText(path_logSQL, GlobalCryptography.Decrypt(File.ReadAllText(path_logSQL)));
                    }
                    catch
                    {
                        MessageBox.Show("No path stored");
                    }

                    if (textBox1.Text != "")
                        File.AppendAllText(path_accessSQL, textBox1.Text + "\n");

                    if (textBox2.Text != "")
                        File.AppendAllText(path_logSQL, textBox2.Text + "\n");

                    File.WriteAllText(path_accessSQL, GlobalCryptography.Encrypt(File.ReadAllText(path_accessSQL)));
                    File.WriteAllText(path_logSQL, GlobalCryptography.Encrypt(File.ReadAllText(path_logSQL)));

                    MessageBox.Show("Path Added", "Information");
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
