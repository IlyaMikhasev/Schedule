using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Schedule
{
    internal class DBConnect
    {
        string my_query;
        SQLiteCommand myQuery;
        SQLiteConnection myConnection;
        string DBName;
        public DBConnect(string _DBName)
        {
            my_query = "CREATE TABLE IF NOT EXISTS Employers (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                                  "Age INTEGER(15) NOT NULL ," +
                                  "Name TEXT(1, 15) UNIQUE NOT NULL," +
                                  "sex  TEXT(10)  NOT NULL," +
                                  "position TEXT(10),  "+
                                  "schedule TEXT NOT NULL);";
            DBName = _DBName;
            sqlConnect(my_query);
            Console.WriteLine("База данных Schedule созданна");
           
        }
        private void sqlConnect(string _my_query)
        {
            string _source = "Data Source=" + DBName + ".db;";
            string _cache = "Cache=Shared;";
            string _mode = "Mode=ReadWriteCreate;";
            myConnection = new SQLiteConnection(_source + _cache + _mode);
            myConnection.Open();
            myQuery = new SQLiteCommand(_my_query, myConnection);
          
        }
        public void AddEmployer(Employer _name) {
            string sex = string.Empty;
            if (_name.Sex)
                sex = "man";
            else sex = "woman";
            my_query = "INSERT INTO Employers(Age, Name, sex, position,schedule) " +
                "VALUES('"+_name.Age+"','" + _name.Name + "','" + sex + "','"+_name.Position+"','"+_name.Work()+"');";
            sqlConnect(my_query); 
            myQuery.ExecuteNonQuery();
        }
        public void SelectSchedulOne(string _name) {
            my_query = "SELECT schedule,Name FROM Employers where Name='" + _name + "';";
            sqlConnect(my_query);
            var dr = myQuery.ExecuteReader();
            string result = string.Empty;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    var position = dr.GetString(1);
                    var name = dr.GetString(2);

                    result += position.ToString() + " \t " + name.ToString() + "\n";
                }
            }
            Console.WriteLine(result);
        }
        public void SelectSchedul() {
            my_query = "SELECT schedule,Name FROM Employers ;";            
            sqlConnect(my_query);
            try
            {
                var dr = myQuery.ExecuteReader();
                string result = string.Empty;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var position = dr.GetString(1);
                        var name = dr.GetString(2);

                        result += position.ToString() + " \t " + name.ToString() + "\n";
                    }
                }
                Console.WriteLine(result);
            }
            catch (Exception e){
                Console.WriteLine($"Error:{e.Message}");
            }
        }
    }
}
