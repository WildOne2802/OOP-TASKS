using System;
using System.Collections.Generic;

namespace Lab4
{
    public class Client
    {
        private readonly Service _service;

        public Client(bool useDb)
        {
            this._service = new Service(useDb);
        }

        void Show(List<String> lines, String title)
        {
            Console.WriteLine($" ==== {title.ToUpper()} ==== ");
            foreach (var line in lines) Console.WriteLine(line);
        }

        void Show(List<Product> lines, String title)
        {
            Console.WriteLine($" ==== {title.ToUpper()} ==== ");
            Console.WriteLine("[name : count]");
            foreach (var line in lines) Console.WriteLine($"{line.Product_} : {line.Amount_}");
        }

        public void AddStores()
        {
            _service.CreateStore("7Я");
            _service.CreateStore("kek");
            _service.CreateStore("Spar");
            _service.CreateStore("Магнит");
            _service.CreateStore("Окей");
        }

        public void AddProducts()
        {
            _service.CreateProduct("Молоко", "7я",  39.99);
            _service.CreateProduct("Молоко", "Окей",  42.99);
            _service.CreateProduct("Молоко", "Магнит",  39.69);
            _service.CreateProduct("Молоко", "Карусель",  39.99);
            
            _service.CreateProduct("Вода", "7я",  139.99);
            _service.CreateProduct("Вода", "Окей",  12.99);
            _service.CreateProduct("Вода", "Магнит", 35);
            _service.CreateProduct("Вода", "Spar",  37.23);
        }

        public void AddConsignment()
        {
            Consignment pr1 = new Consignment("Молоко", 120, 40);
            Consignment pr2 = new Consignment ("Вода", 78, 19);
            Consignment pr3 = new Consignment("Мороженное", 23, 140);
            List<Consignment> consignment1 = new List<Consignment>() {pr1, pr2, pr3};
            _service.AddConsignment(consignment1, "7я");
            
            Consignment pr4 = new Consignment("Молоко", 120, 59);
            Consignment pr5 = new Consignment("Вода", 78,  27.66);
            List<Consignment> consignment2 = new List<Consignment>() {pr4, pr5};
            _service.AddConsignment(consignment2, "Окей");
            
            Consignment pr6 = new Consignment("Молоко", 120, 92);
            Consignment pr7 = new Consignment("Вода", 78,  19.77);
            Consignment pr8 = new Consignment ("Мороженное", 23, 133);
            List<Consignment> consignment3 = new List<Consignment>() {pr6, pr7, pr8};
            _service.AddConsignment(consignment3, "Лента");
        }

        public void FindCheapestStoreForProduct()
        {
            Show(_service.FindStoresWithSmallestPriceForProduct("Молоко"), "Cheapest milk in");
            Show(_service.FindStoresWithSmallestPriceForProduct("Вода"), "Cheapest water in");
        }

        public void BuyingOportunity()
        {
            List<Product> canBuy = _service.GetAmountOfProductICanBuy(272.48, "Лента");
            Show(canBuy, "Buying on 272.48 rubles");
        }

        public void BuyShipment()
        {
            Product pr1 = new Product("молоко", 2);
            Product pr2 = new Product("вода", 7);
            Product pr3 = new Product("мороженное", 1);
            List<Product> consignment = new List<Product>() {pr1, pr2, pr3};
            
            Double price = _service.BuyConsignment(consignment, "лента");
            Console.WriteLine($"price for shipment is {price}");
        }

        public void CheapestStore()
        {
            Product pr1 = new Product("молоко", 2);
            Product pr2 = new Product("вода", 3);
            List<Product> consignment = new List<Product>() {pr1, pr2};

            List<String> stores = _service.FindCheapestStores(consignment);
            Show(stores, "cheapest store for shipment (2 packets of milk + 3 bottles of water)");
        }
    }
}