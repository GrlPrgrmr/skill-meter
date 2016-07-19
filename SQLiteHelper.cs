using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SQLite;
using System.Data.Odbc;

using System.IO;

namespace SkillMeter.DataAccessLayer
{
    public enum ColType
    {
        Text,
        Integer

    }

    public static class SQLiteHelper
    {

        private static string _dbConnectionString { get { return "Data Source=" + System.IO.Directory.GetCurrentDirectory() + "\\Eval ;Pooling=True;Max Pool Size=100;Default Timeout=100;Connect Timeout=100"; } }


        private static SQLiteConnection conObj;
        public static SQLiteConnection ConObj
        {
            get
            {
                if (conObj == null)
                {
                    conObj = new SQLiteConnection(_dbConnectionString);
                    conObj.Open();
                }
                return conObj;
            }
        }



        #region Query



        public static DataTable PerformSelect(string sql)
        {
            var table = new DataTable();


            try
            {

                using (var cmd = ConObj.CreateCommand())
                {
                    cmd.CommandText = sql;
                    using (SQLiteDataReader dataReader = cmd.ExecuteReader())
                    {

                        table.Load(dataReader);
                        dataReader.Close();
                    }

                    cmd.Dispose();
                    //using (SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd))
                    //{
                    //    ad.Fill(table);

                    //}
                }

                return table;
            }
            catch (Exception e)
            {
                MessageBox.Show("SQLite Exception : {0}", e.Message);
            }
            finally
            {

                table.Dispose();
            }

            return null;
        }

        /// <summary>
        /// Executes a NonQuery against the database.
        /// </summary>
        /// <param name="sql">The SQL to execute.</param>
        /// <returns>A double containing the time elapsed since the method has been executed.</returns>
        public static double? ExecuteNonQuery(string sql)
        {

            try
            {
               
                    using (SQLiteTransaction tran = ConObj.BeginTransaction())
                    {
                        using (var cmd = new SQLiteCommand(ConObj) { Transaction = tran, CommandText = sql, CommandTimeout = 100 })
                        {

                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();

                            tran.Commit();
                            cmd.Dispose();
                            tran.Dispose();

                        }
                    }
               
            }


            catch (Exception e)
            {
                Console.WriteLine("SQLite Exception : {0}", e.Message);
            }

            return null;
        }

        /// <summary>
        /// Executes a NonQuery against the database.
        /// </summary>
        /// <param name="sql">The SQL to execute.</param>
        /// <returns>A double containing the time elapsed since the method has been executed.</returns>
        public static double? ExecuteNonQuery(string sql, SQLiteTransaction _tran )
        {

            try
            {
                if (_tran == null)
                {
                    using (SQLiteTransaction tran = ConObj.BeginTransaction())
                    {
                        using (var cmd = new SQLiteCommand(ConObj) { Transaction = tran, CommandText = sql, CommandTimeout = 100 })
                        {

                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();

                            tran.Commit();
                            cmd.Dispose();
                            tran.Dispose();

                        }
                    }
                }
                else
                {
                    using (var cmd = new SQLiteCommand(ConObj) { Transaction = _tran, CommandText = sql, CommandTimeout = 100 })
                    {

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        //tran.Commit();
                        cmd.Dispose();
                        //tran.Dispose();

                    }
                }
            }


            catch (Exception e)
            {
                Console.WriteLine("SQLite Exception : {0}", e.Message);
            }

            return null;
        }

        /// <summary>
        /// Gets a single value from the database.
        /// </summary>
        /// <param name="sql">The SQL to execute.</param>
        /// <returns>Returns the value retrieved from the database.</returns>
        public static string ExecuteScalar(string sql)
        {
            try
            {
               
                    using (SQLiteTransaction transaction = ConObj.BeginTransaction())
                    {
                        using (var cmd = new SQLiteCommand(ConObj) { Transaction = transaction, CommandText = sql })
                        {
                            object value = cmd.ExecuteScalar();
                            transaction.Commit();
                            return value != null ? value.ToString() : "";
                        }
                    }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("SQLite Exception : {0}", e.Message);
            }

            return null;
        }



        /// <summary>
        /// Updates specific rows in the database.
        /// </summary>
        /// <param name="tableName">The table to update.</param>
        /// <param name="data">A dictionary containing Column names and their new values.</param>
        /// <param name="where">The where clause for the update statement.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public static bool Update(String tableName, Dictionary<String, String> data, String where)
        {
            string vals = "";
            if (data.Count >= 1)
            {
                vals = data.Aggregate(vals,
                                      (current, val) =>
                                      current +
                                      String.Format(" {0} = '{1}',", val.Key.ToString(CultureInfo.InvariantCulture),
                                                    val.Value.ToString(CultureInfo.InvariantCulture)));
                vals = vals.Substring(0, vals.Length - 1);
            }
            try
            {
                ExecuteNonQuery(String.Format("update {0} set {1} where {2};", tableName, vals, where));
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("SQLite Exception : {0}", e.Message);
            }

            return false;
        }

        /// <summary>
        /// Deletes specific rows in the database.
        /// </summary>
        /// <param name="tableName">The table from which to delete.</param>
        /// <param name="where">The where clause for the delete.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public static bool Delete(String tableName, String where,SQLiteTransaction trans)
        {
            try
            {
                ExecuteNonQuery(String.Format("delete from {0} where {1};", tableName, where), trans);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("SQLite Exception : {0}", e.Message);
            }

            return false;
        }

        /// <summary>
        /// Inserts new data to the database.
        /// </summary>
        /// <param name="tableName">The table into which the data will be inserted.</param>
        /// <param name="data">A dictionary containing Column names and data to be inserted.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public static bool Insert(String tableName, Dictionary<String, object> data)
        {
            string columns = "";
            string values = "";
            foreach (var val in data)
            {
                columns += String.Format(" {0},", val.Key);
                values += String.Format(" '{0}',", val.Value);
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                ExecuteNonQuery(String.Format("insert into {0}({1}) values({2});", tableName, columns, values));
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("SQLite Exception : {0}", e.Message);
            }

            return false;
        }

        public static T getRandomElement<T>(this List<T> lst)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            var index = rand.Next(0,lst.Count);

            return lst[index];
        }

        #endregion

        /// <summary>
        /// import data from csv file and store it to sqlite Table
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="fileName"></param>
        /// <param name="tableName"></param>
        public static DataTable importCSV( string filePath)
        {
            string ConnString = "provider=Microsoft.Jet.OLEDB.4.0;Data Source='" +filePath + "';Extended Properties=Excel 8.0;";
           
             bool hasHeaders = false;
            string HDR = hasHeaders ? "Yes" : "No";
            DataTable schema = new DataTable();
            string sheetName="";
           
            string fileName = Path.GetFileName(filePath).Split('.').FirstOrDefault();
            string pathOnly = Path.GetDirectoryName(filePath);

            if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                    ConnString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=1;ImportMixedTypes=Text\"";
                else
                ConnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=1;ImportMixedTypes=Text\"";


            System.Data.OleDb.OleDbConnection MyConnection;
            System.Data.DataSet DtSet;
            System.Data.OleDb.OleDbDataAdapter MyCommand;
            
            

            try
            
            {

                using (MyConnection = new System.Data.OleDb.OleDbConnection(ConnString))
                {

                    MyConnection.Open();
                    schema = MyConnection.GetSchema("Tables");

                    if (schema.Rows.Count > 0)
                        sheetName = schema.Rows[0].Field<string>("TABLE_NAME").ToString();


                    MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from ["+sheetName+"]", MyConnection);
                     MyCommand.TableMappings.Add("Table", fileName);
                        DtSet = new System.Data.DataSet();
                        MyCommand.Fill(DtSet);


                    MyCommand.Dispose();

                    return DtSet.Tables[0];
                   
                
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                return null;
            }


        }

        public static void fillTable(DataSet ds)
        {

            try
            {

                var dt = ds.Tables["Questions"];

                foreach( DataRow dr in dt.Rows)
                {
                


                }
                
            }
            catch(Exception ex)
            {


            }
        
        }




        internal static bool Insert(string tableName, List<KeyValuePair<string, object>> data)
        {
            string columns = "";
            string values = "";
            foreach (var val in data)
            {
                columns += String.Format(" {0},", val.Key);
                values += String.Format(" '{0}',", val.Value);
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                ExecuteNonQuery(String.Format("insert into {0}({1}) values({2});", tableName, columns, values));
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("SQLite Exception : {0}", e.Message);
            }

            return false;
        }
    }
}
