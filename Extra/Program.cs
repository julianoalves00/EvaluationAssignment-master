using System;
using System.Collections.Generic;
using System.Linq;
using AvCalc.Logic;
using AvCalc.Models;

namespace AvCalc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var count = 10000;
            if (args.Length > 0 && args[0].Equals("-c"))
            {
                if (!int.TryParse(args[1], out count))
                {
                    Console.WriteLine("Invalid count number");
                    return;
                }
            }

            var sequentialCollisations = 0;
            var continueForever = false;
            var random = new Random(342);
            
            var patternGenerator = new DailyPatternGenerator();
            DailyPattern previous = null;
            var list1 = new List<DailyPattern>();
            var list2 = new List<DailyPattern>();

            for (var i = 0; i < count; i++)
            {
                var minutes = random.Next(-20, 240);
                var dailyPatterns = patternGenerator.GenerateDailyPatternList(DateTime.Now.AddMinutes(minutes).AddDays(i*3), 10, 2, true);

                if (list1.Count > 0 && list1.Last().ColorId.Equals(dailyPatterns.First().ColorId))
                    Console.WriteLine("Sequential collision");

                list1.AddRange(dailyPatterns);
            }

            for (var i = 0; i < count; i++)
            {
                var minutes = random.Next(-20, 180);
                var dailyPatterns = patternGenerator.GenerateDailyPatternList(DateTime.Now.AddMinutes(minutes).AddDays(i*3), 10, 2, true);
                list2.AddRange(dailyPatterns);
            }

            if(list1.Count != list2.Count)
                throw new Exception("List item count mismatch");

            for (var i = 0; i < list1.Count; i++)
            {
                var d1 = list1[i];
                var d2 = list2[i];

                if (i == 0) previous = d1;
                if (i != 0 && previous?.ColorId == d1.ColorId)
                {
                    Console.WriteLine($"Sequential collision, previous ColorId: {previous.ColorId}, current ColorId: {d1.ColorId} ");
                    sequentialCollisations++;
                }
                previous = d1;

                Console.WriteLine($"Color1: {d1.ColorId}, Color2: {d2.ColorId} | PatternId1: {d1.PatternId}, PatternId2: {d2.PatternId} | Valid to (1): {d1.ValidTo}, Valid to (2): {d2.ValidTo}");
                if(!d1.ColorId.Equals(d2.ColorId)) throw new Exception("Inconsistent color generation");
                if (!d1.PatternId.Equals(d2.PatternId)) throw new Exception("Inconsistent pattern generation");

                if (continueForever) continue;

                var key = Console.ReadKey();
                if (key.KeyChar.Equals('q')) break;
                if (key.KeyChar.Equals('c')) { continueForever = true; };
            }

            Console.WriteLine($"Sequential Collisions: {sequentialCollisations} of total {list1.Count} items.");
            Console.ReadKey();
        }
    }
}
