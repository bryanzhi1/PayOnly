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
    public class ApiDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;

        public ApiDAL()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "ServerSQLString");
            conn = new SqlConnection(strConn);
        }

        public Merchant GetMerchant(int merchantID)
        {
            SqlCommand cmd = new SqlCommand
             ("SELECT * FROM Merchants WHERE MerchantID = @merchantid", conn);
            cmd.Parameters.AddWithValue("@merchantid", merchantID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "AccountDetails");
            conn.Close();

            Merchant merchant = new Merchant();

            foreach (DataRow row in result.Tables["AccountDetails"].Rows)
            {
                merchant.MerchantId = merchantID;
                merchant.MerchantName = row["MerchantName"].ToString();
                merchant.InChargeName = row["InChargeName"].ToString();
                merchant.PhoneNumber = row["PhoneNumber"].ToString();
                merchant.Address = row["Address"].ToString();
                merchant.Balance = Convert.ToDouble(row["Balance"]);
            }

            return merchant;
        }

        public User GetUserById(int UserId)
        {
            SqlCommand cmd = new SqlCommand
             ("SELECT * FROM Users WHERE UserID = @userid", conn);
            cmd.Parameters.AddWithValue("@userid", UserId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "AccountDetails");
            conn.Close();

            User user = new User();

            foreach (DataRow row in result.Tables["AccountDetails"].Rows)
            {
                user.UserID = Convert.ToInt32(row["UserID"]);
                user.FirstName = row["FirstName"].ToString();
                user.LastName = row["LastName"].ToString();
                user.PhoneNumber = row["PhoneNumber"].ToString();
                user.Username = row["Username"].ToString();
                user.Password = row["Password"].ToString();
                user.Balance = Convert.ToDouble(row["Balance"]);
            }

            return user;
        }

        public User GetUserByEmail(string email)
        {
            SqlCommand cmd = new SqlCommand
             ("SELECT * FROM Users WHERE Username = @email", conn);
            cmd.Parameters.AddWithValue("@email", email);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "AccountDetails");
            conn.Close();

            User user = new User();

            foreach (DataRow row in result.Tables["AccountDetails"].Rows)
            {
                user.UserID = Convert.ToInt32(row["UserID"]);
                user.FirstName = row["FirstName"].ToString();
                user.LastName = row["LastName"].ToString();
                user.PhoneNumber = row["PhoneNumber"].ToString();
                user.Username = row["Username"].ToString();
                user.Password = row["Password"].ToString();
                user.Balance = Convert.ToDouble(row["Balance"]);
            }

            return user;
        }

        public bool PostTxn(TransactionPost txn)
        {
            SqlCommand cmd = new SqlCommand
              ("INSERT INTO Transactions (TransactionDateTime, TransactionAmount, UserID, MerchantID) OUTPUT INSERTED.TransactionsID VALUES(GETDATE(), @txnamt, @userid, @merchantid)", conn);
            cmd.Parameters.AddWithValue("@txnamt", txn.TransactionAmount);
            cmd.Parameters.AddWithValue("@userid", txn.UserID);
            cmd.Parameters.AddWithValue("@merchantid", txn.MerchantID);
            conn.Open();
            int TransactionsID = (int)(cmd.ExecuteScalar());
            conn.Close();

            return true;
        }
        
        public bool DeductUser(int UserID, double amount)
        {
            SqlCommand cmd = new SqlCommand
             ("SELECT Balance FROM Users WHERE UserID = @userid", conn);
            cmd.Parameters.AddWithValue("@userid", UserID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "AccountDetails");
            conn.Close();

            double userBalance = 0.00;

            foreach (DataRow row in result.Tables["AccountDetails"].Rows)
            {
                userBalance = Convert.ToDouble(row["Balance"]);
            }

            double remainingBalance = userBalance - amount;

            if(remainingBalance > 0){
                SqlCommand cmd1 = new SqlCommand
              ("UPDATE Users SET Balance = @remaining WHERE UserID = @userid", conn);
                cmd1.Parameters.AddWithValue("@remaining", remainingBalance);
                cmd1.Parameters.AddWithValue("@userid", UserID);
                conn.Open();
                cmd1.ExecuteNonQuery();
                conn.Close();

                return true;
            }

            else
            {
                return false;
            }
        }

        public bool CreditMerchant(int MerchantID, double amount)
        {
            SqlCommand cmd = new SqlCommand
             ("SELECT Balance FROM Merchants WHERE MerchantID = @merchantid", conn);
            cmd.Parameters.AddWithValue("@merchantid", MerchantID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "AccountDetails");
            conn.Close();

            double merchantBalance = 0.00;

            foreach (DataRow row in result.Tables["AccountDetails"].Rows)
            {
                merchantBalance = Convert.ToDouble(row["Balance"]);
            }

            double newMerchantBalance = merchantBalance + amount;

            SqlCommand cmd3 = new SqlCommand
              ("UPDATE Merchants SET Balance = @newbalance WHERE MerchantID = @merchantid", conn);
            cmd3.Parameters.AddWithValue("@newbalance", newMerchantBalance);
            cmd3.Parameters.AddWithValue("@merchantid", MerchantID);
            conn.Open();
            cmd3.ExecuteNonQuery();
            conn.Close();

            return true;
        }

        public bool TopUpUser(TopUpModel topup)
        {
            SqlCommand cmd = new SqlCommand
            ("SELECT Balance FROM Users WHERE UserID = @userid", conn);
            cmd.Parameters.AddWithValue("@userid", topup.UserID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "AccountDetails");
            conn.Close();

            double userBalance = 0.00;

            foreach (DataRow row in result.Tables["AccountDetails"].Rows)
            {
                userBalance = Convert.ToDouble(row["Balance"]);
            }

            userBalance = userBalance + topup.topUpAmount;

                SqlCommand cmd1 = new SqlCommand
              ("UPDATE Users SET Balance = @remaining WHERE UserID = @userid", conn);
                cmd1.Parameters.AddWithValue("@remaining", userBalance);
                cmd1.Parameters.AddWithValue("@userid", topup.UserID);
                conn.Open();
                cmd1.ExecuteNonQuery();
                conn.Close();

                return true;
        }

        public List<Transaction> GetTxnByUser(int userID)
        {
            SqlCommand cmd = new SqlCommand
             ("SELECT * FROM Transactions WHERE UserID = @userid", conn);
            cmd.Parameters.AddWithValue("@userid", userID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet result = new DataSet();
            conn.Open();
            da.Fill(result, "TxnDetails");
            conn.Close();

            List<Transaction> txnlist = new List<Transaction>();

            foreach (DataRow row in result.Tables["TxnDetails"].Rows)
            {
                txnlist.Add(
                    new Transaction
                    {
                        TransactionsID = Convert.ToInt32(row["TransactionsID"]),
                        TransactionsDateTime = Convert.ToDateTime(row["TransactionDateTime"]),
                        TransactionAmount = Convert.ToDouble(row["TransactionAmount"]),
                        UserID = Convert.ToInt32(row["UserID"]),
                        MerchantID = Convert.ToInt32(row["MerchantID"])
                    }
                );
            }

            return txnlist;
        }
    }
}
