using System;
using System.Collections.Generic;
using System.IO;

namespace Lab4
{
    class FileDAO : Resource
    {
        private StreamWriter writer;
        private StreamReader reader;
        private String storesPath;
        private String productsPath;

        private Int32 id = 0;

        public FileDAO(String storesPath, String productsPath)
        {
            this.productsPath = $"..\\{productsPath}";
            this.storesPath = $"..\\{storesPath}";
        }

        Int32 GetStoreID(String name)
        {
            Int32 id = -1;
            reader = File.OpenText(storesPath);
            String line;

            while ((line = reader.ReadLine()) != null)
            {
                String[] lines = line.Split(':');
                lines[2].Trim();
                if (lines[2].Equals(name.ToLower())) id = Int32.Parse(lines[1]);
            }

            reader.Close();
            return id;
        }

        public override void CreateStore(String name)
        {
            CreateStoreID(name);
        }
        
        public Int32 CreateStoreID(String name)
        {
            Int32 storeID = GetStoreID(name);
            writer = File.AppendText(storesPath);
            if (storeID == -1)
            {
                writer.WriteLine($"{id} : {name.ToLower()}");
                storeID = id;
                id++;
            }

            writer.Close();
            return storeID;
        }

        public Boolean ProductExistence(String productName, Int32 storeID)
        {
            reader = File.OpenText(productsPath);
            String str;
            Boolean result = false;
            while ((str = reader.ReadLine()) != null)
            {
                string[] lines = str.Split(':');
                lines[0].Trim();
                lines[1].Trim();
                if (Int32.Parse(lines[0]) == storeID && lines[1].Equals(productName.ToLower()))
                {
                    result = true;
                    break;
                }
            }

            reader.Close();
            return result;
        }

        public override void CreateProduct(String productName, String store, Double price)
        {
            CreateProduct(productName, store, 0, price);
        }

        public void CreateProduct(String productName, String store, Int32 amount, Double price)
        {
            Int32 storeID = CreateStoreID(store);

            if (ProductExistence(productName, storeID)) return;

            writer = File.AppendText(productsPath);
            writer.WriteLine($"{storeID} : {productName.ToLower()} : {amount} : {price}");
            writer.Close();
        }


        public override void AddConsignment(List<Consignment> consignments, String store)
        {
            Int32 storeID = GetStoreID(store);
            foreach (var consignment in consignments)
            {
                if (storeID == -1 || !ProductExistence(consignment.productName, GetStoreID(store)))
                    CreateProduct(consignment.productName, store, consignment.amount, consignment.price);
                else
                {
                    reader = File.OpenText(storesPath);
                    List<String> fileData = new List<String>();

                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        String[] lines = line.Split(':');
                        if (Int32.Parse(lines[1].Trim()) == storeID &&
                            lines[1].Trim().Equals(consignment.productName.ToLower().Trim()))
                        {
                            Int32 number = Int32.Parse(lines[2].Trim()) + consignment.amount;
                            Double price = Double.Parse(lines[3].Trim());
                            fileData.Add($"{storeID} : {lines[1]} : {number} : {price}");
                        }
                        else fileData.Add(line);
                    }
                    reader.Close();
                    
                    writer = new StreamWriter(productsPath, false);
                    foreach (var x in fileData)
                    {
                        writer.WriteLine(x);
                    }
                    writer.Close();
                }
            }
        }

        public override void DecreaseAmount(String productName, String store, Int32 amount)
        {
            Int32 id = GetStoreID(store);
            reader = File.OpenText(productsPath);
            List<string> fileData = new List<string>();

            String line;
            while ((line = reader.ReadLine()) != null)
            {
                String[] lines = line.Split(':');
                lines[0].Trim();
                if (Int32.Parse(lines[0]) == id && lines[1].Trim().Equals(productName.ToLower()))
                    fileData.Add(
                        $"{id} : {lines[1].Trim()} : {Int32.Parse(lines[2].Trim()) - amount} : {lines[3].Trim()}");
                else fileData.Add(line);
            }

            reader.Close();

            writer = new StreamWriter(productsPath, false);
            foreach (var x in fileData)
            {
                writer.WriteLine(x);
            }

            writer.Close();
        }

        public override List<String> GetListOfStores()
        {
            List<String> result = new List<String>();
            reader = File.OpenText(storesPath);

            String line;
            while ((line = reader.ReadLine()) != null)
            {
                String[] lines = line.Split(':');
                result.Add(lines[1].Trim());
            }
            reader.Close();
            return result;
        }
        
        private String GetStoreById(Int32 id)
        {
            reader = File.OpenText(storesPath);
            String line;
            while ((line = reader.ReadLine()) != null)
            {
                String[] lines = line.Split(':');
                lines[0].Trim();

                if (Int32.Parse(lines[0]) == id) return lines[1].Trim();
            }

            return null;
        }

        private List<String> GetStoresNamesById(List<Int32> id)
        {
            List<String> result = new List<String>();
            foreach (var useID in id)
            {
                result.Add(GetStoreById(useID));
            }

            return result;
        }
        
        public override List<String> FindStoresWithSmallestPriceForProduct(String productName)
        {
            List<String> data = new List<String>();
            reader = File.OpenText(productsPath);

            String line;
            while ((line = reader.ReadLine()) != null)
            {
                String[] lines = line.Split(':');
                lines[1].Trim();
                if (lines[1].Equals(productName.ToLower()))
                    data.Add($"{GetStoreById(Int32.Parse(lines[0].Trim()))}${lines[1]}");
            }

            reader.Close();

            return data;
        }

        public override List<String> GetProductsInfo(String store)
        { 
            Int32 storeId = GetStoreID(store);
            reader = File.OpenText(productsPath);
            String line;

            List<String> info = new List<String>();
            while ((line = reader.ReadLine()) != null)
            {
                string[] lines = line.Split(':');
                if (storeId == Int32.Parse(lines[0].Trim()))
                    info.Add($"{lines[1].Trim()}${lines[3].Trim()}${lines[2].Trim()}");
            }

            reader.Close();
            return info;
        }
    }
}