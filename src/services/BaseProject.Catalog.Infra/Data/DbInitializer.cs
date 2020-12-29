using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Catalog.Infra.Data
{
    public class DBInitializer
    {
        public static void Initialize(CatalogContext context)
        {
            try
            {
                using var ctx = context;
                
                ctx.Database.Migrate();

                if (ctx.Products.Any()) return;

                //var initialScripts = Query.InitialScripts;

                //ctx.Database.ExecuteSqlRaw(initialScripts);

                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
