using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queries
{
    public class Movie
    {
        public string Title { get; set; }
        public float Rating { get; set; }

        private int _year;
        public int Year {
            get
            {
                Console.WriteLine($"Returning {_year} for {Title}");
                return _year;
            }
            set
            {
                _year = value;
            }
        }

        private int _yearExc;
        public int YearExc
        {
            get
            {
                throw new Exception("Error!");
                Console.WriteLine($"Returning {_yearExc} for {Title}");
                return _yearExc;
            }
            set
            {
                _yearExc = _year;
            }
        }
    }
}
