using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

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
            database = "database";
            uid = "username";
            password = "password";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

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
            for(int i = 0; i <= column.Length; i++)
            {
                set_values = column[i]+"="+values[i]+", ";
            }
            
            string query = "UPDATE "+table+" SET "+set_values+" WHERE "+condition;
            
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
            string query = "DELETE FROM "+table+" WHERE "+condition;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement
        public List<string>[] Select(string table, string[] select_col)
        {
            string col = "";
            int i = 1;
            foreach(string c in select_col)
            {
                if(i == select_col.Length){
                    col += c+", ";
                } else {
                    col += c+" ";   
                }
            }
            
            string query = "SELECT "+col+" FROM "+table;
            
            //Create a list to store the result
            List<string>[] list = new List<string>[select_col.Length];
            for(i = 0; i <= select_col.Length; i++){
                list[i] = new List<string>();   
            }

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    for(i = 0; i <= select_col.Length; i++){
                        list[i].Add(dataReader[select_col[i]] + "");  
                    }
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Count statement
        public int Count(string table, string select_col, bool distinct)
        {
            if(distinct){
                string query = "SELECT Count(DISTINCT "select_col+") FROM "+table;
            } else {
                string query = "SELECT Count("+select_col+") FROM "+table;   
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
}
