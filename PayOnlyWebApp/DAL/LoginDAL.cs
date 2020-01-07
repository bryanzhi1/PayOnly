using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using PayOnlyWebApp.Models;
using System.Collections.Generic;
using System;

namespace PayOnlyWebApp.DAL
{
    public class LoginDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;

        public LoginDAL()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "ServerSQLString");
            conn = new SqlConnection(strConn);
        }

        public int merchantChecker(string username, string pwd)
        {
            SqlCommand cmd = new SqlCommand
               ("SELECT MerchantID FROM Merchants WHERE Username=@selectedUsername AND Password = @selectedPwd", conn);
            cmd.Parameters.AddWithValue("@selectedUsername", username);
            cmd.Parameters.AddWithValue("@selectedPwd", pwd);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "AccountDetails");
            conn.Close();

            int MerchantID = 0;

            if (result.Tables["AccountDetails"].Rows.Count == 1)
            {
                foreach (DataRow row in result.Tables["AccountDetails"].Rows)
                {
                    MerchantID = Convert.ToInt32(row["MerchantID"]);
                }
                return MerchantID;
            }

            else
            {
                return 0;
            }
        }

        public bool adminChecker(string username, string pwd)
        {
            SqlCommand cmd = new SqlCommand
               ("SELECT Username FROM AdminLogin WHERE Username=@selectedUsername AND Password = @selectedPwd", conn);
            cmd.Parameters.AddWithValue("@selectedUsername", username);
            cmd.Parameters.AddWithValue("@selectedPwd", pwd);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "AccountDetails");
            conn.Close();

            if (result.Tables["AccountDetails"].Rows.Count == 1)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
