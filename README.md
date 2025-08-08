Clothes Management System

Overview: 
The Clothes Management System is a comprehensive C# application designed to manage a clothing store's operations. It provides functionality for administrators, cashiers, and customers, with features ranging from user management to order processing and sales analytics.


Features

User Management

Admin Functions:

Add, edit, delete, and search for admins, cashiers, and customers

List all users in each category

Cashier Functions:

Process orders

Delete orders (with authorization checks)

Calculate payments

Customer Functions:

View order details

Rate orders

Product Management
Add, edit, delete, and search for products

List all available products

Track product availability and quantities

Order Management
Create new orders with multiple products

View detailed order information

Process payments

Analytics
Revenue tracking (total and average over specific periods)

Best-selling product analysis

Customer and cashier performance metrics

Supplier pricing information

Technical Details
Database
SQL Server database with tables for:

Users (with role differentiation)

Products

Orders and OrderDetails

Admin, Cashier, and Customer relationships

Architecture
Object-oriented design with classes for:

Users (base class with Admin, Cashier, and Customer subclasses)

Products

Orders and OrderDetails

SQL connection management with proper parameterization to prevent SQL injection

Validation
Input validation for:

Numeric values

Dates

Gender selection

Product categories and sizes

Installation
Prerequisites:

.NET Framework 4.7.2

SQL Server with the database schema set up

Setup:

Clone the repository

Update the connection string in all classes to point to your SQL Server instance

Ensure the database contains the necessary tables (Users, Admin, Cashier, Customer, Product, Orders, OrderDetails)

Running:

Build the solution in Visual Studio

Run the executable

Usage
Login:

Users must log in with valid credentials

System automatically detects user role (Admin, Cashier, or Customer)

Role-based Access:

Admins have full access to all features

Cashiers can process orders and payments

Customers can view their orders and provide ratings

Code Structure
Admin.cs: Contains all admin-specific functionality

Cashier.cs: Handles order processing and payment calculations

Customer.cs: Manages customer order views and ratings

Order.cs & OrderDetails.cs: Define order data structures

Product.cs: Defines product information

User.cs: Base class for all user types

Program.cs: Main entry point with login and menu systems
