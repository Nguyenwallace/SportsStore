using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
//this injection create scope
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SportsStore.Models;

namespace SportsStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host= CreateHostBuilder(args).Build();
            var scope = host.Services.CreateScope();
            
            //Get Application Data
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedData.EnsurePopulated(dbContext);

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            IdentitySeedData.EnsurePopulated(userManager);

            host.Run();
           
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
