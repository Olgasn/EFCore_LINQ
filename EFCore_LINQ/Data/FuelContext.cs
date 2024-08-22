using EFCore_LINQ.Models;
using Microsoft.Data.SqlClient;
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

            ///Установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            IConfigurationRoot configuration = builder.AddUserSecrets<Program>().Build();

            /// Получаем строку подключения
            string connectionString = "";
            //Вариант для Sqlite
            //connectionString = configuration.GetConnectionString("SqliteConnection");

            //Вариант для локального SQL Server
            connectionString = configuration.GetConnectionString("SQLConnection");

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
            _ = optionsBuilder
                .UseSqlServer(connectionString)
                //.UseSqlite(connectionString)
                .Options;
            optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));


        }
    }
}
