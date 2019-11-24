using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4
{
     class Service
    {
        private Resource resource;

        public Service(Boolean DBUsage)
        {
            Connector connector;
            if (DBUsage)
            {
                connector = new DBConnector();
                resource = connector.Create();
            }
            else
            {
                connector = new FileConnector();
                resource = connector.Create();
            }
        }

        String Format(String line)
        {
            line.Trim();
            return Char.ToUpper(line[0]) + line.Substring(1).ToLower();
        }

        String DBFormat(String line)
        {
            return line.ToLower();
        }

        Consignment GetConsignment(List<String> storeStock, String productName)
        {
            Consignment consignment = null;
            foreach (var x in storeStock)
            {
                String[] data = x.Split("$");
                if (Format(data[0]).Equals(Format(productName)))
                {
                    Double price = Double.Parse(data[1]);
                    Int32 amount = Int32.Parse(data[2]);

                    consignment = new Consignment(data[0], amount, price);
                    break;
                }
            }

            return consignment;
        }

        public void CreateStore(String name)
        {
            resource.CreateStore(DBFormat(name));
        }

        public void CreateProduct(String productName, String store, Double price)
        {
            resource.CreateProduct(DBFormat(productName), DBFormat(store), price);
        }

        public void AddConsignment(List<Consignment> consignments, String store)
        {
            resource.AddConsignment(consignments, store);
        }

        public List<String> FindStoresWithSmallestPriceForProduct(String productName)
        {
            List<String> prices = resource.FindStoresWithSmallestPriceForProduct(productName);
            List<String> stores = new List<String>();

            Double lowestPrice = prices.Min(x => Double.Parse(x.Substring(x.IndexOf('$') + 1)));

            foreach (var price in prices)
            {
                String[] info = price.Split("$");
                if (lowestPrice == Double.Parse(info[1]))
                {
                    stores.Add(Format(info[0]));
                }
            }

            return stores;
        }

        public List<Product> GetAmountOfProductICanBuy(Double cash, String store)
        {
            List<String> sqlInfo = resource.GetProductsInfo(store);
            List<Product> products = new List<Product>();

            foreach (var line in sqlInfo)
            {
                String[] infoSet = line.Split("$");
                Double price = Double.Parse(infoSet[1].Trim());
                String productName = Format(infoSet[0]);

                Int32 amount = (Int32) Math.Floor(cash / price);

                if (amount > 0)
                    products.Add(new Product(productName, amount));
            }

            if (products.Count == 0)
                throw new EmptyResponseException();

            return products;
        }

        public Double BuyConsignment(List<Product> products, String store)
        {
            List<String> productsInfo = resource.GetProductsInfo(DBFormat(store));
            Double summ = 0.0;

            List<Int32> boughtAmount = new List<Int32>();

            foreach (var product in products)
            {
                Consignment consignmentInfo = GetConsignment(productsInfo, product.Product_);
                if (consignmentInfo == null) throw new EmptyResponseException();
                if (consignmentInfo.amount < product.Amount_) throw new NotEnoughException();

                summ += consignmentInfo.price * product.Amount_;
                boughtAmount.Add(product.Amount_);
            }

            for (int i = 0; i < products.Count; i++)
                resource.DecreaseAmount(DBFormat(products[i].Product_), DBFormat(store), boughtAmount[i]);

            return summ;
        }


        public List<String> FindCheapestStores(List<Product> products)
        {
            List<String> stores = resource.GetListOfStores();
            List<Pair> pairs = new List<Pair>();

            foreach (var store in stores)
            {
                try
                {
                    pairs.Add(new Pair(BuyConsignment(products, store), store));
                }
                catch (Exception)
                {
                    continue;
                }
            }

            Double minSum = pairs.Min(x => x.price);

            List<String> result = new List<String>();
            
            foreach (var pair in pairs)
            {
                if (pair.price == minSum) result.Add(Format(pair.store));
            }

            if (result.Count == 0) throw new EmptyResponseException();

            return result;
        }
    }
}