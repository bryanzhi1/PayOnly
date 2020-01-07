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
    public class AdminDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;

        public AdminDAL()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "ServerSQLString");
            conn = new SqlConnection(strConn);
        }

        public List<CashOutRequest> GetRequests()
        {
            SqlCommand cmd = new SqlCommand
              ("SELECT * FROM CashOutRequest", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "RequestDetails");
            conn.Close();

            List<CashOutRequest> RequestList = new List<CashOutRequest>();

            foreach (DataRow row in result.Tables["RequestDetails"].Rows)
            {
                RequestList.Add(
                    new CashOutRequest
                    {
                        CashOutID = Convert.ToInt32(row["CashOutID"]),
                        MerchantID = Convert.ToInt32(row["MerchantID"]),
                        BankName = row["BankName"].ToString(),
                        AccountName = row["AccountName"].ToString(),
                        AccountNumber = row["AccountNumber"].ToString(),
                        Amount = Convert.ToDouble(row["Amount"]),
                        Status = row["Status"].ToString()
                    }
                 );
            }

            return RequestList;
        }
    }
}
