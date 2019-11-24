﻿﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace lab2
{

    class Program
    {
        static void Main(string[] args)
        {
            Catalogue catalogue = new Catalogue();
            catalogue.ReadFromFile(/*"/Users/napenshe/RiderProjects/OOP/lab2/input.txt"*/ "C:/Users/timur/RiderProjects/lab2/lab2/input.txt");
            catalogue.LoadGenres(/*"/Users/napenshe/RiderProjects/OOP/lab2/genres.txt"*/ "C:/Users/timur/RiderProjects/lab2/lab2/genres.txt");
            
            catalogue.PrintCatalogue();
            catalogue.RequestHandler();
        }
    }
}