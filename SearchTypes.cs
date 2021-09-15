using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WindowsForms_NET_Framework4
{
    class SearchType
    {


        public static string AllRecords(string dbCondition, string select, string dbTable, string dbLimit, string sortBy, string sortByField)
        {
            string dbSQL;

            if (dbCondition != null)
            {
                dbSQL = select + "\nFrom " + dbTable + "\nwhere\n" + dbCondition + "\nORDER BY " + sortByField + " " + sortBy + dbLimit;
            }
            else
            {
                dbSQL = select + "\nFrom " + dbTable + "\nORDER BY " + sortByField + " " + sortBy + dbLimit;
            }


            return dbSQL;
        }

        public static string LatestRecord(string dbCondition, string select, string dbTable, string dbLimit, string sortBy, string sortByField)
        {

            string dbSQL;

            string s = GetField();


            if (dbCondition != null)
            {
                dbSQL = select + "\nFrom " + "\n(select * \nfrom " + "\n(select * \nfrom " + "\n(select * \nfrom " + dbTable + "\nwhere\n" + dbCondition + "\nORDER BY " + sortByField + " DESC) as U" + "\nGROUP BY " + s + ") as T \nORDER BY " + sortByField + " ASC) as V" + "\nORDER BY " + sortByField + " " + sortBy + dbLimit;
            }
            else
            {
                dbSQL = select + "\nFrom " + "\n(select * \nfrom " + "\n(select * \nfrom " + "\n(select * \nfrom " + dbTable + "\nORDER BY " + sortByField + " DESC) as U" + "\nGROUP BY " + s + ") as T \nORDER BY " + sortByField + " ASC) as V" + "\nORDER BY " + sortByField + " " + sortBy + dbLimit;
            }

            return dbSQL;
        }

        public static string FirstRecord(string dbCondition, string select, string dbTable, string dbLimit, string sortBy, string sortByField)
        {

            string dbSQL;

            string s = GetField();


            if (dbCondition != null)
            {
                dbSQL = select + "\nFrom " + "\n(select * \nfrom " + "\n(select * \nfrom " + dbTable + "\nwhere\n" + dbCondition + "\nGROUP BY " + s + ") as T \nORDER BY " + sortByField + " ASC) as U" + "\nORDER BY " + sortByField + " " + sortBy + dbLimit;
            }
            else
            {
                dbSQL = select + "\nFrom " + "\n(select * \nfrom " + "\n(select * \nfrom " + dbTable + "\nGROUP BY " + s + ") as T \nORDER BY " + sortByField + " ASC) as U" + "\nORDER BY " + sortByField + " " + sortBy + dbLimit;
            }

            return dbSQL;
        }

        public static string AllRecords(string dbCondition, string select, string dbTable, string sortBy, string sortByField)
        {
            string dbSQL;

            if (dbCondition != null)
            {
                dbSQL = select + "\nFrom " + dbTable + "\nwhere\n" + dbCondition + "\nORDER BY " + sortByField + " " + sortBy;
            }
            else
            {
                dbSQL = select + "\nFrom " + dbTable + "\nORDER BY " + sortByField + " " + sortBy;
            }


            return dbSQL;
        }

        public static string LatestRecord(string dbCondition, string select, string dbTable, string sortBy, string sortByField)
        {

            string dbSQL;

            string s = GetFieldSQL();


            if (dbCondition != null)
            {
                dbSQL = select + "\nFrom " + "\n(select * \nfrom " + "\n(select * \nfrom " + "\n(select * \nfrom " + dbTable + "\nwhere\n" + dbCondition + "\nORDER BY " + sortByField + " DESC) as U" + "\nGROUP BY " + s + ") as T \nORDER BY " + sortByField + " ASC) as V" + "\nORDER BY " + sortByField + " " + sortBy;
            }
            else
            {
                dbSQL = select + "\nFrom " + "\n(select * \nfrom " + "\n(select * \nfrom " + "\n(select * \nfrom " + dbTable + "\nORDER BY " + sortByField + " DESC) as U" + "\nGROUP BY " + s + ") as T \nORDER BY " + sortByField + " ASC) as V" + "\nORDER BY " + sortByField + " " + sortBy;
            }

            return dbSQL;
        }

        public static string FirstRecord(string dbCondition, string select, string dbTable, string sortBy, string sortByField)
        {

            string dbSQL;

            string s = GetFieldSQL();


            if (dbCondition != null)
            {
                dbSQL = select + "\nFrom " + "\n(select * \nfrom " + "\n(select * \nfrom " + dbTable + "\nwhere\n" + dbCondition + "\nGROUP BY " + s + ") as T \nORDER BY " + sortByField + " ASC) as U" + "\nORDER BY " + sortByField + " " + sortBy;
            }
            else
            {
                dbSQL = select + "\nFrom " + "\n(select * \nfrom " + "\n(select * \nfrom " + dbTable + "\nGROUP BY " + s + ") as T \nORDER BY " + sortByField + " ASC) as U" + "\nORDER BY " + sortByField + " " + sortBy;
            }

            return dbSQL;
        }

        private static string GetField()
        {
            string field = null;

            foreach (DataRow dataRow in Form1.tableFields.Rows)
            {
                if (dataRow["Field"].ToString() == "SN")
                {
                    field = dataRow["Field"].ToString();
                    break;
                }
                else if (dataRow["Field"].ToString() == "SA_SN")
                {
                    field = dataRow["Field"].ToString();
                    break;
                }
                else if (dataRow["Field"].ToString() == "FG_SN")
                {
                    field = dataRow["Field"].ToString();
                    break;
                }
                else
                {
                    field = dataRow.ItemArray.FirstOrDefault().ToString();
                    break;
                }
            }

            return field;
        }

        private static string GetFieldSQL()
        {
            string field = null;

            foreach (DataRow dataRow in Form1.tableFields.Rows)
            {
                if (dataRow["COLUMN_NAME"].ToString() == "SN")
                {
                    field = dataRow["COLUMN_NAME"].ToString();
                    break;
                }
                else if (dataRow["COLUMN_NAME"].ToString() == "SA_SN")
                {
                    field = dataRow["COLUMN_NAME"].ToString();
                    break;
                }
                else if (dataRow["COLUMN_NAME"].ToString() == "FG_SN")
                {
                    field = dataRow["COLUMN_NAME"].ToString();
                    break;
                }
                else
                {
                    field = dataRow.ItemArray.FirstOrDefault().ToString();
                    break;
                }
            }

            return field;
        }
    }
}
