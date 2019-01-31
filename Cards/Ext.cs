using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards
{
    static class Extensions
    {
        // Implementation of fisher-yates algorithm
        // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        public static void Shuffle<T>(this Random rng, List<T> data)
        {
            int n = data.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T Temp = data[n];
                data[n] = data[k];
                data[k] = Temp;
            }
        }
    }
}
