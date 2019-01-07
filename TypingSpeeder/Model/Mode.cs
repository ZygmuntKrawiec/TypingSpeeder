using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypingSpeeder.Model
{
    public class Mode
    {
        public double Step { get; private set; }
        public int Delay { get; private set; }
        public int TimeDrop { get; private set; }
        public int MinTime { get; private set; }
        public Level Level { get; set; }
        public TextType TextType { get; set; }
        public int MaxNumberRange { get; set; }

        public Mode(double step, int delay, int timeDrop, int minTime)
        {
            Step = step;
            Delay = delay;
            TimeDrop = timeDrop;
            MinTime = minTime;
        }
    }
}
