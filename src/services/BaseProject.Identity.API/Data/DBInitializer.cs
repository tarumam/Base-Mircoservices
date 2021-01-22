using System;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.Identity.API.Data
{
    public class DBInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            try
            {
                Console.WriteLine("Trying to connect to database");
                int retries = 5;
                using var ctx = context;

                while (retries > 0)
                {
                    try
                    {
                        Console.WriteLine($"Trying to connect - retries left: {retries}");
                        ctx.Database.Migrate();
                        if (ctx.Users.Any()) return;
                        ctx.SaveChanges();
                        break;
                    }
                    catch (Exception ex)
                    {
                        retries--;
                        Thread.Sleep(5000);
                    }
                }


                //var initialScripts = Query.InitialScripts;

                //ctx.Database.ExecuteSqlRaw(initialScripts);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
