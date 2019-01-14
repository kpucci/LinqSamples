using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Features
{
    public static class MyLinq
    {
        // Extension method on IEnumerable to count number of items in sequence
        public static int Count(this IEnumerable<Task> sequence)
        {
            int count = 0;
            foreach (var item in sequence)
                count++;

            return count;
        }
    }
}
