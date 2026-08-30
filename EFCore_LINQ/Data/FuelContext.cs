using EFCore_LINQ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EFCore_LINQ.Data
{
    public class FuelContext : DbContext
    {

        public DbSet<Fuel> Fuels => Set<Fuel>();
        public DbSet<Operation> Operations => Set<Operation>();
        public DbSet<Tank> Tanks => Set<Tank>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            var builder = new ConfigurationBuilder();

            ///Установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            var configuration = builder.AddUserSecrets<Program>(optional: true).Build();

            /// Получаем строку подключения
            string connectionString = configuration.GetConnectionString("SQLConnection")
                ?? throw new InvalidOperationException("Не найдена строка подключения 'SQLConnection'.");
            //Вариант для Sqlite
            //connectionString = configuration.GetConnectionString("SqliteConnection");

            //Вариант для локального SQL Server

            ////Вариант для удаленного SQL Server
            ////Считываем пароль и имя пользователя из secrets.json
            //string secretPass = configuration["Database:password"];
            //string secretUser = configuration["Database:login"];
            //SqlConnectionStringBuilder sqlConnectionStringBuilder = new(configuration.GetConnectionString("RemoteSQLConnection"))
            //{
            //    Password = secretPass,
            //    UserID= secretUser
            //};
            //connectionString = sqlConnectionStringBuilder.ConnectionString;

            /// Задание опций подключения
            optionsBuilder.UseSqlServer(connectionString);
            // optionsBuilder.UseSqlite(connectionString);
            optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Operation>().HasIndex(o => new { o.Date, o.FuelID });
            modelBuilder.Entity<Operation>().HasIndex(o => new { o.TankID, o.FuelID });
            modelBuilder.Entity<Tank>().HasIndex(t => t.TankType);
            modelBuilder.Entity<Fuel>().HasIndex(f => f.FuelType);
        }
    }
}
