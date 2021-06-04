using AplicatieWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AplicatieWeb.Models
{
        public class DataContext : DbContext
        {
        public DataContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if(optionsBuilder.IsConfigured)
            //{
            //    var builder = new ConfigurationBuilder()
            //                            .SetBasePath(Directory.GetCurrentDirectory())
            //                            .AddJsonFile("appsettings.json");
            //    var configuration = builder.Build();
            //    optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"], builder => builder.EnableRetryOnFailure());
            //}
            //var builder = new ConfigurationBuilder()
            //                        .SetBasePath(Directory.GetCurrentDirectory())
            //                        .AddJsonFile("appsettings.json");
            //var configuration = builder.Build();
            //optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
        }


        public DataContext(DbContextOptions<DataContext> options)
    : base(options)
        {
        }

        public DbSet<Teste> Teste { get; set; }     
        public DbSet<Intrebari> Intrebari { get; set; }
        public DbSet<Rezultate> Rezultate { get; set; }

        public DbSet<AspNetUsers> AspNetUsers { get; set; }

    }
}
