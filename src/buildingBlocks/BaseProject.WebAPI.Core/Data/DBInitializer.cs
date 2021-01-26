using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace BaseProject.WebAPI.Core.Data
{
    public class DBInitializer<T> where T : DbContext
    {
        public void Initialize(T context)
        {
            try
            {
                Console.WriteLine("DBInitializer - Begin");
                int retries = 5;
                using var ctx = context;

                while (retries > 0)
                {
                    try
                    {
                        Console.WriteLine($"DBInitializer - Trying to connect. Trying number {retries}");

                        ctx.Database.Migrate();

                        Console.WriteLine($"DBInitializer - Migrations Applied.");
                        break;
                    }
                    catch (Exception ex)
                    {
                        retries--;
                        int secsForRetry = (5000 + (1000 * (retries - 6) * -1));
                        Console.WriteLine($"DBInitializer - Can't migrate, retries left: {retries}. Exception: {ex.Message}" );
                        Console.WriteLine($"DBInitializer - Trying again in {secsForRetry} seconds" );

                        Thread.Sleep(secsForRetry);
                    }
                }

                //var initialScripts = Query.InitialScripts;

                //ctx.Database.ExecuteSqlRaw(initialScripts);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Was not possible to create database. { ex.Message }");
                throw;
            }
        }
    }
}
