using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LoadFileWebApp.Models
{
    public class DataAccessLayer
    {
        private static string connectionString = "Server=testinterview.colorful.hr; Database=Archiver;User Id=candidat;Password=NkvDPYVk8Q27EjdT;";
        public string LogFileArchived(FileLog fileLog)
        {
            var con = new SqlConnection(connectionString);
            try
            {
                SqlCommand cmd = 
                    new SqlCommand($"INSERT INTO[dbo].[FileLogs]  VALUES ('{fileLog.Name}', '{fileLog.StartTime}', '{fileLog.Durtation}', '{fileLog.Status}')", con);
               
                cmd.CommandType = CommandType.Text;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return "Data save Successfully";
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return ex.Message.ToString();
            }
        }
    }
}