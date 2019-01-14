using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            var movies = new List<Movie>
            {
                new Movie { Title = "The Dark Knight",   Rating = 8.9f, Year = 2008},
                new Movie { Title = "The King's Speech", Rating = 8.0f, Year = 2010},
                new Movie { Title = "Casablanca",        Rating = 8.5f, Year = 1942},
                new Movie { Title = "Star Wars V",       Rating = 8.7f, Year = 1980}
            };

            // Want movies after year 2000
            // Method query
            Console.WriteLine("-----Movies after 2000 - method query-----\n");
            var query = movies.Where(m => m.Year > 2000);
            foreach (var m in query)
                Console.WriteLine(m.Title);

            // Custom extension method - undeferred
            Console.WriteLine("\n\n-----Movies after 2000 - custom method, undeferred-----\n");
            var query2 = movies.Filter(m => m.Year > 2000);
            Console.WriteLine(query2.Count());
            foreach (var m in query2)
                Console.WriteLine(m.Title);

            // Custom extension method - deferred
            Console.WriteLine("\n\n-----Movies after 2000 - custom method, deferred-----\n");
            var query3 = movies.FilterDeferred(m => m.Year > 2000);
            Console.WriteLine(query3.Count());
            foreach (var m in query3)
                Console.WriteLine(m.Title);

            // Using enumerator
            Console.WriteLine("\n\n-----Movies after 2000 - enumerator-----\n");
            var enumerator = query2.GetEnumerator();
            while (enumerator.MoveNext())
                Console.WriteLine(enumerator.Current.Title);

            // Undeferred execution with exception
            Console.WriteLine("\n\n-----Movies after 2000 - custom method, undeferred, exception-----\n");
            var query4 = Enumerable.Empty<Movie>();
            try
            {
                query4 = movies.FilterDeferred(m => m.YearExc > 2000).ToList();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine(query4.Count());
            foreach (var m in query4)
                Console.WriteLine(m.Title);

            // Deferred execution with exception
            Console.WriteLine("\n\n-----Movies after 2000 - custom method, deferred, exception-----\n");
            var query5 = movies.FilterDeferred(m => m.YearExc > 2000);
            try
            {
                Console.WriteLine(query4.Count());
                foreach (var m in query4)
                    Console.WriteLine(m.Title);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Deferred execution with infinite source
            Console.WriteLine("\n\n-----Infinite list of random numbers, deferred-----\n");
            var numbers = MyLinq.Random().Where(n => n > 0.5).Take(10);
            foreach(var num in numbers)
                Console.WriteLine(num);
        }
    }
}
