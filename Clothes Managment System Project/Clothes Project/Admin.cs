using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.ComponentModel.Design;
using System.Globalization;

namespace Clothes_Project
{
    internal class Admin : User

    {

        private string connectionString = "Data Source=localhost;Initial Catalog=Clothes Managment System;Integrated Security=True;";
        public Admin() : base() { }

        public Admin(string FirstName, string LastName, string Username, string UserPassword, char Gender, int Age) : 
            base(FirstName, LastName, Username, UserPassword, Gender, Age) { 
        }

        public void addAdmin(Admin admin)
        {
            int userId = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string insertUser = @"INSERT INTO Users (FirstName, LastName, UserName, UserPassword, Gender, Age) 
                                     VALUES (@FirstName, @LastName, @UserName, @UserPassword, @Gender, @Age);
                                     SELECT SCOPE_IDENTITY();";

                using (SqlCommand comm = new SqlCommand(insertUser, conn))
                {
                    comm.Parameters.AddWithValue("@FirstName", admin.FirstName);
                    comm.Parameters.AddWithValue("@LastName", admin.LastName);
                    comm.Parameters.AddWithValue("@UserName", admin.Username);
                    comm.Parameters.AddWithValue("@UserPassword", admin.UserPassword);
                    comm.Parameters.AddWithValue("@Gender", admin.Gender);
                    comm.Parameters.AddWithValue("@Age", admin.Age);

                    object result = comm.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out userId))
                    {
                        Console.WriteLine("User ID inserted: " + userId);
                    }
                    else
                    {
                        Console.WriteLine("Failed to retrieve inserted user ID.");
                        return;
                    }
                }

                string insertAdmin = "INSERT INTO Admin (AdminID) VALUES (@AdminID);";
                using (SqlCommand cmd = new SqlCommand(insertAdmin, conn))
                {
                    cmd.Parameters.AddWithValue("@AdminID", userId);
                    cmd.ExecuteNonQuery();
                }


                Console.WriteLine("Admin added successfully.\n");
            }
        }

        public void deleteAdmin(int id)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string checkQuery = "SELECT COUNT(*) FROM Admin WHERE AdminID = @AdminID";
            using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
            {
                checkCmd.Parameters.AddWithValue("@AdminID", id);
                int exists = (int)checkCmd.ExecuteScalar();

                if (exists == 0)
                {
                    Console.WriteLine("Admin Not Found");
                    conn.Close();
                    return;
                }
            }

            string deleteFromAdmin = "Delete from Admin Where AdminID = @AdminID";
            using (SqlCommand comand = new SqlCommand(deleteFromAdmin, conn))
            {
                comand.Parameters.AddWithValue("@AdminID", id);
                comand.ExecuteNonQuery();
            }

            string deleteFromUsers = "Delete From Users Where Id = @Id";

            using (SqlCommand com = new SqlCommand(deleteFromUsers, conn))
            {
                com.Parameters.AddWithValue("@Id", id);
                com.ExecuteNonQuery();
            }

            Console.WriteLine("Admin Deleted Successfully");
            conn.Close();
        }

        public void ListAllAdmins() {
            using (SqlConnection conn = new SqlConnection(connectionString)) {
                conn.Open();
                string listElements = @"Select Id, FirstName, LastName, UserName, Gender, Age from Users Inner Join Admin On(Users.Id = Admin.AdminID)";

                using (SqlCommand com = new SqlCommand(listElements, conn)) {
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["Id"]}, Name: {reader["FirstName"]} {reader["LastName"]}, " +
                                              $"Email: {reader["UserName"]}, Gender: {reader["Gender"]}, Age: {reader["Age"]}");
                        }
                    }
                }

            }
        }

        public void SearchAnAdmin(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Admin WHERE AdminID = @AdminID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@AdminID", id);
                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists == 0)
                    {
                        Console.WriteLine("Admin Not Found");
                        return;
                    }
                }

                string listElements = @"Select Id, FirstName, LastName, UserName, Gender, Age from Users Inner Join Admin 
                                      On(Users.Id = Admin.AdminID)
                                      where Admin.AdminID = @AdminID";

                using (SqlCommand com = new SqlCommand(listElements, conn))
                {
                    com.Parameters.AddWithValue("AdminID", id);
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        Console.WriteLine("Admin Details: \n");
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["Id"]}, Name: {reader["FirstName"]} {reader["LastName"]}, " +
                                              $"Email: {reader["UserName"]}, Gender: {reader["Gender"]}, Age: {reader["Age"]}");
                        }
                    }
                }

            }
        }

        public void EditAdmin(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string getElementId = "SELECT * FROM Users WHERE Id = @Id";
                int userId = -1;

                using (SqlCommand com = new SqlCommand(getElementId, conn))
                {
                    com.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            Console.WriteLine("User Not Found");
                            return;
                        }

                        userId = Convert.ToInt32(reader["Id"]);
                    }
                }

                Console.Write("\nPlease Enter new Admin First name: ");
                string fname = Console.ReadLine();

                Console.Write("Please Enter new Admin Last name: ");
                string lname = Console.ReadLine();

                string username = fname + "." + lname + "@gmail.com";

                Console.Write("Please Enter new Cashier Password: ");
                string pass = Console.ReadLine();

                char Agender = ReadGender("Please Enter new Admin Gender (M/F): ");
                while (Agender != 'M' && Agender != 'F' && Agender != 'm' && Agender != 'f')
                {
                    Agender = ReadGender("Invalid Input, Please Enter (M / F): ");
                }

                int age = ReadInt("Please Enter new Admin Age: ");

                string EditElement = @"UPDATE Users 
                               SET FirstName = @FirstName, LastName = @LastName, 
                               UserName = @UserName, UserPassword = @UserPassword, Gender = @Gender, Age = @Age
                               WHERE Id = @Id";

                using (SqlCommand com = new SqlCommand(EditElement, conn))
                {
                    com.Parameters.AddWithValue("@FirstName", fname);
                    com.Parameters.AddWithValue("@LastName", lname);
                    com.Parameters.AddWithValue("@UserName", username);
                    com.Parameters.AddWithValue("@UserPassword", pass);
                    com.Parameters.AddWithValue("@Gender", Agender);
                    com.Parameters.AddWithValue("@Age", age);
                    com.Parameters.AddWithValue("@Id", userId);

                    int rowsAffected = com.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("\nAdmin data updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Update failed. No rows affected.");
                    }
                }
            }
        }

        public void addCashier(Cashier cashier) {
            int userId = 0;
           
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string insertCashier = @"Insert Into Users (FirstName, LastName, UserName, UserPassword, Gender, Age) 
                                    Values(@FirstName, @LastName, @UserName, @UserPassword, @Gender, @Age);
                                    SELECT SCOPE_IDENTITY();";
            using (SqlCommand com = new SqlCommand(insertCashier, conn))
            {
                com.Parameters.AddWithValue("@FirstName", cashier.FirstName);
                com.Parameters.AddWithValue("@LastName", cashier.LastName);
                com.Parameters.AddWithValue("@UserName", cashier.Username);
                com.Parameters.AddWithValue("@UserPassword", cashier.UserPassword);
                com.Parameters.AddWithValue("@Gender", cashier.Gender);
                com.Parameters.AddWithValue("@Age", cashier.Age);

                object result = com.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out userId))
                {
                    Console.WriteLine("User ID inserted: " + userId);
                }
                else
                {
                    Console.WriteLine("Failed to retrieve inserted user ID.");
                    return;
                }
            }

            string insertCashierInAdmin = "Insert Into Cashier(CashierID) Values (@CashierID);";

            using (SqlCommand comm = new SqlCommand(insertCashierInAdmin, conn))
            {
                comm.Parameters.AddWithValue("@CashierID", userId);
                comm.ExecuteNonQuery();
                Console.WriteLine("Cashier Added Successfully");
            }
            conn.Close();
        }

        public void deleteCashier(int id) {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string checkCashierQuery = "SELECT COUNT(*) FROM Cashier WHERE CashierID = @CashierID";
            using (SqlCommand checkCmd = new SqlCommand(checkCashierQuery, conn))
            {
                checkCmd.Parameters.AddWithValue("@CashierID", id);
                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    Console.WriteLine("Cashier Not Found");
                    return;
                }
            }


            string deleteFromCashier = "Delete From Cashier Where CashierID = @CashierID";

            SqlCommand co = new SqlCommand(deleteFromCashier, conn);

            co.Parameters.AddWithValue("@CashierID", id);
            co.ExecuteNonQuery();

            string deleteFromUsers = "Delete From Users Where Id = @Id";
            SqlCommand com = new SqlCommand(deleteFromUsers, conn);
            
            com.Parameters.AddWithValue("@Id", id);
            com.ExecuteNonQuery();
            

            Console.WriteLine("Cashier deleted successfully");
            conn.Close();
        }

        public void ListAllCashiers() { 
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string Listcashier = @"Select Id, FirstName, LastName, UserName, Gender, Age 
                                 From Users Inner Join Cashier On(Users.Id = Cashier.CashierID)";
            SqlCommand comm = new SqlCommand(Listcashier, conn);

            SqlDataReader reader = comm.ExecuteReader();
            Console.WriteLine("Cashier Users: ");
            while (reader.Read()) {
                
                Console.WriteLine($"ID: {reader["Id"]}, Name: {reader["FirstName"]} {reader["LastName"]}, " +
                                  $"Email: {reader["UserName"]}, Gender: {reader["Gender"]}, Age: {reader["Age"]}");
            }
            conn.Close();
        }

        public void SearchACashier(int id)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string checkCashierQuery = "SELECT COUNT(*) FROM Cashier WHERE CashierID = @CashierID";
            using (SqlCommand checkCmd = new SqlCommand(checkCashierQuery, conn))
            {
                checkCmd.Parameters.AddWithValue("@CashierID", id);
                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    Console.WriteLine("Cashier Not Found");
                    conn.Close();
                    return;
                }
            }

            string Listcashier = @"Select Id, FirstName, LastName, UserName, Gender, Age 
                                 From Users Inner Join Cashier On(Users.Id = Cashier.CashierID) where Cashier.CashierID = @CashierID";
            using (SqlCommand comm = new SqlCommand(Listcashier, conn))
            {
                comm.Parameters.AddWithValue("@CashierID", id);
                SqlDataReader reader = comm.ExecuteReader();
                Console.WriteLine("Cashier Details: ");
                while (reader.Read())
                {

                    Console.WriteLine($"ID: {reader["Id"]}, Name: {reader["FirstName"]} {reader["LastName"]}, " +
                                      $"Email: {reader["UserName"]}, Gender: {reader["Gender"]}, Age: {reader["Age"]}");
                }
            }
            conn.Close();
        }

        public void EditCashier(int id) { 
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            int userId = 0;

            string checkCashierQuery = "SELECT COUNT(*) FROM Cashier WHERE CashierID = @CashierID";
            using (SqlCommand checkCmd = new SqlCommand(checkCashierQuery, conn))
            {
                checkCmd.Parameters.AddWithValue("@CashierID", id);
                int count = (int)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    Console.WriteLine("Cashier Not Found");
                    return;
                }
            }

            string checkID = "Select * From Users Where Id = @Id";
            SqlCommand comm = new SqlCommand(checkID, conn);
            comm.Parameters.AddWithValue("@Id", id);

            using (SqlDataReader reader = comm.ExecuteReader())
            {

                if (!reader.Read())
                {
                    Console.WriteLine("Cashier Not Found");
                    return;
                }

                else
                {
                    userId = Convert.ToInt32(reader["Id"]);
                }
            }

            Console.Write("\nPlease Enter new Cashier First name: ");
            string fname = Console.ReadLine();

            Console.Write("Please Enter new Cashier Last name: ");
            string lname = Console.ReadLine();

            string username = fname + "." + lname + "@gmail.com";

            Console.Write("Please Enter new Cashier Password: ");
            string pass = Console.ReadLine();

            char gender = ReadGender("Please Enter new Cashier Gender (M/F): ");
            while (gender != 'M' && gender != 'F' && gender != 'm' && gender != 'f')
            {
                gender = ReadGender("Invalid Input, Please Enter (M / F): ");
            }

            int age = ReadInt("Please Enter new Cashier Age: ");


            string editCashier = @"Update Users Set FirstName = @FirstName, LastName = @LastName,
                                 UserName = @UserName, UserPassword = @UserPassword, Gender = @Gender, Age = @Age
                                 Where Id = @Id";

            using (SqlCommand com = new SqlCommand(editCashier, conn))
            {
                com.Parameters.AddWithValue("@FirstName", fname);
                com.Parameters.AddWithValue("@LastName", lname);
                com.Parameters.AddWithValue("@UserPassword", pass);
                com.Parameters.AddWithValue("@UserName", username);
                com.Parameters.AddWithValue("@Gender", gender);
                com.Parameters.AddWithValue("@Age", age);
                com.Parameters.AddWithValue("@Id", id);

                int row = com.ExecuteNonQuery();

                if (row > 0)
                {
                    Console.WriteLine("Cashier Updated Successfully");
                }
                else
                {
                    Console.WriteLine("Error, Cashier Doesn't Updated");
                }
            }
            conn.Close();
        
        }

        public void addCustomer(Customer customer) {
            int userId = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string insertUser = @"Insert Into Users(FirstName, LastName, UserName, UserPassword, Gender, Age) 
                                 Values (@FirstName, @LastName, @UserName, @UserPassword, @Gender, @Age);
                                 SELECT SCOPE_IDENTITY();";

            using (SqlCommand comm = new SqlCommand(insertUser, conn)) {
                comm.Parameters.AddWithValue("@FirstName", customer.FirstName);
                comm.Parameters.AddWithValue("@LastName", customer.LastName);
                comm.Parameters.AddWithValue("@UserName", customer.Username);
                comm.Parameters.AddWithValue("@UserPassword", customer.UserPassword);
                comm.Parameters.AddWithValue("@Gender", customer.Gender);
                comm.Parameters.AddWithValue("@Age", customer.Age);

                object result = comm.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out userId))
                {
                    Console.WriteLine("User ID inserted: " + userId);
                }
                else
                {
                    Console.WriteLine("Failed to retrieve inserted user ID.");
                    return;
                }
            }


            string insertCustInUser = "Insert Into Customer(CustomerID) Values(@CustomerID)";

            using (SqlCommand com = new SqlCommand(insertCustInUser, conn)) {
                com.Parameters.AddWithValue("@CustomerID", userId);
                com.ExecuteNonQuery();
                Console.WriteLine("Customer Added Successfully");
            }

            conn.Close();
        }

        public void deleteCustomer(int id) {
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            string checkCust = "Select Count(*) From Customer Where CustomerID = @CustomerID";
            SqlCommand checkcomm = new SqlCommand(checkCust, conn);
            checkcomm.Parameters.AddWithValue("@CustomerID", id);
            int check = (int)checkcomm.ExecuteScalar();

            if (check == 0)
            {
                Console.WriteLine("Customer Not Found");
                return;
            }

            string deleteFromCustomer = "Delete From Customer Where CustomerID = @CustomerID";
            using (SqlCommand com = new SqlCommand(deleteFromCustomer, conn))
            {
                com.Parameters.AddWithValue("@CustomerID", id);
                com.ExecuteNonQuery();
            }

            string deleteFromUser = "Delete From Users Where Id = @Id";
            using (SqlCommand comm = new SqlCommand(deleteFromUser, conn))
            {
                comm.Parameters.AddWithValue("@Id", id);
                comm.ExecuteNonQuery();
            }

            Console.WriteLine("Customer Deleted Successfully");
            conn.Close();
        }

        public void listAllCustomers()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string listcust = @"Select Id, FirstName, LastName, UserName, Gender, Age 
                              From Users Inner Join Customer 
                              On(Users.Id = Customer.CustomerID)";

            using (SqlCommand comm = new SqlCommand(listcust, conn))
            {
                SqlDataReader reader = comm.ExecuteReader();
                Console.WriteLine("Customer Users: ");
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["Id"]}, Name: {reader["FirstName"]} {reader["LastName"]}, " +
                                      $"Email: {reader["UserName"]}, Gender: {reader["Gender"]}, Age: {reader["Age"]}");
                }
            }

            conn.Close();

        }

        public void SearchACustomer(int id)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string checkCust = "SELECT COUNT(*) FROM Customer WHERE CustomerID = @CustomerID";
            SqlCommand checkcomm = new SqlCommand(checkCust, conn);
            checkcomm.Parameters.AddWithValue("@CustomerID", id);
            int check = (int)checkcomm.ExecuteScalar();

            if (check == 0)
            {
                Console.WriteLine("Customer Not Found");
                conn.Close();
                return;
            }

            string listcust = @"
                                SELECT Users.Id, FirstName, LastName, UserName, Gender, Age 
                                FROM Users 
                                INNER JOIN Customer ON Users.Id = Customer.CustomerID 
                                WHERE Customer.CustomerID = @CustomerID";

            using (SqlCommand comm = new SqlCommand(listcust, conn))
            {
                comm.Parameters.AddWithValue("@CustomerID", id);
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("Customer User:");
                    Console.WriteLine($"ID: {reader["Id"]}, Name: {reader["FirstName"]} {reader["LastName"]}, " +
                                      $"Email: {reader["UserName"]}, Gender: {reader["Gender"]}, Age: {reader["Age"]}");
                }
            }

            conn.Close();
        }


        public void EditCustomer(int id) {
            int userid = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string checkCust = "Select Count(*) From Customer Where CustomerID = @CustomerID";
            SqlCommand checkcomm = new SqlCommand(checkCust, conn);
            checkcomm.Parameters.AddWithValue("@CustomerID", id);
            
            int check = (int)checkcomm.ExecuteScalar();
            if (check == 0) {
                Console.WriteLine("Customer Not Found");
                return;
            }

            string checkID = "Select * From Users Where Id = @Id";
            SqlCommand comm = new SqlCommand(checkID, conn);
            comm.Parameters.AddWithValue("@Id", id);

            using (SqlDataReader reader = comm.ExecuteReader())
            {

                if (!reader.Read())
                {
                    Console.WriteLine("Cashier Not Found");
                    return;
                }

                else
                {
                    userid = Convert.ToInt32(reader["Id"]);
                }
            }

            Console.Write("\nPlease Enter new Customer First name: ");
            string fname = Console.ReadLine();

            Console.Write("Please Enter new Customer Last name: ");
            string lname = Console.ReadLine();

            string username = fname + "." + lname + "@gmail.com";

            Console.Write("Please Enter new Customer Password: ");
            string pass = Console.ReadLine();

            char Agender = ReadGender("Please Enter new Customer Gender (M/F): ");
            while (Agender != 'M' && Agender != 'F' && Agender != 'm' && Agender != 'f')
            {
                Agender = ReadGender("Invalid Input, Please Enter (M / F): ");
            }

            int age = ReadInt("Please Enter new Customer Age: ");

            string editcustomer = @"Update Users 
                                  Set FirstName = @FirstName, LastName = @LastName, UserName = @UserName, UserPassword = @UserPassword, Gender = @Gender, Age = @Age
                                  Where Id = @Id";

            using (SqlCommand com = new SqlCommand(editcustomer, conn)) 
            {
                com.Parameters.AddWithValue("@FirstName", fname);
                com.Parameters.AddWithValue("@LastName", lname);
                com.Parameters.AddWithValue("@UserName", username);
                com.Parameters.AddWithValue("@UserPassword", pass);
                com.Parameters.AddWithValue("@Gender", Agender);
                com.Parameters.AddWithValue("@Age", age);
                com.Parameters.AddWithValue("@Id", id);

                int row = com.ExecuteNonQuery();
                if (row > 0)
                {
                    Console.WriteLine("Cutomer Updated Successfully");
                }

                else 
                {
                    Console.WriteLine("Cutomer Doesn't Updated");
                }
            }

            conn.Close();
        }

        public void addProduct(Product product) {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string addPro = @"Insert Into Product(productName, category, size, color, price, quantity, isAvaliable, supplier ) 
                              Values (@productName, @category, @size, @color, @price, @quantity, @isAvaliable, @supplier);
                              SELECT SCOPE_IDENTITY();";

            using (SqlCommand comm = new SqlCommand(addPro, conn)) 
            {
                comm.Parameters.AddWithValue("@productName", product.productName);
                comm.Parameters.AddWithValue("@category", product.category);
                comm.Parameters.AddWithValue("@size", product.size);
                comm.Parameters.AddWithValue("@color", product.color);
                comm.Parameters.AddWithValue("@price", product.price);
                comm.Parameters.AddWithValue("@quantity", product.quantity);
                comm.Parameters.AddWithValue("@isAvaliable", product.isAvaliable);
                comm.Parameters.AddWithValue("@supplier", product.supplier);

                comm.ExecuteNonQuery();
            }
            Console.WriteLine("Product Added Successfully");
            conn.Close();
        }

        public void deleteProduct(int id) {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string checkPro = "Select Count(*) From Product Where productID = @productID";
            using (SqlCommand comm = new SqlCommand(checkPro, conn)) 
            {
                comm.Parameters.AddWithValue("@productID", id);
                int check = (int)comm.ExecuteScalar();

                if (check == 0) {
                    Console.WriteLine("Product Not Found");
                    return;
                }
            }

            string deletePro = "Delete From Product Where productID = @productID";
            using (SqlCommand com = new SqlCommand(deletePro,conn)) 
            {
                com.Parameters.AddWithValue("@productID", id);
                com.ExecuteNonQuery();
            }

            Console.WriteLine("Product Deleted Successfully");

            conn.Close();
        }

        public void listAllProducts() { 
            SqlConnection conn = new SqlConnection (connectionString);
            conn.Open();

            string listPro = "Select * from Product";
            using (SqlCommand comm = new SqlCommand(listPro, conn)) 
            {
                SqlDataReader reader = comm.ExecuteReader();

                
                while (reader.Read())
                {
                    bool avaliable = Convert.ToBoolean(reader["isAvaliable"]);
                    string ava = avaliable ? "Yes" : "No";

                    Console.WriteLine($"ID: {reader["productID"]}, Name: {reader["productName"]}, " +
                                      $"Category: {reader["category"]}, Size: {reader["size"]}, Color: {reader["color"]}, " +
                                      $"Price: {reader["price"]}, Quantity: {reader["quantity"]}, Available: {ava} ");
                    Console.WriteLine("--------------------------------------------------");
                }
            }
            conn.Close();
        }

        public void SearchAProduct(int id)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string checkPro = "SELECT COUNT(*) FROM Product WHERE productID = @productID";
            using (SqlCommand checkCmd = new SqlCommand(checkPro, conn))
            {
                checkCmd.Parameters.AddWithValue("@productID", id);
                int check = (int)checkCmd.ExecuteScalar();

                if (check == 0)
                {
                    Console.WriteLine("Product Not Found");
                    conn.Close();
                    return;
                }
            }

            string listPro = "SELECT * FROM Product WHERE productID = @productID";
            using (SqlCommand getCmd = new SqlCommand(listPro, conn))
            {
                getCmd.Parameters.AddWithValue("@productID", id);
                using (SqlDataReader reader = getCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bool avaliable = Convert.ToBoolean(reader["isAvaliable"]);
                        string ava = avaliable ? "Yes" : "No";

                        Console.WriteLine($"ID: {reader["productID"]}, Name: {reader["productName"]}, " +
                                          $"Category: {reader["category"]}, Size: {reader["size"]}, Color: {reader["color"]}, " +
                                          $"Price: {reader["price"]}, Quantity: {reader["quantity"]}, Available: {ava}");
                        Console.WriteLine("--------------------------------------------------");
                    }
                }
            }

            conn.Close();
        }


        public void editProduct(int id) 
        { 
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string checkPro = "Select Count(*) From Product Where productID = @productID";
            using (SqlCommand comm = new SqlCommand(checkPro, conn))
            {
                comm.Parameters.AddWithValue("@productID", id);
                int check = (int)comm.ExecuteScalar();

                if (check == 0) {
                    Console.WriteLine("Product Not Found");
                    return;
                }
            }

            Console.WriteLine("Please Enter Product Name");
            string Pname = Console.ReadLine();

            Console.WriteLine("Please Enter Product Size (Small, Medium, XL, XLL, 3XL)");
            string Psize = Console.ReadLine();

            while (Psize.ToLower() != "small" && Psize.ToLower() != "medium" && Psize.ToLower() != "xl" && Psize.ToLower() != "xxl" && Psize != "3xl")
            {
                Console.WriteLine("Invalid input. Please enter a valid Size.");
                Psize = Console.ReadLine();
            }

            Console.WriteLine("Please Enter Category (Men, Women, Children)");
            string Pcategory = Console.ReadLine();

            while (Pcategory.ToLower() != "men" && Pcategory.ToLower() != "women" && Pcategory.ToLower() != "children")
            {
                Console.WriteLine("Invalid input. Please enter a valid Category.");
                Pcategory = Console.ReadLine();
            }

            int Pquantity = ReadInt("Please Enter Product Qantity");

            float Pprice = ReadFloat("Please Enter Product Price");

            Console.WriteLine("Please Enter Product Color");
            string Pcolor = Console.ReadLine();

            Console.WriteLine("Please Enter if the Product is Available (true or false):");
            string Pinput = Console.ReadLine();
            bool PAva;

            while (!bool.TryParse(Pinput, out PAva))
            {
                Console.WriteLine("Invalid input. Please enter 'true' or 'false':");
                Pinput = Console.ReadLine();
            }

            Console.WriteLine("Please Enter Product Supplier");
            string Psupp = Console.ReadLine();

            string editPro = @"Update Product 
                             Set productName = @productName, category = @category, size = @size, color = @color, price = @price,
                             quantity = @quantity, isAvaliable = @isAvaliable, supplier = @supplier Where productID = @productID";

            using (SqlCommand comm = new SqlCommand(editPro, conn))  
            {
                comm.Parameters.AddWithValue("@productName", Pname);
                comm.Parameters.AddWithValue("@category", Pcategory);
                comm.Parameters.AddWithValue("@size", Psize);
                comm.Parameters.AddWithValue("@color", Pcolor);
                comm.Parameters.AddWithValue("@price", Pprice);
                comm.Parameters.AddWithValue("@quantity", Pquantity);
                comm.Parameters.AddWithValue("@isAvaliable", PAva);
                comm.Parameters.AddWithValue("@supplier", Psupp);
                comm.Parameters.AddWithValue("@productID", id);

                int row = comm.ExecuteNonQuery();

                if (row > 0)
                {
                    Console.WriteLine("Product Updated Successfully");
                }

                else
                {
                    Console.WriteLine("Product Doesn't Updated");
                }
            }
            conn.Close();
        }

        public void viewOrderDetails() 
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string viewOrders = @"Select o.orderId, p.ProductName, o.productId, o.quantity, o.price, (o.quantity * o.price) AS total
                                  from orderDetails o
                                  Join Product p
                                  on (o.productId = p.productID)";

            using (SqlCommand comm = new SqlCommand(viewOrders, conn)) 
            {
                SqlDataReader reader = comm.ExecuteReader();

                while (reader.Read()) 
                {
                    Console.WriteLine($"OrderId: {reader["orderId"]}, Product Name: {reader["ProductName"]}, " +
                                          $"Price: {reader["price"]}, Quantity: {reader["quantity"]}, Total: {reader["Total"]}");
                    Console.WriteLine("--------------------------------------------------");
                }
            }
            
            conn.Close();
        }

        public double totalAllRevenueOrders() 
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            double result = 0;

            string totalRev = @"Select Sum(price * Quantity) From orderDetails";

            using (SqlCommand comm = new SqlCommand(totalRev, conn))
            {
                result = Convert.ToDouble(comm.ExecuteScalar()); 
            }

            conn.Close();

            return result;
        }

        public double totalRevenueOrders(DateTime start, DateTime end)
        {
            double total = 0;

            string totalRev = @"SELECT SUM(d.Quantity * d.Price) AS TotalRevenue
                        FROM Orders o
                        JOIN OrderDetails d ON o.OrderId = d.OrderId
                        WHERE o.OrderDate BETWEEN @StartDate AND @EndDate;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand comm = new SqlCommand(totalRev, conn))
                {
                    comm.Parameters.AddWithValue("@StartDate", start);
                    comm.Parameters.AddWithValue("@EndDate", end);

                    object result = comm.ExecuteScalar();

                    if (result != null && result != DBNull.Value && double.TryParse(result.ToString(), out total))
                    {
                        return total;
                    }
                    else
                    {
                        Console.WriteLine("No revenue found or value is invalid.");
                        return 0;
                    }
                }
            }
        }


        public double averageAllRevenueOrders()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            double result = 0;

            string totalRev = @"Select avg(price * Quantity) From orderDetails";

            using (SqlCommand comm = new SqlCommand(totalRev, conn))
            {
                result = Convert.ToDouble(comm.ExecuteScalar());
            }

            conn.Close();

            return result;
        }

        public double averageRevenueOrders(DateTime start, DateTime end)
        {
            double total = 0;

            string totalRev = @"SELECT AVG(d.Quantity * d.Price) AS TotalRevenue
                        FROM Orders o
                        JOIN OrderDetails d ON o.OrderId = d.OrderId
                        WHERE o.OrderDate BETWEEN @StartDate AND @EndDate;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(totalRev, conn))
                {
                    comm.Parameters.AddWithValue("@StartDate", start);
                    comm.Parameters.AddWithValue("@EndDate", end);

                    object result = comm.ExecuteScalar();

                    if (result != null && result != DBNull.Value && double.TryParse(result.ToString(), out total))
                    {
                        return total;
                    }
                    else
                    {
                        Console.WriteLine("No revenue found or value is invalid.");
                        return 0;
                    }
                }
            }
        }


        public int noOfPeciesSold(DateTime start, DateTime end)
        {
            int result = 0;

            string noOfPecies = @"SELECT SUM(d.quantity) 
                          FROM orderDetails d
                          JOIN orders o ON d.orderId = o.orderID
                          WHERE o.orderDate BETWEEN @StartDate AND @EndDate;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand comm = new SqlCommand(noOfPecies, conn))
                {
                    comm.Parameters.AddWithValue("@StartDate", start);
                    comm.Parameters.AddWithValue("@EndDate", end);

                    object scalarResult = comm.ExecuteScalar();

                    if (scalarResult != null && scalarResult != DBNull.Value && int.TryParse(scalarResult.ToString(), out int value))
                    {
                        result = value;
                    }
                    else
                    {
                        Console.WriteLine("No pieces sold in the given date range.");
                    }
                }
            }

            return result;
        }


        public void listOfSupplierPricing() 
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string supplierPricing = @"Select productName, supplier, price From Product";

            using (SqlCommand comm = new SqlCommand(supplierPricing, conn))
            { 
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read()) 
                {
                    Console.WriteLine($"Product Name: {reader["ProductName"]}, Supplier: {reader["supplier"]}, Price: {reader["price"]}");
                    Console.WriteLine("--------------------------------------------------");
                }
            }

                conn.Close();
        }

        public void bestSellerProduct(DateTime start, DateTime end)
        {
            string bestseller = @"
        SELECT TOP 1 
            p.productID, 
            p.productName, 
            SUM(d.Quantity) AS TotalSold
        FROM orderDetails d
        JOIN Product p ON p.productID = d.productID
        JOIN Orders o ON d.orderId = o.orderId
        WHERE o.orderDate BETWEEN @StartDate AND @EndDate
        GROUP BY p.productID, p.productName
        ORDER BY TotalSold DESC;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand comm = new SqlCommand(bestseller, conn))
                {
                    comm.Parameters.AddWithValue("@StartDate", start);
                    comm.Parameters.AddWithValue("@EndDate", end);

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("Best Seller Product:");
                            Console.WriteLine($"ID: {reader["ProductID"]}, Name: {reader["ProductName"]}, Total Sold: {reader["TotalSold"]}");
                        }
                        else
                        {
                            Console.WriteLine("No sales data available for the selected date range.");
                        }
                    }
                }
            }
        }


        public void mostRevenueProduct(DateTime start, DateTime end)
        {
            string mostRevenue = @"
        SELECT TOP 1 
            p.productID, 
            p.productName, 
            SUM(d.price * d.Quantity) AS Revenue
        FROM orderDetails d
        JOIN Product p ON p.productID = d.productID
        JOIN Orders o ON d.orderId = o.orderId
        WHERE o.orderDate BETWEEN @StartDate AND @EndDate
        GROUP BY p.productID, p.productName
        ORDER BY Revenue DESC;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand comm = new SqlCommand(mostRevenue, conn))
                {
                    comm.Parameters.AddWithValue("@StartDate", start);
                    comm.Parameters.AddWithValue("@EndDate", end);

                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("Most Revenue Product:");
                            Console.WriteLine($"ID: {reader["ProductID"]}, Name: {reader["ProductName"]}, Revenue: {reader["Revenue"]}");
                        }
                        else
                        {
                            Console.WriteLine("No sales data available for the selected date range.");
                        }
                    }
                }
            }
        }


        public void cashierWithMaxOrders()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string cashier = @"SELECT TOP 1 
                               o.CashierID, 
                               (u.FirstName + ' ' + u.LastName) AS Cashier_Name, 
                               COUNT(DISTINCT o.OrderID) AS No_Of_Orders
                               FROM Orders o
                               JOIN Users u ON u.Id = o.CashierID
                               JOIN OrderDetails d ON d.OrderID = o.OrderID
                               GROUP BY o.CashierID, u.FirstName, u.LastName
                               ORDER BY No_Of_Orders DESC;";

            using (SqlCommand comm = new SqlCommand(cashier,conn))
            {
                SqlDataReader reader = comm.ExecuteReader();

                if (reader.Read()) 
                {
                    Console.WriteLine("Cashier With Max Number Of Orders:");
                    Console.WriteLine($"ID: {reader["cashierID"]}, Name: {reader["Cashier_Name"]}, No Of Orders: {reader["No_Of_Orders"]}");
                }
            }

                conn.Close();
        }

        public void cashierWithMaxRevenue()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string cashier = @"SELECT TOP 1 
                              o.CashierID, 
                              (u.FirstName + ' ' + u.LastName) AS Cashier_Name, 
                              SUM(d.Quantity * d.Price) AS Total_Revenue
                              FROM Orders o
                              JOIN Users u ON u.Id = o.CashierID
                              JOIN OrderDetails d ON d.OrderID = o.OrderID
                              GROUP BY o.CashierID, u.FirstName, u.LastName
                              ORDER BY Total_Revenue DESC;";

            using (SqlCommand comm = new SqlCommand(cashier, conn))
            {
                SqlDataReader reader = comm.ExecuteReader();

                if (reader.Read())
                {
                    Console.WriteLine("Cashier With Max Revenue:");
                    Console.WriteLine($"ID: {reader["cashierID"]}, Name: {reader["Cashier_Name"]}, Revenue: {reader["Total_Revenue"]}");
                }
            }

            conn.Close();
        }

        public void noOfOrdersCashier()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string orderCashier = @"SELECT 
                                    o.CashierID, 
                                    (u.FirstName + ' ' + u.LastName) AS Cashier_Name, 
                                    o.OrderID,
                                    p.ProductName, 
                                    d.Quantity, 
                                    d.Price, 
                                    (d.Quantity * d.Price) AS Total_Price,
                                    COUNT( o.OrderID) OVER (PARTITION BY o.CashierID) AS No_Of_Orders
                                    FROM Orders o
                                    JOIN Users u ON u.Id = o.CashierID
                                    JOIN OrderDetails d ON d.OrderID = o.OrderID
                                    JOIN Product p ON p.ProductID = d.ProductID
                                    ORDER BY o.CashierID, o.OrderID;";

            using (SqlCommand comm = new SqlCommand(orderCashier, conn))
            {
                SqlDataReader reader = comm.ExecuteReader();

                Console.WriteLine("No Of Orders Per Each Cashier And Their Details:");
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["cashierID"]}, Name: {reader["Cashier_Name"]}, OrderID: {reader["orderID"]}, ProductName: {reader["ProductName"]}, Quantity: {reader["Quantity"]}, " +
                                      $"Price: {reader["price"]}, Total Price: {reader["Total_Price"]}, No Of Orders: {reader["No_Of_Orders"]}");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                }
            }

                conn.Close();
        }

        public void customerWithMaxOrders()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string customer = @"SELECT TOP 1 
                               o.customerID, 
                               (u.FirstName + ' ' + u.LastName) AS Customer_Name, 
                               COUNT(DISTINCT o.OrderID) AS No_Of_Orders
                               FROM Orders o
                               JOIN Users u ON u.Id = o.customerID
                               JOIN OrderDetails d ON d.OrderID = o.OrderID
                               GROUP BY o.customerID, u.FirstName, u.LastName
                               ORDER BY No_Of_Orders DESC;";

            using (SqlCommand comm = new SqlCommand(customer, conn))
            {
                SqlDataReader reader = comm.ExecuteReader();

                if (reader.Read())
                {
                    Console.WriteLine("Customer With Max Number Of Orders:");
                    Console.WriteLine($"ID: {reader["customerID"]}, Name: {reader["Customer_Name"]}, No Of Orders: {reader["No_Of_Orders"]}");
                }
            }

            conn.Close();
        }

        public void customerWithMaxRevenue()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string customer = @"SELECT TOP 1 
                              o.CustomerID, 
                              (u.FirstName + ' ' + u.LastName) AS Customer_Name, 
                              SUM(d.Quantity * d.Price) AS Total_Revenue
                              FROM Orders o
                              JOIN Users u ON u.Id = o.CustomerID
                              JOIN OrderDetails d ON d.OrderID = o.OrderID
                              GROUP BY o.CustomerID, u.FirstName, u.LastName
                              ORDER BY Total_Revenue DESC;";

            using (SqlCommand comm = new SqlCommand(customer, conn))
            {
                SqlDataReader reader = comm.ExecuteReader();

                if (reader.Read())
                {
                    Console.WriteLine("Customer With Max Revenue:");
                    Console.WriteLine($"ID: {reader["CustomerID"]}, Name: {reader["Customer_Name"]}, Revenue: {reader["Total_Revenue"]}");
                }
            }

            conn.Close();
        }

        public void noOfOrdersCustomer()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            string orderCashier = @"SELECT 
                                    o.CustomerID, 
                                    (u.FirstName + ' ' + u.LastName) AS Customer_Name, 
                                    o.OrderID,
                                    p.ProductName, 
                                    d.Quantity, 
                                    d.Price, 
                                    (d.Quantity * d.Price) AS Total_Price,
                                    COUNT( o.OrderID) OVER (PARTITION BY o.CashierID) AS No_Of_Orders
                                    FROM Orders o
                                    JOIN Users u ON u.Id = o.CustomerID
                                    JOIN OrderDetails d ON d.OrderID = o.OrderID
                                    JOIN Product p ON p.ProductID = d.ProductID
                                    ORDER BY o.CustomerID, o.OrderID;";

            using (SqlCommand comm = new SqlCommand(orderCashier, conn))
            {
                SqlDataReader reader = comm.ExecuteReader();

                Console.WriteLine("No Of Orders Per Each Customer And Their Details:");
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["customerID"]}, Name: {reader["Customer_Name"]}, OrderID: {reader["orderID"]}, ProductName: {reader["ProductName"]}, Quantity: {reader["Quantity"]}, " +
                                      $"Price: {reader["price"]}, Total Price: {reader["Total_Price"]}, No Of Orders: {reader["No_Of_Orders"]}");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
                }
            }

            conn.Close();
        }

    }

}

