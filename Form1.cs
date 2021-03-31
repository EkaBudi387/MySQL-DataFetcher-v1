using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WindowsForms_NET_Framework4
{
    public partial class Form1 : Form
    {

        string dbTable;
        string dbLimit;
        string sortBy;
        string sortByField;


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



        DataTable tableDroplist;
        DataTable searchList;



        readonly MySqlConnection connection = Form2.connection;

        readonly string showTables = "show tables";




        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

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

            buttonASC.Checked = true;

            limitTB.Text = "100";

            label3.Text = "Count: ";

            timer1.Enabled = true;

            radioButton1.Checked = true;

            tableDroplist = TestToConnectMySQLServer.FillData(showTables, connection);


            foreach (DataRow _ in tableDroplist.Rows)
            {
                string table = _["Tables_in_fg_m"].ToString();

                comboBox1.Items.Add(table);
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            setConditionTable = new DataTable();
            setConditionTable.Rows.Add();
            setConditionTable.Rows.Add();

            comboBox2.Items.Clear();

            radioButton1.Checked = true;

            dbTable = comboBox1.SelectedItem.ToString();

            string dbFields = "show fields from " + dbTable + " from fg_m";

            tableFields = TestToConnectMySQLServer.FillData(dbFields, connection);

            dataGridView2.DataSource = tableFields.DefaultView;

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
            CreateSQL();

            if (checkBox1.Checked)
                searchList = TestToConnectMySQLServer.FillData(dbSQLEditor, connection);
            else
                searchList = TestToConnectMySQLServer.FillData(dbSQL, connection);

        }


        protected async Task<DataTable> Loading()
        {
            await Task.Run(() => { GetTable(); });

            return searchList;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream s = File.Open(saveFileDialog.FileName, FileMode.Create);

                searchList.ToCSV(s);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            int i = 0;

            checkedList = !checkedList;

            foreach (DataRow dataRow in tableFields.Rows)
            {
                checkedListBox1.SetItemChecked(i++, checkedList);
            }

        }


        private void SetCondition(object sender, DataGridViewCellMouseEventArgs e)
        {

            field = dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString();

            if (field != "Time")
            {
                Form3 form3 = new Form3();
                form3.Text = field + " query";
                form3.Show();
            }
            else
            {
                Form4 form4 = new Form4();
                form4.Text = field + " query";
                form4.Show();
            }

        }


        private void button4_Click(object sender, EventArgs e)
        {
            ClearCondition(tableFields);

            MessageBox.Show("Conditions Cleared!", "Information");
        }


        private void ClearCondition(DataTable tableFields)
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


        private void button5_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                useSQLEditor = true;
            else
                useSQLEditor = false;


            CreateSQL();

            Form5 form5 = new Form5();

            form5.Show();
        }

        private void CreateSQL()
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
    }
}
