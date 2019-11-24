using System;

namespace Lab4
{
    public class Consignment
    {
        public readonly String productName;
        public readonly Int32 amount;
        public readonly Double price;

        public Consignment(String productName, Int32 amount, Double price)
        {
            this.productName = productName.ToLower();
            this.amount = amount;
            this.price = price;
        }
    }
}