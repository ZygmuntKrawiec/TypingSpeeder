using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypingSpeeder.Controllers
{
    public static class ScoreController
    {
        static int points = 0;
        static int mishit = 0;

        public static int Point()
        {
            return ++points;
        }

        public static int Missed()
        {
            return ++mishit;
        }

        public static void Reset()
        {
            points = 0;
            mishit = 0;
        }
    }
}
