using System;

namespace BaseProject.Bots.WebScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando Scrapper");
            var getProducts = new GetProductsOnBluesoft();
            getProducts.AlimentosEBebidas();


        }
    }
}
