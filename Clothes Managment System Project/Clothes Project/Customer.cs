using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothes_Project
{
    internal class Customer : User
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=Clothes Managment System;Integrated Security=True;";

        public Customer() : base() { }

        public  Customer(string FirstName, string LastName, string Username, string UserPassword, char Gender, int Age) : 
            base(FirstName, LastName, Username, UserPassword, Gender, Age)
        {
        }

        public void viewOrderDetails(int id) 
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string checkCust = "SELECT COUNT(*) FROM Customer WHERE CustomerID = @CustomerID";
            SqlCommand checkcomm = new SqlCommand(checkCust, conn);
            checkcomm.Parameters.AddWithValue("@CustomerID", id);
            int check = (int)checkcomm.ExecuteScalar();

            if (check == 0)
            {
                Console.WriteLine("You Don't Have Access For This Information");
                conn.Close();
                return;
            }

            string view = @"SELECT o.customerId, p.productName, d.quantity, d.price, 
                           (d.quantity * d.price) AS total
                    FROM orders o
                    JOIN orderDetails d ON o.orderId = d.orderId
                    JOIN Product p ON p.productID = d.productId
                    WHERE o.customerId = @customerId";
            using (SqlCommand comm = new SqlCommand(view, conn)) 
            {
                comm.Parameters.AddWithValue(@"customerId", id);
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    bool hasData = false;
                    while (reader.Read())
                    {
                        hasData = true;
                        Console.WriteLine($"CustomerID: {reader["CustomerID"]}, Product: {reader["ProductName"]} " +
                                          $"Quantity: {reader["Quantity"]}, Price: {reader["Price"]}, Total: {reader["Total"]}");
                    }

                    if (!hasData)
                    {
                        Console.WriteLine("No orders found for this customer.");
                    }
                }

            }

                conn.Close();
        }

        public void rateOrder() 
        
        {
            int number = ReadInt("Please Rate The Order (1 - 5)");

            while (number > 5 || number < 1) 
            {
                Console.WriteLine("Sorry Invalid Input, Please enter number between 1 and 5");
                number = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine("Thanks For Your Rating, We Are Happy To Help You! ");

        }

        

    }
}
