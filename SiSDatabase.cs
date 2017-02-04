using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;

namespace StudentInformationSystem
{
    class SiSDatabase
    {
        /*
         * Class for the database connection and handling. 
         * Dynamic functions alow the same functions to be used for a 
         * variety of tables and SQL operations.
         */
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public SiSDatabase()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "StudentInformationSystem";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";Convert Zero Datetime=True";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Insert statement, takes table name, array of column titles, and an array of values.
        public void Insert(string table, string[] columns, string[] values)
        {
            string col = "";
            int i = 1;
            foreach (string c in columns)
            {
                if(i == columns.Length)
                {
                    col += " " + c + "";
                }
                else
                {
                    i++;
                    col += " " + c + ",";
                }
            }

            string val = "";
            i = 1;
            foreach (string v in values)
            {
                if (i == values.Length)
                {
                    val += "'" + v + "'";
                }
                else
                {
                    i++;
                    val += "'" + v + "',";
                }
            }

            string query = "INSERT INTO " +table+" ("+col+") VALUES("+val+")";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update(string table, string[] column, string[] values, string condition)
        {
            string set_values = "";
            for (int i = 0; i <= column.Length; i++)
            {
                set_values = column[i] + "=" + values[i] + ", ";
            }

            string query = "UPDATE " + table + " SET " + set_values + " WHERE " + condition;

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete(string table, string condition)
        {
            string query = "DELETE FROM " + table + " WHERE " + condition;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement
        public DataTable Select(string table, string[] columns)
        {
            DataTable dt = new DataTable();
            int number_of_columns = 0;
            string query = "";

            if (columns.Length == 1 && columns[0] == "*")
            {
                query = "SELECT * FROM " + table;
                number_of_columns = 1;
            } else
            {
                //Create a list to store the result
                query = "SELECT ";
                for (int i = 0; i < columns.Length; i++)
                {
                    query += columns[i] + ", ";
                    number_of_columns++;
                }
                query += " FROM " + table;
            }
            
            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    dt.Load(dataReader);
                    int row_num = dt.Rows.Count;
                    string test = dt.Rows[0][1].ToString();
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return dt ;
            }
            else
            {
                return dt;
            }
        }

        //Count statement
        public int Count(string table, string select_col, bool distinct)
        {
            string query = "";
            if (distinct)
            {
                query = "SELECT Count(DISTINCT "+select_col+ ") FROM " + table;
            }
            else
            {
                query = "SELECT Count(" + select_col + ") FROM " + table;
            }
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return Count;
            }
            else
            {
                return Count;
            }
        }


        //Backup
        public void Backup()
        {
        }

        //Restore
        public void Restore()
        {
        }

        //Status
        public bool Status()
        {
            bool i = OpenConnection();
            CloseConnection();
            return i;
        }
    }

    class Record
    {
        public Record()
        {

        }
    }
}
