using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.IO;
using System.Security.Principal;
using System.Data.SqlClient;

namespace WindowsForms_NET_Framework4
{
    public partial class Form2 : Form
    {

        public static bool IsMySQL = true;

        public static MySqlConnection connection;

        public static string server;
        public static string db;
        public static string port;
        public static string userID;
        public static string password;

        public static SqlConnection connectionSQL;

        public static string serverSQL;
        public static string dbSQL;
        public static string userIDSQL;
        public static string passwordSQL;


        string path_Server = Path.Combine(Directory.GetCurrentDirectory(), "server.txt");
        string path_Port = Path.Combine(Directory.GetCurrentDirectory(), "port.txt");
        string path_Database = Path.Combine(Directory.GetCurrentDirectory(), "database.txt");
        string path_UserID = Path.Combine(Directory.GetCurrentDirectory(), "username.txt");
        string path_Password = Path.Combine(Directory.GetCurrentDirectory(), "password.txt");
        string path_entropy = Path.Combine(Directory.GetCurrentDirectory(), "entropy.txt");
        string path_log = Path.Combine(Directory.GetCurrentDirectory(), "logPath.txt");
        string path_access = Path.Combine(Directory.GetCurrentDirectory(), "accessPath.txt");
        string path_logger = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");


        string path_ServerSQL = Path.Combine(Directory.GetCurrentDirectory(), "serverSQL.txt");
        string path_DatabaseSQL = Path.Combine(Directory.GetCurrentDirectory(), "databaseSQL.txt");
        string path_UserIDSQL = Path.Combine(Directory.GetCurrentDirectory(), "usernameSQL.txt");
        string path_PasswordSQL = Path.Combine(Directory.GetCurrentDirectory(), "passwordSQL.txt");
        string path_entropySQL = Path.Combine(Directory.GetCurrentDirectory(), "entropySQL.txt");
        string path_logSQL = Path.Combine(Directory.GetCurrentDirectory(), "logPathSQL.txt");
        string path_accessSQL = Path.Combine(Directory.GetCurrentDirectory(), "accessPathSQL.txt");
        string path_loggerSQL = Path.Combine(Directory.GetCurrentDirectory(), "logSQL.txt");


        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MySQL.Checked)
            {

                if (checkBox1.Checked == true)
                {

                    IsMySQL = true;

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

                

                try
                {

                    connection = TestToConnectMySQLServer.OpenConnectionMySQL(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);

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
                finally
                {
                    connection.Close();
                }

            }
            else if (SQLserver.Checked)
            {
                if (checkBox1.Checked == true)
                {

                    IsMySQL = false;

                    File.WriteAllText(path_ServerSQL, textBox1.Text);
                    File.WriteAllText(path_DatabaseSQL, textBox3.Text);



                    byte[] userID = Encoding.UTF8.GetBytes(textBox4.Text);
                    byte[] password = Encoding.UTF8.GetBytes(textBox5.Text);

                    byte[] entropy = new byte[20];

                    using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                    {
                        rng.GetBytes(entropy);
                    }

                    byte[] cipherUserID = ProtectedData.Protect(userID, entropy, DataProtectionScope.CurrentUser);
                    byte[] cipherPassword = ProtectedData.Protect(password, entropy, DataProtectionScope.CurrentUser);

                    File.WriteAllBytes(path_entropySQL, entropy);
                    File.WriteAllBytes(path_UserIDSQL, cipherUserID);
                    File.WriteAllBytes(path_PasswordSQL, cipherPassword);


                }

                connectionSQL = TestToConnectMySQLServer.OpenConnectionSQL(textBox1.Text, textBox3.Text, textBox4.Text, textBox5.Text);

                try
                {
                    string accessPaths = GlobalCryptography.Decrypt(File.ReadAllText(path_accessSQL));

                    string[] accessPath = accessPaths.Split('\n');

                    bool pathExist = false;

                    foreach (string _ in accessPath)
                    {
                        if (_ == Directory.GetCurrentDirectory())
                            pathExist = true;

                    }

                    if (connectionSQL.State == ConnectionState.Open && pathExist)
                    {


                        serverSQL = textBox1.Text;
                        dbSQL = textBox3.Text;
                        userIDSQL = textBox4.Text;
                        passwordSQL = textBox5.Text;


                        Hide();

                        Form1 f1 = new Form1();

                        f1.Show();

                        File.AppendAllText(path_loggerSQL, DateTime.Now + "|" + WindowsIdentity.GetCurrent().Name + "|" + Environment.MachineName + "|" + Directory.GetCurrentDirectory() + "\n");

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

        private void SQLserver_CheckedChanged(object sender, EventArgs e)
        {
            if (SQLserver.Checked)
            {
                IsMySQL = false;

                textBox2.Enabled = false;

                try
                {
                    textBox1.Text = File.ReadAllText(path_ServerSQL);
                    textBox3.Text = File.ReadAllText(path_DatabaseSQL);

                    byte[] userIDByte = File.ReadAllBytes(path_UserIDSQL);
                    byte[] passwordByte = File.ReadAllBytes(path_PasswordSQL);
                    byte[] entropyByte = File.ReadAllBytes(path_entropySQL);

                    byte[] decipherUserID = ProtectedData.Unprotect(userIDByte, entropyByte, DataProtectionScope.CurrentUser);
                    byte[] decipherPassword = ProtectedData.Unprotect(passwordByte, entropyByte, DataProtectionScope.CurrentUser);

                    textBox4.Text = Encoding.UTF8.GetString(decipherUserID);
                    textBox5.Text = Encoding.UTF8.GetString(decipherPassword);
                }
                catch
                {
                    textBox1.Text = null;
                    textBox3.Text = null;
                    textBox4.Text = null;
                    textBox5.Text = null;
                }
            }
            else if (MySQL.Checked)
            {

                IsMySQL = true;

                textBox2.Enabled = true;

                try
                {
                    textBox1.Text = File.ReadAllText(path_Server);
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
                    textBox3.Text = null;
                    textBox4.Text = null;
                    textBox5.Text = null;
                }
            }
        }
    }
}
