using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.IO;
using System.Security.Principal;

namespace WindowsForms_NET_Framework4
{
    public partial class Form2 : Form
    {

        public static MySqlConnection connection;

        public static string server;
        public static string db;
        public static string port;
        public static string userID;
        public static string password;


        string path_Server = Path.Combine(Directory.GetCurrentDirectory(), "server.txt");
        string path_Port = Path.Combine(Directory.GetCurrentDirectory(), "port.txt");
        string path_Database = Path.Combine(Directory.GetCurrentDirectory(), "database.txt");
        string path_UserID = Path.Combine(Directory.GetCurrentDirectory(), "username.txt");
        string path_Password = Path.Combine(Directory.GetCurrentDirectory(), "password.txt");
        string path_entropy = Path.Combine(Directory.GetCurrentDirectory(), "entropy.txt");
        string path_log = Path.Combine(Directory.GetCurrentDirectory(), "logPath.txt");
        string path_access = Path.Combine(Directory.GetCurrentDirectory(), "accessPath.txt");
        string path_logger = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");


        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (checkBox1.Checked == true)
            {

                File.WriteAllText(path_Server, textBox1.Text);
                File.WriteAllText(path_Port, textBox2.Text);
                File.WriteAllText(path_Database, textBox3.Text);



                byte[] userID = Encoding.UTF8.GetBytes(textBox4.Text);
                byte[] password = Encoding.UTF8.GetBytes(textBox5.Text);

                byte[] entropy = new byte[20];

                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(entropy);
                }

                byte[] cipherUserID = ProtectedData.Protect(userID, entropy, DataProtectionScope.CurrentUser);
                byte[] cipherPassword = ProtectedData.Protect(password, entropy, DataProtectionScope.CurrentUser);

                File.WriteAllBytes(path_entropy, entropy);
                File.WriteAllBytes(path_UserID, cipherUserID);
                File.WriteAllBytes(path_Password, cipherPassword);


            }

            connection = TestToConnectMySQLServer.OpenConnection(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);

            try
            {
                string accessPaths = GlobalCryptography.Decrypt(File.ReadAllText(path_access));

                string[] accessPath = accessPaths.Split('\n');

                bool pathExist = false;

                foreach (string _ in accessPath)
                {
                    if (_ == Directory.GetCurrentDirectory())
                        pathExist = true;

                }

                if (connection.State == ConnectionState.Open && pathExist)
                {


                    server = textBox1.Text;
                    port = textBox2.Text;
                    db = textBox3.Text;
                    userID = textBox4.Text;
                    password = textBox5.Text;


                    Hide();

                    Form1 f1 = new Form1();

                    f1.Show();

                    File.AppendAllText(path_logger, DateTime.Now + "|" + WindowsIdentity.GetCurrent().Name + "|" + Environment.MachineName + "|" + Directory.GetCurrentDirectory() + "\n");

                }

                else
                {
                    MessageBox.Show("Current Path is Not Allowed", "Information");
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = File.ReadAllText(path_Server);
                textBox2.Text = File.ReadAllText(path_Port);
                textBox3.Text = File.ReadAllText(path_Database);

                byte[] userIDByte = File.ReadAllBytes(path_UserID);
                byte[] passwordByte = File.ReadAllBytes(path_Password);
                byte[] entropyByte = File.ReadAllBytes(path_entropy);

                byte[] decipherUserID = ProtectedData.Unprotect(userIDByte, entropyByte, DataProtectionScope.CurrentUser);
                byte[] decipherPassword = ProtectedData.Unprotect(passwordByte, entropyByte, DataProtectionScope.CurrentUser);

                textBox4.Text = Encoding.UTF8.GetString(decipherUserID);
                textBox5.Text = Encoding.UTF8.GetString(decipherPassword);
            }
            catch
            {
                textBox1.Text = null;
                textBox2.Text = null;
                textBox3.Text = null;
                textBox4.Text = null;
                textBox5.Text = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void addPath(object sender, EventArgs e)
        {
            Form6 form6 = new Form6();

            form6.Show();
        }
    }
}
