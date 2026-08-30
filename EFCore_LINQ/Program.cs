using EFCore_LINQ.Data;
using EFCore_LINQ.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCore_LINQ
{
    public class Program
    {
        private const int RecordsToDisplay = 5;

        public static void Main()
        {
            using var db = new FuelContext();
            DbInitializer.Initialize(db);

            Pause("====== Будет выполнена выборка данных ========");
            Select(db, RecordsToDisplay);

            Pause("====== Будет выполнена вставка данных ========");
            Insert(db);
            Console.WriteLine("====== Выборка после вставки ========");
            Select(db, RecordsToDisplay);

            Pause("====== Будет выполнено обновление данных ========");
            Update(db);
            Console.WriteLine("====== Выборка после обновления ========");
            Select(db, RecordsToDisplay);

            Pause("====== Будет выполнено удаление данных ========");
            Delete(db);
            Console.WriteLine("====== Выборка после удаления ========");
            Select(db, RecordsToDisplay);
        }

        private static void Pause(string message)
        {
            Console.WriteLine($"{message} (нажмите любую клавишу)");
            Console.ReadKey(intercept: true);
        }

        private static void Print<T>(string comment, IEnumerable<T> items)
        {
            Console.WriteLine(comment);
            Console.WriteLine("Записи: ");

            foreach (var item in items)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();
            Console.ReadKey(intercept: true);
        }

        private static void Insert(FuelContext db)
        {
            var tank = new Tank
            {
                TankType = "Бочка",
                TankMaterial = "Дерево",
                TankVolume = 30,
                TankWeight = 100
            };
            var fuel = new Fuel
            {
                FuelType = "Нитроглицерин",
                FuelDensity = 3
            };
            var operation = new Operation
            {
                Tank = tank,
                Fuel = fuel,
                Inc_Exp = 1000,
                Date = DateTime.Now
            };

            // EF Core сохранит связанные сущности в правильном порядке.
            db.Operations.Add(operation);
            db.SaveChanges();
        }

        private static void Select(FuelContext db, int recordsNumber)
        {
            var yearStart = new DateTime(DateTime.Today.Year, 1, 1);
            var nextYearStart = yearStart.AddYears(1);

            // Запрос 1: выборка отсортированных записей из двух таблиц.
            var operationsQuery = from operation in db.Operations.AsNoTracking()
                                  join fuel in db.Fuels.AsNoTracking()
                                      on operation.FuelID equals fuel.FuelID
                                  where operation.Inc_Exp > 0
                                      && operation.Date >= yearStart
                                      && operation.Date < nextYearStart
                                  orderby operation.FuelID descending
                                  select new
                                  {
                                      Код_операции = operation.OperationID,
                                      Название_топлива = fuel.FuelType,
                                      Приход_Расход = operation.Inc_Exp,
                                      Месяц = operation.Date.Month
                                  };

            Print(
                "1. Результат выборки отсортированных записей из двух таблиц:\r\n",
                operationsQuery.Take(recordsNumber).ToList());

            // Запрос 2: группировка операций и суммирование количества топлива.
            var fuelTotalsQuery = from operation in db.Operations.AsNoTracking()
                                  where operation.Inc_Exp > 0
                                      && operation.Date >= yearStart
                                      && operation.Date < nextYearStart
                                  group operation.Inc_Exp by operation.FuelID into fuelGroup
                                  select new
                                  {
                                      Код_топлива = fuelGroup.Key,
                                      Количество_топлива = fuelGroup.Sum()
                                  };

            Print(
                "2. Результат группировки операций и суммирования:\r\n",
                fuelTotalsQuery.Take(recordsNumber).ToList());

            // Запрос 3: выборка отдельных полей из одной таблицы.
            var tanksQuery = from tank in db.Tanks.AsNoTracking()
                             orderby tank.TankID descending
                             select new
                             {
                                 Название_Емкости = tank.TankType,
                                 Материал_Емкости = tank.TankMaterial,
                                 Объем_Емкости = tank.TankVolume,
                                 Вес = tank.TankWeight
                             };

            Print(
                "3. Результат выборки отдельных полей из таблицы емкостей:\r\n",
                tanksQuery.Take(recordsNumber).ToList());
        }

        private static void Delete(FuelContext db)
        {
            const string tankType = "Бочка1";
            const string fuelType = "Нитроглицерин1";

            using var transaction = db.Database.BeginTransaction();

            // Удаление выполняется на стороне БД, без загрузки сущностей в память.
            db.Operations
                .Where(operation => operation.Tank.TankType == tankType
                    && operation.Fuel.FuelType == fuelType)
                .ExecuteDelete();
            db.Tanks.Where(tank => tank.TankType == tankType).ExecuteDelete();
            db.Fuels.Where(fuel => fuel.FuelType == fuelType).ExecuteDelete();

            transaction.Commit();
        }

        private static void Update(FuelContext db)
        {
            const string tankType = "Бочка";
            const string fuelType = "Нитроглицерин";

            using var transaction = db.Database.BeginTransaction();

            var tank = db.Tanks.FirstOrDefault(item => item.TankType == tankType);
            if (tank is not null)
            {
                tank.TankType = "Бочка1";
                tank.TankMaterial = "Дерево1";
            }

            var fuel = db.Fuels.FirstOrDefault(item => item.FuelType == fuelType);
            if (fuel is not null)
            {
                fuel.FuelType = "Нитроглицерин1";
            }

            if (tank is not null && fuel is not null)
            {
                db.Operations
                    .Where(operation => operation.TankID == tank.TankID
                        && operation.FuelID == fuel.FuelID)
                    .ExecuteUpdate(update => update.SetProperty(operation => operation.Inc_Exp, 0f));
            }

            db.SaveChanges();
            transaction.Commit();
        }
    }
}
