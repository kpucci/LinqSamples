using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var cars = ProcessFuelFile("fuel.csv");
            var manufacturers = ProcessManufacturerFile("manufacturers.csv");

            FindBestCombined(cars, 10);
            FindBestCombinedByManufacturerAndYear(cars, 10, "BMW", 2016);
            FindBestCombinedByManufacturerHq(cars, manufacturers, 10, "BMW");
            FindBestFromEachManufacturer(cars, manufacturers, 2);
            FindBestFromEachManufacturerAndCountry(cars, manufacturers, 2);
            FindBestFromEachCountry(cars, manufacturers, 3);

        }

        private static void FindBestFromEachManufacturer(IEnumerable<Car> cars, IEnumerable<Manufacturer> manufacturers, int num)
        {
            Console.WriteLine($"\n Find top {num} cars in each manufacturer");

            // Method syntax
            var query =
                cars.GroupBy(c => c.Manufacturer.ToUpper())
                    .OrderBy(g => g.Key);

            // Query syntax
            //var query =
            //    from car in cars
            //    group car by car.Manufacturer.ToUpper() into m
            //    orderby m.Key
            //    select m;

            foreach (var result in query)
            {
                Console.WriteLine("\n" + result.Key);
                foreach (var car in result.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }
        }

        private static void FindBestFromEachManufacturerAndCountry(IEnumerable<Car> cars, IEnumerable<Manufacturer> manufacturers, int num)
        {
            Console.WriteLine($"\n Find top {num} cars in each manufacturer");

            // Method syntax
            var query =
                manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) =>
                    new
                    {
                        Manufacturer = m,
                        Cars = g
                    })
                .OrderBy(m => m.Manufacturer.Name);

            // Query syntax
            //var query =
            //    from m in manufacturers
            //    join car in cars on m.Name equals car.Manufacturer
            //        into carGroup
            //    orderby m.Name
            //    select new
            //    {
            //        Manufacturer = m,
            //        Cars = carGroup
            //    };

            foreach (var result in query)
            {
                Console.WriteLine($"\n{result.Manufacturer.Name}, {result.Manufacturer.Headquarters}");
                foreach (var car in result.Cars.OrderByDescending(c => c.Combined).Take(2))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }
        }

        private static void FindBestFromEachCountry(IEnumerable<Car> cars, IEnumerable<Manufacturer> manufacturers, int num)
        {
            Console.WriteLine($"\n Find top {num} cars in each country");

            // Method syntax
            var query =
                manufacturers.GroupJoin(cars, m => m.Name, c => c.Manufacturer, (m, g) =>
                    new
                    {
                        Manufacturer = m,
                        Cars = g
                    })
                .GroupBy(m => m.Manufacturer.Headquarters);

            // Query syntax
            //var query =
            //    from m in manufacturers
            //    join car in cars on m.Name equals car.Manufacturer
            //        into carGroup
            //    orderby m.Name
            //    select new
            //    {
            //        Manufacturer = m,
            //        Cars = carGroup
            //    } into result
            //    group result by result.Manufacturer.Headquarters;

            foreach (var result in query)
            {
                Console.WriteLine($"\n{result.Key}");
                foreach (var car in result.SelectMany(g => g.Cars)
                                          .OrderByDescending(c => c.Combined)
                                          .Take(num))
                {
                    Console.WriteLine($"\t{car.Name} : {car.Combined}");
                }
            }
        }

        private static void FindBestCombinedByManufacturerHq(IEnumerable<Car> cars, IEnumerable<Manufacturer> manufacturers, int num, string manufacturer)
        {
            // Method syntax
            var query = cars.Join(manufacturers,
                                    c => c.Manufacturer,
                                    m => m.Name,
                                    (car, man) => new
                                    {
                                        man.Headquarters,
                                        car.Name,
                                        car.Combined
                                    })
                             .OrderByDescending(c => c.Combined)
                             .ThenBy(c => c.Name);

            //Query syntax
            //var query =
            //    from car in cars
            //    join m in manufacturers 
            //        on car.Manufacturer equals m.Name
            //    orderby car.Combined descending, car.Name ascending
            //    select new
            //    {
            //        m.Headquarters,
            //        car.Name,
            //        car.Combined
            //    };

            Console.WriteLine($"\nTop {num} fuel efficient cars for {manufacturer} in 2016");
            foreach (var car in query.Take(num))
            {
                Console.WriteLine($"{car.Headquarters} - {car.Name} : {car.Combined}");
            }
        }

        private static void FindBestCombined(IEnumerable<Car> cars, int num)
        {
            var query = cars.OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name);

            foreach (var car in query.Take(num))
            {
                Console.WriteLine($"{car.Name} : {car.Combined}");
            }
        }

        private static void FindBestCombinedByManufacturerAndYear(IEnumerable<Car> cars, int num, string manufacturer, int year)
        {
            // Method syntax
            var query = cars.Where(c => c.Manufacturer.Equals(manufacturer) && c.Year == year)
                            .OrderByDescending(c => c.Combined)
                            .ThenBy(c => c.Name);
            //Query syntax
            //var query =
            //    from car in cars
            //    where car.Manufacturer.Equals(manufacturer) && car.Year == 2016
            //    orderby car.Combined descending, car.Name ascending
            //    select car;

            Console.WriteLine($"\nTop {num} fuel efficient cars for {manufacturer} in {year}");
            foreach (var car in query.Take(num))
            {
                Console.WriteLine($"{car.Name} : {car.Combined}");
            }
        }

        private static List<Car> ProcessFuelFile(string path)
        {
            return File.ReadAllLines(path)
                       .Skip(1)
                       .Where(line => line.Length > 1)
                       .Select(Car.ParseFromCsv)
                       .ToList();
        }

        private static List<Car> ProcessFuelFileExtMethod(string path)
        {
            var query =
                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(line => line.Length > 1)
                    .ToCar();

            return query.ToList();
        }

        private static List<Car> ProcessFuelFileQuerySyntax(string path)
        {
            var query =
                from line in File.ReadAllLines(path).Skip(1)
                where line.Length > 1
                select Car.ParseFromCsv(line);

            return query.ToList();
        }

        private static List<Manufacturer> ProcessManufacturerFile(string path)
        {
            return File.ReadAllLines(path)
                       .Where(line => line.Length > 1)
                       .Select(Manufacturer.ParseFromCsv)
                       .ToList();
        }
    }

    public static class CarExtensions
    {
        public static IEnumerable<Car> ToCar(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');
                yield return new Car
                {
                    Year = int.Parse(columns[0]),
                    Manufacturer = columns[1],
                    Name = columns[2],
                    Displacement = double.Parse(columns[3]),
                    Cylinders = int.Parse(columns[4]),
                    City = int.Parse(columns[5]),
                    Highway = int.Parse(columns[6]),
                    Combined = int.Parse(columns[7])
                };
            }

        }
    }
}
