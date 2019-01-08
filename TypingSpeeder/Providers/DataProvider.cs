using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TypingSpeeder.Model;

namespace TypingSpeeder.Providers
{
    static class DataProvider
    {
        static Random random = new Random();
        static DataTextProvider dataTextProvider = new DataTextProvider();


        public static int InitializeInt(int min, int max)
        {
            Thread.Sleep(100); // <-- TODO: FIX this ugly shit!!!
            var newValue = random.Next(min, max);
            return newValue;
        }

        public static double InitializeDouble()
        {
            return (random.NextDouble());
        }

        public static string GenerateRandomValue(Level level, TextType textType, int maxNumber)
        {

            // Set min and max range of generated values.
            var maxRange = (int)textType + ((int)level) * 2;
            var minRange = maxRange;// <- TODO: After implementation a Random option minRange should be 0.
            // Generate a random value to choose type of generated text.
            var rand = InitializeInt(minRange, maxRange);
            // Calculate lenght of the string which represents double value, ex. 99,99 or 888,888
            var substringLenght = (maxNumber.ToString().Count() * 2) - 1;

            // Generate chosen text.
            switch (rand)
            {
                case 0: return InitializeInt(0, maxNumber).ToString();
                case 1: return ((char)InitializeInt(33, 127)).ToString();
                case 2: return (InitializeDouble() * maxNumber).ToString().Substring(0, substringLenght);
                case 3: return dataTextProvider.GetWord();
                case 4: return (InitializeDouble() * maxNumber).ToString();
                case 5: return createSentence();
                default: return null;
            }
        }

        /// <summary>
        /// Resets the data provider.
        /// </summary>
        public static void Reset()
        {
            dataTextProvider.Reset();
        }

        private static string createSentence()
        {
            StringBuilder sentence = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                sentence.Append($"{dataTextProvider.GetWord()} ");
            }
            return sentence.ToString();
        }
    }
}
