using EFCore_LINQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;

// Класс для инициализации базы данных тестовым набором записей.
namespace EFCore_LINQ.Data
{
    public static class DbInitializer
    {
        private const int TanksCount = 35;
        private const int FuelsCount = 35;
        private const int OperationsCount = 3000;

        public static void Initialize(FuelContext db)
        {
            db.Database.EnsureCreated();

            if (db.Fuels.Any())
            {
                Console.WriteLine("====== База данных уже инициализирована ========");
                return;
            }

            var random = new Random(1);
            var tanks = CreateTanks(random);
            var fuels = CreateFuels(random);
            var operations = CreateOperations(random, tanks, fuels);

            db.Tanks.AddRange(tanks);
            db.Fuels.AddRange(fuels);
            db.Operations.AddRange(operations);
            db.SaveChanges();

            Console.WriteLine("====== База данных инициализирована ========");
        }

        private static List<Tank> CreateTanks(Random random)
        {
            string[] tankPrefixes = ["Цистерна_", "Ведро_", "Бак_", "Фляга_", "Цистерна_"];
            string[] materials = ["Сталь", "Платина", "Алюминий", "ПЭТ", "Чугун", "Алюминий", "Сталь"];
            var tanks = new List<Tank>(TanksCount);

            for (var number = 1; number <= TanksCount; number++)
            {
                tanks.Add(new Tank
                {
                    TankType = tankPrefixes[random.Next(tankPrefixes.Length)] + number,
                    TankMaterial = materials[random.Next(materials.Length)],
                    TankWeight = 500 * (float)random.NextDouble(),
                    TankVolume = 200 * (float)random.NextDouble()
                });
            }

            return tanks;
        }

        private static List<Fuel> CreateFuels(Random random)
        {
            string[] fuelPrefixes = ["Нефть_", "Бензин_", "Керосин_", "Мазут_", "Спирт_"];
            var fuels = new List<Fuel>(FuelsCount);

            for (var number = 1; number <= FuelsCount; number++)
            {
                fuels.Add(new Fuel
                {
                    FuelType = fuelPrefixes[random.Next(fuelPrefixes.Length)] + number,
                    FuelDensity = 2 * (float)random.NextDouble()
                });
            }

            return fuels;
        }

        private static List<Operation> CreateOperations(
            Random random,
            List<Tank> tanks,
            List<Fuel> fuels)
        {
            var today = DateTime.Today;
            var operations = new List<Operation>(OperationsCount);

            for (var number = 1; number <= OperationsCount; number++)
            {
                operations.Add(new Operation
                {
                    Tank = tanks[random.Next(tanks.Count)],
                    Fuel = fuels[random.Next(fuels.Count)],
                    Inc_Exp = random.Next(200) - 100,
                    Date = today.AddDays(-number)
                });
            }

            return operations;
        }
    }
}
