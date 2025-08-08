using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothes_Project
{
    internal class orderDetails
    {
        public int orderDetailsId { get; set; }
        public int orderId { get; set; }
        public int orderQuantity { get; set; }
        public int productId { get; set; }  
        public decimal price { get; set; }
        public float subtotal => (float)(price * orderQuantity);

        public orderDetails() { }

        public orderDetails(int orderId, int orderQuantity, int productId) 
        { 
            this.orderId = orderId;
            this.orderQuantity = orderQuantity;
            this.productId = productId;
        }

    }
}
