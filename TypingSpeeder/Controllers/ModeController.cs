using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypingSpeeder.Controllers
{
    static class ModeController
    {

        private static Dictionary<Level, Mode> modes =
            new Dictionary<Level, Mode>
            {
                // Set proper values of the game mode to choose.
                // Mode Step Higher - faster move of the element.
                // Mode Delay Higher - next element apears later.
                // Mode TimeDrop Higher - faster reaches speed maximum.
                {Level.Easy,new Mode(3,5,1,800) {Level=Level.Easy, MaxNumberRange=10 } },
                {Level.Normal,new Mode(5,4,5,500) {Level=Level.Normal, MaxNumberRange=100 } },
                {Level.Hard,new Mode(10,3,10,100) {Level=Level.Hard, MaxNumberRange=1000 } }
            };

        public static Mode GetModeValues(Level mode)
        {
            Mode result = null;
            modes.TryGetValue(mode, out result);
            return result;
        }
    }
}
