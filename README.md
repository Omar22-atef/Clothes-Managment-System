# 👕 Clothes Management System

## 📌 Overview
The **Clothes Management System** is a comprehensive **C# desktop application** for managing a clothing store's operations.  
It supports **Admins**, **Cashiers**, and **Customers**, providing tools for **user management**, **product handling**, **order processing**, and **sales analytics**.

---

## ✨ Features

### 👤 User Management

**Admin Functions:**
- Add, edit, delete, and search for admins, cashiers, and customers
- View a complete list of users in each role

**Cashier Functions:**
- Process customer orders
- Delete orders (with authorization checks)
- Calculate payments

**Customer Functions:**
- View their order details
- Rate completed orders

---

### 🛍 Product Management
- Add, edit, delete, and search for products
- List all available products
- Track stock quantities and availability

---

### 📦 Order Management
- Create new orders with multiple products
- View detailed order summaries
- Process payments securely

---

### 📊 Analytics & Reports
- Track total and average revenue for specific time periods
- Identify best-selling products
- Monitor cashier and customer activity
- View supplier pricing information

---

## 🛠 Technical Details

### 🗄 Database
**SQL Server** database with tables for:
- Users (role-based: Admin, Cashier, Customer)
- Products
- Orders
- OrderDetails

**Relationships:**
- Admin, Cashier, and Customer linked to a shared **Users** table
- Orders linked to both Customers and Cashiers

---

### 🏗 Architecture
- **Object-Oriented Design** with the following classes:
  - `User` (base class)
  - `Admin`, `Cashier`, `Customer` (derived classes)
  - `Product`
  - `Order` and `OrderDetails`
- **SQL Connection Management**:
  - Uses parameterized queries to prevent SQL injection
- **Validation** for:
  - Numeric values
  - Date formats
  - Gender selection
  - Product categories and sizes

---

## ⚙ Installation

### 📋 Prerequisites
- **.NET Framework 4.7.2**
- **SQL Server** with the ability to restore databases

### 🚀 Setup
1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/clothes-management-system.git
