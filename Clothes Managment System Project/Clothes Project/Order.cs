using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothes_Project
{
    internal class Order
    {
        public int orderID { get; set; }
        public int customerID { get; set; }
        public int cashierID { get; set; }
        public DateTime orderDate { get; set; }
        public float totalOrder { get; set; }

        public List<orderDetails> items = new List<orderDetails> ();

        public Order() { }

        public Order(int customerID, int cashierID, float totalOrder) { 
            this.customerID = customerID;
            this.cashierID = cashierID;
            this.orderDate = DateTime.Now;
            this.totalOrder = totalOrder;
        }


    }
}
