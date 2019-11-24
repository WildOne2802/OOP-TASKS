using System;
using System.Collections.Generic;

namespace Lab4
{
    public abstract class Resource
    {
        public abstract void CreateStore(String name);

        public abstract void CreateProduct(String productName, String store, Double price);

        public abstract void AddConsignment(List <Consignment> consignments, String store);

        public abstract void DecreaseAmount(String productName, String store, Int32 amount);

        public abstract List<String> FindStoresWithSmallestPriceForProduct(String productName);

        public abstract List<String> GetListOfStores();

        public abstract List<String> GetProductsInfo(String store);
    }
}