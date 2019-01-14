﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Queries
{
    public static class MyLinq
    {
        public static IEnumerable<double> Random()
        {
            var random = new Random();
            while(true)
            {
                yield return random.NextDouble();
            }
        }

        public static IEnumerable<T> FilterDeferred<T>(this IEnumerable<T> source, 
                                            Func<T, bool> predicate)
        {
            foreach(var item in source)
            {
                if(predicate(item))
                    yield return item;
            }
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source,
                                            Func<T, bool> predicate)
        {
            var items = new List<T>();
            foreach (var item in source)
            {
                if (predicate(item))
                    items.Add(item);
            }

            return items;
        }
    }
}