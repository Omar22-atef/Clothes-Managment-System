using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothes_Project
{
    internal class Product
    {
        public int productID { get; set; }
        public string productName { get; set; }
        public string size { get; set; }
        public string category { get; set; }
        public int quantity { get; set; }
        public float price { get; set; }
        public string color { get; set; }
        public bool isAvaliable { get; set; }
        public string supplier { get; set; }

        public Product() { }

        public Product(string productName, string size, string category, int quantity, float price, string color, bool isAvaliable, string supplier)
        {
            this.productName = productName;
            this.size = size;
            this.category = category;
            this.quantity = quantity;
            this.price = price;
            this.color = color;
            this.isAvaliable = isAvaliable;
            this.supplier = supplier;
        }

    }
}
