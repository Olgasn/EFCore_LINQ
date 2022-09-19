using EFCore_LINQ.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EFCore_LINQ.Data
{
    public class FuelContext : DbContext
    {

        public DbSet<Fuel> Fuels { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Tank> Tanks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationBuilder builder = new();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            IConfigurationRoot config = builder.Build();
            // получаем строку подключения
            //string connectionString = config.GetConnectionString("SqliteConnection");
            string connectionString = config.GetConnectionString("SQLConnection");
            _ = optionsBuilder
                .UseSqlServer(connectionString)
                //.UseSqlite(connectionString)
                .Options;
            optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));


        }
    }
}
