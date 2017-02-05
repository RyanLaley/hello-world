#Database Class

Accesses and manages data of a database.

Required Namespaces:
<li>System.Data
<li>Mysql.Data.MySqlClient

##How to use
Set database connection variables inside the class (accessible parameters from external to the class coming in an update later) before using then simply instantiate a new Database class to begin using.

##Constructors
Database() - Initialises a new instance of the Database class with no argunments

##Methods
**Initialise()** - Called in the constructor. Sets up the the configuration of the database connection.

**OpenConnection()** - Opens the connection to the database.

**CloseDatabase()** - Closes the connection to the datbase.

**Insert(string table, string[] columns, string[] values)** - Inserts a new record into the database. Requires a table name, the list of column headings, and their corresponding values.

**Update(string table, string[] column, string[] values, string condition)** - Alters a record. Requires a table name, the list of column names to change, the values to alter, and the condition of which records to change.

**Delete(string table, string condition)** - Deletes a record. Requires a table name, and the condition of which record to remove.

**Select(string table, string[] columns)** - Returns a Datatable containing records of the database that meet the condition.

**Count(string table, string select_col, bool distinct)** - Counts the number of records in the table. Allows the suer to define it to count all records or just distinct ones.
