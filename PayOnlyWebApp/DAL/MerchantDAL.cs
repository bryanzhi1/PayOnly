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
    public class MerchantDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;

        public MerchantDAL()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "ServerSQLString");
            conn = new SqlConnection(strConn);
        }

        public string GetName(int MerchantID)
        {
            SqlCommand cmd = new SqlCommand
              ("SELECT MerchantName FROM Merchants WHERE MerchantID = @merchantid", conn);
            cmd.Parameters.AddWithValue("@merchantid", MerchantID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "AccountDetails");
            conn.Close();

            string name = "";

            foreach (DataRow row in result.Tables["AccountDetails"].Rows)
            {
                name = row["MerchantName"].ToString();
            }
            return name;
        }

        public double GetBalance(int MerchantID)
        {
            SqlCommand cmd = new SqlCommand
              ("SELECT Balance FROM Merchants WHERE MerchantID = @merchantid", conn);
            cmd.Parameters.AddWithValue("@merchantid", MerchantID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "AccountDetails");
            conn.Close();

            double balance = 0;

            foreach (DataRow row in result.Tables["AccountDetails"].Rows)
            {
                balance = Convert.ToDouble(row["Balance"]);
            }
            return balance;
        }

        public Merchant GetDetails(int MerchantID)
        {
            SqlCommand cmd = new SqlCommand
              ("SELECT * FROM Merchants WHERE MerchantID = @merchantid", conn);
            cmd.Parameters.AddWithValue("@merchantid", MerchantID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "AccountDetails");
            conn.Close();

            Merchant merchantDetails = new Merchant();

            foreach (DataRow row in result.Tables["AccountDetails"].Rows)
            {
                    merchantDetails.MerchantId = Convert.ToInt32(row["MerchantID"]);
                    merchantDetails.MerchantName = row["MerchantName"].ToString();
                    merchantDetails.InChargeName = row["InChargeName"].ToString();
                    merchantDetails.PhoneNumber = row["PhoneNumber"].ToString();
                    merchantDetails.Address = row["Address"].ToString();
            }

            return merchantDetails;
        }

        public bool UpdateDetails(int MerchantID, string merchantName, string inChargeName, string phoneNumber, string address)
        {
            SqlCommand cmd = new SqlCommand
              ("UPDATE Merchants SET MerchantName = @merchantname, InChargeName = @inchargename, PhoneNumber = @phonenumber, Address = @address WHERE MerchantID = @merchantid", conn);
            cmd.Parameters.AddWithValue("@merchantid", MerchantID);
            cmd.Parameters.AddWithValue("@merchantname", merchantName);
            cmd.Parameters.AddWithValue("@inchargename", inChargeName);
            cmd.Parameters.AddWithValue("@phonenumber", phoneNumber);
            cmd.Parameters.AddWithValue("@address", address);
            conn.Open();
            cmd.ExecuteNonQuery(); 
            conn.Close();

            return true;
        }

        public bool changePassword(string MerchantID, string newpwd)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Merchants SET Password = @pwd WHERE MerchantID = @merchantid", conn);

            cmd.Parameters.AddWithValue("@pwd", newpwd);
            cmd.Parameters.AddWithValue("@merchantid", MerchantID);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return true;
        }

        public bool passwordChecker(string MerchantID, string oldpassword)
        {
            SqlCommand cmd = new SqlCommand
                ("SELECT MerchantID FROM Merchants WHERE MerchantID = @merchantid AND Password = @selectedPwd", conn);
            cmd.Parameters.AddWithValue("@merchantID", MerchantID);
            cmd.Parameters.AddWithValue("@selectedPwd", oldpassword);
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

        public List<Transaction> GetTransactions(int MerchantID)
        {
            SqlCommand cmd = new SqlCommand
             ("SELECT * FROM Transactions WHERE MerchantID = @merchantid", conn);
            cmd.Parameters.AddWithValue("@merchantid", MerchantID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "AccountDetails");
            conn.Close();

            List<Transaction> TransactionList = new List<Transaction>();

            foreach (DataRow row in result.Tables["AccountDetails"].Rows)
            {
                TransactionList.Add(
                    new Transaction
                    {
                        TransactionsID = Convert.ToInt32(row["TransactionsID"]),
                        TransactionsDateTime = Convert.ToDateTime(row["TransactionDateTime"]),
                        TransactionAmount = Convert.ToDouble(row["TransactionAmount"]),
                        UserID = Convert.ToInt32(row["UserID"])
                    }
                    );
            }

            return TransactionList;
        }

        public bool CashRequest(int merchantID, string bankName, string accountName, string accountNumber, double amount)
        {
            SqlCommand cmd = new SqlCommand
              ("INSERT INTO CashOutRequest (MerchantID, BankName, AccountName, AccountNumber, Amount, Status) OUTPUT INSERTED.CashOutID VALUES(@merchantid, @bankname, @accname, @accnumber,@amt,@status)", conn);
            cmd.Parameters.AddWithValue("@merchantid", merchantID);
            cmd.Parameters.AddWithValue("@bankname", bankName);
            cmd.Parameters.AddWithValue("@accname", accountName);
            cmd.Parameters.AddWithValue("@accnumber", accountNumber);
            cmd.Parameters.AddWithValue("@amt", amount);
            cmd.Parameters.AddWithValue("@status", "Pending");
            conn.Open();
            int CashOutID = (int)(cmd.ExecuteScalar());
            conn.Close();

            return true;
        }

        public List<CashOutRequest> GetCashRequests(int merchantID)
        {
            SqlCommand cmd = new SqlCommand
             ("SELECT * FROM CashOutRequest WHERE MerchantID = @merchantid", conn);
            cmd.Parameters.AddWithValue("@merchantid", merchantID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "AccountDetails");
            conn.Close();

            List <CashOutRequest> requestList = new List<CashOutRequest>();

            foreach (DataRow row in result.Tables["AccountDetails"].Rows)
            {
                requestList.Add(
                    new CashOutRequest
                    {
                        CashOutID = Convert.ToInt32(row["CashOutID"]),
                        MerchantID = Convert.ToInt32(row["MerchantID"]),
                        BankName = row["BankName"].ToString(),
                        AccountName = row["AccountName"].ToString(),
                        AccountNumber = row["AccountNumber"].ToString(),
                        Amount = Convert.ToDouble("Amount"),
                        Status = row["Status"].ToString()
                    }
                    );
            }

            return requestList;
        }
    }
}
