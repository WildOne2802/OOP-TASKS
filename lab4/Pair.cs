using System;

namespace Lab4
{
    public struct Pair
    {
        public readonly Double price;
        public readonly String store;

        public Pair(Double price, String store)
        {
            this.price = price;
            this.store = store;
        }

    }
}