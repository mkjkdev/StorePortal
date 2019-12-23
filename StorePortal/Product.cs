using System;
using System.Diagnostics.CodeAnalysis;

namespace FinalProj
{
    [Serializable]
    public class Product : IComparable<Product>
    {
        public String name { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
        public double dprice { get; set; }

        public Product()
        {
        }

        public int CompareTo(Product prod)
        {
            return string.Compare(this.name, prod.name, StringComparison.Ordinal);
        }

        override
        public string ToString()
        {
            return name;
        }
        //public String gsName
        //{
        //    get { return name; }
        //    set { this.name = value; }
        //}

        //public int gsQuantity
        //{
        //    get { return quantity; }
        //    set { this.quantity = value; }
        //}

        //public double gsPrice
        //{
        //    get { return price; }
        //    set { this.price = value; }
        //}
    }
}
