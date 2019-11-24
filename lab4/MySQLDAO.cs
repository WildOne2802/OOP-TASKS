using System;
using System.Collections.Generic;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace Lab4
{
    class MySQLDAO : Resource
    {
        private MySqlConnection connection;

        public MySQLDAO(String host, Int32 port, String database, String username, String password)
        {
            this.connection =
                new MySqlConnection(
                    $"Server={host}; Database={database}; Port={port}; User Id={username}; Password={password}");

            connection.Open();
        }

        public void Disconnect()
        {
            connection.Close();
        }

        Boolean Check(String command)
        {
            MySqlCommand sqlCommand = new MySqlCommand(command, connection);
            Int32 amount = Int32.Parse(sqlCommand.ExecuteScalar().ToString());
            return amount != 0;
        }

        Int32 GetStoreID(String store)
        {
            String command = $"SELECT id FROM sqllab4.stores WHERE name = '{store}'";
            MySqlCommand sqlCommand = new MySqlCommand(command, connection);
            Int32 id = Int32.Parse(sqlCommand.ExecuteScalar().ToString());
            return id;
        }

        Boolean CheckStore(String store)
        {
            return Check($"SELECT COUNT(name) FROM sqllab4.stores WHERE name = '{store}'");
        }

        Boolean CheckProduct(String productName, String store)
        {
            if (!CheckStore(store)) return false;
            Int32 id = GetStoreID(store);
            return Check(
                $"SELECT COUNT(name) FROM sqllab4.products WHERE name = '{productName}' AND store_id = {id}");
        }

        public override void CreateStore(String name)
        {
            if (!Check($"SELECT COUNT(name) FROM sqllab4.stores WHERE name = '{name}'"))
            {
                String command = $"INSERT INTO sqllab4.stores (name) VALUE('{name}') ";
                MySqlCommand mySqlCommand = new MySqlCommand(command, connection);
                mySqlCommand.ExecuteNonQuery();
            }
        }

        public override void CreateProduct(String productName, String store, Double price)
        {
            if (!CheckStore(store))
            {
                CreateStore(store);
                CreateProduct(productName, store, price);
            }

            else

            {
                if (CheckProduct(productName, store))
                    return;
                Int32 id = GetStoreID(store);
                String command =
                    $"INSERT INTO sqllab4.products (name, store_id, amount, price)  VALUES ('{productName}', {id}, 0, {price.ToString("F2", CultureInfo.InvariantCulture)})";

                MySqlCommand mySqlCommand = new MySqlCommand(command, connection);
                mySqlCommand.ExecuteNonQuery();
            }
        }

        public override void AddConsignment(List<Consignment> consignments, String store)
        {
            foreach (var consignment in consignments)
            {
                if (!CheckStore(store) || !CheckProduct(consignment.productName, store))
                {
                    CreateProduct(consignment.productName, store, consignment.price);
                    AddConsignment(consignments, store);
                }
                else
                {
                    Int32 id = GetStoreID(store);
                    String command = $"UPDATE sqllab4.products SET number = {consignment.amount} + " +
                                     $"amount, price = {consignment.price.ToString("F2", CultureInfo.InvariantCulture)} WHERE store_id = {id} AND name = '{consignment.productName}'";

                    MySqlCommand mySqlCommand = new MySqlCommand(command, connection);
                    mySqlCommand.ExecuteNonQuery();
                }
            }
        }

        public override void DecreaseAmount(String productName, String store, Int32 amount)
        {
            Int32 id = GetStoreID(store);
            String command =
                $"UPDATE sqllab4.products SET amount = amount - {amount} WHERE store_id = {id} AND name = '{productName}'";

            MySqlCommand mySqlCommand = new MySqlCommand(command, connection);
            mySqlCommand.ExecuteNonQuery();
        }

        public override List<String> GetListOfStores()
        {
            String command = "SELECT name FROM sqllab4.stores";

            MySqlCommand mySqlCommand = new MySqlCommand(command, connection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            List<String> stores = new List<String>();
            while (reader.Read())
                stores.Add(reader[0].ToString());

            reader.Close();
            return stores;
        }

        public override List<String> FindStoresWithSmallestPriceForProduct(String productName)
        {
            String command =
                $"SELECT store.name, products.price FROM sqllab4.stores AS store INNER JOIN sqllab4.products AS products ON store.id = products.store_id WHERE products.name = '{productName}'";
            MySqlCommand mySqlCommand = new MySqlCommand(command, connection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            List<String> data = new List<String>();
            while (reader.Read())
                data.Add(reader[0].ToString() + "$" + reader[1].ToString());

            reader.Close();
            return data;
        }

        public override List<String> GetProductsInfo(String store)
        {
            Int32 id = GetStoreID(store);
            String command =
                $"SELECT name, price, amount FROM sqllab4.products WHERE store_id = {id}";

            MySqlCommand mySqlCommand = new MySqlCommand(command, connection);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            List<String> data = new List<String>();
            while (reader.Read())
                data.Add(reader[0].ToString() + "$" + reader[1].ToString() + "$" + reader[2].ToString());

            reader.Close();
            return data;
        }
    }
}