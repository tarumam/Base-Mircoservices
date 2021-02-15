using System;
using System.Collections.Generic;
using BaseProject.SearchProducts.Helper;
using OpenQA.Selenium;

namespace BaseProject.Bots.WebScrapper
{
    public class ProductInfo
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Barcode { get; set; }
        public string Sql { get; set; }
    }
    public class GetProductsOnBluesoft
    {
        public SeleniumHelper Browser;
        // Tabela com todos os itens  tbl-produtos
        // Pegar lista de LIs
        // buscar imagens na tag img com classe list-item-thumbnail. propriedade src
        // buscar nome na tag h5 com class description texto dentro do A
        // buscar barcode na tag span class barcode  texto dentro do 

        public GetProductsOnBluesoft()
        {
            Browser = SeleniumHelper.Instance();
        }

        public void AlimentosEBebidas()
        {

            var products = new List<ProductInfo>();
            bool hasNextPage = true;
            int page = 1, fileNumber = 1;

            while (hasNextPage)
            {
                //string url = @$"https://cosmos.bluesoft.com.br/categorias/alimentos-e-bebidas/produtos?page={page}";
                string url = $"https://cosmos.bluesoft.com.br/categorias/higiene-e-beleza/produtos?page={page}";
                Browser.NavigateToUrl(url);

                var tabela = Browser.GetElementById("tbl-produtos");
                var linhas = tabela.FindElements(By.TagName("li"));
                foreach (var linha in linhas)
                {
                    try
                    {
                        var prod = new ProductInfo();
                        prod.Image = linha.FindElement(By.TagName("img")).GetAttribute("src");
                        prod.Name = linha.FindElement(By.ClassName("description")).FindElement(By.TagName("a")).Text;
                        prod.Barcode = linha.FindElement(By.ClassName("barcode")).FindElement(By.TagName("a")).Text;
                        prod.Sql = @$"INSERT INTO public.""Product"" (""Id"", ""Barcode"", ""Name"", ""Description"", ""Active"", ""Image"", ""CreatedAt"", ""UpdatedAt"", ""SyncWithWeb"")
                                 VALUES('{Guid.NewGuid()}', '{prod.Barcode}', '{prod.Name}', null, true, '{prod.Image}', current_date, current_date, false);";
                        products.Add(prod);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@$"productsExtracted-{fileNumber}.txt", true))
                {
                    foreach (var item in products)
                    {
                        file.WriteLine(item.Sql);
                    }
                }
                if (page % 50 == 0)
                {
                    fileNumber++;
                }
                Console.WriteLine(page);
                page++;
            }
        }
    }
}
