using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothes_Project
{
    internal class Cashier : User

    {
        private string connectionString = "Data Source=localhost;Initial Catalog=Clothes Managment System;Integrated Security=True;";
        public Cashier() : base() { }

        public Cashier(string FirstName, string LastName, string Username, string UserPassword, char Gender, int Age) 
            : base(FirstName, LastName, Username, UserPassword, Gender, Age) { }

        public void createOrder(Order order)
        {
            int orderId = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string checkCustomer = "SELECT COUNT(*) FROM Customer WHERE CustomerID = @CustomerID";
                using (SqlCommand cmd = new SqlCommand(checkCustomer, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", order.customerID);
                    int count = (int)cmd.ExecuteScalar();
                    if (count == 0)
                    {
                        Console.WriteLine("No customer found with this ID.");
                        return;
                    }
                }

                string checkCashier = "SELECT COUNT(*) FROM Cashier WHERE CashierID = @CashierID";
                using (SqlCommand cmd = new SqlCommand(checkCashier, conn))
                {
                    cmd.Parameters.AddWithValue("@CashierID", order.cashierID);
                    int count = (int)cmd.ExecuteScalar();
                    if (count == 0)
                    {
                        Console.WriteLine("No cashier found with this ID.");
                        return;
                    }
                }

                string insertOrder = @"INSERT INTO Orders (CustomerID, CashierID, OrderDate, Total)
                                    VALUES (@CustomerID, @CashierID, @OrderDate, @Total);
                                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(insertOrder, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", order.customerID);
                    cmd.Parameters.AddWithValue("@CashierID", order.cashierID);
                    cmd.Parameters.AddWithValue("@OrderDate", order.orderDate);
                    cmd.Parameters.AddWithValue("@Total", order.totalOrder);

                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out orderId))
                    {
                        Console.WriteLine("User ID inserted: " + orderId);
                    }
                    else
                    {
                        Console.WriteLine("Failed to retrieve inserted user ID.");
                        return;
                    }
                }

                decimal total = 0;

                foreach (var detail in order.items)
                {
                    string getPriceQuery = "SELECT Price FROM Product WHERE ProductID = @ProductID";
                    decimal price = 0;

                    using (SqlCommand priceCmd = new SqlCommand(getPriceQuery, conn))
                    {
                        priceCmd.Parameters.AddWithValue("@ProductID", detail.productId);
                        var priceResult = priceCmd.ExecuteScalar();
                        if (priceResult != null)
                        {
                            price = Convert.ToDecimal(priceResult);
                        }
                        else
                        {
                            Console.WriteLine($"Product with ID {detail.productId} not found.");
                            continue;
                        }
                    }

                    total += price * detail.orderQuantity;

                    string insertDetail = @"INSERT INTO OrderDetails (orderId, productId, quantity, price)
                            VALUES (@orderId, @productId, @quantity, @price)";
                    using (SqlCommand cmd = new SqlCommand(insertDetail, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderId", orderId);
                        cmd.Parameters.AddWithValue("@productId", detail.productId);
                        cmd.Parameters.AddWithValue("@quantity", detail.orderQuantity);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.ExecuteNonQuery();
                    }

                    string updateQuantity = @"UPDATE Product SET Quantity = Quantity - @OrderedQty 
                              WHERE ProductID = @ProductID AND Quantity >= @OrderedQty And isAvaliable = 1";
                    using (SqlCommand cmd = new SqlCommand(updateQuantity, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderedQty", detail.orderQuantity);
                        cmd.Parameters.AddWithValue("@ProductID", detail.productId);
                        int affected = cmd.ExecuteNonQuery();

                        if (affected == 0)
                        {
                            Console.WriteLine($"Product {detail.productId} does not have enough quantity.");
                        }
                    }
                }

                string updateTotal = "UPDATE Orders SET Total = @Total WHERE OrderID = @OrderID";
                using (SqlCommand updateCmd = new SqlCommand(updateTotal, conn))
                {
                    updateCmd.Parameters.AddWithValue("@Total", total);
                    updateCmd.Parameters.AddWithValue("@OrderID", orderId);
                    updateCmd.ExecuteNonQuery();
                }



                Console.WriteLine("Order and order details saved successfully.");
            }
        }

        public void deleteOrder(int id, int cashierId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string checkCashier = "SELECT cashierID FROM Orders WHERE orderID = @orderID";
                int orderIDC = -1;

                using (SqlCommand cashierComman = new SqlCommand(checkCashier, conn))
                {
                    cashierComman.Parameters.AddWithValue("@orderID", id);
                    object result = cashierComman.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                    {
                        Console.WriteLine("Order Not Found");
                        return;
                    }

                    orderIDC = Convert.ToInt32(result);
                }

                if (orderIDC != cashierId)
                {
                    Console.WriteLine("You are not authorized to delete this order.");
                    return;
                }

                string getDetails = "SELECT ProductID, Quantity FROM OrderDetails WHERE OrderID = @orderID";
                List<(int productId, int quantity)> restoreList = new List<(int, int)>();

                using (SqlCommand cmd = new SqlCommand(getDetails, conn))
                {
                    cmd.Parameters.AddWithValue("@orderID", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int productId = reader.GetInt32(0);
                            int qty = reader.GetInt32(1);
                            restoreList.Add((productId, qty));
                        }
                    }
                }

                foreach (var item in restoreList)
                {
                    string updateProduct = "UPDATE Product SET Quantity = Quantity + @qty WHERE ProductID = @pid";

                    using (SqlCommand cmd = new SqlCommand(updateProduct, conn))
                    {
                        cmd.Parameters.AddWithValue("@qty", item.quantity);
                        cmd.Parameters.AddWithValue("@pid", item.productId);
                        cmd.ExecuteNonQuery();
                    }
                }

                string deleteOrderDetails = "DELETE FROM OrderDetails WHERE OrderID = @orderID";
                using (SqlCommand com = new SqlCommand(deleteOrderDetails, conn))
                {
                    com.Parameters.AddWithValue("@orderID", id);
                    com.ExecuteNonQuery();
                }

                string deleteOrders = "DELETE FROM Orders WHERE OrderID = @orderID";
                using (SqlCommand com = new SqlCommand(deleteOrders, conn))
                {
                    com.Parameters.AddWithValue("@orderID", id);
                    com.ExecuteNonQuery();
                }

                Console.WriteLine("Order Deleted Successfully");
            }
        }


        public void calculatePayment(int id, int cashierId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string checkCashier = "SELECT CashierID FROM Orders WHERE OrderID = @orderID";
                int orderIDC = -1;
                using (SqlCommand cashierCommand = new SqlCommand(checkCashier, conn))
                {
                    cashierCommand.Parameters.AddWithValue("@orderID", id);
                    object result = cashierCommand.ExecuteScalar();

                    if (result == null)
                    {
                        Console.WriteLine("Order Not Found");
                        return;
                    }

                    orderIDC = Convert.ToInt32(result);
                }

                if (orderIDC != cashierId)
                {
                    Console.WriteLine("You are not authorized to view this order.");
                    return;
                }

                string getPayment = "SELECT Total FROM Orders WHERE OrderID = @orderID";
                float total = 0;

                using (SqlCommand getCommand = new SqlCommand(getPayment, conn))
                {
                    getCommand.Parameters.AddWithValue("@orderID", id);
                    object totalObj = getCommand.ExecuteScalar();
                    if (totalObj != null)
                    {
                        total = Convert.ToSingle(totalObj);
                    }
                    else
                    {
                        Console.WriteLine("Could not retrieve total.");
                        return;
                    }
                }

                Console.WriteLine("Total Payment = " + total);
            }
        }

    }
}
