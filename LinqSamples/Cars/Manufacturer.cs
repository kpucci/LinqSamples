using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    public class Manufacturer
    {
        public string Name { get; set; }
        public string Headquarters { get; set; }
        public int Year { get; set; }

        public static Manufacturer ParseFromCsv(string line)
        {
            var columns = line.Split(',');

            return new Manufacturer
            {
                Name = columns[0],
                Headquarters = columns[1],
                Year = int.Parse(columns[2])
            };
        }
    }
}
