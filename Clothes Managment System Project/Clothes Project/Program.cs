using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Clothes_Project
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=localhost;Initial Catalog=Clothes Managment System;Integrated Security=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Connection is running");
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Sorry there are an error" + ex.Message);
                }
            }

            try
            {
                Console.WriteLine("==== Welcom To The Clothest System ====");
                Console.WriteLine("Please Enter Your Username");
                string username = Console.ReadLine();
                string un = username.ToLower();

                Console.WriteLine("Please Enter Your Password");
                string password = Console.ReadLine();

                int userid = -1;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT Id FROM Users WHERE UserName = @Username AND UserPassword = @Password";
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue(@"Username", un);
                        comm.Parameters.AddWithValue(@"Password", password);

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userid = Convert.ToInt32(reader["Id"]);
                            }
                            else
                            {
                                Console.WriteLine("Invalid credentials. Access denied.");
                                return;
                            }
                        }

                    }

                    string role = "";

                    string checkAdmin = @"Select Count(*) From Admin Where AdminId = @Id";
                    using (SqlCommand comm = new SqlCommand(checkAdmin, conn))
                    {
                        comm.Parameters.AddWithValue(@"Id", userid);
                        if ((int)comm.ExecuteScalar() > 0)
                        {
                            role = "Admin";
                        }
                    }

                    if (role == "")
                    {
                        string checkCashier = @"Select Count(*) From Cashier Where CashierID = @Id";
                        using (SqlCommand comm = new SqlCommand(checkCashier, conn))
                        {
                            comm.Parameters.AddWithValue(@"Id", userid);
                            if ((int)comm.ExecuteScalar() > 0)
                            {
                                role = "Cashier";
                            }
                        }
                    }

                    if (role == "")
                    {
                        string checkCustomer = @"Select Count(*) From Customer Where CustomerID = @Id";
                        using (SqlCommand comm = new SqlCommand(checkCustomer, conn))
                        {
                            comm.Parameters.AddWithValue(@"Id", userid);
                            if ((int)comm.ExecuteScalar() > 0)
                            {
                                role = "Customer";
                            }
                        }
                    }

                    if (role == "")
                    {
                        Console.WriteLine("User Not Found. Access Denied");
                        return;
                    }

                    Console.Clear();
                    Console.WriteLine($"Welcome {username}! Role: {role}");

                    switch (role)
                    {
                        case "Admin":
                            showAdminMenu();
                            break;

                        case "Cashier":
                            showCashierMenu();
                            break;

                        case "Customer":
                            showCustomerMenu();
                            break;


                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            void showAdminMenu()
            {
                Admin admin = new Admin();

                while (true)
                {
                    Console.WriteLine("\n=== Admin Menu ===");
                    Console.WriteLine("1. Add Admin");
                    Console.WriteLine("2. Delete Admin");
                    Console.WriteLine("3. Edit Admin");
                    Console.WriteLine("4. Search An Admin");
                    Console.WriteLine("5. List All Admins");

                    Console.WriteLine("6. Add Cashier");
                    Console.WriteLine("7. Delete Cashier");
                    Console.WriteLine("8. Edit Cashier");
                    Console.WriteLine("9. Search A Cashier");
                    Console.WriteLine("10. List All Cashiers");

                    Console.WriteLine("11. Add Customer");
                    Console.WriteLine("12. Delete Customer");
                    Console.WriteLine("13. Edit Customer");
                    Console.WriteLine("14. Search A Customer");
                    Console.WriteLine("15. List All Customers");

                    Console.WriteLine("16. Add Product");
                    Console.WriteLine("17. Delete Product");
                    Console.WriteLine("18. Edit Product");
                    Console.WriteLine("19. Search A Product");
                    Console.WriteLine("20. List All Products");

                    Console.WriteLine("21. View Order's Details");
                    Console.WriteLine("22. Total Revenue Over A Specific Period");
                    Console.WriteLine("23. Average Revenue Over A Specific Period");
                    Console.WriteLine("24. Pieces Sold Over A Specific Period");
                    Console.WriteLine("25. List Of Suppliers And Pricing");
                    Console.WriteLine("26. Best Seller Product Over A Specific Period");
                    Console.WriteLine("27. Most Revenue Product Over A Specific Period");

                    Console.WriteLine("28. Orders Per Each Cashier");
                    Console.WriteLine("29. Cashier With Most Number Of Orders");
                    Console.WriteLine("30. Cashier With Highest Revenue");

                    Console.WriteLine("31. Orders Per Customer");
                    Console.WriteLine("32. Customer With Most Number Of Orders");
                    Console.WriteLine("33. Customer With Highest Revenue");

                    Console.WriteLine("0. Exit");

                    Console.Write("\nEnter your choice: ");
                    string input = Console.ReadLine();

                    if (!int.TryParse(input, out int choice))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                        continue;
                    }

                    Console.Clear();

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Please Enter Admin First Name");
                            string Afname = Console.ReadLine();

                            Console.WriteLine("Please Enter Admin Last Name");
                            string Alname = Console.ReadLine();

                            admin.Username = Afname + "." + Alname + "@gmail.com";

                            Console.WriteLine("Please Enter Admin Password");
                            string Apass = Console.ReadLine();

                            char Agender = ReadGender("Please Enter Admin Gender (M / F)");

                            while (Agender != 'M' && Agender != 'F' && Agender != 'm' && Agender != 'f') 
                            {
                                Console.WriteLine("Invalid Input, Please Enter (M / F)");
                                Agender = ReadGender("Please Enter Admin Gender (M / F)");
                            }
                            int Aage = ReadInt("Please Enter Admin Age");

                            Admin admin1 = new Admin(Afname, Alname, admin.Username, Apass, Agender, Aage);
                            admin.addAdmin(admin1);
                            break;
                        
                        case 2:
                            int Adelete = ReadInt("Please Enter Admin ID You Want To Delete");
                            admin.deleteAdmin(Adelete);
                            break;
                        
                        case 3:
                            int Aedit = ReadInt("Please Enter Admin ID You Want To Edit");
                            admin.EditAdmin(Aedit);
                            break;
                        
                        case 4:
                            int Asearch = ReadInt("Please Enter Admin ID You Want To Search");
                            admin.SearchAnAdmin(Asearch);
                            break;
                        
                        case 5:
                            admin.ListAllAdmins();
                            break;

                        case 6:
                            Console.WriteLine("Please Enter Cashier First Name");
                            string Cfname = Console.ReadLine();

                            Console.WriteLine("Please Enter Cashier Last Name");
                            string Clname = Console.ReadLine();

                            Console.WriteLine("Please Enter Cashier User Name");
                            string Cuser = Console.ReadLine();

                            Console.WriteLine("Please Enter Cashier Password");
                            string Cpass = Console.ReadLine();

                            char Cgender = ReadGender("Please Enter Cashier Gender (M / F)");

                            while (Cgender != 'M' && Cgender != 'F' && Cgender != 'm' && Cgender != 'f')
                            {
                                Console.WriteLine("Invalid Input, Please Enter (M / F)");
                                Cgender = ReadGender("Please Enter Admin Gender (M / F)");
                            }

                            int Cage = ReadInt("Please Enter Cashier Age");

                            Cashier cashier = new Cashier(Cfname, Clname, Cuser, Cpass, Cgender, Cage);
                            admin.addCashier(cashier);
                            break;
                        
                        case 7:
                            int Cdelete = ReadInt("Please Enter Cashier ID You Want To Delete");
                            admin.deleteCashier(Cdelete);
                            break;
                        
                        case 8:
                            int Cedit = ReadInt("Please Enter Cashier ID You Want To Edit");
                            admin.EditCashier(Cedit);
                            break;
                        
                        case 9:
                            int Csearch = ReadInt("Please Enter Cashier ID You Want To Search");
                            admin.SearchACashier(Csearch);
                            break;
                        
                        case 10:
                            admin.ListAllCashiers();
                            break;

                        case 11:
                            Console.WriteLine("Please Enter Customer First Name");
                            string Custfname = Console.ReadLine();

                            Console.WriteLine("Please Enter Customer Last Name");
                            string Custlname = Console.ReadLine();

                            Console.WriteLine("Please Enter Customer User Name");
                            string Custuser = Console.ReadLine();

                            Console.WriteLine("Please Enter Customer Password");
                            string Custpass = Console.ReadLine();

                            char Custgender = ReadGender("Please Enter Customer Gender (M / F)");
                            while (Custgender != 'M' && Custgender != 'F' && Custgender != 'm' && Custgender != 'f')
                            {
                                Console.WriteLine("Invalid Input, Please Enter (M / F)");
                                Custgender = ReadGender("Please Enter Admin Gender (M / F)");
                            }

                            int Custage = ReadInt("Please Enter Customer Age");

                            Customer customer = new Customer(Custfname, Custlname, Custuser, Custpass, Custgender, Custage);
                            admin.addCustomer(customer);
                            break;
                        
                        case 12:
                            int Custdelete = ReadInt("Please Enter Customer ID You Want To Delete");
                            admin.deleteCustomer(Custdelete);
                            break;
                        
                        case 13:
                            int Custedit = ReadInt("Please Enter Customer ID You Want To Edit");
                            admin.EditCustomer(Custedit);
                            break;
                        
                        case 14:
                            int Custsearch = ReadInt("Please Enter Customer ID You Want To Search");
                            admin.SearchACustomer(Custsearch);
                            break;
                        
                        case 15:
                            admin.listAllCustomers();
                            break;

                        case 16:
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

                            Product product = new Product(Pname, Psize, Pcategory, Pquantity, Pprice, Pcolor, PAva, Psupp);
                            admin.addProduct(product);
                            break;
                        
                        case 17:
                            int Pdelete = ReadInt("Please Enter Product ID You Want To Delete");
                            admin.deleteProduct(Pdelete);
                            break;
                        
                        case 18:
                            int Pedit = ReadInt("Please Enter Product ID You Want To Edit");
                            admin.editProduct(Pedit);
                            break;
                        
                        case 19:
                            int Psearch = ReadInt("Please Enter Product ID You Want To Search");
                            admin.SearchAProduct(Psearch);
                            break;
                        
                        case 20:
                            admin.listAllProducts();
                            break;

                        case 21:
                            admin.viewOrderDetails();
                            break;
                        
                        case 22:

                            DateTime start = ReadDate("Please enter the start date (dd/MM/yyyy):");
                            DateTime end = ReadDate("Please enter the end date (dd/MM/yyyy):");

                            if (end < start)
                            {
                                Console.WriteLine("End date must be after start date.");
                            }
                            else
                            {
                                Console.WriteLine($"Start Date: {start:dd/MM/yyyy}");
                                Console.WriteLine($"End Date: {end:dd/MM/yyyy}");
                            }

                            Console.WriteLine("Total Revenue From " + start + " To " + end + " is: " + admin.totalRevenueOrders(start, end));
                            break;
                        
                        case 23:

                            DateTime Astart = ReadDate("Please enter the start date (dd/MM/yyyy):");
                            DateTime Aend = ReadDate("Please enter the end date (dd/MM/yyyy):");

                            if (Aend < Astart)
                            {
                                Console.WriteLine("End date must be after start date.");
                            }
                            else
                            {
                                Console.WriteLine("Average Revenue From " + Astart + " To " + Aend + " is: " + admin.averageRevenueOrders(Astart, Aend));
                            }
                            
                            break;
                        
                        case 24:
                            DateTime Pstart = ReadDate("Please enter the start date (dd/MM/yyyy):");
                            DateTime Pend = ReadDate("Please enter the end date (dd/MM/yyyy):");

                            if (Pend < Pstart)
                            {
                                Console.WriteLine("End date must be after start date.");
                            }
                            else
                            {
                                Console.WriteLine("Number Of Pecies Sold From " + Pstart + " To " + Pend + " is: " + admin.noOfPeciesSold(Pstart, Pend));
                            }                             
                            break;
                        
                        case 25:
                            admin.listOfSupplierPricing();
                            break;
                        
                        case 26:
                            DateTime Beststart = ReadDate("Please enter the start date (dd/MM/yyyy):");
                            DateTime Bestend = ReadDate("Please enter the end date (dd/MM/yyyy):");

                            if (Bestend < Beststart)
                            {
                                Console.WriteLine("End date must be after start date.");
                            }
                            else
                            {
                                Console.WriteLine($"Start Date: {Beststart:dd/MM/yyyy}");
                                Console.WriteLine($"End Date: {Bestend:dd/MM/yyyy}");
                            }
                            admin.bestSellerProduct(Beststart, Bestend);
                            break;
                        
                        case 27:
                            DateTime moststart = ReadDate("Please enter the start date (dd/MM/yyyy):");
                            DateTime mostend = ReadDate("Please enter the end date (dd/MM/yyyy):");

                            if (mostend < moststart)
                            {
                                Console.WriteLine("End date must be after start date.");
                            }
                            else
                            {
                                Console.WriteLine($"Start Date: {moststart:dd/MM/yyyy}");
                                Console.WriteLine($"End Date: {mostend:dd/MM/yyyy}");
                            }
                            admin.mostRevenueProduct(moststart, mostend);
                            break;

                        case 28:
                            admin.noOfOrdersCashier();
                            break;
                        
                        case 29:
                            admin.cashierWithMaxOrders();
                            break;
                       
                        case 30:
                            admin.cashierWithMaxRevenue();
                            break;

                        case 31:
                            admin.noOfOrdersCustomer();
                            break;
                        
                        case 32:
                            admin.customerWithMaxOrders();
                            break;
                        
                        case 33:
                            admin.customerWithMaxRevenue();
                            break;

                        case 0:
                            Console.WriteLine("Exiting...");
                            return;

                        default:
                            Console.WriteLine("Invalid option. Try again.");
                            break;
                    }

                    Console.WriteLine("\nPress any key to return to menu...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            void showCashierMenu()
            {
                Cashier cashier = new Cashier(); 

                bool exit = false;

                while (!exit)
                {
                    Console.WriteLine("\n===== Cashier Menu =====");
                    Console.WriteLine("1. Create Order");
                    Console.WriteLine("2. Delete Order");
                    Console.WriteLine("3. Calculate Payment");
                    Console.WriteLine("0. Exit");
                    Console.Write("Enter your choice: ");

                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            var order = GetOrderFromUser(); 
                            cashier.createOrder(order);
                            break;

                        case "2":
                            Console.Write("Enter Order ID to delete: ");
                            int deleteId = int.Parse(Console.ReadLine());

                            Console.Write("Enter Your Cashier ID: ");
                            int cashierId1 = int.Parse(Console.ReadLine());

                            cashier.deleteOrder(deleteId, cashierId1);
                            break;

                        case "3":
                            Console.Write("Enter Order ID to calculate payment: ");
                            int calcId = int.Parse(Console.ReadLine());

                            Console.Write("Enter Your Cashier ID: ");
                            int cashierId2 = int.Parse(Console.ReadLine());

                            cashier.calculatePayment(calcId, cashierId2);
                            break;

                        case "0":
                            exit = true;
                            Console.WriteLine("Exiting...");
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Try again.");
                            break;
                    }
                }
            }

            void showCustomerMenu()
            {
                Customer customer = new Customer(); 

                bool exit = false;

                while (!exit)
                {
                    Console.WriteLine("\n===== Customer Menu =====");
                    Console.WriteLine("1. View Order Details");
                    Console.WriteLine("2. Rate an Order");
                    Console.WriteLine("0. Exit");
                    Console.Write("Enter your choice: ");
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            Console.Write("Enter your Customer ID: ");
                            if (int.TryParse(Console.ReadLine(), out int customerId))
                            {
                                customer.viewOrderDetails(customerId);
                            }
                            else
                            {
                                Console.WriteLine("Invalid ID. Please enter a valid number.");
                            }
                            break;

                        case "2":
                            customer.rateOrder();
                            break;

                        case "0":
                            exit = true;
                            Console.WriteLine("Exiting...");
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }

            }

             Order GetOrderFromUser()
            {
                Order order = new Order();

                Console.Write("Enter Customer ID: ");
                order.customerID = int.Parse(Console.ReadLine());

                Console.Write("Enter Cashier ID: ");
                order.cashierID = int.Parse(Console.ReadLine());

                Console.Write("Enter number of products in this order: ");
                int itemCount = int.Parse(Console.ReadLine());

                order.items = new List<orderDetails>();

                for (int i = 0; i < itemCount; i++)
                {
                    Console.Write($"Enter Product ID #{i + 1}: ");
                    int productId = int.Parse(Console.ReadLine());

                    Console.Write("Enter quantity: ");
                    int qty = int.Parse(Console.ReadLine());

                    order.items.Add(new orderDetails
                    {
                        productId = productId,
                        orderQuantity = qty
                    });
                }

                order.totalOrder = 0; 

                return order;
            }

            int ReadInt(string message)
            {
                while (true)
                {
                    Console.Write(message);
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out int result))
                        return result;

                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

            float ReadFloat(string message)
            {
                while (true)
                {
                    Console.Write(message);
                    string input = Console.ReadLine();
                    if (float.TryParse(input, out float result))
                        return result;

                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }

             DateTime ReadDate(string prompt)
             {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();
                DateTime date;

                while (!DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    Console.WriteLine("Invalid format. Please enter the date in dd/MM/yyyy format:");
                    input = Console.ReadLine();
                }

                return date;
             }

            char ReadGender(string message)
            {
                while (true)
                {
                    Console.Write(message);
                    string input = Console.ReadLine();
                    if (char.TryParse(input, out char result))
                        return result;

                    Console.WriteLine("Invalid input. Please enter a valid character.");
                }
            }




















            //Admin a = new Admin(3, "Ahmed", "Mohamed", "Ahmed.Mohamed@gmail.com", "12345", 'M', 25);
            //Admin a3 = new Admin(4, "Ayman", "Sayed", "Ayman.Sayed@gmail.com", "14253", 'M', 30);
            ////Admin a2 = new Admin();
            ////a2.addAdmin(a);


            //
            //a2.addAdmin(a3);
            Cashier c1 = new Cashier("Samir", "Saad", "Samir.Saad@gmail.com", "Samir123456", 'M', 23);
            Cashier c2 = new Cashier("Kareem", "Salah", "Kareem.Salah@gmail.com", "Kareem123456", 'M', 21);


            Customer x1 = new Customer("Tarek", "Mahmoud", "Tarek.Mahmoud@gmail.com", "Tarek123456", 'M', 26);
            Customer x2 = new Customer("Salma", "Mahmoud", "Salma.Mahmoud@gmail.com", "Salma123456", 'F', 20);
            Customer x3 = new Customer("Amir", "Ziad", "Amir.Ziad@gmail.com", "Ziad123456", 'M', 30);



            Product p4 = new Product("Boot", "41", "Women", 5, 500, "Blue", true, "Bata");

            Admin a2 = new Admin();

            //a2.addCashier(c2);
            //a2.ListCashiers();
            //a2.EditCashier(10);
            //a2.ListAllAdmins();
            //a2.EditCustomer(16);
            //a2.addProduct(p4);
            //a2.SearchAProduct(3);
            //a2.editProduct(6);

            //Order order = new Order
            //{
            //    customerID = 20,
            //    cashierID = 10,
            //    orderDate = DateTime.Now,
            //    items = new List<orderDetails>
            //{
            //    new orderDetails
            //    {
            //        productId = 2,
            //        orderQuantity = 1
            //    },
            //}
            //};

            //c1.createOrder(order);

            DateTime startDate = new DateTime(2025, 8, 1);
            DateTime endDate = new DateTime(2025, 8, 5);

            ////Console.WriteLine("Total Revenue is: " + a2.totalAllRevenueOrders());
            ////Console.WriteLine("Average Revenue is: " + a2.averageRevenueOrders(startDate, endDate));
            ////Console.WriteLine("Total Revenue From " + startDate + " To " + endDate +" is: " + a2.totalRevenueOrders(startDate, endDate));
            //Console.WriteLine("Number Of Pecies Sold " + startDate + " To " + endDate + " is: " + a2.noOfPeciesSold(startDate, endDate));

            //c2.calculatePayment(2, 10);

            //a2.bestSellerProduct(startDate, endDate);

            //a2.customerWithMaxRevenue();
        }
    }
}
