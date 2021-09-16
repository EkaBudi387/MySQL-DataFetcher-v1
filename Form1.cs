using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.SqlClient;

namespace WindowsForms_NET_Framework4
{

    public partial class Form1 : Form
    {


        string dbTable;
        string dbLimit;
        string sortBy;
        string sortByField;
        string db;


        DateTime startSearch;
        DateTime endSearch;


        public static string dbSQL;
        public static string dbSQLEditor;
        public static string dbCondition;
        public static string field;
        public static bool useSQLEditor;
        public static DataTable tableFields;
        public static DataTable setConditionTable;


        bool checkedList = true;


        DataTable dbDroplist;
        DataTable tableDroplist;
        DataTable searchList;

        MySqlConnection connection;
        SqlConnection connectionSQL;

        readonly string showTables = "show tables from ";
        readonly string showDB = "show databases";

        readonly string showTablesSQL = "INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'";
        readonly string showDBSQL = "SELECT name FROM master.dbo.sysdatabases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb')";





        public Form1()
        {
            InitializeComponent();

            dataGridView1.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dataGridView1, true, null);

            if (Form2.IsMySQL)
            {
                this.Text = "MySql-DataRetriever v1p2";
                db = Form2.db;
                connection = Form2.connection;
            }
            else
            {
                this.Text = "MsSqlServer-DataRetriever v1p2";
                db = Form2.dbSQL;
                connectionSQL = Form2.connectionSQL;
            }
            
        }

        private void buttonExitWindow(object sender, FormClosedEventArgs e)
        {
            try
            {
                ClearCondition(tableFields);
            }
            finally
            {
                Application.Exit();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Form2.IsMySQL)
            {
                buttonASC.Checked = true;

                limitTB.Text = "100";

                label3.Text = "Count: ";

                timer1.Enabled = true;

                radioButton1.Checked = true;

                dbDroplist = TestToConnectMySQLServer.FillDataMySQL(showDB, connection);
                tableDroplist = TestToConnectMySQLServer.FillDataMySQL(showTables + db, connection);


                foreach (DataRow _ in dbDroplist.Rows)
                {
                    string db = _["Database"].ToString();

                    comboBox3.Items.Add(db);
                }

                comboBox3.SelectedItem = db;
            }
            else
            {
                buttonASC.Checked = true;

                limitTB.Text = "100";

                label3.Text = "Count: ";

                timer1.Enabled = true;

                radioButton1.Checked = true;

                dbDroplist = TestToConnectMySQLServer.FillDataSQL(showDBSQL, connectionSQL);
                tableDroplist = TestToConnectMySQLServer.FillDataSQL($"SELECT * FROM {db}.{showTablesSQL}", connectionSQL);


                foreach (DataRow _ in dbDroplist.Rows)
                {
                    string db = _["name"].ToString();

                    comboBox3.Items.Add(db);
                }

                comboBox3.SelectedItem = db;
            }
            

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Form2.IsMySQL)
            {
                setConditionTable = new DataTable();
                setConditionTable.Rows.Add();
                setConditionTable.Rows.Add();

                comboBox2.Items.Clear();

                radioButton1.Checked = true;

                dbTable = comboBox1.SelectedItem.ToString();

                string dbFields = "show fields from " + dbTable + " from " + db;

                tableFields = TestToConnectMySQLServer.FillDataMySQL(dbFields, connection);

                DataTable dataTable = tableFields.Copy();

                foreach (DataColumn _ in tableFields.Columns)
                {
                    if (_.ColumnName != "Field")
                        dataTable.Columns.Remove(_.ColumnName);
                }

                dataGridView2.DataSource = dataTable.DefaultView;

                checkedListBox1.Items.Clear();

                int i = 0;

                foreach (DataRow dataRow in tableFields.Rows)
                {

                    setConditionTable.Columns.Add(dataRow["Field"].ToString(), typeof(string));


                    checkedListBox1.Items.Add(dataRow["Field"]);
                    checkedListBox1.SetItemChecked(i++, true);

                    comboBox2.Items.Add(dataRow["Field"]);

                }



                foreach (DataRow dataRow in tableFields.Rows)
                {

                    comboBox2.SelectedItem = dataRow["Field"].ToString();

                    if (dataRow["Field"].ToString() == "Time")
                    {
                        comboBox2.SelectedItem = dataRow["Field"].ToString();
                        break;
                    }
                    else if (dataRow["Field"].ToString() == "ID")
                    {
                        comboBox2.SelectedItem = dataRow["Field"].ToString();
                        break;
                    }
                    else if (dataRow["Field"].ToString() == "Id")
                    {
                        comboBox2.SelectedItem = dataRow["Field"].ToString();
                        break;
                    }
                    else
                    {
                    }
                }
            }
            else
            {
                setConditionTable = new DataTable();
                setConditionTable.Rows.Add();
                setConditionTable.Rows.Add();

                comboBox2.Items.Clear();

                radioButton1.Checked = true;

                dbTable = comboBox1.SelectedItem.ToString();

                string dbFields = $"SELECT * FROM {db}.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'{dbTable}'";

                tableFields = TestToConnectMySQLServer.FillDataSQL(dbFields, connectionSQL);

                DataTable dataTable = tableFields.Copy();

                foreach (DataColumn _ in tableFields.Columns)
                {
                    if (_.ColumnName != "COLUMN_NAME")
                        dataTable.Columns.Remove(_.ColumnName);
                }

                dataGridView2.DataSource = dataTable.DefaultView;

                checkedListBox1.Items.Clear();

                int i = 0;

                foreach (DataRow dataRow in tableFields.Rows)
                {

                    setConditionTable.Columns.Add(dataRow["COLUMN_NAME"].ToString(), typeof(string));


                    checkedListBox1.Items.Add(dataRow["COLUMN_NAME"]);
                    checkedListBox1.SetItemChecked(i++, true);

                    comboBox2.Items.Add(dataRow["COLUMN_NAME"]);

                }



                foreach (DataRow dataRow in tableFields.Rows)
                {

                    comboBox2.SelectedItem = dataRow["COLUMN_NAME"].ToString();

                    if (dataRow["COLUMN_NAME"].ToString() == "Time")
                    {
                        comboBox2.SelectedItem = dataRow["COLUMN_NAME"].ToString();
                        break;
                    }
                    else if (dataRow["COLUMN_NAME"].ToString() == "ID")
                    {
                        comboBox2.SelectedItem = dataRow["COLUMN_NAME"].ToString();
                        break;
                    }
                    else if (dataRow["COLUMN_NAME"].ToString() == "Id")
                    {
                        comboBox2.SelectedItem = dataRow["COLUMN_NAME"].ToString();
                        break;
                    }
                    else
                    {
                    }
                }
            }
            

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            label7.Text = "Loading Data... Please Wait...";
            startSearch = DateTime.Now;

            try
            {

                await Loading();
                label3.Text = "Count: " + searchList.Rows.Count.ToString();

                if (checkBox2.Checked == false)
                    dataGridView1.DataSource = searchList.DefaultView;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Wrong Input");
            }

            label7.Text = "Data Table";
            endSearch = DateTime.Now;
            label2.Text = "Duration: " + (endSearch - startSearch).ToString();
        }

        private void GetTable()
        {

            if (Form2.IsMySQL)
            {
                CreateSQL();

                connection = TestToConnectMySQLServer.OpenConnectionMySQL(Form2.server, Form2.port, db, Form2.userID, Form2.password);

                if (checkBox1.Checked)
                    searchList = TestToConnectMySQLServer.FillDataMySQL(dbSQLEditor, connection);
                else
                    searchList = TestToConnectMySQLServer.FillDataMySQL(dbSQL, connection);
            }
            else
            {
                CreateSQL();

                connectionSQL = TestToConnectMySQLServer.OpenConnectionSQL(Form2.serverSQL, db, Form2.userIDSQL, Form2.passwordSQL);

                if (checkBox1.Checked)
                    searchList = TestToConnectMySQLServer.FillDataSQL(dbSQLEditor, connectionSQL);
                else
                    searchList = TestToConnectMySQLServer.FillDataSQL(dbSQL, connectionSQL);
            }

        }

        protected async Task<DataTable> Loading()
        {
            await Task.Run(() => { GetTable(); });

            return searchList;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            label7.Text = "Saving to Excel...";

            await SavingExcel();

            label7.Text = "Data Table";

        }

        private void SaveExcel()
        {
            try
            {
                My_DataTable_Extensions.ExportToExcel(searchList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async Task SavingExcel()
        {
            await Task.Run(() => { SaveExcel(); });
        }

        private void ClearCondition(DataTable tableFields)
        {
            if (Form2.IsMySQL)
            {
                foreach (DataRow dataRow in tableFields.Rows)
                {

                    if (dataRow["Field"].ToString() != "Time")
                    {

                        setConditionTable.Rows[0][dataRow["Field"].ToString()] = null;

                    }
                    else
                    {

                        setConditionTable.Rows[0][dataRow["Field"].ToString()] = null;
                        setConditionTable.Rows[1][dataRow["Field"].ToString()] = null;

                    }
                }
            }
            else
            {
                foreach (DataRow dataRow in tableFields.Rows)
                {

                    if (dataRow["COLUMN_NAME"].ToString() != "Time")
                    {

                        setConditionTable.Rows[0][dataRow["COLUMN_NAME"].ToString()] = null;

                    }
                    else
                    {

                        setConditionTable.Rows[0][dataRow["COLUMN_NAME"].ToString()] = null;
                        setConditionTable.Rows[1][dataRow["COLUMN_NAME"].ToString()] = null;

                    }
                }
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Form2.IsMySQL)
            {
                if (checkBox1.Checked)
                {
                    useSQLEditor = true;
                    connection = TestToConnectMySQLServer.OpenConnectionMySQL(Form2.server, Form2.port, db, Form2.userID, Form2.password);
                }

                else
                    useSQLEditor = false;


                CreateSQL();

                Form5 form5 = new Form5();

                form5.Show();
            }
            else
            {
                if (checkBox1.Checked)
                {
                    useSQLEditor = true;
                    connectionSQL = TestToConnectMySQLServer.OpenConnectionSQL(Form2.serverSQL, db, Form2.userIDSQL, Form2.passwordSQL);
                }

                else
                    useSQLEditor = false;


                CreateSQL();

                Form5 form5 = new Form5();

                form5.Show();
            }
            
        }

        private void CreateSQL()
        {
            if (Form2.IsMySQL)
            {
                if (limitTB.Text != "")
                {
                    dbLimit = "\nLimit " + limitTB.Text;
                }
                else
                {
                    dbLimit = "";
                }

                string select = "Select ";
                int i = 1;

                foreach (string s in checkedListBox1.CheckedItems)
                {

                    if (checkedListBox1.CheckedItems.Count - i++ > 0)
                        select += s + ", ";
                    else
                        select += s + " ";
                }

                dbCondition = null;
                string thisAnd = null;

                foreach (DataRow dataRow in tableFields.Rows)
                {
                    if (dataRow["Field"].ToString() != "Time")
                    {
                        string field = null;

                        string s = setConditionTable.Rows[0][dataRow["Field"].ToString()].ToString();

                        if (s != "")
                        {
                            field = dataRow["Field"].ToString();
                            dbCondition += thisAnd;
                            dbCondition += "(";

                            string[] vs = s.Split('\n');

                            string thisOR = null;

                            foreach (var _ in vs)
                            {

                                if (_ != "")
                                {
                                    dbCondition += thisOR;
                                    dbCondition += field + " like " + "\"" + _ + "\"";
                                    thisOR = " OR\n";
                                }

                            }

                            dbCondition += ")";
                            thisAnd = "\nAND\n";
                        }
                    }

                    else
                    {
                        string field = null;

                        string startTime = setConditionTable.Rows[0][dataRow["Field"].ToString()].ToString();
                        string endTime = setConditionTable.Rows[1][dataRow["Field"].ToString()].ToString();

                        if (startTime != "" && endTime != "")
                        {
                            field = dataRow["Field"].ToString();
                            dbCondition += thisAnd;
                            dbCondition += "(";

                            dbCondition += field + " >= " + "\"" + startTime + "\"" + " AND " + field + " <= " + "\"" + endTime + "\"";

                            dbCondition += ")";
                            thisAnd = "'\n'AND'\n'";
                        }
                    }
                }



                if (buttonASC.Checked)
                    sortBy = "ASC";
                if (buttonDESC.Checked)
                    sortBy = "DESC";





                if (radioButton1.Checked)
                {

                    dbSQL = SearchType.AllRecords(dbCondition, select, dbTable, dbLimit, sortBy, sortByField);

                }

                else if (radioButton2.Checked && sortByField == "Time")
                {

                    dbSQL = SearchType.LatestRecord(dbCondition, select, dbTable, dbLimit, sortBy, sortByField);

                }

                else if (radioButton3.Checked && sortByField == "Time")
                {

                    dbSQL = SearchType.FirstRecord(dbCondition, select, dbTable, dbLimit, sortBy, sortByField);

                }

                else
                {

                    dbSQL = SearchType.AllRecords(dbCondition, select, dbTable, dbLimit, sortBy, sortByField);

                }
            }
            else
            {

                string select = $"Select TOP ({limitTB.Text}) ";
                int i = 1;

                foreach (string s in checkedListBox1.CheckedItems)
                {

                    if (checkedListBox1.CheckedItems.Count - i++ > 0)
                        select += s + ", ";
                    else
                        select += s + " ";
                }

                dbCondition = null;
                string thisAnd = null;

                foreach (DataRow dataRow in tableFields.Rows)
                {
                    if (dataRow["COLUMN_NAME"].ToString().Contains("Time") || dataRow["COLUMN_NAME"].ToString().Contains("time") || dataRow["COLUMN_NAME"].ToString().Contains("TIME"))
                    {
                        string field = null;

                        string startTime = setConditionTable.Rows[0][dataRow["COLUMN_NAME"].ToString()].ToString();
                        string endTime = setConditionTable.Rows[1][dataRow["COLUMN_NAME"].ToString()].ToString();

                        if (startTime != "" && endTime != "")
                        {
                            field = dataRow["COLUMN_NAME"].ToString();
                            dbCondition += thisAnd;
                            dbCondition += "(";

                            dbCondition += field + " >= " + "'" + startTime + "'" + " AND " + field + " <= " + "'" + endTime + "'";

                            dbCondition += ")";
                            thisAnd = "'\n'AND'\n'";
                        }
                    }

                    else
                    {

                        string field = null;

                        string s = setConditionTable.Rows[0][dataRow["COLUMN_NAME"].ToString()].ToString();

                        if (s != "")
                        {
                            field = dataRow["COLUMN_NAME"].ToString();
                            dbCondition += thisAnd;
                            dbCondition += "(";

                            string[] vs = s.Split('\n');

                            string thisOR = null;

                            foreach (var _ in vs)
                            {

                                if (_ != "")
                                {
                                    dbCondition += thisOR;
                                    dbCondition += field + " like " + "'" + _ + "'";
                                    thisOR = " OR\n";
                                }

                            }

                            dbCondition += ")";
                            thisAnd = "\nAND\n";
                        }

                    }
                }



                if (buttonASC.Checked)
                    sortBy = "ASC";
                if (buttonDESC.Checked)
                    sortBy = "DESC";





                if (radioButton1.Checked)
                {

                    dbSQL = SearchType.AllRecords(dbCondition, select, dbTable, sortBy, sortByField);

                }

                else if (radioButton2.Checked && sortByField == "Time")
                {

                    dbSQL = SearchType.LatestRecord(dbCondition, select, dbTable, sortBy, sortByField);

                }

                else if (radioButton3.Checked && sortByField == "Time")
                {

                    dbSQL = SearchType.FirstRecord(dbCondition, select, dbTable, sortBy, sortByField);

                }

                else
                {

                    dbSQL = SearchType.AllRecords(dbCondition, select, dbTable, sortBy, sortByField);

                }
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label8.Text = "Time: " + DateTime.Now.ToLongTimeString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            sortByField = comboBox2.SelectedItem.ToString();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox2.Checked == false)
                    dataGridView1.DataSource = searchList.DefaultView;
            }
            catch
            {
                MessageBox.Show("Please click Search button");
            }

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Form2.IsMySQL)
            {
                db = comboBox3.SelectedItem.ToString();

                comboBox1.Items.Clear();
                comboBox2.Items.Clear();

                radioButton1.Checked = true;

                checkedListBox1.Items.Clear();
                dataGridView2.DataSource = null;

                tableDroplist = TestToConnectMySQLServer.FillDataMySQL(showTables + db, connection);

                foreach (DataRow _ in tableDroplist.Rows)
                {
                    string table = _["Tables_in_" + db].ToString();

                    comboBox1.Items.Add(table);
                }
            }
            else
            {
                db = comboBox3.SelectedItem.ToString();

                comboBox1.Items.Clear();
                comboBox2.Items.Clear();

                radioButton1.Checked = true;

                checkedListBox1.Items.Clear();
                dataGridView2.DataSource = null;

                tableDroplist = TestToConnectMySQLServer.FillDataSQL($"SELECT * FROM {db}.{showTablesSQL}", connectionSQL);

                foreach (DataRow _ in tableDroplist.Rows)
                {
                    string table = _["TABLE_NAME"].ToString();

                    comboBox1.Items.Add(table);
                }
            }

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            int i = 0;

            checkedList = !checkedList;

            foreach (DataRow dataRow in tableFields.Rows)
            {
                checkedListBox1.SetItemChecked(i++, checkedList);
            }
        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            field = dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString();

            if (field.Contains("Time") || field.Contains("TIME") || field.Contains("time"))
            {
                Form4 form4 = new Form4();
                form4.Text = field + " query";
                form4.Show();
            }
            else
            {
                Form3 form3 = new Form3();
                form3.Text = field + " query";
                form3.Show();
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            ClearCondition(tableFields);

            MessageBox.Show("Conditions Cleared!", "Information");
        }
    }
}
