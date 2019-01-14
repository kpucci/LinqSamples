using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<Employee> developers = new Employee[]
            {
                new Employee{ Id = 1, Name = "Scott"},
                new Employee{ Id = 2, Name = "Chris"},
                new Employee{ Id = 3, Name = "Katie"},
                new Employee{ Id = 4, Name = "Bob"}
            };

            IEnumerable<Employee> sales = new List<Employee>()
            {
                new Employee { Id = 3, Name = "Alex"}
            };

            foreach(var person in sales)
            {
                Console.WriteLine(person.Name);
            }

            // Check extension method operation
            Console.WriteLine(developers.Count());

            // For loop using enumerator
            IEnumerator<Employee> enumerator = developers.GetEnumerator();
            while(enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Name);
            }

            // ----- List all developers whose name starts with S -----
            // Named method
            Console.WriteLine("Names start with S - Named Method:");
            foreach(var employee in developers.Where(NameStartsWithS))
                Console.WriteLine(employee.Name);

            // Anonymous method
            Console.WriteLine("Names start with C - Anonymous Method:");
            foreach (var employee in developers.Where(
                delegate (Employee employee) 
                {
                    return employee.Name.StartsWith("C");
                }))
            {
                Console.WriteLine(employee.Name);
            }

            // Lambda expression
            Console.WriteLine("Names start with K - Lambda Expression:");
            foreach (var employee in developers.Where(e => e.Name.StartsWith("K")))
                Console.WriteLine(employee.Name);

            // Func type - named method
            Func<int, int> funcNamed = Square;
            Console.WriteLine("Func type - named method:\t3^2 = {0}", funcNamed(3));

            // Func type - lambda expression
            Func<int, int> funcLambda = x => x*x;
            Console.WriteLine("Func type - lambda exp:\t\t3^2 = {0}", funcLambda(3));

            // Func with two input parameters
            Func<int, int, int> add = (x, y) => x + y;
            Console.WriteLine("Func w/ 2 inputs:\t\t3 + 5 = {0}", add(3,5));

            // Action type
            Action<int> write = x => Console.WriteLine(x);
            Console.WriteLine("Action:");
            write(54);

            // Order developers by name
            Console.WriteLine("Names with 5 letters, ordered by name, method syntax:");
            foreach(var employee in developers.Where(e => e.Name.Length == 5)
                                              .OrderBy(e => e.Name))
                Console.WriteLine(employee.Name);

            // Query syntax
            var query = from e in developers
                        where e.Name.Length == 5
                        orderby e.Name
                        select e;

            Console.WriteLine("Names with 5 letters, ordered by name, query syntax:");
            foreach(var employee in query)
                Console.WriteLine(employee.Name);
        }

        private static int Square(int x)
        {
            return x * x;
        }

        private static bool NameStartsWithS(Employee employee)
        {
            return employee.Name.StartsWith("S");
        }
    }
}
