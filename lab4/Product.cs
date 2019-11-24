using System;

namespace Lab4
{
    class Product
    {
        public readonly String name;
        public readonly Int32 amount;

        public Product(String name, Int32 amount)
        {
            this.name = name;
            this.amount = amount;
        }

        public String Product_ => name;
        public Int32 Amount_ => amount;
    }
}